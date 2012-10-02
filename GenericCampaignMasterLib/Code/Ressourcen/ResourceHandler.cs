using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterLib
{
    public class ResourceHandler
    {
        // Todo: Alle IResourceable Objekte verwalten
        List<Resource<clsUnit>> m_lstUnitResources = new List<Resource<clsUnit>>();

        public string addRessourcableObject(Player owner, IResourceable resourceObject)
        {
            string resourceId = "";
            Type resourceType = resourceObject.GetType();
            if (resourceType == typeof(clsUnit))
            {
                clsUnit unit = resourceObject as clsUnit;
                Resource<clsUnit> resUnit = new Resource<clsUnit>();
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
    }
}
