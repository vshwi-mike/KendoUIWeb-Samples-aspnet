﻿var App = App || {};

(function () {
    'use strict';

    var vm;
    App.ViewModel = vm= new kendo.data.ObservableObject({
        element: "#app",

        productList: new kendo.data.DataSource({
            transport: {
                read: {
                    url: App.getApiUrl("products"),
                    dataType: "json"
                }
            },
            schema: {
                model: { id: "ProductID" }
            }
        })

    });

    kendo.bind(vm.element, vm);
})();

