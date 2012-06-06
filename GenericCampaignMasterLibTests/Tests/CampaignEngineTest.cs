using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericCampaignMasterLib;
using GenericCampaignMasterLib.Code.Unit;
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
            testEngine.FieldField = new Field_Schlauch(new List<Sektor>() { new Sektor("0"), new Sektor("1"), new Sektor("2") });
            testEngine.FieldField.Id = 123;
            
            testEngine.setPlayerList(new List<Player>() { new Player(1), new Player(2) });
        }

        [Test]
        public void testSaveAndRestoreCampaignState()
        {
            CampaignState state = new CampaignState();
            state.Save(testEngine);

            CampaignEngine engineRestored = state.Restore();
            Assert.AreEqual(testEngine.FieldField.Id, engineRestored.FieldField.Id, "FieldField aus dem wiederhergestellten State ist nicht identisch.");
            Assert.AreEqual(testEngine.dicPlayers, engineRestored.dicPlayers, "ListPlayers aus dem wiederhergestellten State ist nicht identisch.");
        }

		
        [Test]
		public void testMoveUnitOneSektor()
		{
            // Testfall: Bewegung um einen Sektor
            DummyUnit testUnit = new DummyUnit(666); ;
            string startPos = "0";
            string targetPos = "1";
            
            Sektor targetSektor = testEngine.FieldField.get(targetPos);

            testEngine.FieldField.get(startPos).addUnit(testUnit);
            Assert.AreEqual(startPos, testEngine.getSektorContainingUnit(testUnit).ListUnits.IndexOf(testUnit));

            List<Move> possibleMoves = testEngine.getDefaultMoveCommandsForUnit(testUnit);

            Move commandToTest = null;
            foreach (Move m in possibleMoves)
            {
                if (m.TargetSektor == targetSektor)
                {
                    commandToTest = m;
                    break;
                }
            }
            commandToTest.Execute();

			Sektor newPosition = testEngine.getSektorContainingUnit(testUnit);
            Assert.AreEqual(targetSektor, newPosition);
		}

        [Test]
        public void testGetUnitOwner()
        {
            DummyUnit testUnit = new DummyUnit(999);
            testEngine.dicPlayers[1].ListUnits.Add(testUnit);

            Player owner = testEngine.getUnitOwner(testUnit);

            Assert.AreEqual(testEngine.dicPlayers[1], owner);
        }
        
    }       


}
