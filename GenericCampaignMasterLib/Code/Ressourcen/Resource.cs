using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterLib
{
    [Serializable()]
    public class Resource<T> where T : IResourcable , new()
    {
        private T m_resourceObject;
        private ResourceHandler m_resourceHandler;      
        public Player Owner { get; set; }
        public Guid resourceId { get; set; }

        public Resource()
        {
            resourceId = Guid.NewGuid();
            m_resourceObject = new T();   
        }

        public List<ICommand> getResourceCommands()
        {
            List<ICommand> resourceActions = m_resourceObject.getResourceCommands(Owner);
            
            // TODO
            // Decorator erstellen

            return resourceActions;
        }

    }
}
