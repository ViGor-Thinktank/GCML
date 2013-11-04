using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GenericCampaignMasterLib;
using GenericCampaignMasterModel;
using Ploeh.AutoFixture;

namespace GcmlUnitTests
{
    [TestClass]
    public class CampaignStateUnitTest
    {
        private CampaignState getTestState()
        {
           
            CampaignState result = CampaignState.NewInstance();

            var fixture = new Fixture();
            result.CampaignId = fixture.Create<string>();
            result.CampaignName = fixture.Create<string>();
            result.ListPlayers = fixture.Create<List<PlayerInfo>>();

            return result;
        }

        
        
        [TestMethod]
        public void TestToString()
        {
            CampaignState state0 = getTestState();
            string str0 = state0.ToString();

            CampaignState state1 = CampaignState.FromString(str0);
            string str1 = state1.ToString();


            Assert.AreEqual(str0, str1);
        }

        [TestMethod]
        public void TestFromString()
        { 

        }

    }
}
