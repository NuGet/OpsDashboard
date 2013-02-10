(function ($, window, undefined) {
    var refreshInterval = 5 * 60 * 1000;
    var tickInterval = 1 * 60 * 1000;
    var nextId = 0;

    function PingResultViewModel(title, detail, url, result) {
        var self = {};

        self.title = ko.observable(title);
        self.detail = ko.observable(detail);
        self.url = ko.observable(url);
        self.result = ko.observable(result);

        self.resultMessage = ko.computed(function () {
            if (self.result()) {
                return 'Success';
            } else {
                return 'Failure';
            }
        });

        self.statusMessage = ko.computed(function () {
            return self.title() + ': ' + self.detail();
        });

        return self;
    }

    function EnvironmentViewModel(accessible, title, name, description, url, detailsUrl) {
        var self = {};

        self.domId = 'env_' + nextId++;
        self.lastUpdated = ko.observable(Date.now());
        self.title = ko.observable(title);
        self.name = ko.observable(name);
        self.description = ko.observable(description);
        self.url = ko.observable(url);
        self.loading = ko.observable(true);
        self.pingResults = ko.observableArray([]);

        self.firstFailed = ko.computed(function () {
            var ret = _.find(self.pingResults(), function (r) { return !r.result(); });
            return ret;
        });

        self.detailsUrl = ko.computed(function () {
            return model.detailsUrlTemplate.replace(/__ENVNAME__/, self.name());
        });

        self.statusDetail = ko.computed(function () {
            var message = '';
            _.forEach(self.pingResults(), function (r) {
                if (!r.result()) {
                    message += r.statusMessage() + '\n';
                }
            });
            if (message.length == 0) {
                message = 'All Systems Go!';
            }
            return message;
        });

        self.isUp = ko.computed(function () {
            return _.every(self.pingResults(), function (r) { return r.result(); });
        });
        self.isDown = ko.computed(function () {
            return _.every(self.pingResults(), function (r) { return !r.result(); });
        });

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
            if (self.loading()) {
                return 'icon-spinner icon-spin icon-4x';
            } else if (self.isUp()) {
                return 'icon-ok-sign icon-4x';
            } else if (self.isDown()) {
                return 'icon-minus-sign icon-4x';
            } else {
                return 'icon-warning-sign icon-4x';
            }
        });

        self.load = function () {
            self.loading(true);
            $.getJSON('/api/v1/environments/' + self.name(), function (data, status, xhr) {
                if (data) {
                    self.title(data.title);
                    self.name(data.name);
                    self.description(data.description);
                    self.url(data.url);
                    self.pingResults.removeAll();
                    for (var i = 0; i < data.pingResults.length; i++) {
                        self.pingResults.push(
                            new PingResultViewModel(
                                data.pingResults[i].name, 
                                data.pingResults[i].detail, 
                                data.pingResults[i].target, 
                                data.pingResults[i].success));
                    }
                } else {
                    alert('Error loading data from server: ' + status);
                }
                self.loading(false);
                self.lastUpdated(Date.now());
                ticker(1);
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
                        var environment = new EnvironmentViewModel(
                            data[i].accessibleFromDashboard,
                            data[i].title,
                            data[i].name,
                            data[i].description,
                            data[i].url,
                            data[i].detailsUrl);
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

    var model = {};
    $(function () {
        model = $(document.body).data().model;
        $('time').timeago();
        var viewModel = new PageViewModel();
        ko.applyBindings(viewModel);
        setTimeout(viewModel.load, 0);
    });
})(jQuery, window);