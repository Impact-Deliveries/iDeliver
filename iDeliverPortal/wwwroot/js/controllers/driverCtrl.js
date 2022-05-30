(function (app) {
    'use strict';
    app.controller('Driver', ['$scope', '$rootScope', '$log', 'httpService', 'commonService', 'appsettings', 'NgTableParams', '$resource', '$timeout',
        function ($scope, $rootScope, $log, httpService, commonService, appsettings, NgTableParams, $resource, $timeout) {
            $scope.socialStatus = [
                { ID: 1, EnglishTitle: "Single" },
                { ID: 2, EnglishTitle: "Engaged" },
                { ID: 3, EnglishTitle: "Married" },
                { ID: 4, EnglishTitle: "Divorced" }
            ];

            $scope.JobTime = [
                { ID: 1, EnglishTitle: "Full Time" },
                { ID: 2, EnglishTitle: "Part Time" },
            ];

            $scope.Days = [
                { ID: 1, EnglishTitle: "Saturday", checked: false },
                { ID: 2, EnglishTitle: "Sunday", checked: false },
                { ID: 3, EnglishTitle: "Monday", checked: false },
                { ID: 4, EnglishTitle: "Tuesday", checked: false },
                { ID: 5, EnglishTitle: "Wednesday", checked: false },
                { ID: 6, EnglishTitle: "Thursday", checked: false },
                { ID: 7, EnglishTitle: "Friday", checked: false }
            ];

            $scope.driver = {
                isvalid: false,
                currentdate: moment(new Date()).format("dd-MM-yyyy"),
                tabs: 1,
                myDropzone: null,
                driverid: 0,
                obj: {
                    DriverID: null,
                    username: '',
                    firstname: '',
                    middlename: '',
                    lastname: '',
                    address: '',
                    phone: null,
                    mobile: null,
                    birthday: moment().utc().format("DD-MM-yyyy").toString(),
                    socialStatus: "0",
                    isHaveProblem: false,
                    reason: null,
                    workTime: "0",
                    fromTime: null,
                    toTime: null,
                    startJob: moment().utc().format("DD-MM-yyyy").toString(),
                    college: '',
                    university: '',
                    major: '',
                    graduationyear: '',
                    estimate: '',
                    advancedStudies: '',
                    selecteddays: [],
                    Attachments: null,
                    IsActive: true,
                    nationalNumber: '',
                    OrganizationID: 1
                },

            };

            $scope.changeTab = function (i) {
                $scope.driver.tabs = i;
                switch (i) {
                    case 1:
                        $scope.getDriversTable();
                        break;
                    default:
                }
            };

            $scope.driverTable = {
                page: 1,
                count: 10,
                data: null,
                showResult: false,
                isvalid: false,
                objects: {
                    DriverID: "0",
                    IsActive: "true",
                    DriverName: '',
                    Mobile: "0",
                }
            };

            $scope.checktime = function () {
                if ($scope.driver.obj.fromTime == null || $scope.driver.obj.toTime == null) {
                    return false;
                }
                if (moment($scope.driver.obj.fromTime).format("HH:mm:ss a") > moment($scope.driver.obj.toTime).format("HH:mm:ss a")) {
                    return true;
                } else {
                    return false;
                }
            };

            $scope.checkday = function (i) {
                $scope.Days[i].checked = !$scope.Days[i].checked;
                console.log($scope.Days[i])
            };

            $scope.calculateSequence = function (num) {
                switch (num.toString().length) {
                    case 1:
                        return '00' + num.toString();
                        break;
                    case 2:
                        return '0' + num.toString();
                        break;
                    case 3:
                        return num.toString();
                        break;
                    default:
                }
            };

            $scope.changeVal = function (id) {
                var selectedDriver = $scope.driverTable.data.findIndex(a => a.id == id)
                if (selectedDriver >= 0) {
                    let promise = httpService.httpPost('Driver/ChangeDriverStatus?DriverID=' + id, null,
                        { 'Content-Type': 'application/json' });
                    promise.then(function (res) {
                        switch (res.status) {
                            case 200:
                                $scope.driverTable.data[selectedDriver].isActive = !$scope.driverTable.data[selectedDriver].isActive;
                                break;
                            default:
                                break;
                        }
                    }, function (res) {
                        commonService.redirect();
                    });

                }
            };

            $scope.Submit = function () {
                debugger;
                let totaldays = $scope.Days.filter(a => a.checked == true);
                $scope.driver.isvalid = true;
                if ($scope.driver.obj.firstname == '' || $scope.driver.obj.middlename == '' ||
                    $scope.driver.obj.lastname == '' || $scope.driver.obj.address == '' ||
                    $scope.driver.obj.birthday == '' || $scope.driver.obj.socialStatus == "0" || $scope.driver.obj.mobile == '') {
                    return;
                }
                if ($scope.driver.obj.isHaveProblem == null || ($scope.driver.obj.isHaveProblem == true
                    && $scope.driver.obj.reason == '')
                    || $scope.driver.obj.workTime == "0" || $scope.driver.obj.startJob == '') {
                    return;
                }
                if ($scope.driver.obj.workTime == '2' && totaldays == null && totaldays.length == 0
                    || moment($scope.driver.obj.fromTime).format("HH:mm:ss a") > moment($scope.driver.obj.toTime).format("HH:mm:ss a")
                ) {
                    return;
                }
                $scope.driver.obj.selecteddays = $scope.Days.filter(a => a.checked == true).map(t => t.ID);
                //$scope.driver.obj.birthday = moment($scope.driver.obj.birthday).utc().format("DD-MM-yyyy")
                //$scope.driver.obj.startJob = moment($scope.driver.obj.startJob).utc().format("DD-MM-yyyy")
                $scope.driver.obj.isActive = true;
                let promise = httpService.httpPost('Driver/AddDriver',
                    $scope.driver.obj,
                    { 'Content-Type': 'application/json' });

                promise.then(function (res) {
                    switch (res.status) {
                        case 200:
                            $scope.driver.driverid = res.data;
                            if ($scope.driver.myDropzone != null && $scope.driver.myDropzone.files.length > 0) {
                                $scope.driver.myDropzone.processQueue();
                            }
                            $scope.changeTab(1);

                            break;
                        default:
                            break;
                    }
                }, function (res) {
                    commonService.redirect();
                });
            };

            //#region drivers table
            $scope.$watch('driverTable.objects.IsActive  + driverTable.objects.DriverName', function (newvalue, oldvalue) {
                $scope.driverTable.showResult = false;
            });
            $scope.getDriversTable = function () {
                $scope.driverTable.isvalid = true;
                if ($scope.driverTable.objects.IsActive == '') {
                    return;
                }
                let promise = httpService.httpGet('Driver/GetDrivers', {
                    IsActive: $scope.driverTable.objects.IsActive,
                    DriverName: $scope.driverTable.objects.DriverName
                }, { 'Content-Type': 'application/json' });

                promise.then(function (res) {
                    switch (res.status) {
                        case 200:
                            $scope.driverTable.data = res.data;
                            $scope.driverTable.showResult = true;
                            break;
                        default:
                            break;
                    }
                    // $rootScope.page.loaded = true;
                }, function (res) {

                });

                //$scope.getDriversTable = function () {
                //    var resource = $resource(appsettings.apiBaseUrl + "Driver/GetDrivers");
                //    $scope.dtDrivers = new NgTableParams($scope.driverTable, {
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
                //$scope.tableRows = function (index) {
                //    if (index === null) return;
                //    var result = (($scope.dtDrivers.page() - 1) * $scope.dtDrivers.count()) + (index + 1);
                //    return result;
                //};
            };

            $scope.getDriver = function (id) {
                //$rootScope.page.loaded = false;
                let promise = httpService.httpGet('Driver/' + id, null, { 'Content-Type': 'application/json' });

                promise.then(function (res) {
                    switch (res.status) {
                        case 200:
                            $scope.driver.obj = res.data;
                            var from = moment($scope.driver.obj.fromTime).format("HH:mm");
                            var to = moment($scope.driver.obj.toTime).format("HH:mm");
                            $timeout(function () {
                                $("#fromtime").val(from);
                                $("#totime").val(to);
                            }, 100)
                            if ($scope.driver.obj.workTime.toString() == "2" && $scope.driver.obj.selecteddays != null && $scope.driver.obj.selecteddays.length > 0) {
                                for (var i = 0; i < $scope.driver.obj.selecteddays.length; i++) {
                                    $scope.Days.filter(a => a.ID == $scope.driver.obj.selecteddays[i])[0].checked = true;
                                }
                            }
                            $scope.driver.obj.socialStatus = $scope.driver.obj.socialStatus.toString();
                            $scope.driver.obj.workTime = $scope.driver.obj.workTime.toString();
                            $scope.attachment.data = $scope.driver.obj.attachments ;
                            break;
                        default:
                            break;
                    }
                    $scope.changeTab(3);
                    // $rootScope.page.loaded = true;
                }, function (res) {

                });
            };

            $scope.deleteDriver = function (id) {
                //$rootScope.page.loaded = false;
                let promise = httpService.httpPost('Driver/DeleteDriver', id, { 'Content-Type': 'application/json' });

                promise.then(function (res) {
                    switch (res.status) {
                        case 200:
                            $scope.getDriversTable();
                            break;
                        default:
                            break;
                    }
                    // $rootScope.page.loaded = true;
                }, function (res) {

                });
            };
            $scope.activeDriver = function (id) {
                //$rootScope.page.loaded = false;
                let promise = httpService.httpPost('Driver/ActiveDriver', id, { 'Content-Type': 'application/json' });

                promise.then(function (res) {
                    switch (res.status) {
                        case 200:
                            $scope.getDriversTable();
                            break;
                        default:
                            break;
                    }
                    // $rootScope.page.loaded = true;
                }, function (res) {

                });
            };
            //#endregion
            //#region attachment
            $scope.attachment = {
                data: null,
            };
            $scope.deleteAttachment = function (id) {
                if (!id) return;
                let promise = httpService.httpPost('attachment/DeleteAttachment',
                  id
                ,
                    { 'Content-Type': 'application/json' });
                promise.then(function (res) {
                    switch (res.status) {
                        case 200:
                            $scope.getAttachment();
                            break;
                        default:
                            break;
                    }
                }, function (res) {
                    commonService.redirect();
                });
            };

            $scope.getAttachment = function () {
                let promise = httpService.httpGet('attachment/GetAttachmentByModule?ModuleID=' + $scope.driver.obj.driverID+'&ModuleType=3', null, { 'Content-Type': 'application/json' });
                promise.then(function (res) {
                    switch (res.status) {
                        case 200:
                            $scope.attachment.data  = res.data;
                            break;
                        default:
                            break;
                    }
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
                    $scope.driver.myDropzone = this;
                },

                dictDefaultMessage: 'Drop files here or click to upload.',
                dictRemoveFile: "Remove"
            };
            $scope.dzCallbacks = {

                'addedfile': function (file) {

                },
                'sendingmultiple': function (file, xhr, formData) {
                    var data = {
                        ModuleTypeID: 3,//Driver
                        CreatorID: 5,
                        GroupID: 0,
                        ModuleID: $scope.driver.driverid,
                        Path: "~/userfiles/Driver/",
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
        }]);
})
    (angular.module("iDeliver"));