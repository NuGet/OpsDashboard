// Configure require js
requirejs.config({
    // By default, load unprefixed modules from js/lib
    baseUrl: '../lib',
    paths: {
        // Load "app/" prefixed modules from js/app
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
        'bootstrap': {
            deps: ['jquery']
        }
    }
});

// Start the main app
requirejs(['jquery', 'knockback', 'app/fx/module'],
function  ($       , kb         , Module) {
    // Schedule startup code
    $(function () {
        // Start the router
        var router = new kb.Backbone.Router();
        kb.Backbone.history.start({
            pushState: true
        });

        // Create module routes
        kb.Backbone.history.route('app-packages/*rest', module('app-packages'));
        kb.Backbone.history.route('*rest', module('home'));

        // Hijack internal links
        $(document.body).on('click', 'a:not([data-link="exterior"],[href="#"],[href^="http:"],[href^="https:"],[href^="//"]),a[data-link="interior"]', function (evt) {
            // Send the URL to the router
            evt.preventDefault();
            router.navigate($(this).attr('href'), { trigger: true });
        });
    });
});