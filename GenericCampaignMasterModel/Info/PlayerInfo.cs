using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace GenericCampaignMasterModel
{
    public class PlayerInfo
    {
        public PlayerInfo()
        {
            playerName = "";
            Campaigns = new List<CampaignState>();
        }

        //[Key]
        //public string playerId { get; set; }

        [Key]
        [Required]
        public string playerName { get; set; }

        public List<CampaignState> Campaigns { get; set; }
    }
}
