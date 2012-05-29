using System;
using System.Collections.Generic;

namespace GenericCampaignMasterLib
{
	
	/// <summary>
	/// Testimplementierung für das IUnit Interface. Kann sich 1 Feld fortbewegen.
	/// </summary>
	public class DummyUnit : IUnit
	{
		private int m_intId;
		
		public DummyUnit (int unitId)
		{
			m_intId = unitId;			
		}

		
		#region IUnit implementation
		public List<ICommand> getCommands()
		{
			Move cmd = new Move();
			cmd.IntRange = 1;
			cmd.Unit = this;
		
		    List<ICommand> cmdlist = new List<ICommand>();
			cmdlist.Add(cmd);
			return cmdlist;
		}
		
		public int Id { get { return m_intId; }}

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

