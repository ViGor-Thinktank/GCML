using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericCampaignMasterModel.Commands;

namespace GenericCampaignMasterModel
{
    /// <summary>
    /// Muss von Objekten implementiert werden die als Ressource verwaltet werden.
    /// </summary>
    public interface IResourceable
    {
        List<ICommand> getResourceCommands(Player owner);
    }
}
