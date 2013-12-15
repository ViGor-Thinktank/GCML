
function CampaignInfo() {
    var self = this;
    self.campaignId = "";
    self.campaignName = "";
    self.fieldDimension = [];

    self.sektorField = ko.observableArray();
    self.unitList = ko.observableArray();

    self.hoveredSektor = ko.observable();
    self.selectedSektor = ko.observable();
}

function CampaignViewModel() {
    var self = this;
    self.campaignList = ko.observableArray();

    self.loadedCampaign = ko.observable();
    self.loadedCampaignInfo = ko.observable();      // TODO: Auf CampaignInfo umstellen (für UnitList etc)

    self.selectedSektor = ko.observable();
    self.debug = ko.observable();

    self.fieldWidth = ko.observable();
    self.fieldHeight = ko.observable();
    self.showcolor = 'green';

    // Funktionen
    //self.addCampaignInfo = function () {
    //    var cmp = new CampaignInfo("-1", "New campaign");
    //    self.campaignList.push(cmp);
    //};

    //
    // Funktionen 
    //
    self.loadCampaignList = function () {
        self.campaignList.removeAll();
        $.getJSON("/api/campaign", function (data) {
            self.debug(ko.toJSON(data));
            self.campaignList(data.campaignList);
        });
    };

    self.loadCampaign = function (campaign) {
        self.debug(ko.toJSON(campaign));
        self.loadedCampaign(campaign);

        self.fieldWidth(campaign.sektorField[0].length * 100 + 100);
        self.fieldHeight(campaign.sektorField.length * 100 + 50);

        self.debug(self.fieldWidth() + " # " + self.fieldHeight());

        self.loadAllUnits();

        $("#selectable").selectable();
    };

    self.loadAllUnits = function () {
        $.getJSON("/api/unit", {
            campaignId: self.loadedCampaign().campaignId
        }, function (data) {
            self.debug(ko.toJSON(data));
        });
    };

    //
    // Events
    //
    self.selectSektor = function (sektor) {
        //self.debug(sektor.sektorId);
    };

    // Init
    self.loadCampaignList();

    //self.loadCampaign(self.campaignList[0]);
}


ko.applyBindings(new CampaignViewModel());





