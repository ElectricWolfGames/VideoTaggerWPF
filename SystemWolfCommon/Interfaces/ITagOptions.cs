namespace SystemWolfCommon.Interfaces
{
    /// <summary>
    /// User options for the tag holder and tag list
    /// </summary>
    public interface ITagOptions
    {
        /// <summary>
        /// Add the default tag when empty
        /// </summary>
        bool AddDefaultMissingTag { get; set; }

        /// <summary>
        /// Automatically play the next video
        /// </summary>
        bool AutomaticallyPlayNextVideo { get; set; }

        /// <summary>
        /// Apply the file date
        /// </summary>
        bool ApplyFileDate { get; set; }

        /// <summary>
        /// The date format string to use
        /// </summary
        string DateFormat { get; set; }
    }
}
