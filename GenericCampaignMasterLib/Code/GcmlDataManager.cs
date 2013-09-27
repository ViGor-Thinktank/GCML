﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericCampaignMasterLib;
using GenericCampaignMasterModel;

namespace GenericCampaignMasterLib
{

    /// <summary>
    /// GcmlDataManager liefert den Zugriff auf alle Spieler und Kampagnen. Liefert immer
    /// den CampaignController mit dem aktuell gültigen EngineState. Die eigentliche Speicherung
    /// erfolgt in ICampaignDatabase und IPlayerDatabase und ist damit unabhängig von der 
    /// eigentlichen Datenbank.
    /// </summary>
    public class GcmlDataManager : IGcmlDataManager
    {
        ICampaignDatabase campaignDb;
        IPlayerDatabase playerDb;

        public GcmlDataManager(ICampaignDatabase _campaignDb, IPlayerDatabase _playerDb)
        {
            this.campaignDb = _campaignDb;
            this.playerDb = _playerDb;
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

        public string getPlayerId(string playername)
        {
            throw new NotImplementedException();

        }

        public Player getPlayer(string id)
        {
            return playerDb.getPlayer(id);
        }

        public Dictionary<string, GenericCampaignMasterModel.Player> getPlayerList()
        {
            throw new NotImplementedException();
        }

        public List<string> getRunningPlayerCampaigns(string playerid)
        {
            throw new NotImplementedException();
        }

        public string createNewCampaign(string playerid, string fielddimension)
        {
            throw new NotImplementedException();
        }

        public string createNewCampaign(string playerid, string campaignname, GenericCampaignMasterModel.clsSektorKoordinaten fielddim, int anzUnitsPerPlayer)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
