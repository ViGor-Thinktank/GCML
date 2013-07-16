using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericCampaignMasterModel.Commands;

namespace GenericCampaignMasterModel
{
  
    public class clsUnit
    {
        public static clsUnitTypeCollection objUnitTypeFountain = new clsUnitTypeCollection();

        public clsUnit() { }

        public clsUnit(string unitId, int intUnitTypeID)
        {
            init(unitId,objUnitTypeFountain.getUnitType(intUnitTypeID));
        }
      
        private void init(string unitId, clsUnitType UnitType)
        {
            m_strId = unitId;
            m_objUnitType = UnitType;
            m_strBezeichnung = UnitType.strBez + " " + unitId.ToString();
            this.intResourceValue = UnitType.intResourceValue;
        }

        public int intMovement { get { return m_objUnitType.intMovement; } }
        public int intSichtweite { get { return m_objUnitType.intSichtweite; } }
        public int intResourceValue = 0;


        public string strOwnerID = "";

        private string m_strId = "-1";
        public string Id 
        { 
            get { return m_strId; }
            set { m_strId = value; }
        }

        private string m_strBezeichnung = "";
        public string Bezeichnung
        {
            get { return m_strBezeichnung; }
            set { m_strBezeichnung = value; }
        }

        private clsUnitType m_objUnitType;
        public clsUnitType UnitType
        {
            get { return m_objUnitType; }
            set { m_objUnitType = value; }
        }

        public List<ICommand> getTypeCommands()
        {
            return m_objUnitType.getTypeCommands(this);
        }

        private ICommand m_aktCommand = null;

        public ICommand aktCommand
        {
            get { return m_aktCommand; }
            set { m_aktCommand = value; }
        }
        
        public string strAktCommandInfo
        {
            get
            {
                if (m_aktCommand != null)
                    return m_aktCommand.strInfo;
                else
                    return "";
            }
        }

        public void CampaignController_onTick()
        {
            if (this.aktCommand != null && !this.aktCommand.blnExecuted)
            {
                this.aktCommand.Execute();
            }
        }

        public string strDescription
        {
            get
            {
                return this.m_objUnitType.strDescription.Replace("[%intResourceValue%]", intResourceValue.ToString());
            }
        }

        public string strBez { get { return m_objUnitType.strBez; } }
    }
}
