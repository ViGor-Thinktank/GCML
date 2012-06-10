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
            CampaignEngine testEngine = null;
            Player player1 = null;
            Player player2 = null;
            DummyUnit unit1 = new DummyUnit(666);
            DummyUnit unit2 = new DummyUnit(667);
            Sektor sektor1 = new Sektor("0");
            Sektor sektor2 = new Sektor("1");
            Sektor sektor3 = new Sektor("2");

            testEngine = new CampaignEngine(new Field_Schlauch(new List<Sektor>() { sektor1, sektor2, sektor3 }));
            testEngine.FieldField.Id = 123;

            player1 = new Player(1);
            player2 = new Player(2);

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

