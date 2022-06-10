(function (app) {
    'use strict';

    app.controller('OrderCtrl', ['$scope', '$rootScope', '$log', 'httpService', 'commonService',
        function ($scope, $rootScope, $log, httpService, commonService) {
            $scope.order = {
                tabs: 1,
            };

            $scope.changeTab = function (tab) {
                $scope.order.tabs = tab;
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
            $scope.$watch('merchant.selected+branch.selected', function (newvalue, oldvalue) {
                $scope.employeeTable.showResult = false;
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



        }]);

})(angular.module("iDeliver"));