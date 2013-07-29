using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericCampaignMasterModel;

namespace GenericCampaignMasterModel.Commands
{
    public interface ICommandWithSektor : ICommand
    {
        Sektor TargetSektor { get; set; }        
    }

    public interface ICommand
    {
        void Execute();
        void Register();
        string strInfo { get; }
        string strTypeName { get; }
        bool blnExecuted { get; }
        string CommandId { get; set; }
        CommandInfo getInfo();

        clsFactoryBase getCommandFactory(clsUnitGroup objUnit, Field FieldField);
        event delControllerEvent onControllerEvent;
    }

    public delegate void delControllerEvent(clsEventData objEventData);
    public class clsEventData 
    {
        public clsCommandBaseClass objCommand = null;


    }

    public abstract class clsCommandBaseClass : ICommandWithSektor
    {

        public clsCommandBaseClass(string strTypeName)
        {
            this.m_strTypeName = strTypeName;
            this.CommandId = Guid.NewGuid().ToString();
        }

        public void raiseControllerEvent()
        {
            if (onControllerEvent != null)
            {
                clsEventData obj = new clsEventData();
                obj.objCommand = this;
                onControllerEvent(obj);
            }
        }
        public event delControllerEvent onControllerEvent;

        public abstract string strInfo { get;  }
        
        public string strTypeName
        {
            get
            {
                return m_strTypeName;
            }
        }

        private string m_strTypeName = "";

        //bis jetzt, betrifft ein Command, immer, eine Unit
        protected clsUnitGroup m_objUnitToCommand { get; set; }
        public Sektor TargetSektor { get; set; }

        public abstract clsFactoryBase getCommandFactory(clsUnitGroup objUnit, Field FieldField);

        private bool m_blnExecuted = false;
        public bool blnExecuted { get { return m_blnExecuted; } }

        public string CommandId { get; set; }
        public abstract void Execute();
        protected void markExecute()
        {
            Register();
            m_blnExecuted = true;
            this.raiseControllerEvent();
        }

        public void Register()
        {
            this.m_objUnitToCommand.aktCommand = this;
            
        }

        public CommandInfo getInfo()
        {
            CommandInfo nfo = new CommandInfo();
            nfo.commandId = this.CommandId;
            nfo.actingUnitId = this.m_objUnitToCommand.Id;
            nfo.commandType = this.GetType().ToString();
            nfo.strInfo = this.strInfo;
            nfo.isActive = (this.m_objUnitToCommand.aktCommand == this) ? true : false;
            return nfo;
        }

    }
}
