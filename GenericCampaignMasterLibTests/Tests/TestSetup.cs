using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericCampaignMasterLib;
using NUnit.Framework;

namespace GenericCampaignMasterLibTests
{
    public class TestSetup
    {
        public static CampaignEngine getTestCampaignEngine()
        {
            Player player1 = new Player(1);
            Player player2 = new Player(2);

            DummyUnit unit1 = new DummyUnit(666);
            DummyUnit unit2 = new DummyUnit(667);
   
            CampaignEngine testEngine = new CampaignEngine(new Field_Schlauch(new List<int> { 3 }));
            testEngine.FieldField.Id = 123;

            Sektor sektor1 = testEngine.FieldField.getSektorList()[0];
            Sektor sektor2 = testEngine.FieldField.getSektorList()[1];
            Sektor sektor3 = testEngine.FieldField.getSektorList()[2];

            player1.ListUnits.Add(unit1);
            player2.ListUnits.Add(unit2);
            
            testEngine.addPlayer(player1);
            testEngine.addPlayer(player2);

            sektor1.addUnit(unit1);
            sektor2.addUnit(unit2);

            return testEngine;
        }


    }
}

