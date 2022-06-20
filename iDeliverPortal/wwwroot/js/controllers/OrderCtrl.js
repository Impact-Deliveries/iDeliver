(function (app) {
    'use strict';

    app.controller('OrderCtrl', ['$scope', '$rootScope', '$log', 'httpService', 'commonService', '$timeout', '$resource', 'NgTableParams','appsettings'
        , function ($scope, $rootScope, $log, httpService, commonService, $timeout, $resource, NgTableParams, appsettings) {
            $scope.order = {
                tabs: 1,
                data:null,
                params: {
                    page: 1,
                    count: 10,
                    objects: {
                        fromdate: moment(new Date()).format("DD-MM-yyyy"),
                        toDate: moment(new Date()).format("DD-MM-yyyy"),
                        status: '0',
                        driverID: '0',
                        merchantBranchID: '0',
                        merchantID: '0',
                    }
                }
            };

            $scope.changeTab = function (tab) {
                $scope.order.tabs = tab;
            };

            $scope.drivers = {
                data: null,
                length: 0,
                selected: '0'
            }
            $scope.status = {
                processing: false,
                data: [{ name: 'Pending Order', id: 1, },
                { name: 'Assign To Driver', id: 2, },
                { name: 'Driver Rejected ', id: 3, },
                { name: 'Driver Accepted ', id: 4, },
                { name: 'Merchant Arrived', id: 5, },
                { name: 'Order Picked ', id: 6, },
                { name: 'Order Arrived', id: 7 },]
            };

            //#region merchant
            $scope.merchant = {
                data: null,
                selected: "0"
            };
            $scope.getMerchant = function () {
                let promise = httpService.httpGet('Merchant/GetMerchants?IsActive=' + true, null, { 'Content-Type': 'application/json' });
                promise.then(function (res) {
                    switch (res.status) {
                        case 200:
                            $scope.merchant.data = res.data;
                            break;
                        default:
                            break;
                    }
                    //$scope.changeTab(3);
                    // $rootScope.page.loaded = true;
                }, function (res) {

                });
            }
            //#endregion
            //#region branches

            $scope.$watch('merchant.selected', function (newvalue, oldvalue) {
                $scope.getbranches();
                $scope.branch.selected = '0';
            });
            $scope.$watch('branch.selected', function (newvalue, oldvalue) {
                $scope.price.selected = '0';
                $scope.price.delivery = "0";
                $scope.getPrices();
            });

            $scope.branch = {
                data: null,
                selected: "0"
            };
            $scope.getbranches = function () {
                if ($scope.merchant.selected == '0') {
                    return;
                }
                let promise = httpService.httpGet('MerchantBranch/GetBranchesByMerchantID', {
                    MerchantID: Number($scope.merchant.selected),
                    LocationID: null,
                    IsActive: true
                }, { 'Content-Type': 'application/json' });
                promise.then(function (res) {
                    switch (res.status) {
                        case 200:
                            $scope.branch.data = res.data;
                            break;
                        default:
                            break;
                    }
                    //$scope.changeTab(3);
                    // $rootScope.page.loaded = true;
                }, function (res) {

                });
            }
            //#endregion

            //#region Branch Delivery Price
            $scope.price = {
                data: null,
                valid: false,
                selected: "0",
                delivery: '0',
                Note: '',
                bill: "0",
                total: "0",
            };
            $scope.getPrices = function () {
                if (!$scope.branch || $scope.branch.selected == null || $scope.branch.selected == "0") {
                    return;
                }
                let promise = httpService.httpGet('MerchantDeliveryPrice/GetDeliveryPrices', {
                    MerchantBranchID: Number($scope.branch.selected),

                }, { 'Content-Type': 'application/json' });
                promise.then(function (res) {
                    switch (res.status) {
                        case 200:
                            $scope.price.data = res.data;
                            if ($scope.price.data.deliveryStatus == 3) {
                                $scope.price.delivery = $scope.price.data.deliveryPriceOffer;
                            }
                            break;
                        default:
                            break;
                    }
                    //$scope.changeTab(3);
                    // $rootScope.page.loaded = true;
                }, function (res) {

                });
            }

            $scope.$watch('price.selected', function (newvalue, oldvalue) {
                $scope.getDeliveryPrice();
            });
            $scope.$watch('price.bill+price.delivery', function (newvalue, oldvalue) {
                $scope.price.total = Number($scope.price.bill) + Number($scope.price.delivery)
            });

            $scope.getDeliveryPrice = function () {
                if ($scope.price.selected == "0" || $scope.price.selected == null) return;
                var obj = {
                    amount: 0,
                    locationID: 0,
                    id: 0,
                    deliveryStatus: $scope.price.data.deliveryStatus
                };
                switch ($scope.price.data.deliveryStatus) {
                    case 1:
                        if ($scope.price.data.merchantDeliveryPrice != null
                            && $scope.price.data.merchantDeliveryPrice.length > 0) {
                            var DeliveryPrice = $scope.price.data.merchantDeliveryPrice.filter(a => a.id == $scope.price.selected)[0];
                            if (DeliveryPrice) {
                                obj.amount = DeliveryPrice.amount;
                                obj.id = DeliveryPrice.id;
                                obj.locationID = DeliveryPrice.locationId;
                            }
                        }
                        break;
                    case 2:
                        if ($scope.price.data.merchantDeliveryPrice != null
                            && $scope.price.data.merchantDeliveryPrice.length > 0) {
                            var DeliveryPrice = $scope.price.data.merchantDeliveryPrice.filter(a => a.id == $scope.price.selected)[0];
                            if (DeliveryPrice) {
                                obj.amount = DeliveryPrice.amount;
                                obj.id = DeliveryPrice.id;
                            }
                        }
                        break;
                    case 3:
                        obj.amount = $scope.price.data.deliveryPriceOffer
                        break;
                    default:
                }
                $scope.price.delivery = obj.amount;
                return obj;
            };
            $scope.getDriverLocations = function () {
                if ($rootScope.page.PageID != "8" || !$rootScope.page.PageID) return;
                let promise = httpService.httpGet('DriverCase/GetOnlineDrivers', null, { 'Content-Type': 'application/json' });
                promise.then(function (res) {
                    switch (res.status) {
                        case 200:
                            // $scope.drivers.data = res.data;
                            var objs = [];
                            var obj1 = {
                                id: '0',
                                name: "Please Select",
                            };
                            objs.push(obj1);
                            if (res.data != null && res.data.length > 0) {
                                for (var i = 0; i < res.data.length; i++) {
                                    if (res.data[i].status == 1) {
                                        obj1 = {
                                            id: res.data[i].driverID.toString(),
                                            name: res.data[i].name,
                                        }
                                        objs.push(obj1);
                                    }
                                }
                            }

                            $scope.drivers.data = objs;
                            $timeout(function () {
                                $scope.getDriverLocations();
                            }, 3000)

                            break;
                        default:

                            $timeout(function () {
                                $scope.getDriverLocations();
                            }, 3000)

                            break;
                    }
                }, function (res) {

                });
            };

            $scope.Submit = function () {
                $scope.price.valid = true;
                if ($scope.drivers.selected == '0' || $scope.price.bill == '0' || $scope.price.bill == ''
                    || $scope.branch.selected == '0' || $scope.merchant.selected == '0') {
                    return;
                }
                if (($scope.price.data.deliveryStatus == 1 || $scope.price.data.deliveryStatus == 2) && $scope.price.selected == '0') {
                    return;
                }
                let model = {
                    MerchantBranchId: Number($scope.branch.selected),
                    TotalAmount: Number($scope.price.total),
                    DeliveryAmount: Number($scope.price.delivery),
                    Status: 2,
                    Note: $scope.price.Note,
                    DriverID: Number($scope.drivers.selected),
                    MerchantDeliveryPriceID: $scope.price.selected
                };


                let promise = httpService.httpPost('Order/AddOrder', model,
                    { 'Content-Type': 'application/json' });
                promise.then(function (res) {
                    switch (res.status) {
                        case 200:
                            $scope.changeTab(1);
                            break;
                        default:
                            break;
                    }
                }, function (res) {
                    commonService.redirect();
                });


            };
            //#endregion

            //#region

            $scope.tableRows = function (index) {
                if (index === null) return;
                var result = (($scope.dtOrders.page() - 1) * $scope.dtOrders.count()) + (index + 1);
                return result;
            };
            $scope.search = function () {

                $scope.order.params.fromdate = moment($scope.order.params.fromdate).format("DD-MM-yyyy")
                $scope.order.params.toDate = moment($scope.order.params.toDate).format("DD-MM-yyyy")
                $scope.dtOrders = new NgTableParams({
                    page: 1,
                    count: 10,
                    objects:null
                }, {
                    filterDelay: 300,
                    total: 10,
                    counts: [10, 25, 50, 100],
                    getData: function (param) {
                        // request to api

                        return $resource(appsettings.apiBaseUrl + 'Order/GetOrders')
                            .save(param.url(), $scope.order.params.objects)
                            .$promise.then(function (data) {
                                $scope.order.data = data.results;
                                param.total(data.total);
                                return data.results;
                            }).catch(function (response) {
                            });
                    },
                });

            };
            //#endregion

            $scope.getMerchant();
            $scope.getDriverLocations();
        }]);

})(angular.module("iDeliver"));