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
            testEngine.FieldField = new Field();
            testEngine.FieldField.Id = 123;
            testEngine.FieldField.ListSektors = new List<Sektor>() { new Sektor(1), new Sektor(2), new Sektor(3) };
            testEngine.ListPlayers = new List<Player>() { new Player(1), new Player(2) };
        }

        [Test]
        public void testSaveAndRestoreCampaignState()
        {
            CampaignState state = new CampaignState();
            state.Save(testEngine);

            CampaignEngine engineRestored = state.Restore();
            Assert.AreEqual(testEngine.FieldField.Id, engineRestored.FieldField.Id, "FieldField aus dem wiederhergestellten State ist nicht identisch.");
            Assert.AreEqual(testEngine.ListPlayers, engineRestored.ListPlayers, "ListPlayers aus dem wiederhergestellten State ist nicht identisch.");
        }

		
        [Test]
		public void testMoveUnit()
		{
			int startPos = 0;
			DummyUnit du = new DummyUnit(666);
			testEngine.FieldField.ListSektors[startPos].addUnit(du);
			
			Assert.AreEqual(startPos, testEngine.FieldField.getSektorContainingUnit(du).ListUnits.IndexOf(du));
			
			
			List<ICommand> cmds = testEngine.getCommandsForUnit(du);
			foreach (ICommand c in cmds)
			{
				if (c.GetType() == typeof(Move))
				{
					c.Execute();
					break;
				}
				
			}
			
			Sektor newSektor = testEngine.FieldField.getSektorContainingUnit(du);
			int newPos = testEngine.FieldField.ListSektors.IndexOf(newSektor);
			
			Assert.AreEqual(1, newPos);
			Assert.AreNotEqual(newPos, startPos);			
		}


        
    }       


}
