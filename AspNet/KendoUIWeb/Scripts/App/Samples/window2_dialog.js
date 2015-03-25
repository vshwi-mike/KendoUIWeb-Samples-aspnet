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
                        model.set("RegionID", 0);
                        model.set("isNew", true);
                    } else {
                        model.set("isNew", false);
                    }
                    vm.set("model", model);
                });
            } else {
                var model = kendo.observable({
                    RegionID: 0,
                    RegionDescription: "",
                    isNew: true
                });
                vm.set("model", model);
            }
        },

        onSave: function (e) {
            if (e) e.preventDefault();
            var model = vm.get("model");
            var data_str = JSON.stringify(model);

            $.ajax({
                url: "http://localhost:50194/api/regions/" + (!model.isNew && model.RegionID),
                type: model.isNew ? "POST" : "PUT",
                data: data_str,
                processData: false,
                contentType: "application/json; charset=utf-8"
            }).done(function (result) {
                alert("Data saved successfully.");
                vm.trigger("saved", { data: model });        //savedイベントを発生して親画面に通知。
                vm.onClose();
            }).fail(function (jqXHR, textStatus, errorThrown) {
                var s = jqXHR.status + ": " + errorThrown;
                s += jqXHR.responseText && "\n\n" + jqXHR.responseText;
                alert(s);
            });
            
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
