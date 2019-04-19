using System.ComponentModel;

namespace SystemWolfCommon
{
    /// <summary>
    /// The item runner interface
    /// </summary>
	public interface IItemRunner
    {
        /// <summary>
        /// The run method
        /// </summary>
		void Run();

        /// <summary>
        /// Run in background
        /// </summary>
        /// <param name="o">The object</param>
        /// <param name="e">The Event args</param>
		void Run_BackGroundWorker(object o, DoWorkEventArgs e);
    }
}
