using System;
using System.Collections.Generic;
using System.IO;
using SystemWolfCommon.Interfaces;

namespace SystemWolfCommon
{
    /// <summary>
    /// Holder the information about a image file
    /// </summary>
    public class ImageFileData
    {
        /// <summary>
        /// Get the list if tags
        /// </summary>
        public List<string> FileNameTags
        {
            get { return _filenameTags; }
        }

        /// <summary>
        /// Gets the path of the image
        /// </summary>
        public string FilePath
        {
            get { return _filePath; }
        }

        /// <summary>
        /// Gets the file Extension
        /// </summary>
        public string FileExtension
        {
            get { return _fileExtension; }
        }

        /// <summary>
        /// Get the filename
        /// </summary>
        public string FileName
        {
            get
            {
                return _fileName;
            }
            set
            {
                if (_fileName == value)
                    return;

                _fileName = value;

                UpdateFileInfo();
                UpdateFileTagsFrom(_fileName);
            }
        }

        /// <summary>
        /// Update the list if tags
        /// </summary>
        /// <param name="selectedList">The text to create the tags from.</param>
        public void UpdateTags(string selectedList)
        {
            _filenameTags.Clear();

            _originalName = string.Empty;

            string[] tags = selectedList.Split(' ');
            for (int i = 0; i < tags.Length; i++)
            {
                string tagName = tags[i];

                if (_originalName == string.Empty)
                    _originalName = tagName;
                else
                    _filenameTags.Add(tagName);
            }
        }

        /// <summary>
        /// Update the list if tags
        /// </summary>
        /// <param name="selectedList">The text to create the tags from.</param>
        public void UpdateTagsFromData(string selectedList)
        {
            _filenameTags.Clear();

            string[] tags = selectedList.Split(' ');
            for (int i = 0; i < tags.Length; i++)
            {
                string tag = tags[i];

                _filenameTags.Add(tag);
            }
        }

        /// <summary>
        /// Get the full file name with the tags
        /// </summary>
        /// <param name="options">User options</param>
        /// <returns>The full file name with tags</returns>
        public string FileNameWithTags(ITagOptions options)
        {
            return CreateTagsForFileName(options).Trim();
        }

        /// <summary>
        /// Get more info about the file
        /// </summary>
        private void UpdateFileInfo()
        {
            _fileDataTime = File.GetCreationTime(_fileName);
        }

        /// <summary>
        /// Get the files date in the format given
        /// </summary>
        /// <param name="options">User options</param>
        /// <returns>The file name of the file</returns>
        private string GetFileDataformat(ITagOptions options)
        {
            return _fileDataTime.ToString(options.DateFormat);
        }

        /// <summary>
        /// Create the file name from the tags selected
        /// </summary>
        /// <param name="options">User options</param>
        /// <returns>The filename</returns>
        private string CreateTagsForFileName(ITagOptions options)
        {
            string fileName = string.Empty;
            if (options.ApplyFileDate)
            {
                fileName = GetFileDataformat(options) + " ";
            }
            else
            {
                fileName = _originalName + " ";
            }

            if (_filenameTags.Count != 0)
            {
                foreach (string tag in _filenameTags)
                {
                    fileName += tag + " ";
                }
            }
            else
            {
                if (options.AddDefaultMissingTag)
                    fileName += "NOTTAGGED" + " ";
            }

            return fileName;
        }

        /// <summary>
        /// Update the list of tags from the file name
        /// </summary>
        /// <param name="filename">the file name get create the tags from</param>
        private void UpdateFileTagsFrom(string filename)
        {
            string fileNameOnly = Path.GetFileNameWithoutExtension(filename);
            _filePath = filename.Replace(Path.GetFileName(filename), string.Empty);
            _fileExtension = Path.GetExtension(filename);
            UpdateTags(fileNameOnly);
        }

        /// <summary>
        /// The original name
        /// </summary>
        private string _originalName;

        /// <summary>
        /// The file name
        /// </summary>
        private string _fileName;

        /// <summary>
        /// The file path
        /// </summary>
        private string _filePath;

        /// <summary>
        /// The file extension
        /// </summary>
        private string _fileExtension;

        /// <summary>
        /// The filename tags array
        /// </summary>
        private List<string> _filenameTags = new List<string>();

        /// <summary>
        /// The date time of the file
        /// </summary>
        private DateTime _fileDataTime;
    }
}
