using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericCampaignMasterModel.Commands;

namespace GenericCampaignMasterModel
{
    public class ResourceHandler
    {
        // Todo: Alle IResourceable Objekte verwalten
        List<Resource<clsUnitType>> m_lstUnusedUnitResources = new List<Resource<clsUnitType>>();
        List<Resource<clsUnitType>> m_lstUsedUnitResources = new List<Resource<clsUnitType>>();
        Dictionary<Guid, ResourceCommandDecorated> m_dicResourcesForExecution = new Dictionary<Guid, ResourceCommandDecorated>();  // Das Command das für eine Resource am Ende der Runde ausgeführt wird

        public Stack<string> CreatedUnitIds = new Stack<string>();

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
                m_lstUnusedUnitResources.Add(resUnit);
                resourceId = resUnit.resourceId.ToString();
            }

            return resourceId;
        }

        // Eigentlich überflüssig da der Tick() Event des Controllers 
        // behandelt wird. --> kann raus.
        public void onResourceIsUsed(Guid resourceId)
        {
        }

        public List<ICommand> getResourceCommands(string resourceId)
        {
            List<ICommand> result = new List<ICommand>();
            Guid resid = new Guid(resourceId);

            var query = from r in m_lstUnusedUnitResources
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
            foreach (var res in m_lstUnusedUnitResources)
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

        /// <summary>
        /// Wird vom CampaignController am Ende einer Runde ausgelöst.
        /// </summary>
        public void CampaignController_onTick()
        {
            foreach (var res in m_dicResourcesForExecution)
            {
                Guid id = res.Key;
                ResourceCommandDecorated cmd = res.Value as ResourceCommandDecorated;

                cmd.Execute();
                

                var remRes = (from r in m_lstUnusedUnitResources
                             where r.resourceId == id
                             select r).First();

                m_lstUnusedUnitResources.Remove(remRes);
                m_lstUsedUnitResources.Add(remRes);

                if (cmd.InnerCommand.GetType() == typeof(comPlaceUnit))
                {
                    comPlaceUnit pu = cmd.InnerCommand as comPlaceUnit;
                    string unitId = pu.UnitId;
                    this.CreatedUnitIds.Push(unitId);
                }
            }

            m_dicResourcesForExecution.Clear();
        }

    }
}
