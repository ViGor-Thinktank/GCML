using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using GcmlDataAccess.SQL;
using GenericCampaignMasterLib;
using GenericCampaignMasterModel;

namespace GcmlDataAccess
{
    public class GcmlDataAccessSQL : IGcmlDataAccess, IDisposable
    {
        private SqlCeConnection conn;
        private SqlCeDataAdapter daCampaignStates;
        private SqlCeDataAdapter daPlayers;
        private GcmlData ds;

        public GcmlDataAccessSQL()
        {
            string selCampaigns = "select * from CampaignStates";
            string selPlayers = "select * from Player";
            string cs = ConfigurationManager.ConnectionStrings["GcmlDbContext"].ConnectionString;
            conn = new SqlCeConnection(cs);
            
            daCampaignStates = new SqlCeDataAdapter(selCampaigns, conn);
            daPlayers = new SqlCeDataAdapter(selPlayers, conn);

            SqlCeCommandBuilder cpcampaigns = new SqlCeCommandBuilder(daCampaignStates);
            SqlCeCommandBuilder cpplayers = new SqlCeCommandBuilder(daPlayers);

            ds = new GcmlData();
            daCampaignStates.Fill(ds);
            daPlayers.Fill(ds);
        }

        public CampaignController getCampaignController(string campaignId)
        {
            CampaignController result = null;
            var stateRow = ds.CampaignStates.FirstOrDefault(r => r.CampaignId == campaignId);
            if (stateRow != null)
            {
                string statedata = stateRow.CampaignData;
                CampaignState state = CampaignState.FromString(statedata);
                CampaignEngine engine = CampaignEngine.restoreFromState(state);
                result = new CampaignController(engine);
            }

            return result;
        }


        public string createNewCampaign(CampaignInfo info)
        {
            string result = "";
            CampaignState state = CampaignEngine.createNewCampaign(info);
            CampaignEngine engine = CampaignEngine.restoreFromState(state);
            CampaignController controller = new CampaignController(engine);

            if (safeCampaignState(controller))
                result = state.CampaignId;

            return result;
        }

        public bool safeCampaignState(CampaignController controller)
        {
            CampaignState state = controller.CampaignEngine.getState();
            string stateStr = state.ToString();

            var stateRow = ds.CampaignStates.FirstOrDefault(r => r.CampaignId == state.CampaignId);
            if (stateRow != null)
            {
                stateRow.CampaignData = stateStr;
                
            }
            else
            {
                ds.CampaignStates.AddCampaignStatesRow(state.CampaignId, state.CampaignName, stateStr, DateTime.Now);
            }

            daCampaignStates.Update(ds.CampaignStates);
            daPlayers.Update(ds.Player);

            ds.AcceptChanges();
            return true;
        }

        public GenericCampaignMasterModel.PlayerInfo getPlayer(string playername)
        {
            throw new NotImplementedException();
        }

        public List<GenericCampaignMasterModel.PlayerInfo> getPlayers()
        {
            throw new NotImplementedException();
        }

        public List<GenericCampaignMasterModel.CampaignInfo> getCampaignsForPlayer(string playername)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
