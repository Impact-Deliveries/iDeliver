(function (app) {
    'use strict';
    app.controller('Branch', ['$scope', '$rootScope', '$log', 'httpService', 'commonService', 'appsettings', 'NgTableParams', '$resource', '$timeout',
        function ($scope, $rootScope, $log, httpService, commonService, appsettings, NgTableParams, $resource, $timeout) {
            $scope.branch = {
                isvalid: false,
                tabs: 1,
                branchid: 0,
                showResult: false,
                myDropzone: null,
                showDropzone: true,
                obj: {
                    Id: null,
                    MerchantId: '',
                    BranchName: '',
                    Overview: '',
                    Mobile: '',
                    Phone: '',
                    BranchPicture: '',
                    LocationId: 0,
                    Address: '',
                    Latitude: '',
                    Longitude: '',
                    IsActive: true,
                    deliveryStatus: '1',
                    deliveryPriceOffer: 0,
                    Attachments: [],
                },
            };

            //#region branch
            $scope.getbranchsTable = function () {
                $scope.branch.isvalid = true;
                if ($scope.merchant.selected == null || $scope.merchant.selected == '0') {
                    return;
                }
                let promise = httpService.httpGet('MerchantBranch/GetBranchesByMerchantID', {
                    LocationID: Number($scope.location.selected),
                    MerchantID: Number($scope.merchant.selected),
                    IsActive: $scope.branchTable.objects.IsActive
                });
                promise.then(function (res) {
                    switch (res.status) {
                        case 200:
                            $scope.branchTable.data = res.data;
                            $scope.branch.showResult = true;
                            break;
                        default:
                            break;
                    }
                }, function (res) {

                });
            };

            $scope.getbranch = function (id) {
                //$rootScope.page.loaded = false;
                let promise = httpService.httpGet('MerchantBranch/' + id, null, { 'Content-Type': 'application/json' });
                promise.then(function (res) {
                    switch (res.status) {
                        case 200:
                            $scope.branch.obj.Id = res.data.id;
                            $scope.branch.obj.merchantId = res.data.merchantId;
                            $scope.branch.obj.BranchName = res.data.branchName;
                            $scope.branch.obj.Overview = res.data.overview;
                            $scope.branch.obj.Address = res.data.address;
                            $scope.branch.obj.Phone = Number(res.data.phone);
                            $scope.branch.obj.Mobile = Number(res.data.mobile);
                            $scope.branch.obj.Longitude = res.data.longitude;
                            $scope.branch.obj.Latitude = res.data.latitude;
                            $scope.location.selected = res.data.locationId.toString();
                            $scope.merchant.selected = res.data.merchantId.toString();
                            $scope.branch.obj.attachments = res.data.attachments;
                            $scope.deliveryPrice.selected = $scope.branch.obj.deliveryStatus.toString();
                            $scope.deliveryPrice.data = res.data.deliveryPrice;
                            $scope.CalculatePrices($scope.deliveryPrice.selected);
                            $scope.deliveryPrice.isvalid = false;
                            $scope.branch.obj.deliveryPriceOffer = res.data.deliveryPriceOffer;
                            if ($scope.branch.obj.attachments != null && $scope.branch.obj.attachments.length > 0) {
                                $scope.branch.showDropzone = false;
                            } else {
                                $scope.branch.showDropzone = true;

                            }
                            $scope.attachment.data = $scope.branch.obj.attachments;
                            $scope.initMap(Number($scope.branch.obj.Latitude), Number($scope.branch.obj.Longitude));
                            $timeout(function () {
                                var myLatlng = new google.maps.LatLng(Number($scope.branch.obj.Latitude), Number($scope.branch.obj.Longitude));
                                $scope.placeMarker($scope.google.map, myLatlng);

                            }, 300);


                            $scope.merchant.selected = res.data.merchantId.toString();
                            break;
                        default:
                            break;
                    }
                    $scope.changeTab(3);
                    // $rootScope.page.loaded = true;
                }, function (res) {

                });
            };

            $scope.$watch('merchant.selected + location.selected +branchTable.objects.IsActive', function (newvalue, oldvalue) {
                $scope.branch.showResult = false;
            });
            $scope.changeTab = function (i) {
                $scope.branch.tabs = i;

                $scope.branch.isvalid = false;
                switch (i) {
                    case 1:
                        $scope.location.selected = "0";
                        $scope.merchant.selected = "0";
                        break;
                    case 2:
                        $scope.branch.obj.Id = null,
                            $scope.branch.obj.MerchantId = '',
                            $scope.branch.obj.BranchName = '',
                            $scope.branch.obj.Overview = '',
                            $scope.branch.obj.Mobile = '',
                            $scope.branch.obj.Phone = '',
                            $scope.branch.obj.BranchPicture = '',
                            $scope.branch.obj.LocationId = 0,
                            $scope.branch.obj.Address = '',
                            $scope.branch.obj.Latitude = '',
                            $scope.branch.obj.Longitude = '',
                            $scope.branch.obj.IsActive = true,
                            $scope.branch.obj.Attachments = []
                        $scope.location.selected = "0";
                        $scope.merchant.selected = "0";
                        $scope.initMap(31.972907, 35.9092202);

                        break
                    case 4:
                        break;
                    default:
                }
            };
            $scope.branchTable = {
                page: 1,
                count: 10,
                data: null,
                objects: {
                    branchID: "0",
                    IsActive: "true",
                    branchName: null,
                    Mobile: "0",
                }
            };
            $scope.submit = function () {
                $scope.branch.isvalid = true;
                $scope.branch.obj.merchant = $scope.merchant.selected;
                if ($scope.merchant.selected == null || $scope.merchant.selected == '0') {
                    return;
                }
                if ($scope.location.selected == null || $scope.location.selected == '0') {
                    return;
                }
                if ($scope.branch.obj.Overview == '' || $scope.branch.obj.BranchName == ''
                    || $scope.branch.obj.Mobile == '' || $scope.branch.obj.Phone == ''
                    || $scope.branch.obj.Mobile == '0' || $scope.branch.obj.Phone == '0' || $scope.branch.obj.Address == '') {
                    return;
                }
                if ($scope.branch.obj.Latitude == '' || $scope.branch.obj.Longitude == '') {
                    return;
                }
                $scope.branch.obj.deliveryStatus = $scope.deliveryPrice.selected
                $scope.branch.obj.LocationId = $scope.location.selected;
                $scope.branch.obj.MerchantId = $scope.merchant.selected;
                let promise = httpService.httpPost('MerchantBranch/AddMerchantBranch',
                    $scope.branch.obj,
                    { 'Content-Type': 'application/json' });
                promise.then(function (res) {
                    switch (res.status) {
                        case 200:
                            $scope.branch.branchid = res.data.id;
                            $scope.branch.obj.Id = res.data.id;
                            if ($scope.branch.myDropzone != null && $scope.branch.myDropzone.files.length > 0) {
                                $scope.branch.myDropzone.processQueue();
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
            $scope.activebranch = function (id) {
                //$rootScope.page.loaded = false;
                let promise = httpService.httpPost('MerchantBranch/ActiveMerchantBranch', id, { 'Content-Type': 'application/json' });

                promise.then(function (res) {
                    switch (res.status) {
                        case 200:
                            $scope.getbranchsTable();
                            break;
                        default:
                            break;
                    }
                    // $rootScope.page.loaded = true;
                }, function (res) {

                });
            };
            //#endregion
            //#region location
            $scope.location = {
                data: null,
                selected: "0"
            };
            $scope.getLocation = function () {
                let promise = httpService.httpGet('Location/GetLocations', null, { 'Content-Type': 'application/json' });
                promise.then(function (res) {
                    switch (res.status) {
                        case 200:
                            $scope.location.data = res.data;
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

            //#region Price
            $scope.deliveryPrice = {
                selected: '1',
                deliveryPriceOffer: 0.0,
                data: null,
                locations: [],
                distance: [],
                isvalid: false
            };
            $scope.CalculatePrices = function (value) {
                switch (value) {
                    case '1':
                        let objs = [];

                        if (!$scope.location.data) {
                            $scope.deliveryPrice.locations = [];
                            return;
                        }
                        for (var i = 0; i < $scope.location.data.length; i++) {
                            var amount = '', id = 0, location = null, valid = false;
                            if ($scope.deliveryPrice.data) {
                                location = $scope.deliveryPrice.data.filter(a => a.locationId == $scope.location.data[i].id)[0];
                                if (location) {
                                    amount = location.amount;
                                    valid = true;
                                    id = location.id
                                }
                            }

                            let obj = {
                                amount: amount,
                                locationId: $scope.location.data[i].id,
                                merchantBranchId: $scope.branch.obj.Id,
                                address: $scope.location.data[i].address,
                                valid: valid,
                                toDistance: null,
                                fromDistance: null,
                                id: id
                            };
                            objs.push(obj);
                        }
                        $scope.deliveryPrice.locations = objs;
                        break;
                    case '2':
                        if ($scope.deliveryPrice.data) {
                            let Dobjs = [];
                            let distance = $scope.deliveryPrice.data.filter(a => a.toDistance != null && a.fromDistance != null);
                            if (distance && distance.length > 0) {
                                for (var j = 0; j < distance.length; j++) {

                                    let obj = {
                                        amount: distance[j].amount,
                                        toDistance: distance[j].toDistance,
                                        fromDistance: distance[j].fromDistance,
                                        merchantBranchId: $scope.branch.obj.Id,
                                        valid: true,
                                        address: null,
                                        locationId: null,
                                        fromValid: true,
                                        toValid: true,
                                        id: distance[j].id
                                    };
                                    Dobjs.push(obj);
                                }
                                $scope.deliveryPrice.distance = Dobjs;
                            } else {
                                $scope.deliveryPrice.distance = [];
                            }
                        }
                        break;
                    default:
                }
            };


            $scope.$watch('deliveryPrice.selected', function (newvalue, oldvalue) {
                $scope.CalculatePrices(newvalue);

            });

            $scope.setDeliveryPrice = function (index, i) {
                switch (i) {
                    case 1://amount
                        if ($scope.deliveryPrice.locations[index].amount != null && $scope.deliveryPrice.locations[index].amount != '') {
                            $scope.deliveryPrice.locations[index].valid = true;
                        } else {
                            $scope.deliveryPrice.locations[index].valid = false;
                        }
                        break;
                    case 2://from distance
                        if ($scope.deliveryPrice.distance[index].fromDistance != null && $scope.deliveryPrice.distance[index].fromDistance != '') {
                            $scope.deliveryPrice.distance[index].fromValid = true;
                        } else {
                            $scope.deliveryPrice.distance[index].fromValid = false;
                        }
                        break;
                    case 3://to distance
                        if ($scope.deliveryPrice.distance[index].toDistance != null && $scope.deliveryPrice.distance[index].toDistance != '') {
                            $scope.deliveryPrice.distance[index].toValid = true;
                        } else {
                            $scope.deliveryPrice.distance[index].toValid = false;
                        }
                        break;
                    case 4://amount distance
                        if ($scope.deliveryPrice.distance[index].amount != null && $scope.deliveryPrice.distance[index].amount != '') {
                            $scope.deliveryPrice.distance[index].valid = true;
                        } else {
                            $scope.deliveryPrice.distance[index].valid = false;
                        }
                        break;
                    default:
                }
                // $scope.deliveryPrice.data.filter(a => a.locationId == locationID)[0].valid = true;


            };

            $scope.SaveDeliveryPrices = function () {
                $scope.deliveryPrice.isvalid = true;
                debugger;
                let object = {
                    MerchantBranchID: $scope.branch.obj.Id,
                    Amount: 0,
                    DeliveryStatus: $scope.deliveryPrice.selected,
                    DeliveryPrice: []

                }

                switch ($scope.deliveryPrice.selected) {
                    case '1'://location
                        var loc = $scope.deliveryPrice.locations.filter(a => a.valid == false)
                        if (loc == null || loc.length > 0) return;
                        for (var i = 0; i < $scope.deliveryPrice.locations.length; i++) {
                            let obj = {
                                Id: null,
                                MerchantBranchId: null,
                                LocationId: $scope.deliveryPrice.locations[i].locationId,
                                FromDistance: null,
                                ToDistance: null,
                                Amount: $scope.deliveryPrice.locations[i].amount,
                            };
                            object.DeliveryPrice.push(obj);
                        }

                        break;
                    case '2'://distance
                        var dis = $scope.deliveryPrice.distance.filter(a => a.fromValid == false || a.toValid == false || a.valid == false );
                        if (dis == null || dis.length > 0) return;
                        for (var i = 0; i < $scope.deliveryPrice.distance.length; i++) {
                            let obj = {
                                Id: null,
                                MerchantBranchId: null,
                                LocationId: null,
                                FromDistance: $scope.deliveryPrice.distance[i].fromDistance,
                                ToDistance: $scope.deliveryPrice.distance[i].toDistance,
                                Amount: $scope.deliveryPrice.distance[i].amount,
                            };
                            object.DeliveryPrice.push(obj);
                        }

                        break;
                    case '3'://offer
                        if (!$scope.deliveryPrice.deliveryPriceOffer || $scope.deliveryPrice.deliveryPriceOffer == '') return;
                        object.Amount = $scope.deliveryPrice.deliveryPriceOffer;
                        break;
                    default:
                        break;
                }

                let promise = httpService.httpPost('MerchantDeliveryPrice/SaveDeliveryPrices',
                    object,
                    { 'Content-Type': 'application/json' });
                promise.then(function (res) {
                    switch (res.status) {
                        case 200:

                            $scope.changeTab(3);
                            break;
                        default:
                            break;
                    }
                }, function (res) {
                    commonService.redirect();
                });
            };
            $scope.distanceActions = function (action, index) {
                switch (action) {
                    case 1://add
                        $scope.deliveryPrice.distance.push({
                            amount: '',
                            toDistance:'',
                            fromDistance:'',
                            merchantBranchId: $scope.branch.obj.Id,
                            valid: false,
                            address: null,
                            locationId: null,
                            fromValid: false,
                            toValid: false,
                            id: 0
                        });
                        break;
                    case 2://delete
                        let obj = [];
                        for (var i = 0; i < $scope.deliveryPrice.distance.length; i++) {
                            if (i != index ) {
                                obj.push($scope.deliveryPrice.distance[i]);
                            }
                        }
                        $scope.deliveryPrice.distance = obj;
                        break;
                    default:
                }
            }
            //#endregion
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
                let promise = httpService.httpGet('attachment/GetAttachmentByModule?ModuleID=' + $scope.branch.obj.Id + '&ModuleType=4', null, { 'Content-Type': 'application/json' });
                promise.then(function (res) {
                    switch (res.status) {
                        case 200:
                            $scope.attachment.data = res.data;
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
                acceptedMimeTypes: '.jpeg,.jpg,.png,.gif,.svg',
                maxFiles: 1,
                parallelUploads: 1,
                uploadMultiple: true,
                autoProcessQueue: false,
                addRemoveLinks: true,
                init: function () {
                    $scope.branch.myDropzone = this;
                },

                dictDefaultMessage: 'Drop files here or click to upload.',
                dictRemoveFile: "Remove"
            };
            $scope.dzCallbacks = {

                'addedfile': function (file) {

                },
                'sendingmultiple': function (file, xhr, formData) {
                    debugger;
                    var data = {
                        ModuleTypeID: 4,//Merchant Branch
                        CreatorID: 1,
                        GroupID: 0,
                        ModuleID: $scope.branch.obj.Id,
                        Path: "~/userfiles/MerchantBranch/",
                    };

                    formData.append("data", JSON.stringify(data));
                },
                'success': function (file, xhr) {
                    $scope.changeTab(1);
                },
                'removedfile': function (file, xhr) {
                    $scope.reomveAttachmentItem(file);
                },
                'maxfilesexceeded': function (file) {
                },
                'error': function (file, xhr) {
                    $scope.dzMethods.removeFile(file);
                }
            };

            $scope.dzMethods = {};

            //$scope.removeAllAttachmentItems = function () {

            //};

            //$scope.reomveAttachmentItem = function (file) {
            //}
            //#endregion

            //#region map
            $scope.google = {
                map: null,
                markers: [],

            };
            $scope.initMap = function (lat, lng) {
                $timeout(function () {
                    $scope.google.map = new google.maps.Map(document.getElementById("map"), {
                        center: { lat: lat, lng: lng },
                        zoom: 14,
                    });
                    google.maps.event.addListener($scope.google.map, 'click', function (event) {
                        $scope.placeMarker($scope.google.map, event.latLng);
                    });
                }, 300);



            };
            $scope.placeMarker = function (map, location) {
                const marker = new google.maps.Marker({
                    position: location,
                    map: map,
                });
                if ($scope.google.markers != null && $scope.google.markers.length > 0) {
                    $scope.deleteMarkers();
                };

                $scope.google.markers.push(marker);
                marker.setMap(null);
                $timeout(function () {
                    $scope.branch.obj.Latitude = location.lat().toString();
                    $scope.branch.obj.Longitude = location.lng().toString();
                    marker.setMap($scope.google.map);
                }, 10);

                //var infowindow = new google.maps.InfoWindow({
                //    content: 'Latitude: ' + location.lat() +
                //        '<br>Longitude: ' + location.lng()
                //});
                //infowindow.open(map, marker);
            }

            $scope.setMapOnAll = function (map) {
                for (var i = 0; i < $scope.google.markers.length; i++) {
                    $scope.google.markers[i].setMap(map);
                }
            }

            // Removes the markers from the map, but keeps them in the array.
            $scope.clearMarkers = function () {
                $scope.setMapOnAll(null);
            }

            // Shows any markers currently in the array.
            $scope.showMarkers = function () {
                $scope.setMapOnAll(map);
            }

            // Deletes all markers in the array by removing references to them.
            $scope.deleteMarkers = function () {
                $scope.clearMarkers();
                $scope.google.markers = [];
            }
            //#endregion

            $scope.changeTab(1);
            $scope.getLocation();
            $scope.getMerchant();
        }]);
})
    (angular.module("iDeliver"));