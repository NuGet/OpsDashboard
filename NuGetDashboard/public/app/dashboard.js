(function ($, global, undefined) {
    // Prototypal Inheritance Helper (see http://javascript.crockford.com/prototypal.html)
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

    function TaskRunViewModel(data) {
        var self = {};

        self.pid = ko.observable((data.pid && parseInt(data.pid)) || 0);
        self.at = ko.observable((data.at && new Date(data.at)));
        self.duration = ((data.duration && parseInt(data.duration)) || 0);
        self.status = ko.observable(data.status || '');
        self.message = ko.observable(data.message || '');

        self.displayDate = ko.computed(function () {
            return self.at().toUTCString();
        });

        return self;
    }

    function TaskViewModel(name, runs) {
        var self = {};

        self.jobName = name;
        self.runs = ko.observableArray($.map(runs, function(elem) { return new TaskRunViewModel(elem); }));

        return self;
    }

    function OpsViewModel(opsRootUrl) {
        var self = {};

        self.jobs = ko.observableArray([]);
        self.isLoading = ko.observable(true);

        self.refresh = function () {
            self.isLoading(true);
            $.get('/api/v1/jobs')
             .fail(function (xhr) {
                 alert('Error talking to the service. Try refreshing the page');
             })
             .done(function (data) {
                 if (typeof data === "string") {
                     data = JSON.parse(data); // Temporary, until I fix up the api
                 }
                 var items = [];
                 for(var key in data) {
                     if(data.hasOwnProperty(key)) {
                         items.push(new TaskViewModel(key, data[key]));
                     }
                 }
                 self.jobs(items);
                 self.isLoading(false);
             });
        }

        return self;
    }
    

    var pages = {};
    
    function openPage(pageName) {
        var page = pages[pageName];
        $('div.page.active').removeClass('active');
        if (!page) {
            alert('No such view: ' + pageName);
        }

        $(page.view).addClass('active');

        // Update Menu
        $('ul.nav li.active').removeClass('active');
        $('ul.nav li[data-menu=\'' + pageName + '\']').addClass('active');

        return page;
    }

    var AppRouter = Backbone.Router.extend({
        routes: {
            '': 'packages',
            'app-packages': 'packages',
            'ops': 'ops'
        },

        ops: function() {
            var page = openPage('ops');

            if (!page.model) {
                page.model = new OpsViewModel($(document.body).data().opsroot);
                ko.applyBindings(page.model, page.view);
            }
            setTimeout(page.model.refresh, 0);
        },

        packages: function () {
            // Select the view
            var page = openPage('app-packages');
            
            // Create the view model and bind it if necessary
            if (!page.model) {
                page.model = new PackagesViewModel();
                ko.applyBindings(page.model, page.view);
            }
            setTimeout(page.model.refresh, 0);
        }
    });

    $(function () {
        // Load pages table
        $('div.page').each(function() {
            var name = $(this).data().view;
            pages[name] = {
                view: this
            };
        })

        // Start the router
        var router = new AppRouter();
        Backbone.history.start({ pushState: true });

        // Hijack internal links
        $(document.body).on('click', 'a:not([data-link="exterior"],[href="#"],[href^="http:"],[href^="https:"],[href^="//"]),a[data-link="interior"]', function (evt) {
            // Send the URL to the router
            evt.preventDefault();
            router.navigate($(this).attr('href'), { trigger: true });
        });
    });
})(jQuery, window);