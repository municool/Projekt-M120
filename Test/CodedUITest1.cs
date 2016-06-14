using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UITest.Extension;
using Keyboard = Microsoft.VisualStudio.TestTools.UITesting.Keyboard;


namespace Test
{
    /// <summary>
    /// Summary description for CodedUITest1
    /// </summary>
    [CodedUITest]
    public class CodedUITest1
    {
        public CodedUITest1()
        {
        }

        [TestInitialize]
        public void MyTestInitialize()
        {
            ApplicationUnderTest.Launch(@"..\..\..\M120-LB2-FS16\bin\Debug\M120-LB2-FS16.exe");
        }

        [TestMethod]
        public void CreateEinsatzUiTest()
        {
            // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
            this.UIMap.testCreateEinsatz();
            this.UIMap.CreateEinsatz_Assert();
            this.UIMap.CloseWindow();

        }

        [TestMethod]
        public void UpdateEinsatzUITest()
        {
            this.UIMap.updateEinsatz();
            this.UIMap.updateEinsatz_Assert();
            this.UIMap.CloseWindow();
        }

        [TestMethod]
        public void CreateEinsatz()
        {

            this.UIMap.CreateEinsatzinFlaseTimespan();
            this.UIMap.EinsatzinFalseTimespan_Assert();
            this.UIMap.CloseWindow();

        }

        #region Additional test attributes

        // You can use the following additional attributes as you write your tests:

        ////Use TestInitialize to run code before running each test 
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{        
        //    // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
        //}

        ////Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{        
        //    // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
        //}

        #endregion

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
        private TestContext testContextInstance;

        public UIMap UIMap
        {
            get
            {
                if ((this.map == null))
                {
                    this.map = new UIMap();
                }

                return this.map;
            }
        }

        private UIMap map;
    }
}
