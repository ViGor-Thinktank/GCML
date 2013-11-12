using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace GenericCampaignMasterModel
{
    public class PlayerInfo
    {
        [Key]
        [Display(Name = "Player Id")]
        public string playerId { get; set; }

        [Display(Name = "Player Name")]
        public string playerName { get; set; }
    }
}
