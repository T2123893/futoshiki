/**
 * $Id$
 */

using System;
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
        ///A test for GetCells
        ///</summary>
        [TestMethod()]
        public void GetCellsTest()
        {
            int seedSize = 9;
            int rowSize = 2*seedSize - 1;
            int candidatesAmount = seedSize;
            int cellsAmount = rowSize*rowSize;
            CellService target = new CellService(seedSize);

            Cell[] actualCells = target.GetCells();  
            showFutoshiki(actualCells, seedSize);
            
            // check the cells amount
            Assert.AreEqual(cellsAmount, actualCells.Length);

//             to remove candidate from current cell after setting value
//            Assert.AreEqual(candidatesAmount-1,actualCells[0].Candidates.Length);

//             deal with related row
//            Assert.AreEqual(candidatesAmount - 1, actualCells[2].Candidates.Length);

//             deal with related column
//            Assert.AreEqual(candidatesAmount-1,actualCells[2*rowSize].Candidates.Length);

//             check a non-numeric cell
            Cell signCell = new Cell {Row = 1, Column = 8};
            Assert.AreEqual(signCell.IsNumeric, actualCells[rowSize].IsNumeric);

        }

        private void showFutoshiki(Cell[] actualCells, int rowSize)
        {
            Console.WriteLine("");

            int n = 0;
            for (int i = 0; i < actualCells.Length; i++)
            {                               
                if (actualCells[i].IsNumeric)
                {
                    Console.Write("{0, 3}", actualCells[i].Value);
                    n++;
                }
                if (n == rowSize)
                {                        
                    Console.WriteLine("");
                    n = 0;
                }
            }
        }



        
    }
}
