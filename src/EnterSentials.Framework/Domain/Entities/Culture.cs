using EnterSentials.Framework.Properties;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace EnterSentials.Framework
{
    public class Culture : NameKeyedEntityBase
    {
        private static KeyedCollection<string, Culture> AllInstances = null;
        private static readonly object AllInstancesLock = new object();

        public static readonly Culture English_US = new Culture
        {            
            Name = Settings.Default.Culture_EnglishUS_Name 
        };


        
        public static IEnumerable<Culture> GetAll()
        {
            if (AllInstances == null)
            {
                lock (AllInstancesLock)
                {
                    if (AllInstances == null)
                    {
                        AllInstances = typeof(Culture).GetFields(BindingFlags.Static | BindingFlags.Public)
                            .Where(field => field.FieldType == typeof(Culture))
                            .Select(field => field.GetValue(null))
                            .Cast<Culture>()
                            .ToKeyedCollection<CultureKeyedCollection, string, Culture>();
                    }
                }
            }

            return AllInstances;
        }


        public static Culture For(string cultureName)
        { return GetAll().ToKeyedCollection<CultureKeyedCollection, string, Culture>()[cultureName]; }


        private class CultureKeyedCollection : KeyedCollection<string, Culture>
        {
            protected override string GetKeyForItem(Culture item)
            { return item.Name; }
        }
    }
}
