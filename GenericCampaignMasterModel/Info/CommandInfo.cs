﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterModel
{
    public struct CommandInfo
    {
        public string commandId;
        public string commandType;
        public string actingUnitId;
        public string targetId;         // Kontextabhängig, ID eines Sektors oder einer Unit.
        public string strInfo;
        public bool isActive;
    }
}
