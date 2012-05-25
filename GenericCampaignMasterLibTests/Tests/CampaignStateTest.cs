using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericCampaignMasterLib;
using NUnit.Framework;

namespace GenericCampaignMasterLibTests.Tests
{
    [TestFixture]
    public class CampaignStateTest
    {
        [Test]
        public void saveAndRestoreCampaignState()
        {
            CampaignEngine engine_origin = new CampaignEngine();
            engine_origin.CField = new Field();
            engine_origin.CField.Id = 123;
            engine_origin.CPlayers = new List<Player>() { new Player(123), new Player(456) };

            CampaignState state = new CampaignState();
            state.Save(engine_origin);

            CampaignEngine engine1 = state.Restore();
            Assert.AreEqual(engine_origin.CField, engine1.CField, "CField aus dem wiederhergestellten State ist nicht identisch.");
            Assert.AreEqual(engine_origin.CPlayers, engine1.CPlayers, "CPlayers aus dem wiederhergestellten State ist nicht identisch.");
        }
        
    }       


}
