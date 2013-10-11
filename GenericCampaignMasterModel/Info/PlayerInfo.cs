using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace GenericCampaignMasterModel
{
    public class PlayerInfo
    {
        [Display(Name = "Player id")]
        public string playerId { get; set; }

        [Display(Name = "Player name")]
        public string playerName { get; set; }
    }
}
