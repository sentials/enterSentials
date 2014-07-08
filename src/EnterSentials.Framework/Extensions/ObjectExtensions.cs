using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Script.Serialization;

namespace EnterSentials.Framework
{
    public static class ObjectExtensions
    {
        public static bool IsValidByDataAnnotations(
            this object @object, 
            ValidationContext validationContext, 
            out IEnumerable<ValidationResult> validationResults
        )
        {
            var results = new Collection<ValidationResult>();

            var isValidOrNot = Validator.TryValidateObject(
                @object,
                validationContext,
                results,
                validateAllProperties: true
            );

            validationResults = results;

            return isValidOrNot;
        }


        public static bool IsValidByDataAnnotations(this object @object, out IEnumerable<ValidationResult> validationResults)
        { return @object.IsValidByDataAnnotations(@object.GetSimpleValidationContext(), out validationResults); }


        public static bool IsValidByDataAnnotations(this object @object)
        {
            var ignored = Enumerable.Empty<ValidationResult>();
            return IsValidByDataAnnotations(@object, out ignored);
        }

        public static ValidationContext GetSimpleValidationContext(this object @object)
        { return new ValidationContext(@object, serviceProvider: null, items: null); }


        public static IEnumerable<T> ToEnumerable<T>(this T @object)
        { return @object is IEnumerable<T> ? ((IEnumerable<T>) @object) : new T[] { @object }; }


        public static string SerializeToJson<T>(this T @object)
        { return Components.Instance.Get<IJsonSerializer>().Serialize(@object); }

        public static byte[] SerializeToJsonBytes<T>(this T @object)
        { return Components.Instance.Get<IJsonSerializer>().SerializeToBytes(@object); }
        

        public static string SerializeToXml<T>(this T @object)
        { return Components.Instance.Get<IXmlSerializer>().Serialize(@object); }


        public static bool IsMany(this object o)
        {
            IEnumerable manyObjects;
            return o.TryAsMany(out manyObjects);
        }

        public static bool IsMany<T>(this object o)
        {
            IEnumerable<T> manyObjects;
            return o.TryAsMany<T>(out manyObjects);
        }


        public static IEnumerable AsMany(this object o)
        {
            IEnumerable manyObjects;
            if (!o.TryAsMany(out manyObjects))
                throw new InvalidCastException();
            return manyObjects;
        }

        public static IEnumerable<T> AsMany<T>(this object o)
        {
            IEnumerable<T> manyObjects;
            if (!o.TryAsMany<T>(out manyObjects))
                throw new InvalidCastException();
            return manyObjects;
        }


        public static bool TryAsMany(this object o, out IEnumerable manyObjects)
        {
            bool canConvertOrNot = o is IEnumerable;
            manyObjects = canConvertOrNot ? (IEnumerable)o : null;
            return canConvertOrNot;
        }


        public static bool TryConvertTo<T>(this object o, out T convertedObject)
        {
            object co;
            bool ableToConvertOrNot = TryConvertTo(o, typeof(T), out co);
            convertedObject = ableToConvertOrNot ? (T)co : default(T);
            return ableToConvertOrNot;
        }


        public static bool TryConvertTo(this object o, Type type, out object convertedObject)
        {
            Type fromType = o.GetType();
            Type toType = type;

            convertedObject = null;
            bool canConvertOrNot = true;
            TypeConverter typeConverter;

            try
            {
                if (toType.IsAssignableFrom(fromType))
                    convertedObject = o;
                else if ((typeConverter = TypeDescriptor.GetConverter(fromType)).CanConvertTo(toType))
                    convertedObject = typeConverter.ConvertTo(o, toType);
                else if ((typeConverter = TypeDescriptor.GetConverter(toType)).CanConvertFrom(fromType))
                    convertedObject = TypeDescriptor.GetConverter(toType).ConvertFrom(fromType);
                else
                    canConvertOrNot = false;
            }
            catch
            { canConvertOrNot = false; }

            return canConvertOrNot;
        }


