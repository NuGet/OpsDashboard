// Configure require js
requirejs.config({
    // By default, load unprefixed modules from /public/lib
    baseUrl: '/public/lib',
    paths: {
        // Load "app/" prefixed modules from public/app
        app: '../app',

        // Versions used
        jquery: 'jquery-1.9.0',
        knockout: 'knockout-2.2.1'
    },
    shim: {
        'backbone': {
            deps: ['underscore', 'jquery'],
            exports: 'Backbone'
        },
        'backbone-subroute': {
            exports: 'Backbone'
        },
        'bootstrap': {
            deps: ['jquery']
        },
        'underscore': {
            exports: '_'
        }
    }
});

// Start the main app
requirejs([
    'jquery',
    'backbone',
    'knockout'
], function  ($, Backbone, ko) {
    function module(name, urlPrefix) {
        if (!urlPrefix) {
            urlPrefix = name;
        }
        return function () {
            require(['app/' + urlPrefix + '/index'], function (m) {
                m(name + '/');
            });
        }
    }

    var appViewModel = {
        firstName: ko.observable(),
        lastName: ko.observable(),
        userName: ko.observable(),
        menuItems: ko.observableArray([]),
        activeUrl: ko.observable(),

        // NOT used to secure data. The server secures data, but this flag lets us hide irrelevant UI from users who can't use it.
        isLoggedIn: ko.observable(false)
    };
    appViewModel.displayName = ko.computed(function () {
        return appViewModel && ('Hello, ' + appViewModel.firstName() + ' ' + appViewModel.lastName());
    });

    function MenuItemViewModel(title, url, icon, adminOnly) {
        var self = {};

        self.title = ko.observable(title);
        self.url = ko.observable(url);
        self.icon = ko.observable(icon);

        self.isActive = ko.computed(function() {
            return appViewModel.activeUrl() == self.url();
        });

        self.isVisible = ko.computed(function () {
            return !adminOnly || appViewModel.isLoggedIn();
        });

        return self;
    }

    appViewModel.menuItems.push(new MenuItemViewModel('Deployments', '/deployments/available', 'icon-cloud', true));

    // Schedule startup code
    $(function () {
        // Load user token if present
        var userToken = $(document.body).data().user;
        if (userToken) {
            appViewModel.firstName(userToken.FirstName);
            appViewModel.lastName(userToken.LastName);
            appViewModel.userName(userToken.UserName);
            appViewModel.isLoggedIn(true);
        }
        ko.applyBindings(appViewModel);

        // Set up the router
        var AppRouter = Backbone.Router.extend({
            routes: {
                '': 'home',
                'deployments/*subroute': 'deploymentsModule',
                'operations/*subroute': 'operationsModule',
                '*any': 'notfound'
            },
            deploymentsModule: module('deployments'),
            operationsModule: module('operations'),
            home: function () { },
            notfound: function () { Backbone.history.navigate('', { trigger: true }); }
        });
        
        // Start the router
        var router = new AppRouter();
        Backbone.history.start({
            pushState: true
        });

        // Hijack internal links
        $(document.body).on('click', 'a:not([data-link="exterior"],[href="#"],[href^="http:"],[href^="https:"],[href^="//"]),a[data-link="interior"]', function (evt) {
            // Clear the view
            var container = document.getElementById('root-container');
            container.innerHTML = '';

            // Send the URL to the router
            evt.preventDefault();
            var target = $(this).attr('href');
            Backbone.history.navigate(target, { trigger: true });
            appViewModel.activeUrl(target);
        });
    });
});