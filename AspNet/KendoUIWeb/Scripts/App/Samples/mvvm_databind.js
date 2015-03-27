var App = App || {};

(function () {
    'use strict';

    var vm;
    App.ViewModel = vm= new kendo.data.ObservableObject({
        element: "#app",

        title: "",
        date: new Date(),
        customer: { CustomerID: "SEVES", CompanyName: "Seven Seas Imports" },
        staff_name: "",

        getDateStr: function () {
            return kendo.toString(vm.get("date"), "yyyy/MM/dd");
        },

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
        }),

        onShowJSON: function (e) {
            if (e) e.preventDefault();
            var data = {
                date: vm.getDateStr(),
                customer_id: vm.get("customer.CustomerID"),
                staff_name: vm.get("staff_name")
            };
            var data_str = JSON.stringify(data, null, 2);
            console.log(data_str);
            alert(data_str);
        }

    });

    kendo.bind(vm.element, vm);

    //コンボボックスのリスト部分の幅をセット。
    $(vm.element).find("#combobox").data("kendoComboBox").list.width(400);
})();

