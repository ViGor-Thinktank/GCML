using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericCampaignMasterLib;
using NUnit.Framework;

namespace GenericCampaignMasterLibTests.Tests
{
    [TestFixture]
    public class CampaignControllerTest
    {
        CampaignEngine testEngine;
        CampaignController testController;

        [SetUp]
        public void init()
        {
            testEngine = TestSetup.getTestCampaignEngine();
            testController = new CampaignController(testEngine);
        }


        [Test]
        public void testLoadAndSaveState()
        {
            testController.saveGameState();
        }



        [Test]
        public void testUnitCollisions()
        {
            Player player1 = testEngine.dicPlayers.Values.ElementAt(0);
            Player player2 = testEngine.dicPlayers.Values.ElementAt(1);

            Sektor sektor1 = testEngine.FieldField.getSektorList() [0];
            Sektor sektor2 = testEngine.FieldField.getSektorList() [1];

            List<Move> lstCmd = testEngine.getDefaultMoveCommandsForUnit(player1.ListUnits [0]);
            Move move = (from m in lstCmd where m.TargetSektor == sektor2 select m).First();
            move.Execute();

            List<Sektor> collisions = testController.getUnitCollisions();
            Assert.AreEqual(true, collisions.Contains(sektor2));

            // Unit 1 verliert - Rückzug
            lstCmd = testEngine.getDefaultMoveCommandsForUnit(player1.ListUnits [0]);
            Move retreat = (from m in lstCmd where m.TargetSektor == sektor1 select m).First();
            retreat.Execute();

            collisions = testController.getUnitCollisions();
            Assert.AreEqual(false, collisions.Contains(sektor2));
        }
    }
}
