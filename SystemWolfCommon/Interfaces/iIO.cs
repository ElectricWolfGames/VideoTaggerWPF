namespace SystemWolfCommon
{
    /// <summary>
    /// IO interface
    /// </summary>
    public interface IIO
    {
        /// <summary>
        /// Save out the data
        /// </summary>
        /// <returns>True is we have saved the data</returns>
		bool Save();

        /// <summary>
        /// load the data
        /// </summary>
        /// <returns>True if we loaded the data</returns>
		bool Load();
    }
}
