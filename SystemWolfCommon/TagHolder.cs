using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace SystemWolfCommon
{
    /// <summary>
    /// Tag Holder : holders a list of tags
    /// </summary>
    public class TagHolder
    {
        /// <summary>
        /// The default TagHolder
        /// </summary>
        /// <param name="filename">The filename of the tag file</param>
        public TagHolder(string filename)
        {
            _filename = filename;
            LoadTags();
        }

        /// <summary>
        /// Add a new item to the list and oder the list
        /// </summary>
        /// <param name="newTags">The new tag to add</param>
        public void AddList(List<string> newTags)
        {
            List<TagData> tagList = _tagHolderData.TagList.ToList<TagData>();
            foreach (string tag in newTags)
            {
                string taglower = tag.ToLower();

                if (tagList.FirstOrDefault(s => s.OriginalName != null && s.OriginalName.ToLower() == taglower) == null)
                {
                    tagList.Add(new TagData(tag));
                }
            }

            tagList = tagList.OrderBy(e => e.Name).ToList();

            _tagHolderData.TagList.Clear();

            foreach (TagData tag in tagList)
            {
                _tagHolderData.TagList.Add(tag);
            }
        }

        /// <summary>
        /// Gets the Tag list
        /// </summary>
        public ObservableCollection<TagData> TagList
        {
            get { return _tagHolderData.TagList; }
        }

        /// <summary>
        /// Save all the tags in the list
        /// </summary>
        public void SaveTags()
        {
            ReadWriteFileHelper.WriteToXmlFile<TagHolderData>(Filename, _tagHolderData, false);
        }

        /// <summary>
        /// Load the Tags
        /// </summary>
        private void LoadTags()
        {
            try
            {
                _tagHolderData = ReadWriteFileHelper.ReadFromXmlFile<TagHolderData>(Filename);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Get the filename
        /// </summary>
        private string Filename
        {
            get { return Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), _filename + @".xml"); }
        }

        /// <summary>
        /// The tag holder
        /// </summary>
        private TagHolderData _tagHolderData = new TagHolderData();

        /// <summary>
        /// The file name to save out the list of available tags
        /// </summary>
        private readonly string _filename;
    }

    /// <summary>
    /// The Tag data holder
    /// </summary>
    [Serializable]
    public class TagHolderData
    {
        /// <summary>
        /// Gets the list of tags
        /// </summary>
        public ObservableCollection<TagData> TagList
        {
            get { return _tagList; }
            set { _tagList = value; }
        }

        /// <summary>
        /// The list of tags
        /// </summary>
        private ObservableCollection<TagData> _tagList = new ObservableCollection<TagData>();
    }

    /// <summary>
    /// The tag data
    /// </summary>
    [Serializable]
    public class TagData
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public TagData()
        {
        }

        /// <summary>
        /// Constructor to take a name
        /// </summary>
        /// <param name="tagName">The tag to add</param>
        public TagData(string tagName)
        {
            _originalName = tagName;
            _name = TextHelper.CamelCaseWords(tagName);
        }

        /// <summary>
        /// Gets the tag name (safe for filenames)
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Get the original tag string
        /// </summary>
        public string OriginalName
        {
            get { return _originalName; }
            set { _originalName = value; }
        }

        /// <summary>
        /// The file safe name
        /// </summary>
        private string _name;

        /// <summary>
        /// The original name typed in by the user
        /// </summary>
        private string _originalName;
    }
}
