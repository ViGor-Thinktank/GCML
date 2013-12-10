using System.Collections.Generic;
using GenericCampaignMasterModel;
using GenericCampaignMasterModel.Commands;

namespace GenericCampaignMasterLib
{
    public interface ICampaignController
    {
        CampaignEngine CampaignEngine { get; set; }
        string CampaignKey { get; }
        Field FieldField { get; }
        event Field.delStatus onStatus;
        event CampaignController.delTick onTick;
        event CampaignController.delTick onHasTicked;
        void Global_onStatus(string strText);
        void Tick();
        string GameState_saveCurrent();
        void GameState_restoreByKey(string key);
        Faction Faction_add(string strFactionName, List<clsUnitType> listUnitspawn);
        Player Player_getByID(string pID);
        Player Player_getByName(string strName);
        Player Player_add(string strPlayerName, Faction fac);
        List<Player> Player_getPlayerList();
        List<clsUnit> Player_getUnitsForPlayer(Player player);
        void Player_endRound(Player p);
        clsUnit Unit_createNew(string strPlayerID, int intUnitTypeID, string strSpawnSektorKoord = "");
        void Unit_Remove(string strPlayerID, string strUnitID);
        void Unit_RemoveSubunit(string strPlayerID, string strUnitID, int intSubUnitID);
        clsUnit Unit_getByID(string strUnitId);
        UnitInfo Unit_getInfoByID(string unitId);
        clsCommandCollection Unit_getCommandsForUnit(clsUnit unit);
        Sektor Unit_getSektorForUnit(clsUnit unit);
        void Command_onControllerEvent(clsEventData objEventData);
        ICommand Command_getByID(string commandId);
        Sektor Sektor_getByID(string sektorId);
        List<Sektor> Sektor_getUnitCollisions();
        Player Campaign_getStateForPlayer(string pID, string strState = "");
        CampaignInfo Campaign_getInfo();
        List<ResourceInfo> Ressource_getRessourcesForPlayer(Player player);
        void Ressource_add(ResourceInfo resInfo);
        List<ICommand> Ressource_getCommandsByID(string resourceId);
        int UnitType_addNew(clsUnitType newUnit);
        clsUnitType UnitType_getTypeByName(string strUnitName);
        clsUnitType UnitType_getTypeByID(int intUnitID);
    }
}