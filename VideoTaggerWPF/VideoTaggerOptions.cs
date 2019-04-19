using System;
using SystemWolfCommon.Interfaces;

// TODO : Should realy mark this files as dirty so it can be saved just once.

namespace VideoTaggerWPF
{
    /// <summary>
    /// The Savable Options
    /// </summary>
    [Serializable]
    public class VideoTaggerOptions : ITagOptions
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public VideoTaggerOptions()
        {
        }

        /// <summary>
        /// The last selected folder
        /// </summary>
        public string LastfolderSelected
        {
            get
            {
                return _lastfolderSelected;
            }
            set
            {
                if (_lastfolderSelected == value)
                    return;

                _lastfolderSelected = value;
                _optionChangedEvent?.Invoke(this, null);
            }
        }

        /// <summary>
        /// Do we need to add the Not tagged to empty tags lists
        /// </summary>
        public bool AddDefaultMissingTag
        {
            get
            {
                return _addEmptyTag;
            }
            set
            {
                if (_addEmptyTag == value)
                    return;

                _addEmptyTag = value;
                _optionChangedEvent?.Invoke(this, null);
            }
        }

        /// <summary>
        /// Automatically play the next video
        /// </summary>
        public bool AutomaticallyPlayNextVideo
        {
            get
            {
                return _automaticallyPlayNextVideo;
            }

            set
            {
                if (_automaticallyPlayNextVideo == value)
                    return;

                _automaticallyPlayNextVideo = value;
                _optionChangedEvent?.Invoke(this, null);
            }
        }

        /// <summary>
        /// Apply the file date
        /// </summary>
        public bool ApplyFileDate
        {
            get
            {
                return _applyFileDate;
            }
            set
            {
                if (_applyFileDate == value)
                    return;

                _applyFileDate = value;
                _optionChangedEvent?.Invoke(this, null);
            }
        }

        /// <summary>
        /// The date format string to use
        /// </summary>
        public string DateFormat
        {
            get
            {
                return _dateFormat;
            }
            set
            {
                if (_dateFormat == value)
                    return;

                _dateFormat = value;
                _optionChangedEvent?.Invoke(this, null);
            }
        }

        /// <summary>
        /// Adds the changed data event
        /// </summary>
        /// <param name="eventHandler">The event to call when the data has changed</param>
        public void AddChangedEvent(EventHandler eventHandler)
        {
            _optionChangedEvent = eventHandler;
        }

        /// <summary>
        /// The change data now event
        /// </summary>
        private static event EventHandler _optionChangedEvent;

        /// <summary>
        /// The last folder selected
        /// </summary>
        private string _lastfolderSelected = string.Empty;

        /// <summary>
        /// Add the default no tag if no tag is added
        /// </summary>
        private bool _addEmptyTag = true;

        /// <summary>
        /// Automatically play the next video when the current video has finished
        /// </summary>
        private bool _automaticallyPlayNextVideo = true;

        /// <summary>
        /// Automatically use the file date as the start of the filename when saved
        /// </summary>
        private bool _applyFileDate = true;

        /// <summary>
        /// The format of the date for the filename
        /// </summary>
        private string _dateFormat = "yyyy-MM-dd_HH-mm-ss";
    }
}