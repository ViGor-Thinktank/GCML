using NUnit.Framework;
using System;
using GenericCampaignMasterLib;

namespace GenericCampaignMasterLibTests
{
    [TestFixture()]
    public class CampaignStateTest
    {
        CampaignEngine testEngine;

        [SetUp]
        public void init()
        {
            //testEngine = TestSetup.getTestCampaignEngine();
        }

        [Test()]
        public void TestTestSaveRestore()
        {

            //CampaignState state = new CampaignState();
            //state.Save(testEngine);

            //CampaignEngine engineRestored = state.Restore();
            //Assert.AreEqual(testEngine.FieldField.Id, engineRestored.FieldField.Id, "FieldField aus dem wiederhergestellten State ist nicht identisch.");
            
            //Assert.AreEqual(testEngine.ListPlayers, engineRestored.ListPlayers, "ListPlayers aus dem wiederhergestellten State ist nicht identisch.");
        }

    }
}

