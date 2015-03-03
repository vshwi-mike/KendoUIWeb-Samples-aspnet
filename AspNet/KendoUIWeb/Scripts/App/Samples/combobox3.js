var App = App || {};

(function () {
    var vm;
    App.ViewModel = vm= new kendo.data.ObservableObject({
        element: "#app",

        region: { RegionID: null, RegionDescription: "" },
        territory: { TerritoryID: null, TerritoryDescription: "" },
        employee: { EmployeeID: null, LastName: "", FirstName: "" },

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

        territoryList: new kendo.data.DataSource({
            transport: {
                read: {
                    url: "http://localhost:50194/api/territories",
                    dataType: "json",
                    data: function () {
                        return { RegionID: $(vm.element).find("#drp_region").val() };
                    }
                }
            },
            serverFiltering: true,
            schema: {
                model: { id: "TerritoryID" }
            }
        }),

        employeeList: new kendo.data.DataSource({
            transport: {
                read: {
                    url: "http://localhost:50194/api/employees",
                    dataType: "json",
                    data: function () {
                        return {
                            RegionID: $(vm.element).find("#drp_region").val(),
                            TerritoryID: $(vm.element).find("#drp_territory").val()
                        };
                    }
                }
            },
            serverFiltering: true,
            schema: {
                model: { id: "EmployeeID" }
            }
        }),

        getEmployeeName: function () {
            var e = vm.get("employee");
            return e.FirstName + " " + e.LastName;
        }

    });

    kendo.bind(vm.element, vm);

    //コンボボックスのリスト部分の幅をセット。
    $(vm.element).find("#drp_region").data("kendoComboBox").list.width(400);
    $(vm.element).find("#drp_territory").data("kendoComboBox").list.width(400);
    $(vm.element).find("#drp_employee").data("kendoComboBox").list.width(400);
})();

