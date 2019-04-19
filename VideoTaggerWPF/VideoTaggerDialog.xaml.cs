using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

// using System.Windows.Forms;
using System.Windows.Threading;
using SystemWolfCommon;

namespace VideoTaggerWPF
{
    /// <summary>
    /// Interaction logic for VideoTaggerDialog
    /// </summary>
    public partial class VideoTaggerDialog : Window, INotifyPropertyChanged
    {
        /// <summary>
        /// The default constructor
        /// </summary>
        public VideoTaggerDialog()
        {
            InitializeComponent();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += TimerIntervalTick;
            timer.Start();

            DataContext = this;
            _videoFileName = string.Empty;

            _tagListBox.ItemsSource = _videoTaggerViewModel.GetTagsList;

            mePlayer.MediaEnded += MePlayer_MediaEnded;

            pathField.Text = _videoTaggerViewModel.SaveableOptions.LastfolderSelected;
        }

        /// <summary>
        /// The name of the video file to play
        /// </summary>
        public string VideoFileNameToPlay
        {
            get
            {
                return _videoFileName;
            }
            set
            {
                _videoFileName = value;
                OnPropertyChanged("VideoFileNameToPlay");
            }
        }

        /// <summary>
        /// The property changed event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Call back on the timer
        /// </summary>
        /// <param name="sender">The sender of the object</param>
        /// <param name="e">The event data</param>
        public void TimerIntervalTick(object sender, EventArgs e)
        {
            if (mePlayer.Source != null)
            {
                if (mePlayer.NaturalDuration.HasTimeSpan)
                    lblStatus.Content = string.Format("{0} / {1}", mePlayer.Position.ToString(@"mm\:ss"), mePlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));
            }
            else
                lblStatus.Content = "No file selected...";
        }

        /// <summary>
        /// On property changed event
        /// </summary>
        /// <param name="name">The name of the property that has changed</param>
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        /// <summary>
        /// User has selected an item in the tag list
        /// </summary>
        /// <param name="selectedItem">The item we has changed</param>
        private void UpdateTaggeditemSelected(ImageFileData selectedItem)
        {
            _tagListBox.UnselectAll();

            List<string> tags = new List<string>(selectedItem.FileNameTags);
            foreach (string tag in tags)
            {
                TagData td = _videoTaggerViewModel.FindNewTag(tag);

                _tagListBox.SelectedItems.Add(td);
            }
        }

        /// <summary>
        /// The Text has changed
        /// </summary>
        /// <param name="sender">The sender of the object</param>
        /// <param name="e">The args</param>
        private void pathField_TextChanged(object sender, TextChangedEventArgs e)
        {
            _videoTaggerViewModel.SaveableOptions.LastfolderSelected = pathField.Text;
        }

