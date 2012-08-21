using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using GenericCampaignMasterLib;

namespace GcmlWebService
{
    // HINWEIS: Mit dem Befehl "Umbenennen" im Menü "Umgestalten" können Sie den Schnittstellennamen "IService1" sowohl im Code als auch in der Konfigurationsdatei ändern.
    [ServiceContract]
    public interface ICampaignMasterService
    {
        [OperationContract]
        string getPlayerId(string playername);

        [OperationContract]
        List<string> getPlayerCampaigns(string playerid);

        [OperationContract]
        clsSektorKoordinaten getFieldKoord(string campaignid);

        [OperationContract]
        SektorInfo getSektor(string campaignid, clsSektorKoordinaten sektorkoord);

        [OperationContract]
        List<string> getSektorList(string campaignid);

        [OperationContract]
        UnitInfo getUnit(string campaignid, string unitid);

        [OperationContract]
        List<SektorInfo> getUnitCollisions(string campaignid);

        [OperationContract]
        List<CommandInfo> getCommandsForUnit(string campaignid, string unitid);

        [OperationContract]
        string createNewCampaign(string playerid, string fielddimension);

        [OperationContract]
        void addPlayerToCampaign(string playerid, string campaignid);

        [OperationContract]
        void executeCommand(string campaignid, CommandInfo command);

        [OperationContract]
        void addUnitToField(string campaignid, string unit, string targetsektor);
    }
}
