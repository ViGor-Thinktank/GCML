﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericCampaignMasterLib;
using GenericCampaignMasterLib.Code.Unit;
using NUnit.Framework;

namespace GenericCampaignMasterLibTests.Tests
{
    [TestFixture]
    public class CampaignControllerTest
    {
        CampaignController testController;
        Player player1 = new Player(664);
        Player player2 = new Player(665);
        DummyUnit unit1 = new DummyUnit(666);
        DummyUnit unit2 = new DummyUnit(667);
        Sektor sektor1 = new Sektor("1");
        Sektor sektor2 = new Sektor("2");
        Sektor sektor3 = new Sektor("3");

        [SetUp]
        public void init()
        {
            CampaignEngine testEngine = new CampaignEngine();
            testEngine.FieldField = new Field_Schlauch(new List<Sektor>() { sektor1, sektor2, sektor3 });
            testEngine.FieldField.Id = 123;

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
            



        }
    }
}