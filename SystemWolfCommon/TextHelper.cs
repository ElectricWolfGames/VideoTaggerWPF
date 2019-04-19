using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SystemWolfCommon
{
    /// <summary>
    /// The text helper class
    /// </summary>
    public static class TextHelper
    {
        /// <summary>
        /// Make the first letter of the string upper case
        /// </summary>
        /// <param name="text">Text to update</param>
        /// <returns>The update text with the first letter now upper case</returns>
        public static string MakeFirstLetterUpperCase(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            string str = text.Substring(0, 1).ToUpper();
            str += text.Substring(1).ToLower();
            return str;
        }

        /// <summary>
        /// Make the words camel case and remove the spaces
        /// </summary>
        /// <param name="tagName">The words to make camel case</param>
        /// <returns>The camel cased words</returns>
        public static string CamelCaseWords(string tagName)
        {
            string[] words = tagName.Split(' ');
            StringBuilder sb = new StringBuilder();

            foreach (string word in words)
            {
                sb.Append(MakeFirstLetterUpperCase(word));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Gets the short name
        /// </summary>
        /// <param name="names">The list of names</param>
        /// <returns>The shortest name</returns>
        public static string GetShortest(List<string> names)
        {
            var ordered = names.OrderBy(x => x.Length);
            return ordered.First();
        }

        /// <summary>
        /// Gets the longest name
        /// </summary>
        /// <param name="names">The list of names</param>
        /// <returns>The longest name</returns>
        public static string GetLongest(List<string> names)
        {
            var ordered = names.OrderBy(x => x.Length);
            return ordered.Last();
        }
    }
}
