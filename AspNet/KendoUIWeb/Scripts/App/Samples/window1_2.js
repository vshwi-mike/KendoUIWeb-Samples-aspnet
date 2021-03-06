﻿var App = App || {};

(function () {
    'use strict';

    var vm;
    App.ViewModel = vm = new kendo.data.ObservableObject({
        element: "#app",

        regionList: new kendo.data.DataSource({
            transport: {
                read: {
                    url: App.getApiUrl("regions"),
                    dataType: "json"
                }
            },
            schema: {
                model: { id: "RegionID" }
            }
        }),

        onEdit: function (e) {
            if (e) e.preventDefault();
            console.log("onEdit", e);
            var grid = $("#grid").data("kendoGrid");
            var tr = $(e.target).closest("tr");
            var data = grid.dataItem(tr);
            console.log("data=", data);

            //ダイアログウィンドウを開く。
            App.DialogViewModel.open(data.RegionID);
        }

    });

    kendo.bind(vm.element, vm);

    //グリッドのダブルクリック時のイベントハンドラをセット。
    $(document).on("dblclick ", "#grid tbody tr", vm.onEdit);

})();

