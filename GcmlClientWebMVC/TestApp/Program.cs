using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericCampaignMasterLib;
using GenericCampaignMasterModel;
using GcmlDataAccess;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            CampaignController ctrl = CampaignBuilder.Instance.buildNew();
            CampaignState state = ctrl.CampaignEngine.getState();

            GcmlDbContext ctx = new GcmlDbContext();
            ctx.CampaignStates.Add(state);
            


        }
    }
}
