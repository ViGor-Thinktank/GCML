﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericCampaignMasterLib;
using GenericCampaignMasterModel;

namespace GcmlDataAccess
{

    /// <summary>
    /// GcmlDataManager liefert den Zugriff auf alle Spieler und Kampagnen. Liefert immer
    /// den CampaignController mit dem aktuell gültigen EngineState. Die eigentliche Speicherung
    /// erfolgt in ICampaignDatabase und IPlayerDatabase und ist damit unabhängig von der 
    /// eigentlichen Datenbank.
    /// </summary>
    public class GcmlDataManagerRaptorDb : IGcmlDataManager
    {
        CampaignDbRaptor campaignDb;
        PlayerDbRaptor playerDb;

        public GcmlDataManagerRaptorDb(CampaignDbRaptor _campaignDb, PlayerDbRaptor _playerDb)
        {
            this.campaignDb = _campaignDb;
            this.playerDb = _playerDb;
        }

        ~GcmlDataManagerRaptorDb()
        {
            this.campaignDb.close();
            this.playerDb.close();
        }

        // Das Interface stellt die Abwärtskompatibilität sicher.
        #region IGcmlDataManager
        public List<string> getRunningCampaignIds()
        {
            return campaignDb.getAllCampaignKeys();
        }

        public CampaignController getController(string campaignId)
        {
            CampaignState state = campaignDb.getCampaignStateForCampaign(campaignId);
            CampaignEngine engine = CampaignEngine.restoreFromState(state);
            CampaignController controller = new CampaignController(engine);
            return controller;
        }

        public PlayerInfo getPlayerByName(string playername)
        {
            return playerDb.getPlayerByName(playername);
        }

        public string getPlayerId(string playername)
        {
            PlayerInfo p = playerDb.getPlayerByName(playername);
            return p.playerId;
        }

        public PlayerInfo getPlayer(string id)
        {
            return playerDb.getPlayer(id);
        }

        public Dictionary<string, PlayerInfo> getPlayerList()
        {
            return playerDb.getAllPlayers().ToDictionary(x => x.playerId, x => x);
        }

        public List<CampaignInfo> getRunningPlayerCampaigns(string playerid)
        {
            PlayerInfo p = playerDb.getPlayer(playerid);
            return campaignDb.getCampaignsForPlayer(p.playerId);
        }

        public string createNewCampaign(string campaignname, clsSektorKoordinaten fielddim)
        {
            return campaignDb.createNewCampaign(campaignname, fielddim);   
        }

        public string createNewCampaign(string playerid, string campaignname, clsSektorKoordinaten fielddim, int anzUnitsPerPlayer)
        {
            PlayerInfo p = playerDb.getPlayer(playerid);
            if (p == null)
                return null;

            string campaignId = campaignDb.createNewCampaign(campaignname, fielddim);
            return campaignId;

            // TODO: Player hinzufügen
        }

        public void safeCampaignState(CampaignState state)
        {
            campaignDb.saveGameState(state);
        }

        public void addPlayer(PlayerInfo player)
        {
            playerDb.addPlayer(player);
        }

        public void updateCampaign(CampaignInfo cmpinfo)
        {
            string id = cmpinfo.campaignId;
            CampaignState state = campaignDb.getCampaignStateForCampaign(id);

            state.CampaignName = cmpinfo.campaignName;

            if (cmpinfo.ListPlayerInfo != null)
            {
                foreach (var pinfo in cmpinfo.ListPlayerInfo)
                {
                    if (!playerDb.getAllPlayers().Exists(p => p.playerId == pinfo.playerId))
                        playerDb.addPlayer(pinfo);

                    if (!state.ListPlayers.Exists(pl => pl.playerId == pinfo.playerId))
                    {
                        state.ListPlayers.Add(pinfo);
                    }

                }
            }

            campaignDb.saveGameState(state);
        }

        #endregion

    }
}
