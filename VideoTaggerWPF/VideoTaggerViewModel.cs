using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using SystemWolfCommon;

namespace VideoTaggerWPF
{
    /// <summary>
    /// The video tagger view model
    /// </summary>
    public class VideoTaggerViewModel
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="tagFileName">The filename of the tag file</param>
        public VideoTaggerViewModel(string tagFileName)
        {
            _tagHolder = new TagHolder(tagFileName);
            LoadOptions();
        }

        /// <summary>
        /// Gets the list of tags
        /// </summary>
        public ObservableCollection<TagData> GetTagsList
        {
            get { return _tagHolder.TagList; }
        }

        /// <summary>
        /// Create the list of files from the given path
        /// </summary>
        /// <param name="path">The path to get the files from</param>
        internal void CreateListOfFiles(string path)
        {
            _currentVideoIndex = 0;
            _imageFileDataHolder.Clear();

            if (!Directory.Exists(path))
                return;

            string[] files = Directory.GetFiles(path, "*.avi", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                string fileNameLower = file.ToLower();
                ImageFileData ifd = new ImageFileData { FileName = file };
                _tagHolder.AddList(ifd.FileNameTags);
                _imageFileDataHolder.Add(ifd);
            }
        }

        /// <summary>
        /// Add a new tag to the list of tags
        /// </summary>
        /// <param name="newTagItem">The new tag to add</param>
        internal void AddNewTag(string newTagItem)
        {
            _tagHolder.AddList(new List<string> { newTagItem });
            _tagHolder.SaveTags();
        }

        /// <summary>
        /// Find the tag date from it's saved name
        /// </summary>
        /// <param name="tagName">The name to find</param>
        /// <returns>The tag data for this item</returns>
        internal TagData FindNewTag(string tagName)
        {
            TagData td = _tagHolder.TagList.FirstOrDefault(s => s.Name == tagName);
            if (td == null)
            {
                _tagHolder.AddList(new List<string> { tagName });
                _tagHolder.SaveTags();
                td = _tagHolder.TagList.FirstOrDefault(s => s.Name == tagName);
            }
            return td;
        }

        /// <summary>
        /// Remove the tag from the list
        /// </summary>
        /// <param name="tagToRemove">The tag to remove</param>
        internal void RemoveTag(TagData tagToRemove)
        {
            _tagHolder.TagList.Remove(tagToRemove);
            _tagHolder.SaveTags();
        }

        /// <summary>
        /// Gets the current index.
        /// </summary>
        internal int Index
        {
            get { return _currentVideoIndex; }
        }

        /// <summary>
        /// Get the savable options
        /// </summary>
        internal VideoTaggerOptions SaveableOptions
        {
            get
            {
                return _saveableOptions;
            }
        }

        /// <summary>
        /// Request the saving of the options
        /// </summary>
        internal void RequestSave()
        {
            SaveOptions(null, null);
        }

        /// <summary>
        /// Set the next video index
        /// </summary>
        /// <param name="path">The path to get the file from</param>
        internal void SetNextVideo(string path)
        {
            if (_imageFileDataHolder.FileCount == 0)
                CreateListOfFiles(path);
            else
            {
                SaveCurrentFile(Index);
                _currentVideoIndex++;
            }

            if (_currentVideoIndex > _imageFileDataHolder.FileCount - 1)
                _currentVideoIndex = _imageFileDataHolder.FileCount - 1;
        }

        /// <summary>
        /// Set the index to be the previous index in the list if any
        /// </summary>
        /// <param name="path">The path to get the file from</param>
        internal void SetPrevVideo(string path)
        {
            if (_imageFileDataHolder.FileCount == 0)
                CreateListOfFiles(path);

            SaveCurrentFile(Index);
            _currentVideoIndex--;
            if (_currentVideoIndex < 0)
                _currentVideoIndex = 0;
        }

        /// <summary>
        /// Get the video data
        /// </summary>
        /// <param name="videoIndex">The index of the video to get</param>
        /// <returns>The image file data for the video index</returns>
        internal ImageFileData GetVideoData(int videoIndex)
        {
            return GetImageFileData(videoIndex);
        }

        #region Private Methods

        /// <summary>
        /// Resave the file - with the updated file name
        /// </summary>
        /// <param name="videoIndex">The index of the file to save</param>
        private void SaveCurrentFile(int videoIndex)
        {
            if (videoIndex < 0)
                return;

            try
            {
                ImageFileData ifd = GetImageFileData(videoIndex);
                if (ifd == null)
                    return;

                string name = ifd.FileName;
                string taggedName = ifd.FileNameWithTags(SaveableOptions) + ".avi";
                string filepath = Path.GetDirectoryName(name);
                File.Move(name, Path.Combine(filepath, taggedName));
                ifd.FileName = Path.Combine(filepath, taggedName);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Get the image data
        /// </summary>
        /// <param name="index">The index of the ImageData</param>
        /// <returns>The image data from the index</returns>
        private ImageFileData GetImageFileData(int index)
        {
            return _imageFileDataHolder.GetImageFileData(index);
        }

        /// <summary>
        /// Save out the user options
        /// </summary>
        /// <param name="sender">The sender of the object</param>
        /// <param name="e">The event data</param>
        private void SaveOptions(object sender, EventArgs e)
        {
            ReadWriteFileHelper.WriteToXmlFile<VideoTaggerOptions>(OptionsFilename, _saveableOptions, false);
        }

        /// <summary>
        /// Load the Tags
        /// </summary>
        private void LoadOptions()
        {
            try
            {
                _saveableOptions = ReadWriteFileHelper.ReadFromXmlFile<VideoTaggerOptions>(OptionsFilename);
            }
            catch
            {
                _saveableOptions = new VideoTaggerOptions();
            }

            _saveableOptions.AddChangedEvent(SaveOptions);
        }

        /// <summary>
        /// The path to the options details
        /// </summary>
        private string OptionsFilename
        {
            get { return Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"VideoTaggerOptions.xml"); }
        }

        #endregion Private Methods

        /// <summary>
        /// The tag holder
        /// </summary>
        private readonly TagHolder _tagHolder;

        /// <summary>
        /// Holders all the image data file information
        /// </summary>
        private readonly ImageFileDataHolder _imageFileDataHolder = new ImageFileDataHolder();

        /// <summary>
        /// The current video index
        /// </summary>
        private int _currentVideoIndex = 0;

        /// <summary>
        /// The savable options
        /// </summary>
        private VideoTaggerOptions _saveableOptions;
    }
}