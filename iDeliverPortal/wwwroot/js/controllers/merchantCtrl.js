(function (app) {
    'use strict';
    app.controller('Merchant', ['$scope', '$rootScope', '$log', 'httpService', 'commonService', 'appsettings', 'NgTableParams', '$resource', '$timeout',
        function ($scope, $rootScope, $log, httpService, commonService, appsettings, NgTableParams, $resource, $timeout) {
            $scope.merchant = {
                isvalid: false,
                tabs: 1,
                merchantid: 0,
                obj: {
                    Id: null,
                    OrganizationId: 1,
                    MerchantName: '',
                    Overview: '',
                    //address: '',
                    Phone: '',
                    Mobile: '',
                    Email: "",
                    ModifiedDate: null,
                    CreationDate: '',
                    QutationNumber: '',
                    Position: '',
                    Owner: '',
                    OwnerNumber: '',
                    Attachments: [],
                    IsActive: true
                },
            };
            $scope.changeTab = function (i) {
                $scope.merchant.tabs = i;
                switch (i) {
                    case 1:
                        $scope.getmerchantsTable();
                        break;
                    default:
                }
            };
            $scope.merchantTable = {
                page: 1,
                count: 10,
                data: null,
                showResult: false,
                isValid: false,
                objects: {
                    merchantID: "0",
                    IsActive: "true",
                    merchantName: null,
                    Mobile: "0",
                }
            };


            //#region Organization
            $scope.organization = {
                data: null,
                selected: "1"
            };
            let promise = httpService.httpGet('Organization/GetActiveOrNotOrganizations?IsActive=' + true, null, { 'Content-Type': 'application/json' });
            promise.then(function (res) {
                switch (res.status) {
                    case 200:
                        $scope.organization.data = res.data;
                        break;
                    default:
                        break;
                }
                //$scope.changeTab(3);
                // $rootScope.page.loaded = true;
            }, function (res) {

            });

            //#endregion
            $scope.submit = function () {
                $scope.merchant.isvalid = true;
                $scope.merchant.obj.Organization = $scope.organization.selected;
                if ($scope.organization.selected == null || $scope.organization.selected == '0') {
                    return;
                }
                if ($scope.merchant.obj.Position == '' || $scope.merchant.obj.QutationNumber == '' || $scope.merchant.obj.Owner == '') {
                    return;
                }
                if ($scope.merchant.obj.MerchantName == '' || $scope.merchant.obj.Overview == '' || $scope.merchant.obj.Email == ''
                    || $scope.merchant.obj.Phone == '' || $scope.merchant.obj.Mobile == '') {
                    return;
                }
                let promise = httpService.httpPost('merchant/Addmerchant',
                    $scope.merchant.obj,
                    { 'Content-Type': 'application/json' });
                promise.then(function (res) {
                    switch (res.status) {
                        case 200:
                            $scope.merchant.merchantid = res.data;
                            $scope.changeTab(1);
                            break;
                        default:
                            break;
                    }
                }, function (res) {
                    commonService.redirect();
                });
            };

            //#region merchants table
            //$scope.getmerchantsTable = function () {
            //    var resource = $resource(appsettings.apiBaseUrl + "merchant/Getmerchants");
            //    $scope.dtmerchants = new NgTableParams($scope.merchantTable, {
            //        filterDelay: 300,
            //        total: 1,
            //        getData: function ($defer) {
            //            // $scope.result.processing = true;
            //            // request to api
            //            return resource.get(JSON.stringify($defer.url())).$promise.then(function (response) {
            //                // $scope.result.processing = false;
            //                $defer.total(response.total);
            //                return response.results;
            //            }).catch(function (response) {
            //                //$scope.result.processing = false;
            //            });
            //        }
            //    });
            //    $scope.tableRows = function (index) {
            //        if (index === null) return;
            //        var result = (($scope.dtmerchants.page() - 1) * $scope.dtmerchants.count()) + (index + 1);
            //        return result;
            //    };
            //};
            $scope.$watch('merchantTable.objects.IsActive + merchantTable.objects.merchantName', function (newvalue, oldvalue) {
                $scope.merchantTable.showResult = false;
            });
            $scope.getmerchantsTable = function () {
                $scope.merchantTable.isValid = true;
                if ($scope.merchantTable.objects.IsActive == '') {
                    return;
                }
                let promise = httpService.httpGet('Merchant/GetMerchants', {
                    IsActive: $scope.merchantTable.objects.IsActive,
                    MerchantName: $scope.merchantTable.objects.merchantName
                });
                promise.then(function (res) {
                    switch (res.status) {
                        case 200:
                            $scope.merchantTable.data = res.data;
                            $scope.merchantTable.showResult = true;
                            break;
                        default:
                            break;
                    }
                }, function (res) {

                });
            };

            $scope.getmerchant = function (id) {
                //$rootScope.page.loaded = false;
                let promise = httpService.httpGet('merchant/' + id, null, { 'Content-Type': 'application/json' });
                promise.then(function (res) {
                    switch (res.status) {
                        case 200:
                            //(res.data)
                            $scope.merchant.obj.Id = res.data.id;
                            $scope.merchant.obj.OrganizationId = res.data.organizationId;
                            $scope.merchant.obj.MerchantName = res.data.merchantName;
                            $scope.merchant.obj.Overview = res.data.overview;
                            $scope.merchant.obj.address = res.data.address;
                            $scope.merchant.obj.Phone = Number(res.data.phone);
                            $scope.merchant.obj.Mobile = Number(res.data.mobile);
                            $scope.merchant.obj.Email = res.data.email;
                            $scope.merchant.obj.QutationNumber = res.data.QutationNumber;
                            $scope.merchant.obj.Position = res.data.Position;
                            $scope.merchant.obj.Owner = res.data.Owner;


                            $scope.organization.selected = res.data.organizationId.toString();
                            break;
                        default:
                            break;
                    }
                    $scope.changeTab(3);
                    // $rootScope.page.loaded = true;
                }, function (res) {

                });
            };
            //#endregion

            $scope.changeTab(1);

        }]);
})
    (angular.module("iDeliver"));