using System.Collections.Generic;
using GcmlDataAccess;
using GcmlWebApi.Controllers;
using GenericCampaignMasterLib;
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

        private Fixture fixture;
        private MoqMockingKernel kernel;
        private TestContext testContextInstance;
        private Mock<ICampaignController> _mockController;
        private Mock<ICampaignRepository> _gcmlRepository;
        private Player _testPlayer;


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
                    {fixture.Create<SektorInfo>(), fixture.Create<SektorInfo>()},
                    {fixture.Create<SektorInfo>(), fixture.Create<SektorInfo>()}
                },
                ListUnits = new List<UnitInfo>()
                {
                    fixture.Create<UnitInfo>()
                }
            };

            // Testplayer
            _testPlayer = new Player(new PlayerInfo() {playerName = _playername});

            // Mock für CampaignController
            _mockController = kernel.GetMock<ICampaignController>();
            _mockController.Setup(c => c.Player_getByName(_playername)).Returns(_testPlayer);
            _mockController.Setup(c => c.Player_getUnitsForPlayer(_testPlayer)).Returns(new List<clsUnit>()
            {
                 new clsUnit()
                 {
                     Id = fixture.Create<string>()
                 }
            });

            // Mock für CampaignRepository
            _gcmlRepository = kernel.GetMock<ICampaignRepository>();
            _gcmlRepository.Setup(m => m.getCampaignsForPlayer(_playername)).Returns(new List<CampaignInfo>() {campaignInfo});
            _gcmlRepository.Setup(m => m.getCampaignController(_campaignId)).Returns(_mockController.Object);

            return _gcmlRepository;
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
        /// Get - Leere Parameter
        ///</summary>
        [TestMethod()]
        public void Get_UnitListDto_Test_EmptyParams()
        {
            var repoMock = getGcmlRepository();
            UnitController target = new UnitController(repoMock.Object);
            string campaignId = string.Empty; 
            
            UnitListDto actual;
            actual = target.Get(campaignId);
            
            Assert.IsNotNull(actual);
            Assert.IsInstanceOfType(actual, typeof(UnitListDto));
            Assert.IsTrue((actual.unitList == null) || (actual.unitList.Count == 0));
        }


        /// <summary>
        /// Get - CampaignId - soll alle Units liefern.
        ///</summary>
        [TestMethod()]
        public void Get_UnitListDto_Test_WithCampaign()
        {
            var repoMock = getGcmlRepository();
            UnitController target = new UnitController(repoMock.Object);

            UnitListDto actual = target.Get(_campaignId);

            Assert.IsNotNull(actual);
            Assert.IsInstanceOfType(actual, typeof(UnitListDto));
            _gcmlRepository.Verify(r => r.getCampaignController(_campaignId));
            _mockController.Verify(c => c.Player_getUnitsForPlayer(_testPlayer));
        }
    }
}
