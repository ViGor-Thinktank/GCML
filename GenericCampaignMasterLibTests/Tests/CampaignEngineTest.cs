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
            //testEngine = new CampaignEngine(new Field_Schlauch(new List<Sektor>() { new Sektor("0"), new Sektor("1"), new Sektor("2") }));
            
            //testEngine.FieldField.Id = 123;

            //testEngine.addPlayer("Player 1");
            //testEngine.addPlayer("Player 2");
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
            testEngine.ListPlayers[0].ListUnits.Add(testUnit);

            Player owner = testEngine.getUnitOwner(testUnit);

            Assert.AreEqual(testEngine.ListPlayers[0], owner);
        }
        
    }       


}
