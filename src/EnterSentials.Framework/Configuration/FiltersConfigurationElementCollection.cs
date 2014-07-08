using System.Collections.Generic;

namespace EnterSentials.Framework
{
    public class FiltersConfigurationElementCollection : ConfigurationElementCollection<FilterConfigurationElement>
    {
        protected override string ElementName
        { get { return "filter"; } }


        public FiltersConfigurationElementCollection()
        { }

        public FiltersConfigurationElementCollection(FilterConfigurationElement[] filters) : base(filters)
        { }

        public FiltersConfigurationElementCollection(IEnumerable<FilterConfigurationElement> filters) : base(filters)
        { }
    }
}
