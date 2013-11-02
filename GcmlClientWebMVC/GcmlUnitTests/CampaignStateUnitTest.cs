using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GenericCampaignMasterLib;
using GenericCampaignMasterModel;

namespace GcmlUnitTests
{
    [TestClass]
    public class CampaignStateUnitTest
    {
        private CampaignState getTestState()
        {
            CampaignState result = CampaignState.NewInstance();


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
