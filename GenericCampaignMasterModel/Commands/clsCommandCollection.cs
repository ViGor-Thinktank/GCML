using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterModel.Commands
{
    public class clsCommandCollection
    {
        public clsUnit aktUnit = null;

        public List<ICommand> listReadyCommands = null;
        public List<ICommand> listRawCommands = null;

        public event Field.delStatus onStatus;

        void m_objCommandFactory_onNewStatus(string strStatus)
        {
            if (this.onStatus != null)
                this.onStatus(strStatus);
        }

        void m_objFactory_onNewCommand(ICommand readyCmd)
        {
            listReadyCommands.Add(readyCmd);
        }

        public void useFactory(clsFactoryBase objCommandFactory)
        {
            objCommandFactory.onNewCommand += new facMoveFactory.delNewCommand(m_objFactory_onNewCommand);
            objCommandFactory.onNewStatus += new facMoveFactory.delNewStatus(m_objCommandFactory_onNewStatus);
            objCommandFactory.go();
		}

        public List<ICommand> listReadyCommandsWithTypeFilter(ICommand aktCommandType)
        {
            List<ICommand> listReadyCommandsFilter = new List<ICommand>();

            foreach (ICommand aktCom in listReadyCommands)
            {
                if (aktCom.GetType() == aktCommandType.GetType())
                    listReadyCommandsFilter.Add(aktCom);
            }

            return listReadyCommandsFilter;
        }
    }
}
