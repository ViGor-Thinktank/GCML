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
        [Required]
        [Display(Name = "Player Id")]
        public string playerId { get; set; }

        [Required]
        [Display(Name = "Player Name")]
        public string playerName { get; set; }

        public List<CampaignState> Campaigns { get; set; }
    }
}
