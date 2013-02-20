(function ($, window, undefined) {
    function NavBarViewModel(initialUp, initialDown) {
        var self = {};

        return self;
    }
    
    function App() {
        var self = {};
        var navBarModel;

        self.start = function() {
            navBarModel = new NavBarViewModel();
            ko.applyBindings(navBarModel, $('#site-header')[0]);
        };

        self.bind = function(vm) {
            ko.applyBindings(vm, $('#root-container')[0]);
        };

        return self;
    }
    window.app = new App();

    $(function () {
        window.app.start();
        $('time').timeago();
    });
})(jQuery, window);