        public static bool TryAsMany<T>(this object o, out IEnumerable<T> manyObjects)
        {
            IEnumerable someObjects;
            manyObjects = null;
            bool canConvertOrNot = o.TryAsMany(out someObjects);

            if (canConvertOrNot)
            {
                if (o == null)
                    manyObjects = new T[] { };
                if (o is IEnumerable<T>)
                    manyObjects = (IEnumerable<T>)o;
                else
                {
                    var convertedObjects = new Collection<T>();
                    var enumerator = someObjects.GetEnumerator();
                    T convertedObject;

                    if (enumerator.MoveNext())
                    {
                        do
                        {
                            if (!enumerator.Current.TryConvertTo<T>(out convertedObject))
                            {
                                canConvertOrNot = false;
                                break;
                            }
                            else
                                convertedObjects.Add(convertedObject);
                        } while (enumerator.MoveNext());

                        if (canConvertOrNot)
                            manyObjects = convertedObjects;
                    }
                    else
                    {
                        if (typeof(T) == typeof(object))
                            canConvertOrNot = true;
                        else
                        {
                            canConvertOrNot = false;

                            var enumerableInterfaces = o.GetType().GetInterfaces().Where(i =>
                                (typeof(IEnumerable).IsAssignableFrom(i)) &&
                                (i.IsGenericType) &&
                                (i.GetGenericArguments().Count() == 1)
                            );

                            foreach (var enumerableInterface in enumerableInterfaces)
                            {
                                if (typeof(T).IsAssignableFrom(
                                    enumerableInterface.GetGenericArguments().ElementAt(0)))
                                {
                                    canConvertOrNot = true;
                                    break;
                                }
                            }
                        }

                        manyObjects = new T[] { };
                    }
                }
            }

            return canConvertOrNot;
        }


        public static IEnumerable<KeyValuePair<string, object>> GetEnumeratedProperties(this object @object)
        {
            return @object.GetType().GetProperties().ToDictionary(
                propertyInfo => propertyInfo.Name,
                propertyInfo => propertyInfo.GetValue(@object)
            );
        }


        public static T CopyFrom<T, TSource>(
            this T @object, 
            TSource source, 
            SourceProperties cloning = SourceProperties.ByReference,
            DefinedCopiers with = DefinedCopiers.Included)
        {
            var mappers = Components.Instance.Get<IObjectMapperFactory>();
            var copier = mappers.GetCopier<TSource, T>(cloning: cloning, with: with);
            copier.Copy(source, @object);
            return @object;
        }


        public static T CopyFrom<T>(
            this T @object,
            Type sourceType,
            object source,
            SourceProperties cloning = SourceProperties.ByReference,
            DefinedCopiers with = DefinedCopiers.Included)
        {
            var mappers = Components.Instance.Get<IObjectMapperFactory>();
            var copier = mappers.GetCopier(sourceType, typeof(T), cloning: cloning, with: with);
            copier.Copy(source, @object);
            return @object;
        }


        public static object CopyFrom<TSource>(
            this object @object,
            TSource source,
            SourceProperties cloning = SourceProperties.ByReference,
            DefinedCopiers with = DefinedCopiers.Included)
        { return @object.CopyFrom(typeof(TSource), source, cloning: cloning, with: with); }

        
        public static TTo Translated<TFrom, TTo>(
            this TFrom @object,
            SourceProperties cloning = SourceProperties.ByReference,
            DefinedTranslators with = DefinedTranslators.Included)
        {
            var mappers = Components.Instance.Get<IObjectMapperFactory>();
            var translator = mappers.GetTranslator<TFrom, TTo>(cloning: cloning, with: with);
            return translator.Translate(@object);
        }


        public static TTo TranslatedTo<TTo>(
            this object @object,
            SourceProperties cloning = SourceProperties.ByReference,
            DefinedTranslators with = DefinedTranslators.Included)
        {
            var mappers = Components.Instance.Get<IObjectMapperFactory>();
            var translator = mappers.GetTranslator(@object.GetType(), typeof(TTo), cloning: cloning, with: with);
            return (TTo)translator.Translate(@object);
        }


        public static T Clone<T>(this T @object)
        {
            var mappers = Components.Instance.Get<IObjectMapperFactory>();
            
            var translator = mappers.GetTranslator<T, T>(
                cloning: SourceProperties.ByValueRecursively, 
                with: DefinedTranslators.Excluded);

            return translator.Translate(@object);
        }
    }
}