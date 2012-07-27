using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using GenericCampaignMasterLib;

namespace GcmlWebService
{
    // HINWEIS: Mit dem Befehl "Umbenennen" im Menü "Umgestalten" können Sie den Klassennamen "CampaignMasterService" sowohl im Code als auch in der Konfigurationsdatei ändern.
    public class CampaignMasterService : ICampaignMasterService
    {
        private Dictionary<string, ICampaignDatabase> m_dictRunningCampaigns = new Dictionary<string, ICampaignDatabase>();
        private string strStorepath = Environment.CurrentDirectory;


        #region ICampaignMasterService Member

        public string getPlayer(string playername)
        {
            throw new NotImplementedException();
        }

        public List<string> getPlayerCampaigns(string playerid)
        {
            throw new NotImplementedException();
        }

        public string getFieldKoord(string campaignid)
        {
            throw new NotImplementedException();
        }

        public string getSektor(string campaignid, string sektorkoord)
        {
            throw new NotImplementedException();
        }

        public List<string> getSektorList(string campaignid)
        {
            throw new NotImplementedException();
        }

        public string getUnit(string campaignid, string unitid)
        {
            throw new NotImplementedException();
        }

        public List<string> getUnitCollisions(string campaignid)
        {
            throw new NotImplementedException();
        }

        public List<string> getCommandsForUnit(string campaignid, string unitid)
        {
            throw new NotImplementedException();
        }

        public string createNewCampaign(string playerid, string fielddimension)
        {
            throw new NotImplementedException();
        }

        public void addPlayerToCampaign(string playerid, string campaignid)
        {
            throw new NotImplementedException();
        }

        public void executeCommand(string campaignid, string command)
        {
            throw new NotImplementedException();
        }

        public void addUnitToField(string campaignid, string unit, string targetsektor)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
