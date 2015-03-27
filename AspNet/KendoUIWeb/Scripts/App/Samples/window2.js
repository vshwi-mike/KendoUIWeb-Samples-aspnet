var App = App || {};

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

        refreshList: function (e) {
            if (e) e.preventDefault();
            vm.get("regionList").read();
        },

        onEdit: function (e) {
            if (e) e.preventDefault();
            console.log("onEdit", e);
            var grid = $("#grid").data("kendoGrid");
            var tr = $(e.target).closest("tr");
            var data = grid.dataItem(tr);
            console.log("data=", data);

            //ダイアログウィンドウを開く。
            App.DialogViewModel.open(data.RegionID);
        },

        onCreate: function (e) {
            if (e) e.preventDefault();
            var grid = $("#grid").data("kendoGrid");
            grid.clearSelection();
            App.DialogViewModel.open(null);
        },

        onCopy: function (e) {
            if (e) e.preventDefault();
            var grid = $("#grid").data("kendoGrid");
            var item = grid.dataItem(grid.select());
            if (!item) {
                alert("Please select a row.");
                return;
            }
            var region_id = item.get("RegionID");
            App.DialogViewModel.open(region_id, true);
        },

        onDelete: function (e) {
            if (e) e.preventDefault();
            var grid = $("#grid").data("kendoGrid");
            var item = grid.dataItem(grid.select());
            if (!item) {
                alert("Please select a row.");
                return;
            }

            var region_id = item.get("RegionID");
            if (confirm("Are you sure to delete '" + region_id + "'?")) {
                $.ajax({
                    url: App.getApiUrl("regions/DeleteRegion/" + region_id),
                    type: "POST"
                }).done(function (result) {
                    alert("The region '" + region_id + "' has been deleted.");
                    vm.refreshList();
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    var s = jqXHR.status + ": " + errorThrown;
                    s += jqXHR.responseText && "\n\n" + jqXHR.responseText;
                    alert(s);
                });
            }
        }
    });

    kendo.bind(vm.element, vm);

    //グリッドのダブルクリック時のイベントハンドラをセット。
    $(document).on("dblclick ", "#grid tbody tr", vm.onEdit);

    $(vm.element).on("click", "#grid .k-grid-add", vm.onCreate);
    $(vm.element).on("click", "#grid .k-grid-copy", vm.onCopy);
    $(vm.element).on("click", "#grid .k-grid-delete", vm.onDelete);
    $(vm.element).on("click", "#grid .k-grid-refresh", vm.refreshList);
    $(vm.element).find(".k-grid-refresh").addClass("pull-right");     //Toolbar内のRefreshボタンを右端に移動。

    //ダイアログウィンドウで保存されたら親画面の一覧を更新。
    App.DialogViewModel.bind("saved", function (e) {
        console.log("saved event.", e);
        vm.refreshList();
    });

})();

