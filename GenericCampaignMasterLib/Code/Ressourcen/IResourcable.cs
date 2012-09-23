using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterLib
{
    /// <summary>
    /// Muss von Objekten implementiert werden die als Ressource verwaltet werden.
    /// </summary>
    public interface IResourcable
    {
        List<ICommand> getResourceCommands(Player owner);
    }
}
