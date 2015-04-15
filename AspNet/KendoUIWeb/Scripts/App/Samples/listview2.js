var App = App || {};

(function () {
    'use strict';

    var CartItem = kendo.data.Model.define({
        id: "cartItemId",
        fields: {
            "cartItemId": { type: "number" },
            "ProductID" : {type : "string"},
            "ProductName": { type: "string" },
            "UnitPrice": { type: "number" },
            "quantity": { type: "number" }
        },
        getPrice : function () {
            return kendo.toString(this.get("UnitPrice"), "#,0.00");
        },
        getAmount : function () {
            return kendo.toString(this.get("quantity") * this.get("UnitPrice"), "#,0.00");
        },
        changeQuantity: function (diff) {
            var qty = this.get("quantity");
            if (qty + diff > 0) {
                this.set("quantity", qty + diff);
            }
        }
    });


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
        }),

        cart: new kendo.data.DataSource({
            data: []
        }),

        cartIsEmpty: function () {
            return vm.get("cart").total() == 0;
        },

        onAdd: function (e) {
            if (e) e.preventDefault();
            console.log("onAdd");

            var list = $(vm.element).find("#products").data("kendoListView");
            var selected = list.select();
            if (selected && selected.length > 0) {
                selected.each(function (index, el) {
                    var uid = $(el).data("uid");
                    console.log("index=" + index + ", uid=" + uid);
                    var product = vm.productList.getByUid(uid);
                    console.log(product);
                    if (product) {
                        var item = new CartItem({
                            ProductID: product.ProductID,
                            ProductName: product.ProductName,
                            quantity: 1,
                            UnitPrice: product.UnitPrice
                        });
                        vm.cart.add(item);
                    }
                });
            }
            list.clearSelection();
        },

        onIncrement: function (e) {
            if (e) e.preventDefault();
            console.log("onIncrement");

            var uid = $(e.target).closest("li").data("uid");
            var item = vm.cart.getByUid(uid);
            if (item) {
                item.changeQuantity(1);
            }
        },

        onDecrement: function (e) {
            if (e) e.preventDefault();
            console.log("onDecrement");
            var uid = $(e.target).closest("li").data("uid");
            var item = vm.cart.getByUid(uid);
            if (item) {
                item.changeQuantity(-1);
            }
        },

        onRemove: function(e){
            if (e) e.preventDefault();
            console.log("onRemove");
            var uid = $(e.target).closest("li").data("uid");
            var item = vm.cart.getByUid(uid);
            if (item) {
                vm.cart.remove(item);
            }
        },

        onClearAll: function (e) {
            if (e) e.preventDefault();
            console.log("onClearAll");

            vm.cart.data([]);
        }

    });

    kendo.bind(vm.element, vm);
})();

