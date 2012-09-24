using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterLib
{
    public class ResourceHandler
    {
        // Todo: Alle IResourceable Objekte verwalten
        List<Resource<clsUnit>> lstUnitResources = new List<Resource<clsUnit>>(); 
        
        public void addRessourcableObject(Player owner, IResourceable resourceObject)
        {
            Type resourceType = resourceObject.GetType();
            
            if(resourceType == typeof(clsUnit))
            {
                clsUnit unit = resourceObject as clsUnit;
                Resource<clsUnit> resUnit = new Resource<clsUnit>();
                resUnit.Owner = owner;
                resUnit.resourceId = Guid.NewGuid();
                resUnit.resourceHandler = this;
                lstUnitResources.Add(resUnit);
            }
        }
 





        /// <summary>
        /// Wird von beim Ausführen eines ICommands durch ResourceCommandDecorated aufgerufen. 
        /// </summary>
        /// <param name="resourceId">Eindeutige ID der Resource</param>
        public void onResourceIsUsed(Guid resourceId)
        {


        }



    
public  Type Resource { get; set; }}
}
