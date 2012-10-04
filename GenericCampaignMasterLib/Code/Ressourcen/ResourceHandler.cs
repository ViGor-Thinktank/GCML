using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterLib
{
    public class ResourceHandler
    {
        // Todo: Alle IResourceable Objekte verwalten
        List<Resource<clsUnitType>> m_lstUnitResources = new List<Resource<clsUnitType>>();
        Dictionary<Guid, ResourceCommandDecorated> m_dicResourcesForExecution = new Dictionary<Guid, ResourceCommandDecorated>();  // Das Command das für eine Resource am Ende der Runde ausgeführt wird


        public string addRessourcableObject(Player owner, IResourceable resourceObject)
        {
            string resourceId = "";
            Type resourceType = resourceObject.GetType();
            if (resourceType == typeof(clsUnitType))
            {
                // Todo: Anstatt eine neue Instanz erstellen evtl. in der Ressource nur Info-Objekte speichern?
                Resource<clsUnitType> resUnit = new Resource<clsUnitType>();
                resUnit.Owner = owner;
                resUnit.resourceId = Guid.NewGuid();
                resUnit.resourceHandler = this;
                m_lstUnitResources.Add(resUnit);
                resourceId = resUnit.resourceId.ToString();
            }

            return resourceId;
        }

        /// <summary>
        /// Wird von beim Ausführen eines ICommands durch ResourceCommandDecorated aufgerufen. 
        /// </summary>
        /// <param name="resourceId">Eindeutige ID der Resource</param>
        public void onResourceIsUsed(Guid resourceId)
        {


        }

        internal List<ICommand> getResourceCommands(string resourceId)
        {
            List<ICommand> result = new List<ICommand>();
            Guid resid = new Guid(resourceId);

            var query = from r in m_lstUnitResources
                        where r.resourceId == resid
                        select r;

            // Todo Handler registrieren
            if (query.Count() > 0)
                result = query.First().getResourceCommands();

            return result;
        }


        /// <summary>
        /// Liefert ResourceInfo Objekte für alle verwalteten Ressourcen.
        /// </summary>
        /// <returns></returns>
        public List<ResourceInfo> getResourceInfo()
        {
            List<ResourceInfo> result = new List<ResourceInfo>();
            foreach (var res in m_lstUnitResources)
            {
                ResourceInfo resinf = res.getInfo();
                result.Add(resinf);
            }

            return result;
        }

        internal void RegisterResourceForExecution(Guid m_resourceId, ResourceCommandDecorated resourceCommandDecorated)
        {
            m_dicResourcesForExecution[m_resourceId] = resourceCommandDecorated;
        }


        public void CampaignController_onTick()
        {
            foreach (var res in m_dicResourcesForExecution)
            {
                Guid id = res.Key;
                ICommand cmd = res.Value as ICommand;

                cmd.Execute();
                

                var remRes = (from r in m_lstUnitResources
                             where r.resourceId == id
                             select r).First();

                m_lstUnitResources.Remove(remRes);
            }

            m_dicResourcesForExecution.Clear();
        }

    }
}
