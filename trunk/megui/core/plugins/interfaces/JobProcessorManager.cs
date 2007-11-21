using System;
using System.Collections.Generic;
using System.Text;
using MeGUI;

namespace MeGUI.core.plugins.implemented
{
    public class JobProcessorManager<TJob> : GenericRegisterer<JobProcessorFactory>
        where TJob : Job
    {
        public IJobProcessor CreateProcessor(MainForm info, Job job)
        {
            Dictionary<JobProcessorFactory, IJobProcessor> processors = new Dictionary<JobProcessorFactory,IJobProcessor>();
            foreach (JobProcessorFactory factory in this.Values)
            {
                IJobProcessor processor = factory.Factory(info, job);
                if (processor != null)
                    processors.Add(factory, processor);
            }
            return null;
        }
    }
}
