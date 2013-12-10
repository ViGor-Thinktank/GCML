using System.Collections.Generic;
using System.Linq;
using DotNetOpenAuth.OAuth.Messages;
using GcmlWebApi.Controllers;
using GenericCampaignMasterModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using GcmlDataAccess;
using GcmlWebApi;
using Ninject;
using Ninject.MockingKernel;
using Ninject.MockingKernel.Moq;
using Ploeh.AutoFixture;
using GenericCampaignMasterModel;

namespace GcmlWebApi.Tests
{
    
    
    /// <summary>
    ///This is a test class for CampaignControllerTest and is intended
    ///to contain all CampaignControllerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CampaignControllerTest
    {
        Fixture fixture = new Fixture();
        private MoqMockingKernel kernel;
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
        [TestInitialize()]
        public void CamapignControllerTestInitialize()
        {
            var fixture = new Fixture();
            kernel = new MoqMockingKernel();
        }

        
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for CampaignController Constructor
        ///</summary>
        [TestMethod()]
        public void CampaignControllerConstructorTest()
        {
            var mock = kernel.GetMock<ICampaignRepository>();
            CampaignController target = new CampaignController(mock.Object);
            Assert.IsNotNull(target);
        }


        /// <summary>
        ///A test for Delete
        ///</summary>
        //[TestMethod()]
        //public void DeleteTest()
        //{
        //    CampaignController target = new CampaignController(); // TODO: Initialize to an appropriate value
        //    int id = 0; // TODO: Initialize to an appropriate value
        //    target.Delete(id);
        //    Assert.Inconclusive("A method that does not return a value cannot be verified.");
        //}

        /// <summary>
        ///A test for Get
        ///</summary>
        [TestMethod()]
        public void Get_CampaignListDto_Test()
        {
            string curruser = System.Environment.UserDomainName + "\\" + System.Environment.UserName;
            var mock = kernel.GetMock<ICampaignRepository>();
            mock.Setup(m => m.getCampaignsForPlayer(curruser)).Returns(new List<CampaignInfo>()
            {
                new CampaignInfo()
                {
                    campaignId = fixture.Create<string>(),
                    campaignName = fixture.Create<string>(),
                    //ListPlayerInfo = fixture.Create<List<PlayerInfo>>(),
                    SektorField = new SektorInfo[,]
                    {
                        { fixture.Create<SektorInfo>(), fixture.Create<SektorInfo>() },
                        { fixture.Create<SektorInfo>(), fixture.Create<SektorInfo>() }
                    }
                }
            });
            
            CampaignController target = new CampaignController(mock.Object);
            CampaignListDto actual = target.Get();

            Assert.IsInstanceOfType(actual, typeof (CampaignListDto));
            mock.Verify(c => c.getCampaignsForPlayer(curruser));
        }

        /// <summary>
        ///A test for Get
        ///</summary>
        // TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        // http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        // whether you are testing a page, web service, or a WCF service.
        //[TestMethod()]
        //[HostType("ASP.NET")]
        //[UrlToTest("http://localhost:59340")]
        //public void GetTest1()
        //{
        //    CampaignController target = new CampaignController(); // TODO: Initialize to an appropriate value
        //    int id = 0; // TODO: Initialize to an appropriate value
        //    string expected = string.Empty; // TODO: Initialize to an appropriate value
        //    string actual;
        //    actual = target.Get(id);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        /// <summary>
        ///A test for Post
        ///</summary>
        // TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        // http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        // whether you are testing a page, web service, or a WCF service.
        //[TestMethod()]
        //[HostType("ASP.NET")]
        //[UrlToTest("http://localhost:59340")]
        //public void PostTest()
        //{
        //    CampaignController target = new CampaignController(); // TODO: Initialize to an appropriate value
        //    string value = string.Empty; // TODO: Initialize to an appropriate value
        //    target.Post(value);
        //    Assert.Inconclusive("A method that does not return a value cannot be verified.");
        //}

        /// <summary>
        ///A test for Put
        ///</summary>
        // TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        // http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        // whether you are testing a page, web service, or a WCF service.
        //[TestMethod()]
        //[HostType("ASP.NET")]
        //[UrlToTest("http://localhost:59340")]
        //public void PutTest()
        //{
        //    CampaignController target = new CampaignController(); // TODO: Initialize to an appropriate value
        //    int id = 0; // TODO: Initialize to an appropriate value
        //    string value = string.Empty; // TODO: Initialize to an appropriate value
        //    target.Put(id, value);
        //    Assert.Inconclusive("A method that does not return a value cannot be verified.");
        //}
    }
}
