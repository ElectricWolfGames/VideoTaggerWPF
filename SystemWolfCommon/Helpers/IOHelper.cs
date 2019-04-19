using System.IO;
using System.Linq;
using System.Text;

namespace SystemWolfCommon.Helpers
{
    public static class IOHelper
    {
        /// <summary>
        /// Gets a valid file name
        /// </summary>
        /// <param name="fileName">The fileName to make valid</param>
        /// <returns>The valid version of the fileName</returns>
        public static string CheckValidFileName(string fileName)
        {
            StringBuilder sb = new StringBuilder();
            char[] invalidChars = Path.GetInvalidFileNameChars();

            foreach (char c in fileName)
            {
                if (!invalidChars.Contains(c))
                    sb.Append(c);
            }
            return sb.ToString();
        }
    }
}
