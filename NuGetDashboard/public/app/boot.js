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
requirejs(['jquery', 'backbone'],
function  ($       , Backbone) {
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

    // Schedule startup code
    $(function () {
        // Set up the router
        var AppRouter = Backbone.Router.extend({
            routes: {
                '': 'home',
                'deployments/*subroute': 'deploymentsModule',
                'operations/*subroute': 'operationsModule',
            },
            deploymentsModule: module('deployments'),
            operationsModule: module('operations'),
            home: function () { }
        });

        // Start the router
        var router = new AppRouter();
        Backbone.history.start({
            pushState: true
        });

        
        // Hijack internal links
        $(document.body).on('click', 'a:not([data-link="exterior"],[href="#"],[href^="http:"],[href^="https:"],[href^="//"]),a[data-link="interior"]', function (evt) {
            // Send the URL to the router
            evt.preventDefault();
            Backbone.history.navigate($(this).attr('href'), { trigger: true });
        });
    });
});