/// <reference path="lib/require.js" />
requirejs.config({
    // By default, load unprefixed modules from js/lib
    baseUrl: '/js/lib',
    paths: {
        // Load "app/" prefixed modules from js/app
        app: '../app',

        // Versions used
        'jquery': 'jquery-1.9.0',
        'knockout': 'knockout-2.2.1'
    },
    shim: {
        'backbone': {
            deps: ['underscore', 'jquery'],
            exports: 'window.Backbone'
        },
        'bootstrap': {
            deps: ['jquery']
        }
    }
});