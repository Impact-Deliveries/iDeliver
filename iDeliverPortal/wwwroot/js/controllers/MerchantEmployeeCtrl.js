(function (app) {
    'use strict';
    app.controller('MerchantEmployee', ['$scope', '$rootScope', '$log', 'httpService', 'commonService', 'appsettings', 'NgTableParams', '$resource', '$timeout',
        function ($scope, $rootScope, $log, httpService, commonService, appsettings, NgTableParams, $resource, $timeout) {

            $scope.employee = {
                isvalid: false,
                currentdate: moment(new Date()).format("dd-MM-yyyy"),
                tabs: 1,
                myDropzone: null,
                Id: 0,
                obj: {
                    Id: 0,
                    FirstName :'',
                    MiddleName : '',
                    LastName : '',
                    NationalNumber : '',
                    Mobile : '',
                    Phone : '',
                    MerchantBranchId :'',
                    Attachments: null,
                    IsActive: 'true'
                },

            };
            $scope.changeTab = function (i) {
                $scope.employee.tabs = i;
                switch (i) {
                    case 1:
                        //$scope.getemployeesTable();
                        break;
                    default:
                }
            };
            $scope.employeeTable = {
                page: 1,
                count: 10,
                data: null,
                showResult: false,
                isvalid: false,
                objects: {
                    Id: "0",
                    IsActive: "true",
                    employeeName: '',
                    Mobile: "0",
                }
            };

            $scope.Submit = function () {
                $scope.employee.isvalid = true;
                if ($scope.employee.obj.FirstName == '' || $scope.employee.obj.MiddleName == '' ||
                    $scope.employee.obj.LastName == '' || $scope.employee.obj.Mobile == '' ||
                    $scope.employee.obj.Phone == '' || $scope.branch.selected == '' || $scope.branch.selected=='0') {
                    return;
                }
                $scope.employee.obj.MerchantBranchId = Number($scope.branch.selected)
                let promise = httpService.httpPost('MerchantEmployee/AddEmployee',
                    $scope.employee.obj,
                    { 'Content-Type': 'application/json' });

                promise.then(function (res) {
                    switch (res.status) {
                        case 200:
                            $scope.employee.Id = res.data;
                            if ($scope.employee.myDropzone != null && $scope.employee.myDropzone.files.length > 0) {
                                $scope.employee.myDropzone.processQueue();
                            } else {
                                $scope.changeTab(1);
                            }
                          

                            break;
                        default:
                            break;
                    }
                }, function (res) {
                    commonService.redirect();
                });
            };
            $scope.activeemployee = function (id) {
                //$rootScope.page.loaded = false;
                let promise = httpService.httpPost('MerchantEmployee/ActiveEmployee', id, { 'Content-Type': 'application/json' });

                promise.then(function (res) {
                    switch (res.status) {
                        case 200:
                            $scope.getemployeesTable();
                            break;
                        default:
                            break;
                    }
                    // $rootScope.page.loaded = true;
                }, function (res) {

                });
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
                if ($scope.merchant.selected=='0') {
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

            //#region employees table

            $scope.getemployeesTable = function () {
                $scope.employeeTable.isvalid = true;
                if ($scope.merchant.selected == '0' || $scope.branch.selected == '0') {
                    return;
                }
                let promise = httpService.httpGet('MerchantEmployee/GetBranchEmployees', {
                    MerchantBranchID: Number($scope.branch.selected),
                    IsActive: $scope.employee.obj.IsActive
                }, { 'Content-Type': 'application/json' });

                promise.then(function (res) {
                    switch (res.status) {
                        case 200:
                            $scope.employeeTable.data = res.data;
                            $scope.employeeTable.showResult = true;
                            break;
                        default:
                            break;
                    }
                    // $rootScope.page.loaded = true;
                }, function (res) {

                });

            };

            $scope.getemployee = function (id) {
                //$rootScope.page.loaded = false;
                let promise = httpService.httpGet('MerchantEmployee/' + id, {
                }, { 'Content-Type': 'application/json' });
                promise.then(function (res) {
                    switch (res.status) {
                        case 200:
                            $scope.employee.obj = res.data;
                            debugger
                            $scope.employee.obj.Id = res.data.id ;
                            $scope.employee.obj.FirstName = res.data.firstName;
                            $scope.employee.obj.MiddleName = res.data.middleName;
                            $scope.employee.obj.LastName = res.data.lastName;
                            $scope.employee.obj.NationalNumber = res.data.nationalNumber;
                            $scope.employee.obj.Mobile = res.data.mobile;
                            $scope.employee.obj.Phone = res.data.phone;
                            $scope.employee.obj.MerchantBranchId = res.data.merchantBranchId.toString();
                            $scope.changeTab(3);
                            break;
                        default:
                            break;
                    }
                    //$scope.changeTab(3);
                    // $rootScope.page.loaded = true;
                }, function (res) {

                });
            };
            //#endregion

            //#region dropzone
            $scope.dzOptions = {
                url: appsettings.apiBaseUrl + "Attachment/UploadAttachments",
                method: 'POST',
                maxFilesize: '100',
                acceptedMimeTypes: '.doc,.xls,.flv,.mp3,.mp4,.pdf,.ppt,.rar,.zip,.wav,.docx,.xlsx,.pptx,.jpg,.bmp,.gif,.png,.jpeg,.tif,.txt',
                maxFiles: 10,
                parallelUploads: 10,
                uploadMultiple: true,
                autoProcessQueue: false,
                addRemoveLinks: true,
                init: function () {
                    $scope.employee.myDropzone = this;
                },

                dictDefaultMessage: 'Drop files here or click to upload.',
                dictRemoveFile: "Remove"
            };
            $scope.dzCallbacks = {

                'addedfile': function (file) {

                },
                'sendingmultiple': function (file, xhr, formData) {
                    var data = {
                        ModuleTypeID: 5,//ProfilePicture
                        CreatorID: 5,
                        GroupID: 0,
                        ModuleID: $scope.employee.Id ,
                        Path: "~/userfiles/employee/",
                    };

                    formData.append("data", JSON.stringify(data));
                },
                'success': function (file, xhr) {
                    $scope.changeTab(1);
                },
                'removedfile': function (file, xhr) {
                    //  $scope.reomveAttachmentItem(file);
                },
                'maxfilesexceeded': function (file) {
                },
                'error': function (file, xhr) {
                    // $scope.dzMethods.removeFile(file);
                }
            };

            $scope.dzMethods = {};

            //$scope.removeAllAttachmentItems = function () {
            //};

            //$scope.reomveAttachmentItem = function (file) {

            //}
            //#endregion
            $scope.changeTab(1);
            $scope.getMerchant();
        }]);
})
    (angular.module("iDeliver"));