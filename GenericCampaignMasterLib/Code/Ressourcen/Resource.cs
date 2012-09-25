using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterLib
{
    [Serializable()]
    public class Resource<T> where T : IResourceable , new()
    {
        private T m_resourceObject;
        public ResourceHandler resourceHandler { get; set; }     
        public Player Owner { get; set; }
        public Guid resourceId { get; set; }

        public Resource()
        {
            m_resourceObject = new T();   
        }

        public List<ICommand> getResourceCommands()
        {
            List<ICommand> resourceActions = m_resourceObject.getResourceCommands(Owner);
            
            // TODO
            // Decorator erstellen

            return resourceActions;
        }

        public ResourceInfo getInfo()
        {
            ResourceInfo nfo = new ResourceInfo();
            nfo.ownerId = Owner.Id;
            nfo.resourceId = resourceId.ToString();
            nfo.resourceableType = m_resourceObject.GetType().ToString();
            return nfo;
        }

    }
}
