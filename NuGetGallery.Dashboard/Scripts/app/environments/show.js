(function ($, window, undefined) {
    
    function StatusEntryViewModel(job, processId, timestamp, duration, status, message) {
        var self = {};

        self.job = ko.observable(job);
        self.processId = ko.observable(processId);
        self.timestamp = ko.observable(timestamp);
        self.duration = ko.observable(duration);
        self.status = ko.observable(status);
        self.message = ko.observable(message);

        return self;
    }

    function JobViewModel(name, data) {
        var self = {};

        self.name = ko.observable(name);
        self.statuses = ko.observableArray(_.map(data, function(item) {
            return new StatusEntryViewModel(self, item.pid, item.at, item.duration, item.status, item.message);
        }));

        self.latestStatus = ko.computed(function() {
            var arr = self.statuses();
            if (arr.length > 0) {
                return arr[arr.length - 1];
            }
            return null;
        });

        return self;
    }

    function EnvironmentViewModel(name, title) {
        var self = {};

        self.name = ko.observable(name);
        self.title = ko.observable(title);
        self.jobs = ko.observableArray([]);

        self.loading = ko.observable(true);

        self.load = function () {
            self.loading(true);
            $.getJSON('/api/v1/opsstatus/' + name, function(data, status, xhr) {
                if (data && xhr.status == 200) {
                    self.jobs.removeAll();
                    for (var key in data) {
                        if (data.hasOwnProperty(key)) {
                            self.jobs.push(new JobViewModel(key, data[key]));
                        }
                    }
                }
                self.loading(false);
            });
        };

        return self;
    }

    var model = {};
    var viewModel = {};
    $(function () {
        model = $(document.body).data().model;
        viewModel = new EnvironmentViewModel(model.environmentName, model.environmentTitle);
        app.bind(viewModel);
        setTimeout(viewModel.load, 0);
    });
})(jQuery, window);