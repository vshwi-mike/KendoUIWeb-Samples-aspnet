var App = App || {};

(function () {
    'use strict';

    var vm;
    App.DialogViewModel = vm = kendo.observable({
        element: "#editWindow",
        model: {},

        open: function (id, isCopy) {

            var validator = $(vm.element).find("#editForm").data("kendoValidator");
            validator.hideMessages();       //前回のValidationメッセージをクリア。

            var win = $(vm.element).data("kendoWindow");
            win.center().open();

            if (id) {
                $.ajax({
                    url: "http://localhost:50194/api/regions/" + id,
                    type: "GET",
                    cache: false
                }).done(function (result) {
                    console.log("Returned from server: ", result);
                    var model = kendo.observable(result);

                    if (isCopy) {
                        model.set("RegionID", null);
                        model.set("isNew", true);
                    } else {
                        model.set("isNew", false);
                    }
                    vm.set("model", model);
                });
            } else {
                var model = kendo.observable({
                    RegionID: null,
                    RegionDescription: "",
                    isNew: true
                });
                vm.set("model", model);
            }
        },

        onSave: function (e) {
            if (e) e.preventDefault();
            var model = vm.get("model").toJSON();
            alert("Data:\n" + JSON.stringify(model) + "\n\nActual saving process is not implemented...");
            vm.onClose(e);
            vm.trigger("saved", { data: model } );        //savedイベントを発生して親画面に通知。
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
