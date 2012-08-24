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

        public List<string> getPlayerCampaigns(string playerid)
        {
            return GcmlDataManager.Instance.getRunningPlayerCampaigns(playerid);
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
            BaseUnit unit = controller.getUnit(unitid);
            List<ICommand> cmdlist = controller.getCommandsForUnit(unit);
            foreach (ICommand icmd in cmdlist)
                result.Add(icmd.getInfo());

            return result;
        }

        public string createNewCampaign(string playerid, string fielddimension)
        {
            return GcmlDataManager.Instance.createNewCampaign(playerid, fielddimension);
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
            cmd.Execute();
        }

        public void addUnitToField(string campaignid, string unit, string targetsektor)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

}
