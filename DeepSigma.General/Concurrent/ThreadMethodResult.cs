using System.Collections.Concurrent;

namespace DeepSigma.General.Concurrent
{
    public class ThreadMethodResult<T>
    {
        /// <summary>
        /// Method messages logged during execution.
        /// </summary>
        public ConcurrentQueue<string> MessageLog { get; set; } = [];

        /// <summary>
        /// Results of the method execution.
        /// </summary>
        public required T MethodOutputResult { get; set; }
    }
}
