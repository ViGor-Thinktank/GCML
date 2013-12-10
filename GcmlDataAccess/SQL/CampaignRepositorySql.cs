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
    public class CampaignRepositorySql : ICampaignRepository
    {
        private SqlCeConnection conn;
        private SqlCeDataAdapter daCampaignStates;
        private SqlCeDataAdapter daPlayers;
        private GcmlData ds;

        public CampaignRepositorySql()
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
            daCampaignStates.Fill(ds.CampaignStates);
            daPlayers.Fill(ds.Player);
        }

        public ICampaignController getCampaignController(string campaignId)
        {
            CampaignController result = null;
            var stateRow = ds.CampaignStates.FirstOrDefault(r => r.CampaignId == campaignId);
            if (stateRow != null)
            {
                result = getControllerFromStateRow(stateRow);
            }

            return result;
        }

        public ICampaignController createNewCampaign(CampaignInfo info)
        {
            CampaignController controller = CampaignEngine.createNewCampaign(info);
            safeCampaignState(controller);
            return controller;
        }

        public bool safeCampaignState(ICampaignController controller)
        {
            CampaignState state = controller.CampaignEngine.getState();
            string stateStr = state.ToString();

            var stateRow = ds.CampaignStates.FirstOrDefault(r => r.CampaignId == state.CampaignId);
            if (stateRow == null)
                ds.CampaignStates.AddCampaignStatesRow(state.CampaignId, state.CampaignName, stateStr, DateTime.Now);
            else
                stateRow.CampaignData = stateStr;

            foreach (var p in state.ListPlayers)
            {
                var playerrow = ds.Player.FindByPlayerName(p.playerName);
                if (playerrow == null)
                    ds.Player.AddPlayerRow(p.playerName, p.playerName);
            }

            daCampaignStates.Update(ds.CampaignStates);
            daPlayers.Update(ds.Player);

            ds.AcceptChanges();
            return true;
        }

        public PlayerInfo getPlayer(string playername)
        {
            PlayerInfo result = new PlayerInfo();
            var playerrow =  ds.Player.FindByPlayerName(playername);
            if (playerrow != null)
            {
                result = new PlayerInfo();
                result.playerName = playerrow.PlayerName;
            }

            return result;
        }

        public List<PlayerInfo> getPlayers()
        {
            return ds.Player.Select(p => new PlayerInfo() {playerName = p.PlayerName}).ToList();
        }

        public List<CampaignInfo> getCampaignsForPlayer(string playername)
        {
            List<CampaignInfo> result = new List<CampaignInfo>();
            PlayerInfo player = getPlayer(playername);
            if (player != null)
            {
                // Todo: Optimierung CampaignInfos mit Playern beim Laden erzeugen.
                foreach (var stater in ds.CampaignStates)
                {
                    CampaignController ctrl = getControllerFromStateRow(stater);
                    if(ctrl.CampaignEngine.lisPlayers.Find(p => p.Playername == playername) != null)
                        result.Add(ctrl.Campaign_getInfo());
                }
            }

            return result;
        }

        public void Dispose()
        {
            conn.Close();
        }


        private CampaignController getControllerFromStateRow(GcmlData.CampaignStatesRow stateRow)
        {
            CampaignController result;
            string statedata = stateRow.CampaignData;
            CampaignState state = CampaignState.FromString(statedata);
            CampaignEngine engine = CampaignEngine.restoreFromState(state);
            result = new CampaignController(engine);
            return result;
        }
    }
}
