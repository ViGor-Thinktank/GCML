using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterLib
{
    public interface IUnit
    {
         int Id { get; set; }             // Dummy
         string Bezeichnung { get; set; } // Dummy
		
		 List<ICommand> getCommands();		// Liefert alle Aktionen, die von der Unit ausgeführt werden können. Unabhängig vom Kontext.
    }
}
