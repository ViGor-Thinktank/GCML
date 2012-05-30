using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace GenericCampaignMasterLib
{
    [Serializable()]
    public class CampaignState
    {
        MemoryStream stream = new MemoryStream();
        BinaryFormatter formatter = new BinaryFormatter();
        
        public CampaignState Save(CampaignEngine engine)
        {
            formatter.Serialize(stream, engine);
            return this;
        }
        
        public CampaignEngine Restore()
        {
            stream.Seek(0, SeekOrigin.Begin);
            CampaignEngine engine = (CampaignEngine)formatter.Deserialize(stream);
            return engine;
        }

    }
}
