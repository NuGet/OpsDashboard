define(['app/modular', 'jquery', 'knockout'], function (Modular, $, ko) {
    function PackageViewModel(data) {
        var self = {};

        self.url = ko.observable(data.Url || '');
        self.commitHash = ko.observable(data.CommitHash || '');
        self.branch = ko.observable(data.Branch || '');
        self.commitInfo = ko.observable();

        self.name = ko.computed(function () {
            return 'Commit ' + self.commitHash() + ' from branch \'' + self.branch() + '\'';
        });

        return self;
    }

    function PackagesViewModel() {
        var self = {};

        self.isLoading = ko.observable(true);
        self.items = ko.observableArray([]);
        self.refresh = function () {
            self.isLoading(true);
            $.get('/api/v1/packages')
             .fail(function (xhr) {
                 alert('Error talking to the service. Try refreshing the page');
             })
             .done(function (data) {
                 var results = $.map(data, function (elem) { return new PackageViewModel(elem); })
                 self.items(results);
                 self.isLoading(false);
             });
        }

        return self;
    }


    return Modular.module({
        '': function () {
            var vm = new PackagesViewModel();
            Modular.showView('packages', vm);
            setTimeout(vm.refresh, 0);
        }
    });
});