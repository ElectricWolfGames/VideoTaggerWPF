using System;
using System.IO;

namespace SystemWolfCommon
{
    /// <summary>
    /// The file data store
    /// </summary>
    public class FileDataStore
    {
        /// <summary>
        /// The last time the file was modified
        /// </summary>
        public DateTime LastModifitedTime
        {
            get;
            set;
        }

        /// <summary>
        /// Get and sets the file name
        /// </summary>
        public string FName
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
            }
        }

        /// <summary>
        /// Get and set the full path
        /// </summary>
        public string FullPath
        {
            get
            {
                return _path;
            }

            set
            {
                _path = value;
                _name = Path.GetFileName(_path);
            }
        }

        /// <summary>
        /// Get and set the path
        /// </summary>
        public string PathSet
        {
            get
            {
                return _patSet;
            }

            set
            {
                _patSet = value;
            }
        }

        /// <summary>
        /// The item name
        /// </summary>
        private string _name;

        /// <summary>
        /// The item path
        /// </summary>
        private string _path;

        /// <summary>
        /// The path set
        /// </summary>
        private string _patSet;
    }
}
