(function (app) {
    'use strict';
    app.controller('Location', ['$scope', '$rootScope', '$log', 'httpService', 'commonService', 'appsettings', 'NgTableParams', '$resource', '$timeout',
        function ($scope, $rootScope, $log, httpService, commonService, appsettings, NgTableParams, $resource, $timeout) {

            $scope.location = {
                isvalid: false,
                currentdate: moment(new Date()).format("dd-MM-yyyy"),
                tabs: 1,
                Id: 0,
                obj: {
                    Id: 0,
                    Address: '',
                    CountryId: '',
                    City: '',
                },

            };
            $scope.changeTab = function (i) {
                $scope.location.tabs = i;
                switch (i) {
                    case 1:
                        $scope.getlocations();
                        break;
                    default:
                }
            };
            $scope.locationTable = {
                page: 1,
                count: 10,
                data: null,
                showResult: false,
                isvalid: false,
                objects: {

                }
            };
            $scope.getlocations = function () {
                $scope.locationTable.showResult = false;
                let promise = httpService.httpGet('Location/all', null, { 'Content-Type': 'application/json' });
                promise.then(function (res) {
                    switch (res.status) {
                        case 200:
                            $scope.locationTable.data = res.data;
                            $scope.locationTable.showResult = true;
                            break;
                        default:
                            break;
                    }
                    //$scope.changeTab(3);
                    // $rootScope.page.loaded = true;
                }, function (res) {

                });
            }

            $scope.getSelectedlocation = function (selectedid) {
                
                $scope.locationTable.showResult = false;
                let promise = httpService.httpGet('Location/' + selectedid, null
                    , { 'Content-Type': 'application/json' });
                promise.then(function (res) {
                    switch (res.status) {
                        case 200:
                            $scope.changeTab(3);
                            //$scope.location.obj = res.data;
                            $scope.location.obj.Id = selectedid;
                            $scope.location.obj.Address = res.data.address;
                            $scope.location.obj.CountryId = res.data.countryId;
                            $scope.location.obj.City = res.data.city;
                            break;
                        default:
                            break;
                    }
                    //$scope.changeTab(3);
                    // $rootScope.page.loaded = true;
                }, function (res) {

                });
            };
            $scope.Submit = function () {
                $scope.location.isvalid = true;
                if ($scope.location.obj.Address == '' || $scope.location.obj.City == '') {
                    return;
                }
                let promise = httpService.httpPost('Location/Addlocation',
                  $scope.location.obj
                ,
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
            $scope.deleteLocation = function (id) {
                //$rootScope.page.loaded = false;
                let promise = httpService.httpPost('Location/DeleteLocation', id, { 'Content-Type': 'application/json' });

                promise.then(function (res) {
                    switch (res.status) {
                        case 200:
                            $scope.getlocations();
                            break;
                        default:
                            break;
                    }
                    // $rootScope.page.loaded = true;
                }, function (res) {

                });
            };
 
            $scope.getlocations();
        }]);
})
    (angular.module("iDeliver"));