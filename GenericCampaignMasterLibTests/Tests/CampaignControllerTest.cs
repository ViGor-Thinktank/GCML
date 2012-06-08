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
        CampaignEngine testEngine = null;
        CampaignController testController;
        Player player1 = null;
        Player player2 = null;
        DummyUnit unit1 = new DummyUnit(666);
        DummyUnit unit2 = new DummyUnit(667);
        Sektor sektor1 = new Sektor("1");
        Sektor sektor2 = new Sektor("2");
        Sektor sektor3 = new Sektor("3");

        [SetUp]
        public void init()
        {
            testEngine = new CampaignEngine(new Field_Schlauch(new List<Sektor>() { sektor1, sektor2, sektor3 }));
            testEngine.FieldField.Id = 123;

            player1 = new Player(1, testEngine);
            player2 = new Player(2, testEngine);

            player1.ListUnits.Add(unit1);
            player2.ListUnits.Add(unit2);
            
            testEngine.addPlayer(player1);
            testEngine.addPlayer(player2);

            sektor1.addUnit(unit1);
            sektor2.addUnit(unit2);

            testController = new CampaignController(testEngine);
        }

        [Test]
        public void testUnitCollisions()
        {
            List<Move> lstCmd = testEngine.getDefaultMoveCommandsForUnit(unit1);
            Move move = (from m in lstCmd where m.TargetSektor == sektor2 select m).First();
            move.Execute();

            List<Sektor> collisions = testController.getUnitCollisions();
            Assert.AreEqual(true, collisions.Contains(sektor2));

            // Unit 1 verliert - Rückzug
            lstCmd = testEngine.getDefaultMoveCommandsForUnit(unit1);
            Move retreat = (from m in lstCmd where m.TargetSektor == sektor1 select m).First();
            retreat.Execute();

            collisions = testController.getUnitCollisions();
            Assert.AreEqual(false, collisions.Contains(sektor2));
        }
    }
}
