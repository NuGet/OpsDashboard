define(['knockout', 'backbone', 'backbone-subroute'], function (ko, Backbone) {
    var activeModulePrefix = null;
    function module(routes) {
        return function (modulePrefix) {
            var counter = 0;
            var baseObject = { routes: {} };

            for (var key in routes) {
                if (routes.hasOwnProperty(key)) {
                    var handlerName = '_handler_' + counter++;
                    baseObject.routes[key] = handlerName;
                    baseObject[handlerName] = function () {
                        var _oldActiveModule = activeModulePrefix;
                        activeModulePrefix = modulePrefix;
                        routes[key]();
                        activeModulePrefix = _oldActiveModule;
                    }
                }
            }
            var ModuleRouter = Backbone.SubRoute.extend(baseObject);
            new ModuleRouter(modulePrefix);
        };
    }

    function showView(name, viewModel) {
        viewModel = viewModel || {};

        // Load the view
        require(['text!app/' + activeModulePrefix + '/' + name + '.html'], function (content) {
            // Find the root container
            var container = document.getElementById('root-container');

            // Inject the content
            container.innerHTML = content;

            // Apply bindings
            ko.applyBindings(viewModel, container);
        });
    }

    return {
        module: module,
        showView: showView
    };
});