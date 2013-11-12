using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace GenericCampaignMasterModel
{
    public class ResourceInfo
    {
        [Key]
        public string resourceId { get; set; }
        public string ownerId { get; set; }
        public string resourceableType { get; set; }
    }
}
