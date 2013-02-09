(function ($, window, undefined) {
    var refreshInterval = 5 * 60 * 1000;
    var tickInterval = 1 * 1000;

    function EnvironmentViewModel(accessible, name, description, url) {
        var self = {};

        self.lastUpdated = ko.observable(Date.now());
        self.accessible = ko.observable(accessible);
        self.name = ko.observable(name);
        self.description = ko.observable(description);
        self.url = ko.observable(url);
        self.loading = ko.observable(true);

        var ticker = ko.observable(0);
        self.lastUpdatedString = ko.computed(function () {
            if (ticker() > 0) {
                return $.timeago(self.lastUpdated());
            }
            return '';
        });
        setInterval(function () {
            ticker(ticker() + 1)
        }, tickInterval);

        self.icon = ko.computed(function () {
            if (self.loading()) { // Not actually false, thus must be "falsey" (null/undefined)
                return 'icon-spinner icon-spin icon-4x';
            } else if (self.accessible() === true) {
                return 'icon-ok-sign icon-4x';
            } else if (self.accessible() === false) {
                return 'icon-minus-sign icon-4x';
            } else {
                return '';
            }
        });

        self.load = function () {
            self.loading(true);
            $.getJSON('/api/v1/environments/' + self.name(), function (data, status, xhr) {
                if (data) {
                    self.accessible(data.AccessibleFromDashboard);
                    self.name(data.Name);
                    self.description(data.Description);
                    self.url(data.Url);
                } else {
                    alert('Error loading data from server: ' + status);
                }
                self.loading(false);
                self.lastUpdated(Date.now());
            });
        }

        return self;
    }

    function RowViewModel() {
        var self = {};

        self.environments = ko.observableArray([]);

        return self;
    }

    function PageViewModel() {
        var self = {};

        self.rows = ko.observableArray([]);
        self.loading = ko.observable(true);
        
        self.load = function () {
            $.getJSON('/api/v1/environments', function (data, status, xhr) {
                if (data) {
                    var currentRow = null;
                    self.rows.removeAll();
                    for (var i = 0; i < data.length; i++) {
                        if (currentRow == null) {
                            currentRow = new RowViewModel();
                            self.rows.push(currentRow);
                        }
                        var environment = new EnvironmentViewModel(data[i].AccessibleFromDashboard, data[i].Name, data[i].Description, data[i].Url);
                        currentRow.environments.push(environment);
                        setTimeout(environment.load, 0);
                        if (currentRow.environments().length == 3) {
                            currentRow = null;
                        }
                    }
                } else {
                    alert('Error loading data from the server: ' + status);
                }
                self.loading(false);
            });
            setTimeout(self.load, refreshInterval);
        }

        return self;
    }

    $(function () {
        $('time').timeago();
        var viewModel = new PageViewModel();
        ko.applyBindings(viewModel);
        setTimeout(viewModel.load, 0);
    });
})(jQuery, window);