using System.Collections.Generic;

namespace SystemWolfCommon
{
    /// <summary>
    /// The image file data holder
    /// </summary>
    public class ImageFileDataHolder
    {
        /// <summary>
        /// Gets the count of image in the list
        /// </summary>
        public int FileCount
        {
            get { return _imageList.Count; }
        }

        /// <summary>
        /// Get the image data
        /// </summary>
        /// <param name="index">The index of the ImageData</param>
        /// <returns>The image data from the index</returns>
        public ImageFileData GetImageFileData(int index)
        {
            if (_imageList == null)
                return null;

            if (index >= _imageList.Count)
                return null;

            ImageFileData[] array = _imageList.ToArray();
            return array[index];
        }

        /// <summary>
        /// Clear the list of images
        /// </summary>
        public void Clear()
        {
            _imageList.Clear();
        }

        /// <summary>
        /// Adds the image file data to the list
        /// </summary>
        /// <param name="ifd">The image file data</param>
        public void Add(ImageFileData ifd)
        {
            _imageList.Add(ifd);
        }

        /// <summary>
        /// The list of video files
        /// </summary>
        private List<ImageFileData> _imageList = new List<ImageFileData>();
    }
}