var App = App || {};

(function () {
    var vm;
    App.ViewModel = vm= new kendo.data.ObservableObject({
        element: "#app",

        title: "",
        date: new Date(2015, 5, 21),
        Customer: { CustomerID: "SEVES", CompanyName: "Seven Seas Imports" },

        getDateStr: function () {
            return kendo.toString(vm.get("date"), "yyyy/MM/dd");
        },

        dataSource: new kendo.data.DataSource({
            transport: {
                read: {
                    url: "http://localhost:50194/api/customers",
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
    $(vm.element).find("#combobox").data("kendoComboBox").list.width(400);
})();

