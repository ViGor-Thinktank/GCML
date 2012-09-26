using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using GenericCampaignMasterLib;
using System.Web.Script.Serialization;

namespace GcmlWebService
{
    public class CampaignMasterService : ICampaignMasterService
    {
        private JavaScriptSerializer m_serializer = new JavaScriptSerializer();
       
        #region ICampaignMasterService Member

        public string getPlayerId(string playername)
        {
            return GcmlDataManager.Instance.getPlayerId(playername);   
        }

        public PlayerInfo getPlayerInfo(string playerid)
        {
            Player player = GcmlDataManager.Instance.getPlayer(playerid);
            PlayerInfo nfo = player.getPlayerInfo();
            return nfo;
        }

        public List<string> getPlayerCampaigns(string playerid)
        {
            return GcmlDataManager.Instance.getRunningPlayerCampaigns(playerid);
        }

        public CampaignInfo getCampaignInfo(string campaignId)
        {
            CampaignController controller = GcmlDataManager.Instance.getController(campaignId);
            CampaignInfo nfo = controller.getCampaignInfo();
            return nfo;
        }

        public clsSektorKoordinaten getFieldKoord(string campaignid)
        {
            CampaignController controller = GcmlDataManager.Instance.getController(campaignid);
            clsSektorKoordinaten fieldKoord = controller.campaignEngine.FieldField.FieldDimension;
            return fieldKoord;
        }

        public SektorInfo getSektor(string campaignid, clsSektorKoordinaten sektorkoord)
        {
            CampaignController controller = GcmlDataManager.Instance.getController(campaignid);
            Sektor sektor = controller.campaignEngine.FieldField.get(sektorkoord);
            return sektor.getInfo();
        }

        public List<string> getSektorList(string campaignid)
        {
            throw new NotImplementedException();
        }

        public UnitInfo getUnit(string campaignid, string unitid)
        {
            CampaignController controller = GcmlDataManager.Instance.getController(campaignid);
            UnitInfo result = controller.getUnitInfo(unitid);
            return result;
        }

        public List<SektorInfo> getUnitCollisions(string campaignid)
        {
            List<string> lstStrSektorClollisions = new List<string>();
            CampaignController controller = GcmlDataManager.Instance.getController(campaignid);
            List<Sektor> lstsek = controller.getUnitCollisions();

            List <SektorInfo> collInfo = (from s in lstsek
                                         select s.getInfo()).ToList<SektorInfo>();
            return collInfo;
        }

        public List<CommandInfo> getCommandsForUnit(string campaignid, string unitid)
        {
            List<CommandInfo> result = new List<CommandInfo>();
            CampaignController controller = GcmlDataManager.Instance.getController(campaignid);
            clsUnit unit = controller.getUnit(unitid);
            List<ICommand> cmdlist = controller.getCommandsForUnit(unit);
            foreach (ICommand icmd in cmdlist)
                result.Add(icmd.getInfo());

            return result;
        }

        public string createNewCampaign(string playerid, string campaignname, int x, int y, int anzUnits)
        {
            clsSektorKoordinaten fielddim = new clsSektorKoordinaten(x, y);
            string newCampaignId = GcmlDataManager.Instance.createNewCampaign(playerid, campaignname, fielddim, anzUnits);
            return newCampaignId;
        }


        public void addPlayerToCampaign(string playerid, string campaignid)
        {
            CampaignController controller = GcmlDataManager.Instance.getController(campaignid);
            Player player = GcmlDataManager.Instance.getPlayer(playerid);
            controller.campaignEngine.addPlayer(player);
            controller.saveCurrentGameState();
        }

        public void executeCommand(string campaignid, CommandInfo command)
        {
            CampaignController controller = GcmlDataManager.Instance.getController(campaignid);
            ICommand cmd = controller.getCommand(command.commandId);
            
            //cmd.Execute();
            cmd.Register();
        }

        public void addUnitToField(string campaignid, string unit, string targetsektor)
        {
            throw new NotImplementedException();
        }

        
        public void endRoundForPlayer(string campaignid, string playerid)
        {
            CampaignController controller = GcmlDataManager.Instance.getController(campaignid);
            Player p = controller.getPlayer(playerid);
            controller.endRound(p);

        }

        public CommandInfo getCommandInfo(string campaignid, string commandId)
        {
            CampaignController controller = GcmlDataManager.Instance.getController(campaignid);
            ICommand cmd = controller.getCommand(commandId);
            CommandInfo cmdInfo = cmd.getInfo();
            return cmdInfo;
        }

        public List<ResourceInfo> getResourcesForPlayer(string campaignId, string playerId)
        {
            List<ResourceInfo> result = new List<ResourceInfo>();
            CampaignController controller = GcmlDataManager.Instance.getController(campaignId);

            return result;
        }

        public List<CommandInfo> getCommandsForResource(string campaignId, string resourceId)
        {
            List<CommandInfo> result = new List<CommandInfo>();

            return result;
        }

        public void addResource(string campaignId, ResourceInfo resinfo)
        {
            CampaignController controller = GcmlDataManager.Instance.getController(campaignId);
            controller.addResource(resinfo);
        }

        #endregion
    }

}
