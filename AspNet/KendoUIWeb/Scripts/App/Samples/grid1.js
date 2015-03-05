var App = App || {};

(function () {
    'option strict';

    var vm;
    App.ViewModel = vm= new kendo.data.ObservableObject({
        element: "#app",

        productList: new kendo.data.DataSource({
            transport: {
                read: {
                    url: "http://localhost:50194/api/products",
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

