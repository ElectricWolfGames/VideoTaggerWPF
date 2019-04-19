using System;
using System.Collections.Generic;
using System.IO;

namespace SystemWolfCommon
{
    /// <summary>
    /// The base item runner
    /// </summary>
    public class BaseItemRunner
    {
        /// <summary>
        /// Gets or sets the reporter
        /// </summary>
        public string Report
        {
            get
            {
                return _report;
            }

            set
            {
                _report = value;
            }
        }

        /// <summary>
        /// Gets all of the files from a folder
        /// </summary>
        /// <param name="dir">The folder to get from</param>
        /// <returns>The list of files</returns>
        public List<CompareFiles.CF_FileData> GetFilesFromFolder(string dir)
        {
            // TODO : A : remove and replace with Directory.GetDirectories(sDir, true)
            List<CompareFiles.CF_FileData> lister = new List<CompareFiles.CF_FileData>();

            foreach (string f in Directory.GetFiles(dir))
            {
                // Console.WriteLine(f);
                lister.Add(new CompareFiles.CF_FileData(f));
            }

            DirSearch(dir, lister);

            return lister;
        }

        /// <summary>
        /// Search a directory
        /// </summary>
        /// <param name="dir">The folder to search</param>
        /// <param name="list">The list to add the files to</param>
        /// <returns>Returns the list</returns>
        public List<CompareFiles.CF_FileData> DirSearch(string dir, List<CompareFiles.CF_FileData> list)
        {
            // TODO : A : remove and replace with Directory.GetDirectories(sDir, true)
            try
            {
                foreach (string d in Directory.GetDirectories(dir))
                {
                    foreach (string f in Directory.GetFiles(d))
                    {
                        // Console.WriteLine(f);
                        list.Add(new CompareFiles.CF_FileData(f));
                    }

                    DirSearch(d, list);
                }
            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }

            return list;
        }

        /// <summary>
        /// Get the report log.
        /// </summary>
        /// <returns>html displayable string of the report</returns>
        public string GetReport()
        {
            string html = "<body>";

            html += "<font size='3'>";

            html += Report;
            html += "Done";

            html += "</font></body>";
            return html;
        }

        /// <summary>
        /// Get the current date for the report to use.
        /// </summary>
        /// <returns>Current date and time for logging</returns>
        public string GetCurrentDate()
        {
            DateTime dt = DateTime.Now;

            return dt.ToShortDateString() + " " + dt.ToShortTimeString() + " :";
        }

        /// <summary>
        /// The list of files
        /// </summary>
        public List<FileDataStore> FileDataStore
        {
            get { return _fileDataStore; }
        }

        /// <summary>
        /// The list of files
        /// </summary>
        public List<string> DeleteFileList
        {
            get { return _deleteFileList; }
        }

        /// <summary>
        /// Add a message the report - error format
        /// </summary>
        /// <param name="errorMessage">Error Message to add to the report.</param>
        protected void Report_Error(string errorMessage)
        {
            Report += GetCurrentDate() + "<font color='red'>" + errorMessage + "</font><br />";
        }

        /// <summary>
        /// The main report
        /// </summary>
        [NonSerialized]
        private string _report;

        /// <summary>
        /// The list of files
        /// </summary>
        [NonSerialized]
        private readonly List<FileDataStore> _fileDataStore = null;

        /// <summary>
        /// The list of deleted files
        /// </summary>
        private readonly List<string> _deleteFileList = new List<string>();
    }
}