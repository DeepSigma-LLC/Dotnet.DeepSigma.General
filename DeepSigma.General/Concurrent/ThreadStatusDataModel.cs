using System.Collections.Concurrent;
using System.Diagnostics;

namespace DeepSigma.General.Concurrent
{
    /// <summary>
    /// Data model for tracking the status of a thread's execution.
    /// </summary>
    public class ThreadStatusDataModel
     {  
         public Guid Guid { get; } = Guid.NewGuid();
         public bool IsTaskStarted { get; private set; } = false;
         public bool IsTaskRunning { get; private set; } = false;
         public bool IsTaskComplete { get; private set; } = false;
         public DateTime? StartDateTime { get; private set; }
         public DateTime? EndDateTime { get; private set; }
         public int ExecutionManagedThreadId { get; private set; }
         public string? CurrentThreadName { get; private set; }
         public int CurrentProcessorId { get; private set; }
         public ConcurrentQueue<string> TaskMessageLog { get; set; } = [];
         
         private Stopwatch _taskStopWatch = new();

        /// <summary>
        /// Initializes task and class properties for current task execution.
        /// </summary>
        public void TaskStart()
        {
            StartDateTime = DateTime.Now;
            ExecutionManagedThreadId = Thread.CurrentThread.ManagedThreadId;
            CurrentThreadName = Thread.CurrentThread.Name;
            CurrentProcessorId = Thread.GetCurrentProcessorId();
            IsTaskStarted = true;
            IsTaskRunning = true;
            _taskStopWatch.Start();
        }

        /// <summary>
        /// Mark current task as complete.
        /// </summary>
        public void TaskComplete()
        {
            IsTaskComplete = true;
            TaskStop();
        }

        /// <summary>
        /// Stops task.
        /// </summary>
        public void TaskStop() 
        {
            _taskStopWatch.Stop();
            EndDateTime = DateTime.Now;
            IsTaskRunning = false;
        }

        /// <summary>
        /// Returns task current run time.
        /// </summary>
        /// <returns></returns>
        public long GetRunTime()
        {
            //Start then start stopwatch in order for ElapsedTime to be updated.
            _taskStopWatch.Stop();
            _taskStopWatch.Start();
            return _taskStopWatch.ElapsedMilliseconds;
        }
    }
}
