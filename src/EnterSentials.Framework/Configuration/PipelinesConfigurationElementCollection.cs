using System.Collections.Generic;

namespace EnterSentials.Framework
{
    public class PipelinesConfigurationElementCollection : ConfigurationElementCollection<PipelineConfigurationElement>
    {
        protected override string ElementName
        { get { return "pipeline"; } }


        public PipelinesConfigurationElementCollection()
        { }

        public PipelinesConfigurationElementCollection(PipelineConfigurationElement[] pipelines) : base(pipelines)
        { }

        public PipelinesConfigurationElementCollection(IEnumerable<PipelineConfigurationElement> pipelines) : base(pipelines)
        { }
    }
}