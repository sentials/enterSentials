using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Linq;

namespace EnterSentials.Framework
{
    public abstract class DomainObjectBase : IDomainObject
    {
        private static readonly BindingFlags CollectionPropertiesBindingFlags = BindingFlags.Public | BindingFlags.Instance;
        private static readonly IDictionary<Type, IEnumerable<Tuple<PropertyInfo, Type>>> typeMetadata = new Dictionary<Type, IEnumerable<Tuple<PropertyInfo, Type>>>();
        

        private static IEnumerable<Tuple<PropertyInfo, Type>> GetCollectionPropertiesMetadataFor(Type type)
        {
            var properties = (IEnumerable<Tuple<PropertyInfo, Type>>) null;

            if (!typeMetadata.TryGetValue(type, out properties))
            {
                lock (typeMetadata)
                {
                    if (!typeMetadata.TryGetValue(type, out properties))
                    {
                        var collectionProperties = new Collection<Tuple<PropertyInfo, Type>>();

                        foreach (var property in type.GetProperties(CollectionPropertiesBindingFlags))
                        {
                            var genericArguments = (IEnumerable<Type>) null;
                            
                            if (property.SetMethod != null && 
                                (property.PropertyType.IsFulfilledGenericType(typeof(ICollection<>), out genericArguments) || 
                                 property.PropertyType.IsFulfilledGenericType(typeof(Collection<>), out genericArguments)))
                            {
                                collectionProperties.Add(
                                    new Tuple<PropertyInfo, Type>(
                                        property,
                                        typeof(Collection<>).MakeGenericType(genericArguments.Single()))); 
                            }
                        }

                        properties = typeMetadata[type] = collectionProperties;
                    }
                }
            }

            return properties;
        }


        private void InitializeCollectionProperties()
        {
            foreach (var propertyMetadata in GetCollectionPropertiesMetadataFor(this.GetType()))
                propertyMetadata.Item1.SetValue(this, Activator.CreateInstance(propertyMetadata.Item2));
        }


        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedOn { get; set; }

        public DomainObjectBase()
        { 
            CreatedOn = DomainPolicy.GetCurrentTime();
            InitializeCollectionProperties();
        }
    }
}