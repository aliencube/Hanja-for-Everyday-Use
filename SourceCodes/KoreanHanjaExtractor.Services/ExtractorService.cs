using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using HtmlAgilityPack;

namespace Aliencube.Utilities.KoreanHanjaExtractor.Services
{
    /// <summary>
    /// This represents the extractor service entity.
    /// </summary>
    public class ExtractorService
    {
        #region Constructors
        private const string INPUT_FILENAME = "korean-hanja.html";
        private const string OUTPUT_FILENAME = "hanja-for-everyday-use.xml";
        #endregion

        #region Methods
        /// <summary>
        /// Gets the HTML node from the file.
        /// </summary>
        /// <returns>Returns the HTML node from the file.</returns>
        public HtmlNode GetHtmlNode()
        {
            var doc = new HtmlDocument();
            doc.DetectEncodingAndLoad(INPUT_FILENAME);

            var html = doc.DocumentNode.Element("html");
            return html;
        }

        /// <summary>
        /// Gets the data table node (&lt;table&gt;...&lt;/table&gt;) from the HTML node.
        /// </summary>
        /// <param name="html">HTML node.</param>
        /// <returns>Returns the data table node from the HTML node.</returns>
        public HtmlNode GetDataTable(HtmlNode html)
        {
            var table = html.Element("body")
                            .Descendants("table")
                            .Single(p => p.Attributes.Contains("class") && p.Attributes["class"].Value == "datatable");
            return table;
        }

        /// <summary>
        /// Gets the list of table rows (&lt;tr&gt;...&lt;/tr&gt;) from the table node.
        /// </summary>
        /// <param name="table">Table node.</param>
        /// <returns>Returns the list of table rows from the table node.</returns>
        public IList<HtmlNode> GetTableRows(HtmlNode table)
        {
            var rows = table.Elements("tr")
                            .Where(p => p.Attributes.Contains("valign"))
                            .ToList();
            return rows;
        }

        /// <summary>
        /// Gets the list of table cells (&lt;td&gt;...&lt;/td&gt;) from the table node.
        /// </summary>
        /// <param name="rows">List of table rows.</param>
        /// <returns>Returns the list of table cells from the table rows.</returns>
        public IList<HtmlNode> GetTableCells(IList<HtmlNode> rows)
        {
            var cells = new List<HtmlNode>();
            foreach (var row in rows)
                cells.AddRange(row.Elements("td").Where(p => !String.IsNullOrWhiteSpace(p.InnerText.Trim())));
            return cells;
        }

        /// <summary>
        /// Gets the list of words from the table cells.
        /// </summary>
        /// <param name="cells">List of table cells.</param>
        /// <returns>Returns the list of words from the table cells.</returns>
        public IList<string> GetWords(IList<HtmlNode> cells)
        {
            var collection = new List<string>();
            foreach (var cell in cells)
                collection.AddRange(cell.InnerText
                                        .Trim()
                                        .Split(new string[] {"•"}, StringSplitOptions.RemoveEmptyEntries));

            var words = collection.Where(p => !p.Contains("/"))
                                  .Select(p => p.Trim())
                                  .ToList();
            foreach (var word in collection.Where(p => p.Contains("/")).Select(p => p.Trim()))
            {
                var segments = word.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)
                                   .Select(p => p.Trim())
                                   .ToList();
                var match = Regex.Match(segments[segments.Count - 1], @"(?<Translation>\(.+\)?)");
                if (match.Success)
                    for (var i = 0; i < segments.Count - 1; i++)
                        segments[i] = String.Format("{0} {1}", segments[i], match.Groups["Translation"].Value.Trim());

                words.AddRange(segments);
            }
            return words;
        }

        /// <summary>
        /// Converts the list of extracted words to the list of Hanja objects.
        /// </summary>
        /// <param name="words">List of extracted words.</param>
        /// <returns>Returns the list of Hanja objects.</returns>
        public IList<Hanja> ConvertWords(IList<string> words)
        {
            var hanjas = words.Select(word => word.Split(new string[] { " ", "(", ")" }, StringSplitOptions.RemoveEmptyEntries))
                              .Select(segments => new Hanja()
                              {
                                  Character = segments[0],
                                  Meaning = String.Join(" ", segments.Take(segments.Length - 1).Skip(1)),
                                  Pronunciation = segments[segments.Length - 1]
                              })
                              .OrderBy(p => p.Pronunciation)
                              .ThenBy(p => p.Meaning)
                              .ThenBy(p => p.Character)
                              .ToList();
            return hanjas;
        }

        /// <summary>
        /// Saves the list of Hanja objects to an XML document.
        /// </summary>
        /// <param name="hanjas">List of Hanja objects.</param>
        /// <returns>Returns <c>True</c>, if XML is created successfully; otherwise returns <c>False</c>.</returns>
        public bool SaveXml(IList<Hanja> hanjas)
        {
            bool saved;
            try
            {
                using (var writer = new XmlTextWriter(OUTPUT_FILENAME, Encoding.UTF8) { Formatting = Formatting.Indented })
                {
                    var collection = new HanjaCollection() { Hanja = hanjas.ToArray() };
                    var serialiser = new XmlSerializer(typeof(HanjaCollection));
                    serialiser.Serialize(writer, collection);
                }
                saved = true;
            }
            catch (Exception ex)
            {
                saved = false;
            }
            return saved;
        }
        #endregion
    }
}
