using System;
using System.Windows;
using System.Windows.Controls;
using SystemWolfCommon.Helpers;

namespace VideoTaggerWPF
{
    /// <summary>
    /// Interaction logic for OptionsPage
    /// </summary>
    public partial class OptionsPage : Page
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public OptionsPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The user has clicked on the close button
        /// </summary>
        /// <param name="sender">The sender of the object</param>
        /// <param name="e">The args</param>
        private void _butClose_Click(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Hidden;
            _videoTaggerOptions.AutomaticallyPlayNextVideo = _autoPlayNextVideo.IsChecked.GetValueOrDefault();
            _videoTaggerOptions.AddDefaultMissingTag = _addEmptyTag.IsChecked.GetValueOrDefault();
            _videoTaggerOptions.ApplyFileDate = _applyFileDate.IsChecked.GetValueOrDefault();
            _videoTaggerOptions.DateFormat = _dateFormat.Text;
        }

        /// <summary>
        /// Video Tagger Options
        /// </summary>
        public VideoTaggerOptions VideoTaggerOptions
        {
            get
            {
                return _videoTaggerOptions;
            }
            set
            {
                _videoTaggerOptions = value;
                InitializationOptions();
            }
        }

        /// <summary>
        /// Initialize the options on the page
        /// </summary>
        private void InitializationOptions()
        {
            _autoPlayNextVideo.IsChecked = _videoTaggerOptions.AutomaticallyPlayNextVideo;
            _addEmptyTag.IsChecked = _videoTaggerOptions.AddDefaultMissingTag;
            _applyFileDate.IsChecked = _videoTaggerOptions.ApplyFileDate;
            _dateFormat.Text = _videoTaggerOptions.DateFormat;
            ApplyCurrentDateTime();
        }

        /// <summary>
        /// Apply the current time to the example time
        /// </summary>
        private void ApplyCurrentDateTime()
        {
            if (_exmapleData == null)
                return;

            try
            {
                DateTime dt = DateTime.Now;
                string dateTime = dt.ToString(_dateFormat.Text);
                _exmapleData.Content = dateTime;
            }
            catch (Exception)
            {
                _exmapleData.Content = "That not a valid time format.";
            }
        }

        /// <summary>
        /// The date format has changed
        /// </summary>
        /// <param name="sender">The sender of the data</param>
        /// <param name="e">The data sent</param>
        private void _dateFormat_TextChanged(object sender, TextChangedEventArgs e)
        {
            _dateFormat.Text = IOHelper.CheckValidFileName(_dateFormat.Text);
            ApplyCurrentDateTime();
        }

        #region Private Fields

        private VideoTaggerOptions _videoTaggerOptions;

        #endregion Private Fields
    }
}