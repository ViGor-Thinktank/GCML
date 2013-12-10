using System.Collections.Generic;
using GcmlDataAccess;
using GcmlWebApi.Controllers;
using GenericCampaignMasterModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using GcmlWebApi;
using Moq;
using Ninject;
using Ninject.MockingKernel;
using Ninject.MockingKernel.Moq;
using Ploeh.AutoFixture;

namespace GcmlWebApi.Tests
{
    /// <summary>
    ///This is a test class for UnitControllerTest and is intended
    ///to contain all UnitControllerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class UnitControllerTest
    {
        private string _campaignId;
        private string _campaignName;
        private string _playername;
        private string _sektorId;

        private Fixture fixture;
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
        public void UnitControllerTestInitialize()
        {
            fixture = new Fixture();
            kernel = new MoqMockingKernel();
            _campaignId = fixture.Create<string>();
            _campaignName = fixture.Create<string>();
            _playername = System.Environment.UserDomainName + "\\" + System.Environment.UserName;
            _sektorId = fixture.Create<string>();

            //kernel.Bind<ICampaignRepository>().ToMock();
        }
        
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        /// <summary>
        /// Erzeugt und Konfiguriert ein Mockobjekt für das Repository
        /// incl. Testdaten.
        /// </summary>
        /// <returns></returns>
        private Mock<ICampaignRepository> getGcmlRepository()
        {

            var campaignInfo = new CampaignInfo()
            {
                campaignId = _campaignId,
                campaignName = _campaignName,
                //ListPlayerInfo = fixture.Create<List<PlayerInfo>>(),
                SektorField = new SektorInfo[,]
                {
                    {new SektorInfo() {sektorId = _sektorId}, fixture.Create<SektorInfo>()},
                    {fixture.Create<SektorInfo>(), fixture.Create<SektorInfo>()}
                },
                ListUnits = new List<UnitInfo>()
                {
                    fixture.Create<UnitInfo>()
                }
            };

            var mockRepo = kernel.GetMock<ICampaignRepository>();
            mockRepo.Setup(m => m.getCampaignsForPlayer(_playername)).Returns(new List<CampaignInfo>() {campaignInfo});

            return mockRepo;
        }

        /// <summary>
        ///A test for UnitController Constructor
        ///</summary>
        [TestMethod()]
        public void UnitControllerConstructorTest()
        {
            var repoMock = kernel.GetMock<ICampaignRepository>();
            UnitController target = new UnitController(repoMock.Object);
            Assert.IsNotNull(target);
        }

        /// <summary>
        /// Leere Parameter
        ///</summary>
        [TestMethod()]
        public void Get_UnitListDto_Test_EmptyParams()
        {
            var repoMock = getGcmlRepository();
            UnitController target = new UnitController(repoMock.Object);
            string campaignId = string.Empty; 
            string sektorId = string.Empty; 
            
            UnitListDto actual;
            actual = target.Get(campaignId, sektorId);
            
            Assert.IsNotNull(actual);
            Assert.IsInstanceOfType(actual, typeof(UnitListDto));
            Assert.IsTrue((actual.unitList == null) || (actual.unitList.Count == 0));
            repoMock.Verify(r => r.getCampaignsForPlayer(_playername));
        }


        /// <summary>
        /// Nur CampaignId - sollte alle Units liefern.
        ///</summary>
        [TestMethod()]
        public void Get_UnitListDto_Test_WithCampaign()
        {
            var repoMock = getGcmlRepository();
            UnitController target = new UnitController(repoMock.Object);

            UnitListDto actual;
            actual = target.Get(_campaignId, string.Empty);

            Assert.IsNotNull(actual);
            Assert.IsInstanceOfType(actual, typeof(UnitListDto));
            Assert.IsTrue(actual.unitList.Count > 0);
        }

        /// <summary>
        /// CampaignId und SectorId - Liefert Units für einen Sektor.
        ///</summary>
        [TestMethod()]
        public void Get_UnitListDto_Test_WithCampaignAndSector()
        {
            var repoMock = getGcmlRepository();
            UnitController target = new UnitController(repoMock.Object);
            
            UnitListDto actual;
            actual = target.Get(_campaignId, _sektorId);

            Assert.IsNotNull(actual);
            Assert.IsInstanceOfType(actual, typeof(UnitListDto));
        }
    }
}
