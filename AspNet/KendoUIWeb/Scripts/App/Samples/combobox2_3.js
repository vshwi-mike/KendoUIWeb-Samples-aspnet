var App = App || {};

(function () {
    var vm;
    App.ViewModel = vm= new kendo.data.ObservableObject({
        element: "#app",

        Customer: { CustomerID: "SEVES", CompanyName: "Seven Seas Imports" },

        dataSource: new kendo.data.DataSource({
            transport: {
                read: {
                    url: App.getApiUrl("customers"),
                    dataType: "json"
                }
            },
            schema: {
                model: { id: "CustomerID" }
            }
        })

    });

    kendo.bind(vm.element, vm);

    //コンボボックスのリスト部分の幅をセット。
    $(vm.element).find("#drp_customer").data("kendoComboBox").list.width(300);
})();

