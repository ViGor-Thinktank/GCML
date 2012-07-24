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
            BaseUnit player1unit1 = player1.ListUnits[0];
            Sektor targetSektor = controller.getSektorForUnit(player2.ListUnits[0]);
            Sektor previousSektor = new Sektor();       // Vorherige Position merken
            do
            {
                Sektor currentSektor = controller.getSektorForUnit(player1unit1);
                List<ICommand> lstCmd = controller.getCommandsForUnit(player1unit1);
                var moves = from m in lstCmd 
                             where m.GetType() == typeof(Move) 
                             select m as Move;

                Move movExec = moves.First(t => t.TargetSektor != previousSektor);
                movExec.Execute();
                previousSektor = currentSektor;
            }
            while(controller.getUnitCollisions().Count == 0);


            List<Sektor> collisions = controller.getUnitCollisions();
            Assert.AreEqual(true, collisions.Contains(targetSektor));
       }
    }
}
