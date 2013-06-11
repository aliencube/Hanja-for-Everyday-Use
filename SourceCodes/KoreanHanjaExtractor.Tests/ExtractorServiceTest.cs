using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using NUnit.Framework;
using Aliencube.Utilities.KoreanHanjaExtractor.Services;

namespace Aliencube.Utilities.KoreanHanjaExtractor.Tests
{
    /// <summary>
    /// A test class for the ExtractorService class.
    /// </summary>
    [TestFixture]
    public class ExtractorServiceTest
    {
        #region Setup and Tear down
        /// <summary>
        /// This runs only once at the beginning of all tests and is used for all tests in the 
        /// class.
        /// </summary>
        [TestFixtureSetUp]
        public void InitialSetup()
        {

        }

        /// <summary>
        /// This runs only once at the end of all tests and is used for all tests in the class.
        /// </summary>
        [TestFixtureTearDown]
        public void FinalTearDown()
        {

        }

        /// <summary>
        /// This setup funcitons runs before each test method
        /// </summary>
        [SetUp]
        public void SetupForEachTest()
        {
        }

        /// <summary>
        /// This setup funcitons runs after each test method
        /// </summary>
        [TearDown]
        public void TearDownForEachTest()
        {
        }
        #endregion

        /// <summary>
        /// Tests HTMl node is returned.
        /// </summary>
        [Test]
        public void ReadPage_SendFilename_ReturnHtmlNode()
        {
            var service = new ExtractorService();
            var html = service.GetHtmlNode();

            Assert.IsTrue(html != null && html.Name.ToLower() == "html");
        }

        /// <summary>
        /// Tests a data table is extracted.
        /// </summary>
        [Test]
        public void GetTable_SendHtmlNode_ReturnTable()
        {
            var service = new ExtractorService();
            var html = service.GetHtmlNode();
            var table = service.GetDataTable(html);

            Assert.IsTrue(table.Attributes["class"].Value == "datatable");
        }

        [Test]
        public void GetTableRows_SendTableNode_ReturnTableRows()
        {
            var service = new ExtractorService();
            var html = service.GetHtmlNode();
            var table = service.GetDataTable(html);
            var rows = service.GetTableRows(table);

            Assert.IsTrue(rows.Count > 0);
        }

        [Test]
        public void GetTableCells_SendTableRows_ReturnTableCells()
        {
            var service = new ExtractorService();
            var html = service.GetHtmlNode();
            var table = service.GetDataTable(html);
            var rows = service.GetTableRows(table);
            var cells = service.GetTableCells(rows);

            Assert.IsTrue(cells.Count > 0);
        }

        [Test]
        public void GetWords_SendTableCells_ReturnWords()
        {
            var service = new ExtractorService();
            var html = service.GetHtmlNode();
            var table = service.GetDataTable(html);
            var rows = service.GetTableRows(table);
            var cells = service.GetTableCells(rows);
            var words = service.GetWords(cells);

            Assert.IsTrue(words.Count(p => p.Contains("/")) == 0);
        }

        [Test]
        public void ConvertWords_SendWords_ReturnListOfWords()
        {
            var service = new ExtractorService();
            var html = service.GetHtmlNode();
            var table = service.GetDataTable(html);
            var rows = service.GetTableRows(table);
            var cells = service.GetTableCells(rows);
            var words = service.GetWords(cells);
            var hanjas = service.ConvertWords(words);

            Assert.IsInstanceOf<List<Hanja>>(hanjas);
            Assert.IsTrue(hanjas != null && hanjas.Count == words.Count);
            Assert.IsTrue(hanjas.Count(p => p.Meaning.Contains(" ")) > 0);
        }

        [Test]
        public void StoreXmlDocument_SendWords_ReturnXmlDocument()
        {
            var service = new ExtractorService();
            var html = service.GetHtmlNode();
            var table = service.GetDataTable(html);
            var rows = service.GetTableRows(table);
            var cells = service.GetTableCells(rows);
            var words = service.GetWords(cells);
            var hanjas = service.ConvertWords(words);
            var saved = service.SaveXml(hanjas);

            Assert.IsTrue(saved);
        }
    }
}
