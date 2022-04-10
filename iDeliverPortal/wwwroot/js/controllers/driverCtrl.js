(function (app) {
    'use strict';
    app.controller('Driver', ['$scope', '$rootScope', '$log', 'httpService', 'commonService', 'appsettings', 'NgTableParams', '$resource',
        function ($scope, $rootScope, $log, httpService,commonService, appsettings, NgTableParams, $resource) {
            $scope.SocialStatus = [
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
                    firstname: '',
                    middlename: '',
                    lastname: '',
                    address: '',
                    mobile2: null,
                    mobile: null,
                    birthday: moment(new Date()).format("DD-MM-yyyy"),
                    SocialStatus: "0",
                    isHaveProblem: false,
                    reason: null,
                    WorkTime: "0",
                    fromTime: null,
                    toTime: null,
                    startJob: moment(new Date()).format("DD-MM-yyyy"),
                    college: '',
                    university: '',
                    major: '',
                    graduationyear: '',
                    estimate: '',
                    avancedstudies: '',
                    selecteddays: []
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
                objects: {
                    DriverID: "0",
                    IsActive: "1",
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
            }

            $scope.Submit = function () {
                let totaldays = $scope.Days.filter(a => a.checked == true);
                $scope.driver.isvalid = true;
                if ($scope.driver.obj.firstname == '' || $scope.driver.obj.middlename == '' ||
                    $scope.driver.obj.lastname == '' || $scope.driver.obj.address == '' ||
                    $scope.driver.obj.birthday == '' || $scope.driver.obj.SocialStatus == "0" || $scope.driver.obj.mobile == '') {
                    return;
                }
                if ($scope.driver.obj.isHaveProblem == null || ($scope.driver.obj.isHaveProblem == true
                    && $scope.driver.obj.reason == '')
                    || $scope.driver.obj.WorkTime == "0" || $scope.driver.obj.startJob == '') {
                    return;
                }
                if ($scope.driver.obj.WorkTime == '2' && totaldays == null && totaldays.length == 0
                    || moment($scope.driver.obj.fromTime).format("HH:mm:ss a") > moment($scope.driver.obj.toTime).format("HH:mm:ss a")
                ) {
                    return;
                }
                $scope.driver.obj.selecteddays = $scope.Days.filter(a => a.checked == true).map(t => t.ID);
                $scope.driver.obj.birthday = moment($scope.driver.obj.birthday).format("DD-MM-yyyy")
                $scope.driver.obj.startJob = moment($scope.driver.obj.startJob).format("DD-MM-yyyy")
                let promise = httpService.httpPost('Driver/AddOrEditDriver',
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
            $scope.getDriversTable = function () {
                var resource = $resource(appsettings.apiBaseUrl + "Driver/GetDrivers");
                $scope.dtDrivers = new NgTableParams($scope.driverTable, {
                    filterDelay: 300,
                    total: 1,
                    getData: function ($defer) {
                       // $scope.result.processing = true;
                        // request to api
                        return resource.get($defer.url()).$promise.then(function (response) {
                            // $scope.result.processing = false;
                            $defer.total(response.total);
                            return response.results;
                        }).catch(function (response) {
                            //$scope.result.processing = false;
                        });
                    }
                });
                $scope.tableRows = function (index) {
                    if (index === null) return;
                    var result = (($scope.dtDrivers.page() - 1) * $scope.dtDrivers.count()) + (index + 1);
                    return result;
                };
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
                    //    $scope.homework.model.attachments.push(file.upload);

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
            //    if ($scope.homework.myDropzone && $scope.homework.model.attachments.length > 0) {
            //        $scope.homework.model.attachments = [];
            //        $scope.dzMethods.removeAllFiles();
            //    }
            //};

            //$scope.reomveAttachmentItem = function (file) {
            //    var index = -1;
            //    for (var i = 0; i < $scope.homework.model.attachments.length; i++) {
            //        if ($scope.homework.model.attachments[i].uuid === file.upload.uuid) {
            //            index = i;
            //            break;
            //        }
            //    }

            //    if (index != -1)
            //        $scope.homework.model.attachments.splice(index, 1);
            //}
            //#endregion
            $scope.changeTab(1);

        }]);
})
    (angular.module("iDeliver"));