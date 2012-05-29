using System;
using System.Collections.Generic;

namespace GenericCampaignMasterLib
{
	
	/// <summary>
	/// Testimplementierung für das IUnit Interface. Kann sich 1 Feld fortbewegen.
	/// </summary>
	public class DummyUnit : IUnit
	{
		public DummyUnit () 
		{
		}

		
		#region IUnit implementation
		public List<ICommand> getCommands ()
		{
			Move cmd = new Move();
			cmd.SektorRange = 1;
		    List<ICommand> cmdlist = new List<ICommand>();
			cmdlist.Add(cmd);
			return cmdlist;
		}
		

		public int Id {
			get {
				throw new System.NotImplementedException ();
			}
			set {
				throw new System.NotImplementedException ();
			}
		}

		public string Bezeichnung {
			get {
				throw new System.NotImplementedException ();
			}
			set {
				throw new System.NotImplementedException ();
			}
		}
		#endregion
	}
}

