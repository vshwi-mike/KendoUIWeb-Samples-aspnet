var App = App || {};

(function () {
    'option strict';

    var vm;
    App.ViewModel = vm= new kendo.data.ObservableObject({
        element: "#app",

        regionList: new kendo.data.DataSource({
            transport: {
                read: {
                    url: "http://localhost:50194/api/regions",
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
            if (!data) return;
            console.log("data=", data);

            //選択行のデータをコピーして新たなオブジェクトを作る。
            var model = $.extend({}, data.toJSON());        //toJSON()を呼ばないと余計なプロパティまで含んでしまう。

            //ダイアログウィンドウを開く。
            App.DialogViewModel.open(model);
        }

    });

    kendo.bind(vm.element, vm);

    //グリッドのダブルクリック時のイベントハンドラをセット。
    $(document).on("dblclick ", "#grid tbody tr", vm.onEdit);

})();

