using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using GenericCampaignMasterLib;
using GenericCampaignMasterModel;

namespace GcmlDataAccess
{
    public class GcmlDbContext : DbContext
    {
        public DbSet<CampaignState> CampaignStates { get; set; }
        public DbSet<PlayerInfo> Players { get; set; }


    }
}
