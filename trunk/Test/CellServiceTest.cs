/**
 * $Id
 */

using BLL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;

namespace Test
{
    
    
    /// <summary>
    ///This is a test class for CellServiceTest and is intended
    ///to contain all CellServiceTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CellServiceTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        /// <summary>
        ///A test for DoBlankCells
        ///</summary>
        [TestMethod()]
        public void DoBlankCellsTest()
        {
            int seedSize = 3;
            int rowSize = 2*seedSize - 1;
            int candidatesAmount = seedSize;
            int cellsAmount = rowSize*rowSize;
            CellService target = new CellService();

            Cell[] actualCells = target.DoBlankCells(seedSize);          
            Assert.AreEqual(cellsAmount, actualCells.Length);
            Assert.AreEqual(candidatesAmount,actualCells[0].Candidates.Length);

            Cell expectedCell = new Cell {Row = 4, Column = 4, Candidates = new[]{1,2,3}};
            Assert.AreEqual(expectedCell.ToString(), actualCells[24].ToString());

            Cell signCell = new Cell {Row = 3, Column = 2};
            Assert.AreEqual(signCell.ToString(), actualCells[17].ToString());
        }

        /// <summary>
        /// A test ofr DoRandomVal
        /// </summary>
        [TestMethod()]
        public void DoRandomValTest()
        {
            CellService target = new CellService();
            Cell[] actualCells = target.DoRandomVal();            
            Assert.AreNotEqual(null,actualCells[0].Value);
            Assert.AreEqual(null,actualCells[1].Value);
            Assert.AreEqual(4,actualCells[0].Candidates.Length);
        }

    }
}
