var App = App || {};

(function () {
    'use strict';

    var vm;
    App.DialogViewModel = vm = kendo.observable({
        element: "#editWindow",
        model: {},

        open: function (model) {
            vm.set("model", model);

            var win = $(vm.element).data("kendoWindow");
            win.center().open();
        },

        onSave: function (e) {
            if (e) e.preventDefault();
            var model = vm.get("model");
            alert("Data:\n" + JSON.stringify(model) + "\n\nActual saving process is not implemented...");
            vm.onClose(e);
        },

        onClose: function (e) {
            if (e) e.preventDefault();
            $(e.target).closest("[data-role='window']").data("kendoWindow").close();
        }
    });

    $(function () {
        kendo.bind(vm.element, vm);
    });

})();
