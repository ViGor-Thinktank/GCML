using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterLib
{
    public interface ICommand
    {
        void Execute();
        void Register();
        string strInfo { get; }
        bool blnExecuted { get; }
        string CommandId { get; set; }
        CommandInfo getInfo();
    }
}
