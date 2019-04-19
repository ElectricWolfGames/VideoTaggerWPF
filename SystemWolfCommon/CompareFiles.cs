using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace SystemWolfCommon
{
    /// <summary>
    /// Compare files class
    /// </summary>
	public class CompareFiles : BaseItemRunner, IItemRunner
    {
        /// <summary>
        /// Get or sets the folder path
        /// </summary>
        public string Path
        {
            get
            {
                return _path;
            }

            set
            {
                _path = value;
            }
        }

        /// <summary>
        /// Gets or set the do action flag
        /// </summary>
        public bool Action
        {
            get
            {
                return _action;
            }

            set
            {
                _action = value;
            }
        }

        /// <summary>
        /// Get the list of files to delete
        /// </summary>
        /// <returns>The list of files to delete</returns>
        public List<string> GetDeleteFileList()
        {
            return DeleteFileList;
        }

        /// <summary>
        /// The main run
        /// </summary>
        public void Run()
        {
        }

        /// <summary>
        /// The back ground worker
        /// </summary>
        /// <param name="o">The object</param>
        /// <param name="e">the event data</param>
        public void Run_BackGroundWorker(object o, DoWorkEventArgs e)
        {
            BackgroundWorker b = o as BackgroundWorker;

            b.ReportProgress(0);

            Report += "<h6>";

            // TODO - add a recursion on the folders here...
            _fileList = GetFilesFromFolder(Path);

            int fileCount = _fileList.Count();
            float modifier = 100 / fileCount;
            Dictionary<string, List<int>> hashList = new Dictionary<string, List<int>>();

            int count = 0;
            foreach (CF_FileData cf in _fileList)
            {
                string fileMD5 = string.Empty;
                if (cf != null)
                {
                    for (int im = 0; im < 16; im++)
                    {
                        fileMD5 += cf.GetBytes(im);
                    }
                }

                List<int> arry = new List<int>();

                if (hashList.ContainsKey(fileMD5))
                {
                    arry = hashList[fileMD5];
                }
                else
                {
                    hashList.Add(fileMD5, arry);
                }

                arry.Add(count);
                count++;
            }

            foreach (KeyValuePair<string, List<int>> pair in hashList)
            {
                Console.WriteLine("{0}, {1}", pair.Key, pair.Value);
            }

            /*
            //
            for (int i = 0; i < nFileCount; i++)
            {
                CF_FileData CFFD = m_FileList[i];

                string FileA_MD5 = "";
                for (int im = 0; im < 16; im++)
                {
                    FileA_MD5 += CFFD.GetBytes(im);
                }

                for (int i2 = 0; i2 < nFileCount; i2++)
                {
                    CF_FileData CFFD_in = m_FileList[i2];
                    if (CFFD != CFFD_in)
                    {
                        string FileB_MD5 = "";
                        for (int im = 0; im < 16; im++)
                        {
                            FileB_MD5 += CFFD_in.GetBytes(im);
                        }
                        if (FileA_MD5 == FileB_MD5)
                        {
                            int p = 0;
                            p++;
                        }
                    }
                }
            }
            */

            /*for (int i = 0; i < nFileCount; i++ )
            {
                CF_FileData CFFD = m_FileList[i];
                for (int i2 = i; i2 < nFileCount; i2++)
                {
                    CF_FileData CFFD_in = m_FileList[i2];
                    if (CFFD != CFFD_in)
                    {
                        string FileA_MD5 = "";
                        string FileB_MD5 = "";

                        for (int im = 0; im < 16; im++)
                        {
                            FileA_MD5 += CFFD.GetBytes(im);
                            FileB_MD5 += CFFD_in.GetBytes(im);
                        }

                        if (FileA_MD5 == FileB_MD5)
                        {
                            // need to add both files to the results log.
                            DateTime CFFD_Time = GetTimeOfFile(CFFD.FileName);
                            DateTime CFFD_In_Time = GetTimeOfFile(CFFD_in.FileName);

                            // m_sDeleteFileList

                            // TODO - Create a list of file that can be deleted -

                            Report += "<ul>";
                            if (CFFD_Time < CFFD_In_Time)
                            {
                                Report += "<li>keep " + CFFD.FileName + " - " + FileA_MD5 + "</li>";
                                Report += "<li>del " + CFFD_in.FileName + " - " + FileB_MD5 + "</li>";
                                m_sDeleteFileList.Add(CFFD_in.FileName);
                                // Delete the older file..
                                //File.Delete(CFFD_in.FileName);
                            }
                            else
                            {
                                Report += "<li>del " + CFFD.FileName + " - " + FileA_MD5 + "</li>";
                                Report += "<li>keep " + CFFD_in.FileName + " - " + FileB_MD5 + "</li>";
                                m_sDeleteFileList.Add(CFFD.FileName);
                                // Delete the older file..
                                //File.Delete(CFFD.FileName);
                            }
                            Report += "</ul>";
                        }
                    }
                }
            }*/
            Report += "</h6>";
        }

        /// <summary>
        /// Gets the time of the file was created
        /// </summary>
        /// <param name="fileName">The filename</param>
        /// <returns>The time</returns>
        private DateTime GetTimeOfFile(string fileName)
        {
            System.IO.FileInfo file1 = new System.IO.FileInfo(fileName);
            return file1.LastWriteTimeUtc;
        }

        /// <summary>
        /// The file data
        /// </summary>
        public class CF_FileData
        {
            /// <summary>
            /// Standard constructor
            /// </summary>
            /// <param name="fileName">The filename</param>
            public CF_FileData(string fileName)
            {
                _name = fileName;

                MD5 md5 = MD5.Create();
                using (var stream = File.OpenRead(fileName))
                {
                    _hash = md5.ComputeHash(stream);
                }
            }

            /// <summary>
            /// Gets and sets the file name
            /// </summary>
            public string FileName
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
            /// Get the bytes
            /// </summary>
            /// <param name="index">The hash index</param>
            /// <returns>The Byte at the index</returns>
            public byte GetBytes(int index)
            {
                return _hash[index];
            }

            /// <summary>
            /// The name
            /// </summary>
            private string _name;

            /// <summary>
            /// The unique has code
            /// </summary>
            private readonly byte[] _hash;
        }

        /// <summary>
        /// The list of files
        /// </summary>
        private List<CF_FileData> _fileList = new List<CF_FileData>();

        /// <summary>
        /// The do action flag
        /// </summary>
        private bool _action = false;

        /// <summary>
        /// The folder path
        /// </summary>
        private string _path;
    }
}
