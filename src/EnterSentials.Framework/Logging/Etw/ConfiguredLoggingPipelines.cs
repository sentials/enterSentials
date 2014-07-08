using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EnterSentials.Framework
{
    public class ConfiguredLoggingPipelines : ScopeLimitedObject, ILoggingPipelines
    {
        private static readonly object pipelinesLock = new object();
        private static bool pipelinesAreInitialized = false;
        private static ICollection<LoggingPipeline> pipelines = new Collection<LoggingPipeline>();


        private static void InitializePipelinesIfNecessary(
            ILoggingPipelineConfigurations pipelineConfigurations,
            ILoggingPipelineFactory pipelineFactory)
        {
            if (!pipelinesAreInitialized)
            {
                lock (pipelinesLock)
                {
                    if (!pipelinesAreInitialized)
                    {
                        foreach (var configuration in pipelineConfigurations)
                            pipelines.Add(pipelineFactory.Get(configuration));

                        pipelinesAreInitialized = true;
                    }
                }
            }
        }


        private static void DisposeOfPipelinesIfNecessary()
        {
            if (pipelinesAreInitialized)
            {
                lock (pipelinesLock)
                {
                    if (pipelinesAreInitialized)
                    {
                        foreach (var pipeline in pipelines)
                            pipeline.Dispose();

                        pipelines.Clear();
                        pipelines = null;
                        pipelinesAreInitialized = false;
                    }
                }
            }
        }


        public IEnumerator<LoggingPipeline> GetEnumerator()
        {
            lock (pipelinesLock)
            { return pipelines.GetEnumerator(); }
        }

        IEnumerator IEnumerable.GetEnumerator()
        { return GetEnumerator(); }


        protected void OnDispose()
        { DisposeOfPipelinesIfNecessary(); }


        protected override void OnDisposeExplicit()
        {
            base.OnDisposeExplicit();
            OnDispose();
        }


        protected override void OnDisposeImplicit()
        {
            base.OnDisposeImplicit();
            OnDispose();
        }
        

        public ConfiguredLoggingPipelines(ILoggingPipelineConfigurations pipelineConfigurations, ILoggingPipelineFactory pipelineFactory)
        {
            Guard.AgainstNull(pipelineConfigurations, "pipelineConfigurations");
            Guard.AgainstNull(pipelineFactory, "pipelineFactory");

            InitializePipelinesIfNecessary(pipelineConfigurations, pipelineFactory);
        }
    }
}
