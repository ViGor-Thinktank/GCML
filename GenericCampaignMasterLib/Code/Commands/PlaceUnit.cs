using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterLib
{
    public class PlaceUnit : ICommand
    {
        private CampaignController m_controller; 
        public UnitInfo UnitToPlace { get; set; }

        public PlaceUnit(CampaignController controller)
        {
            m_controller = controller;
        }


        # region ICommand Member
        public void Execute()
        {
            //m_controller.addUnit(UnitToPlace);
        }

        public void Register()
        {
            throw new NotImplementedException();
        }

        public string strInfo
        {
            get { throw new NotImplementedException(); }
        }

        public bool blnExecuted
        {
            get { throw new NotImplementedException(); }
        }

        public string CommandId
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public CommandInfo getInfo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
