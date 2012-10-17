using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterModel
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
            
            // Anstatt der originalen Resource-Commands Decoratoren erzeugen.
            // Die Decorator-Objekte behalten die Eigenschaften des ICommands, benachrichtigen beim
            // Aufruf von Execute() aber zusätzlich den ResourceHandler mit der Methode onResourceIsUsed()
            // damit die Resource aus den verfügbaren Ressourcen entfernt wird.
            List<ICommand> resourceCmdDecorated = new List<ICommand>();
            foreach (ICommand cmd in resourceActions)
            {
                ResourceCommandDecorated cmdDec = new ResourceCommandDecorated(cmd, resourceHandler, resourceId);
                resourceCmdDecorated.Add(cmdDec);
            }
            
            return resourceCmdDecorated;
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
