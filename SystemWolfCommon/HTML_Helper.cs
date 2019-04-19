using System.Collections.Generic;
using System.Text;

namespace SystemWolfCommon
{
    /// <summary>
    /// The HTML helper class
    /// </summary>
    public class HTML_Helper
    {
        /// <summary>
        /// Reformat the xml to be more readable
        /// </summary>
        /// <param name="originalXMLdata">The original xml</param>
        /// <returns>The formatted XML</returns>
        public static string ReformatHTML(string originalXMLdata)
        {
            List<TagData> tagList = new List<TagData>();

            TagData td = new TagData();
            var pattern = @"(\<(?:.*?)\>)";
            foreach (var m in System.Text.RegularExpressions.Regex.Split(originalXMLdata, pattern))
            {
                string stag = (string)m;
                if (string.IsNullOrWhiteSpace(stag))
                    continue;

                if (stag == "\r\n")
                    continue;

                if (stag.Length > 1)
                {
                    if (stag[1] == '/')
                    {
                        td = new TagData { Name = stag, TagType = TagData.TAGTYPE.TagClose };
                        tagList.Add(td);
                        continue;
                    }

                    if (stag[1] == '!')
                    {
                        td = new TagData { Name = stag, TagType = TagData.TAGTYPE.Data };
                        tagList.Add(td);
                        continue;
                    }
                }

                if (stag[0] == '<' && (stag[1] != '/' && stag[stag.Length - 2] == '/'))
                {
                    td = new TagData { Name = stag, TagType = TagData.TAGTYPE.TagOpenClosed };
                    tagList.Add(td);
                    continue;
                }

                if (stag[0] == '<' && (stag[1] != '/'))
                {
                    td = new TagData { Name = stag, TagType = TagData.TAGTYPE.TagOpen };
                    tagList.Add(td);
                    continue;
                }

                if (stag[0] != '<')
                {
                    td = new TagData { Name = stag, TagType = TagData.TAGTYPE.Data };
                    tagList.Add(td);
                }
            }
            GroupTagsOnToOneLine(ref tagList);
            string prettyXML = CreateXMLFromTags(tagList);
            return prettyXML;
        }

        /// <summary>
        /// Group tag together that can be on the same line
        /// </summary>
        /// <param name="tags">The list of tags to be grouped</param>
        private static void GroupTagsOnToOneLine(ref List<TagData> tags)
        {
            for (int i = 0; i < tags.Count - 3; i++)
            {
                if (tags[i].TagType == TagData.TAGTYPE.TagOpen)
                    if (tags[i + 1].TagType == TagData.TAGTYPE.Data)
                        if (tags[i + 2].TagType == TagData.TAGTYPE.TagClose)
                        {
                            tags[i].Group = true;
                            tags[i + 1].Group = true;
                            tags[i + 2].Group = true;
                        }
            }
        }

        /// <summary>
        /// Convert tags in to the final XML data
        /// </summary>
        /// <param name="tags">The list of TagData to use</param>
        /// <returns>The final XML data</returns>
        private static string CreateXMLFromTags(List<TagData> tags)
        {
            StringBuilder sb = new StringBuilder();
            int nestSize = 0;

            foreach (TagData td in tags)
            {
                if (td.Group)
                {
                    // To show on one line
                    if (td.TagType == TagData.TAGTYPE.TagOpen)
                    {
                        sb.Append(GetNest(nestSize) + td.Name);
                    }
                    if (td.TagType == TagData.TAGTYPE.Data)
                    {
                        sb.Append(td.Name);
                    }
                    if (td.TagType == TagData.TAGTYPE.TagClose)
                    {
                        sb.Append(td.Name);
                        sb.Append("\r\n");
                    }
                }
                else
                {
                    if (td.TagType == TagData.TAGTYPE.TagOpenClosed)
                    {
                        sb.Append(GetNest(nestSize) + td.Name);
                        sb.Append("\r\n");
                    }

                    if (td.TagType == TagData.TAGTYPE.Data)
                    {
                        sb.Append(GetNest(nestSize) + td.Name);
                        sb.Append("\r\n");
                    }
                    if (td.TagType == TagData.TAGTYPE.TagOpen)
                    {
                        sb.Append(GetNest(nestSize) + td.Name);
                        sb.Append("\r\n");
                        nestSize++;
                    }
                    if (td.TagType == TagData.TAGTYPE.TagClose)
                    {
                        nestSize--;
                        sb.Append(GetNest(nestSize) + td.Name);
                        sb.Append("\r\n");
                    }
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Get the Nested off set as spaces
        /// </summary>
        /// <param name="nestSize">The number of nests to get</param>
        /// <returns>The nesting in spaces</returns>
        private static string GetNest(int nestSize)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < nestSize; i++)
            {
                sb.Append("    ");
            }

            return sb.ToString();
        }

        /// <summary>
        /// The TagData for each XML elements
        /// </summary>
        private class TagData
        {
            /// <summary>
            /// The tag types
            /// </summary>
            public enum TAGTYPE
            {
                /// <summary>
                /// No known type
                /// </summary>
                None,

                /// <summary>
                /// Tag open
                /// </summary>
                TagOpen,

                /// <summary>
                /// Tag closed
                /// </summary>
                TagClose,

                /// <summary>
                /// Tag is opened and closed
                /// </summary>
                TagOpenClosed,

                /// <summary>
                /// Data with out open or closing tags
                /// </summary>
                Data,
            }

            /// <summary>
            /// Gets or sets the name
            /// </summary>
            public string Name
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
            /// The tag type
            /// </summary>
            public TAGTYPE TagType
            {
                get
                {
                    return _tagType;
                }

                set
                {
                    _tagType = value;
                }
            }

            /// <summary>
            /// The group flag
            /// </summary>
            public bool Group
            {
                get
                {
                    return _group;
                }

                set
                {
                    _group = value;
                }
            }

            /// <summary>
            /// The name
            /// </summary>
            private string _name;

            /// <summary>
            /// The tag type
            /// </summary>
            private TAGTYPE _tagType = TAGTYPE.None;

            /// <summary>
            /// Group flag
            /// </summary>
            private bool _group = false;
        }
    }
}
