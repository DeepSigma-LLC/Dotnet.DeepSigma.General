using System.Collections.Concurrent;

namespace DeepSigma.General.Concurrency
{
    /// <summary>
    /// Represents a collection of tasks.
    /// </summary>
    public class TaskCollection
    {
        public ConcurrentQueue<string> MessageLog { get; set; } = [];
        public ConcurrentQueue<ThreadStatusDataModel> ThreadStatusDataModels { get; set; } = [];

        /// <summary>
        /// Returns the percentage of tasks that are complete in the collection.
        /// </summary>
        /// <returns></returns>
        public double GetPercentageComplete()
        {
            double completedTasks = ThreadStatusDataModels.Where(x => x.IsTaskComplete == true).Count();
            return completedTasks / GetTotalTaskCount() * 100;
        }

        /// <summary>
        /// Returns the total number of tasks in the collection.
        /// </summary>
        /// <returns></returns>
        public double GetTotalTaskCount()
        {
            return ThreadStatusDataModels.Count;
        }
    }
}
