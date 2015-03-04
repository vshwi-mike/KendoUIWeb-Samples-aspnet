var vm = new kendo.data.ObservableObject({
    element: "#app",

    productList: new kendo.data.DataSource({
        transport: {
            read: {
                //url: "http://demos.telerik.com/kendo-ui/service/products",
                //dataType: "jsonp" // "jsonp" is required for cross-domain requests; use "json" for same-domain requests

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
