using System;
using System.Collections.Generic;
using System.Text;

namespace MeGUI
{
    public delegate void JobProcessingStatusUpdateCallback(StatusUpdate su);
    public delegate void Processor(MainForm info, Job job);
    
    /// <summary>
    /// Returns an IJobProcessor if this job can be processed, returns null otherwise.
    /// </summary>
    /// <param name="info"></param>
    /// <param name="job"></param>
    /// <returns></returns>
    public delegate IJobProcessor ProcessorFactory(MainForm info, Job job);
    public sealed class JobPreProcessor : IIDable
    {
        private string id;
        public string ID
        {
            get { return id; }
        }
        private Processor processor;
        public Processor PreProcessor
        {
            get { return processor; }
        }
        public JobPreProcessor(Processor p, string id)
        {
            this.id = id;
            this.processor = p;
        }
    }
    public sealed class JobPostProcessor : IIDable
    {
        private string id;
        public string ID
        {
            get { return id; }
        }
        private Processor processor;
        public Processor PostProcessor
        {
            get { return processor; }
        }
        public JobPostProcessor(Processor p, string id)
        {
            this.id = id;
            this.processor = p;
        }
    }
    public sealed class JobProcessorFactory : IIDable
    {
        private string id;
        public string ID
        {
            get { return id; }
        }
        private ProcessorFactory factory;
        public ProcessorFactory Factory
        {
            get { return factory; }
        }
        public JobProcessorFactory(ProcessorFactory factory, string id)
        {
            this.factory = factory;
            this.id = id;
        }
    }
	
    public interface IJobProcessor
    {
        /// <summary>
        /// sets up encoding
        /// </summary
        /// <param name="job">the job to be processed</param>
        /// <param name="error">output for any errors that might ocurr during this method</param>
        /// <returns>true if the setup has succeeded, false if it has not</returns>
        bool setup(Job job, out string error);
        /// <summary>
        /// starts the encoding process
        /// </summary>
        /// <param name="error">output for any errors that might ocurr during this method</param>
        /// <returns>true if encoding has been successfully started, false if not</returns>
        bool start(out string error);
        /// <summary>
        /// stops the encoding process
        /// </summary>
        /// <param name="error">output for any errors that might ocurr during this method</param>
        /// <returns>true if encoding has been successfully stopped, false if not</returns>
        bool stop(out string error);
        /// <summary>
        /// pauses the encoding process
        /// </summary>
        /// <param name="error">output for any errors that might ocurr during this method</param>
        /// <returns>true if encoding has been successfully paused, false if not</returns>
        bool pause(out string error);
        /// <summary>
        /// resumes the encoding process
        /// </summary>
        /// <param name="error">output for any errors that might ocurr during this method</param>
        /// <returns>true if encoding has been successfully started, false if not</returns>
        bool resume(out string error);
        /// <summary>
        /// changes the priority of the encoding process/thread
        /// </summary>
        /// <param name="priority">the priority to change to</param>
        /// <param name="error">output for any errors that might ocurr during this method</param>
        /// <returns>true if the priority has been changed, false if not</returns>
        bool changePriority(ProcessPriority priority, out string error);
        /// <summary>
        /// returns whether this jobprocessor can process the job given as parameter
        /// </summary>
        /// <param name="job">the job to be processed</param>
        /// <returns>true of the job can be processed, false if not</returns>
        bool canBeProcessed(Job job);
        event JobProcessingStatusUpdateCallback StatusUpdate;
    }
}
