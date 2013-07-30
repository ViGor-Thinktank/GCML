using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericCampaignMasterModel.Commands;

namespace GenericCampaignMasterModel
{
    public class clsUnitGroup
    {
        public static clsUnitTypeCollection objUnitTypeFountain = new clsUnitTypeCollection();

        public clsUnitGroup() { }

        public clsUnitGroup(Player objOwner, int intUnitTypeID)
        {
            this.m_strId = objOwner.Id + objOwner.ListUnits.Count.ToString();
            addNewUnit(objOwner, intUnitTypeID);
        }

        public void addNewUnit(Player objOwner, int intUnitTypeID)
        {
            clsUnitType UnitType = objUnitTypeFountain.getUnitType(intUnitTypeID);

            clsUnit newUnit = new clsUnit(UnitType);
            if (newUnit.objUnitType.m_listUnitSpawn != null && newUnit.objUnitType.m_listUnitSpawn.Count == 0)
            {
                newUnit.objUnitType.m_listUnitSpawn = objOwner.objPlayerFaction.listUnitspawn;
            }

            this.m_listUnits.Add(newUnit);
        }

        private List<clsUnit> m_listUnits = new List<clsUnit>();

        public int intMovement { get { return m_listUnits.Min<clsUnit>(w => w.objUnitType.intMovement); } }

        public int intSichtweite { get { return m_listUnits.Max<clsUnit>(w => w.objUnitType.intSichtweite); } }

        public int intResourceValue
        {
            get { return this.m_intResourceValue; }
            set { this.m_intResourceValue = value; }
        }
        public int m_intResourceValue = 0;

        public int intMaxResourceValue { get { return this.m_listUnits.Sum<clsUnit>(p => p.intMaxResourceValue); } }

        public int addResourceValue(int value)
        {
            int überschuss = 0;
            m_intResourceValue += value;
            if (intResourceValue > this.intMaxResourceValue)
            {
                überschuss = this.m_intResourceValue - this.intMaxResourceValue;
                m_intResourceValue = this.intMaxResourceValue;
            }
            return überschuss;
        }

        public string strOwnerID = "";

        public int cnt = -1;

        private string m_strId = "-1";
        public string Id
        {
            get { return m_strId; }
            set { m_strId = value; }
        }

        public string strBezeichnung
        {
            get
            {
                string strBez = this.m_strId + "(";

                foreach (clsUnit u in this.m_listUnits)
                {
                    strBez += u.objUnitType.strBez;
                }

                return strBez + ")";
            }
        }

        public List<ICommand> getTypeCommands()
        {
            List<ICommand> cmdlist = new List<ICommand>();

            ICommand cmd;

            if (this.intMovement > 0)
            {
                cmd = new comMove(this);
                cmdlist.Add(cmd);
            }

            if (this.intResourceValue > 0)
            {
                cmd = new comDropResource(this, null);
                cmdlist.Add(cmd);

                if (this.blnCanSpawnUnits)
                {
                    cmd = new comPlaceUnit(this);
                    cmdlist.Add(cmd);
                }
            }

            if (this.intCreateValuePerRound > 0)
            {
                cmd = new comCreateResource(this);
                cmdlist.Add(cmd);
            }

            return cmdlist;
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
                string strDes = "";
                foreach (clsUnit u in m_listUnits)
                {
                    strDes += u.objUnitType.strDescription.Replace("[%intResourceValue%]", intResourceValue.ToString());
                    strDes += System.Environment.NewLine;
                }
                return strDes;
            }
        }

        public string strBez
        {
            get
            {
                string strDes = "";
                foreach (clsUnit u in m_listUnits)
                {
                    strDes += u.objUnitType.strBez;
                    strDes += System.Environment.NewLine;
                }
                return strDes;
            }
        }



        public clsUnitType firstUnitSpawnType { 
            get
            { return m_listUnits.First<clsUnit>(t => t.objUnitType.m_listUnitSpawn != null).objUnitType.m_listUnitSpawn[0]; }
        }

        public int intCreateValuePerRound { get { return m_listUnits.Sum<clsUnit>(w => w.objUnitType.intCreateValuePerRound); } }

        public bool blnCanStoreResourceValue { get { return (this.m_listUnits.Count<clsUnit>(n => n.objUnitType.blnCanStoreResourceValue == true)) > 0; } }

        public bool blnCanSpawnUnits { get { return (this.m_listUnits.Count<clsUnit>(n => n.objUnitType.blnCanSpawnUnits == true)) > 0; } }

        public string strClientData
        {
            get
            {
                string str = "";
                foreach (clsUnit u in this.m_listUnits)
                {
                    str += u.objUnitType.strClientData + "#";
                }
                return str;

            }
        }

        public bool blnAlywaysVisible { get { return (this.m_listUnits.Count<clsUnit>(n => n.objUnitType.blnAlywaysVisible == true)) > 0; } }


    }
}
