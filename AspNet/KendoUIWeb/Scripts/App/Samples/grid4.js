﻿var App = App || {};

(function () {
    'use strict';

    var vm;
    App.ViewModel = vm= new kendo.data.ObservableObject({
        element: "#app",

        criteria: { category_id: null, supplier_id: null, product_name:"", active_only: true },

        categoryList : new kendo.data.DataSource({
            transport: {
                read: {
                    url: App.getApiUrl("categories"),
                    dataType: "json"
                }
            }
        }), 

        productList: new kendo.data.DataSource({
            transport: {
                read: {
                    url: App.getApiUrl("ProductsPaged"),
                    dataType: "json",
                    data: function () {
                        return vm.get("criteria").toJSON();
                    }
                }
            },
            schema: {
                model: { id: "ProductID" },
                data: "list",
                total: "total"
            },
            serverPaging: true,
            pageSize: 10
        }),

        refreshProductList: function () {
            vm.productList.read();
        }

    });

    kendo.bind(vm.element, vm);

    //コンボボックスのリスト部分の幅をセット。
    $(vm.element).find("input[data-role='combobox']").each(function (index, item) {
        var combo = $(item).data("kendoComboBox");
        if (combo) combo.list.width(300);
    });

})();