        /// <summary>
        /// Called then the current video has finished
        /// </summary>
        /// <param name="sender">The sender of the object</param>
        /// <param name="e">The args</param>
        private void MePlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            _videoTaggerViewModel.SetNextVideo(pathField.Text);
            ShowNextVideoDetails();
        }

        /// <summary>
        /// Called when the GetFiles button is pressed
        /// </summary>
        /// <param name="sender">The sender of the object</param>
        /// <param name="e">The args</param>
        private void BtnGetFileList_Click(object sender, RoutedEventArgs e)
        {
            _videoTaggerViewModel.CreateListOfFiles(pathField.Text);
            ShowNextVideoDetails();
        }

        /// <summary>
        /// Called when the Next button is pressed
        /// </summary>
        /// <param name="sender">The sender of the object</param>
        /// <param name="e">The args</param>
        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            _videoTaggerViewModel.SetNextVideo(pathField.Text);
            ShowNextVideoDetails();
        }

        /// <summary>
        /// User has clicked on the previous
        /// </summary>
        /// <param name="sender">The sender of the object</param>
        /// <param name="e">The args</param>
        private void BottonPrev_Click(object sender, RoutedEventArgs e)
        {
            _videoTaggerViewModel.SetPrevVideo(pathField.Text);
            ShowNextVideoDetails();
        }

        /// <summary>
        /// Update the UI with the next video details
        /// </summary>
        private void ShowNextVideoDetails()
        {
            _selectedItem = null;
            if (Index >= 0)
            {
                ImageFileData ifd = _videoTaggerViewModel.GetVideoData(Index);
                if (ifd == null)
                    return;

                VideoFileNameToPlay = ifd.FileName;
                UpdateTaggeditemSelected(ifd);
                _selectedItem = ifd;
            }

            mePlayer.Play();
            UpdateFileNameText();
        }

        /// <summary>
        /// Gets the current video index
        /// </summary>
        private int Index
        {
            get { return _videoTaggerViewModel.Index; }
        }

        /// <summary>
        /// Called when the play button is pressed
        /// </summary>
        /// <param name="sender">The sender of the object</param>
        /// <param name="e">The args</param>
        private void BtnPlay_Click(object sender, RoutedEventArgs e)
        {
            mePlayer.Play();
        }

        /// <summary>
        /// Called when the pause button is pressed
        /// </summary>
        /// <param name="sender">The sender of the object</param>
        /// <param name="e">The args</param>
        private void BtnPause_Click(object sender, RoutedEventArgs e)
        {
            mePlayer.Pause();
        }

        /// <summary>
        /// Called when the stop button is pressed
        /// </summary>
        /// <param name="sender">The sender of the object</param>
        /// <param name="e">The args</param>
        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            mePlayer.Stop();
        }

        /// <summary>
        /// Called when the list selection is changed
        /// </summary>
        /// <param name="sender">The sender of the object</param>
        /// <param name="e">The args</param>
        private void TagListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_selectedItem == null)
                return;

            string selectedList = string.Empty;

            foreach (TagData tagData in _tagListBox.SelectedItems)
            {
                selectedList += tagData.Name + " ";
            }

            _selectedItem.UpdateTagsFromData(selectedList);
            UpdateFileNameText();
        }

        /// <summary>
        /// Update the UI for the displayed filename
        /// </summary>
        private void UpdateFileNameText()
        {
            if (_selectedItem == null)
                return;

            textBox.Text = _selectedItem.FileNameWithTags(_videoTaggerViewModel.SaveableOptions);
        }

        /// <summary>
        /// Called when the add new tag button is pressed
        /// </summary>
        /// <param name="sender">The sender of the object</param>
        /// <param name="e">The args</param>
        private void AddNewTagButton_Click(object sender, RoutedEventArgs e)
        {
            _videoTaggerViewModel.AddNewTag(_addnewItemTextBox.Text);
        }

        /// <summary>
        /// Update the playback speed
        /// </summary>
        /// <param name="sender">The sender of the object</param>
        /// <param name="e">The args that hold the new speed value</param>
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (e.NewValue > 0)
                mePlayer.SpeedRatio = (e.NewValue * 5) + 1;
            else
                mePlayer.SpeedRatio = e.NewValue + 1;
        }

        /// <summary>
        /// The new tag text has changed
        /// </summary>
        /// <param name="sender">The sender of the object</param>
        /// <param name="e">The data sent</param>
        private void AddnewItemTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_addTagButton == null)
                return;

            _addTagButton.IsEnabled = !string.IsNullOrWhiteSpace(_addnewItemTextBox.Text);
        }

        /// <summary>
        /// Browse for a folder with the video in
        /// </summary>
        /// <param name="sender">The sender of the object</param>
        /// <param name="e">The data sent</param>
        private void BtnBrowseFolder_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog browse = new FolderBrowserDialog();
            DialogResult result = browse.ShowDialog();

            if (System.Windows.Forms.DialogResult.OK == result)
            {
                pathField.Text = browse.SelectedPath;
                _videoTaggerViewModel.SaveableOptions.LastfolderSelected = pathField.Text;
            }
        }

        /// <summary>
        /// User has click on the options button
        /// </summary>
        /// <param name="sender">The sender of the object</param>
        /// <param name="e">The data sent</param>
        private void ButOptions_Click(object sender, RoutedEventArgs e)
        {
            object oc = this.Content;
            OptionsPage page = new OptionsPage();
            this.Content = page;
            page.VideoTaggerOptions = _videoTaggerViewModel.SaveableOptions;
            page.IsVisibleChanged += (s, ea) =>
            {
                if (ea.Property.Name == "IsVisible" && (bool)ea.NewValue == false)
                {
                    this.Content = oc;
                }
            };
        }

        /// <summary>
        /// User has click on the remove button on a tag
        /// </summary>
        /// <param name="sender">The sender of the object</param>
        /// <param name="e">The data sent</param>
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as System.Windows.Controls.Button;
            if (button != null)
            {
                TagData task = button.DataContext as TagData;
                _videoTaggerViewModel.RemoveTag(task);
            }
        }

        #region Private Fields

        /// <summary>
        /// The model for the view
        /// </summary>
        private VideoTaggerViewModel _videoTaggerViewModel = new VideoTaggerViewModel("WolfVideoTagList");

        /// <summary>
        /// The file name of the file
        /// </summary>
        private string _videoFileName;

        /// <summary>
        /// The currently selected item
        /// </summary>
        private ImageFileData _selectedItem = null;

        #endregion Private Fields
    }
}