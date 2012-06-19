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
        [SetUp]
        public void init()
        {
        }


        [Test]
        public void testLoadAndSaveState()
        {
            //string state = testController.getGameState();

            //// Controller aus String wiederherstellen
            //CampaignController testControllerNew = new CampaignController();
            //testControllerNew.loadGameState(state);



        }





        [Test]
        public void testUnitCollisions()
        {
            CampaignController controller = new CampaignBuilderTicTacTod().buildNew();
            Player player1 = controller.getPlayerList()[0];
            Player player2 = controller.getPlayerList()[1];

            // Unit von Player 1 solange bewegen bis Kollision
            IUnit player1unit1 = player1.ListUnits[0];
            Sektor targetSektor = controller.getSektorForUnit(player2.ListUnits[0]);
            do
            {
                List<ICommand> lstCmd = controller.getCommandsForUnit(player1unit1);
                Move move = (from m in lstCmd where m.GetType() == typeof(Move) select m as Move).First();
                move.Execute();
            }
            while(controller.getUnitCollisions().Count > 0);


            List<Sektor> collisions = controller.getUnitCollisions();
            Assert.AreEqual(true, collisions.Contains(targetSektor));

            //// Unit 1 verliert - Rückzug
            //lstCmd = testEngine.getDefaultMoveCommandsForUnit(player1.ListUnits [0]);
            //Move retreat = (from m in lstCmd where m.TargetSektor == sektor1 select m).First();
            //retreat.Execute();

            //collisions = testController.getUnitCollisions();
            //Assert.AreEqual(false, collisions.Contains(sektor2));
        }
    }
}
