var App = App || {};

(function () {
    'use strict';

    var vm;
    App.DialogViewModel = vm = kendo.observable({
        element: "#editWindow",
        model: {},

        open: function (id) {

            var win = $(vm.element).data("kendoWindow");
            win.center().open();

            $.ajax({
                url: "http://localhost:50194/api/regions/" + id,
                type: "GET",
                cache: false
            }).done(function (result) {
                alert("Returned from server:\n" + JSON.stringify(result));
                vm.set("model", result);
            });

        },

        onSave: function (e) {
            if (e) e.preventDefault();
            var model = vm.get("model");
            alert("Data:\n" + JSON.stringify(model) + "\n\nActual saving process is not implemented...");
            vm.onClose(e);
        },

        onClose: function (e) {
            if (e) e.preventDefault();
            $(vm.element).data("kendoWindow").close();
        }
    });

    $(function () {
        kendo.bind(vm.element, vm);
    });

})();
