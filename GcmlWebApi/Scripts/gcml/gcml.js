$(function() {
    $.getJSON("/api/Campaign", function(data) {
        var viewModel = ko.mapping.fromJS(data);
        ko.applyBindings(viewModel);
    });
});