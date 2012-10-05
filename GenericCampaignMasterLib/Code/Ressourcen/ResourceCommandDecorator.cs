using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterLib
{
    /// <summary>
    /// Erweitert eine ICommand-Klasse: Benachrichtigt den RessourceHandler beim Execute 
    /// mit der ResourceID, bzw. registriert das ICommand für die Ausführung durch den Handler.
    /// </summary>
    class ResourceCommandDecorated : ICommand
    {
        ResourceHandler m_resourceHandler;
        ICommand m_command;
        Guid m_resourceId;
       
        public ICommand InnerCommand 
        {
            get { return m_command; }
        }

        public ResourceCommandDecorated(ICommand command, ResourceHandler handler, Guid resourceId)
        {
            m_command = command;
            m_resourceHandler = handler;
            m_resourceId = resourceId;
        }

        #region ICommand-Member
        public void Execute()
        {
            m_command.Execute();
        }

        public void Register()
        {
            m_resourceHandler.RegisterResourceForExecution(m_resourceId, this);
        }

        public string strInfo
        {
            get { return m_command.strInfo; }
        }

        public bool blnExecuted
        {
            get { return m_command.blnExecuted; }
        }

        public string CommandId
        {
            get
            {
                return m_command.CommandId;
            }
            set
            {
                m_command.CommandId = value;
            }
        }

        public CommandInfo getInfo()
        {
            return m_command.getInfo();
        }

        #endregion
    }
}
