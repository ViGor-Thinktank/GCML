﻿
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
    self.selectedSektor = ko.observable();
    self.debug = ko.observable();

    // Funktionen
    //self.addCampaignInfo = function () {
    //    var cmp = new CampaignInfo("-1", "New campaign");
    //    self.campaignList.push(cmp);
    //};

    self.loadCampaignList = function() {
        self.campaignList.removeAll();
        $.getJSON("/api/campaign", function (data) {
            self.debug(ko.toJSON(data));
            self.campaignList(data.campaignList);
        });
    };

    self.loadCampaign = function (campaign) {
        self.debug(ko.toJSON(campaign));
        self.loadedCampaign(campaign);

        $(".sektor").selectable();
    };
    

    self.selectSektor = function (sektor) {
        //alert("sektor");
        self.debug(sektor.sektorId);
    };
    
    // Init
    self.loadCampaignList();

    self.loadCampaign(self.campaignList[0]);
}


ko.applyBindings(new CampaignViewModel());





