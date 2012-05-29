using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericCampaignMasterLib;
using NUnit.Framework;

namespace GenericCampaignMasterLibTests.Tests
{
    [TestFixture]
    public class CampaignEngineTest
    {
        CampaignEngine testEngine;
        
        [SetUp]
        public void init()
        {
            testEngine = new CampaignEngine();
            testEngine.CField = new Field();
            testEngine.CField.Id = 123;
            testEngine.CField.SektorList = new List<Sektor>() { new Sektor(1), new Sektor(2), new Sektor(3) };
            testEngine.CPlayers = new List<Player>() { new Player(1), new Player(2) };
        }

        [Test]
        public void saveAndRestoreCampaignState()
        {
            CampaignState state = new CampaignState();
            state.Save(testEngine);

            CampaignEngine engineRestored = state.Restore();
            Assert.AreEqual(testEngine.CField, engineRestored.CField, "CField aus dem wiederhergestellten State ist nicht identisch.");
            Assert.AreEqual(testEngine.CPlayers, engineRestored.CPlayers, "CPlayers aus dem wiederhergestellten State ist nicht identisch.");
        }

        [Test]
        public void testCampaignEngineFunctions()
        {
            DummyUnit unit1 = new DummyUnit();
            Sektor targetSektor = testEngine.CField.SektorList[0];
            testEngine.CField.putUnit(unit1, targetSektor);

            Assert.Contains(unit1, targetSektor);
        }
        
    }       


}
