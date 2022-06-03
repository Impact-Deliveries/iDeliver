(function (app) {
    'use strict';

    app.controller('dashboardCtrl', ['$scope', '$rootScope', '$log', 'httpService', 'commonService', '$timeout', 'appsettings',
        function ($scope, $rootScope, $log, httpService, commonService, $timeout, appsettings) {
            $scope.braches = {
                data: null,
                length: 0
            }
            $scope.drivers = {
                data: null,
                length: 0
            }

            //#region map
            $scope.google = {
                map: null,
                markers: [],

            };
            $scope.initMap = function (lat, lng) {
                $timeout(function () {
                    $scope.google.map = new google.maps.Map(document.getElementById("map"), {
                        center: { lat: lat, lng: lng },
                        zoom: 12,
                    });

                }, 10);
            };
            $scope.GetActiveBranches = function () {
                if (!$rootScope.page.PageID == "1" || !$rootScope.page.PageID) return;
                let promise = httpService.httpGet('MerchantBranch/GetActiveBranches', null, { 'Content-Type': 'application/json' });
                promise.then(function (res) {
                    switch (res.status) {
                        case 200:
                            $scope.braches.data = res.data;
                            $scope.braches.length = res.data.length;
                           const svgMarker = {
                           
                              // path: "M10.453 14.016l6.563-6.609-1.406-1.406-5.156 5.203-2.063-2.109-1.406 1.406zM12 2.016q2.906 0 4.945 2.039t2.039 4.945q0 1.453-0.727 3.328t-1.758 3.516-2.039 3.070-1.711 2.273l-0.75 0.797q-0.281-0.328-0.75-0.867t-1.688-2.156-2.133-3.141-1.664-3.445-0.75-3.375q0-2.906 2.039-4.945t4.945-2.039z",
                               fillColor: "yellow",
                               fillOpacity: 1,
                               strokeWeight: 0,
                               rotation: 0,
                               scale: 2,
                               anchor: new google.maps.Point(15, 30),
                           };
                            // var icon = appsettings.baseUrl + 'userfiles/icon-restaurant.jpg';
                            $timeout(function () {
                                for (var i = 0; i < res.data.length > 0; i++) {
                                    var myLatlng = new google.maps.LatLng(Number(res.data[i].latitude), Number(res.data[i].longitude));
                                    const marker = new google.maps.Marker({
                                        position: myLatlng,
                                        map: map,
                                        icon: svgMarker,
                                        title: res.data[i].merchantName + ' ' + res.data[i].branchName + ' (' + res.data[i].phone + ')',
                                        //optimized: true
                                    });

                                    $scope.google.markers.push(marker);
                                    marker.setMap($scope.google.map);
                                }
                                $scope.getDriverLocations();
                            }, 10);
                            break;
                        default:

                            break;
                    }
                }, function (res) {

                });
            };
            $scope.getDriverLocations = function () {
                if ($rootScope.page.PageID != "1" || !$rootScope.page.PageID) return;
                let promise = httpService.httpGet('DriverCase/GetOnlineDrivers', null, { 'Content-Type': 'application/json' });
                promise.then(function (res) {
                    switch (res.status) {
                        case 200:
                            $scope.drivers.data = res.data;
                            $scope.deleteMarkers();
                            for (var i = 0; i < res.data.length > 0; i++) {
                                var myLatlng = new google.maps.LatLng(Number(res.data[i].latitude), Number(res.data[i].longitude));
                                $scope.placeMarker($scope.google.map, myLatlng, res.data[i]);
                            }
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
            $scope.placeMarker = function (map, location, obj) {
                var color = "green";
                if (obj.status==2) {
                    color = "red";
                }
                const svgMarker = {

                    path: "M10.453 14.016l6.563-6.609-1.406-1.406-5.156 5.203-2.063-2.109-1.406 1.406zM12 2.016q2.906 0 4.945 2.039t2.039 4.945q0 1.453-0.727 3.328t-1.758 3.516-2.039 3.070-1.711 2.273l-0.75 0.797q-0.281-0.328-0.75-0.867t-1.688-2.156-2.133-3.141-1.664-3.445-0.75-3.375q0-2.906 2.039-4.945t4.945-2.039z",
                    fillColor: color,
                    fillOpacity: 1,
                    strokeWeight: 0,
                    rotation: 0,
                    scale: 2,
                    anchor: new google.maps.Point(15, 30),
                };

                const marker = new google.maps.Marker({
                    position: location,
                    map: map,
                    title: obj.name+ ' (' + obj.phone + ')',
                    icon: svgMarker,
                });
                // if ($scope.google.markers != null && $scope.google.markers.length > 0) {
                //     $scope.deleteMarkers();
                // };

                $scope.google.markers.push(marker);
                // marker.setMap(null);

                marker.setMap($scope.google.map);

            }

            $scope.setMapOnAll = function (map, index) {
                for (var i = index; i < $scope.google.markers.length; i++) {
                    $scope.google.markers[i].setMap(map);
                }
            }


            // Shows any markers currently in the array.
            $scope.showMarkers = function () {
                $scope.setMapOnAll($scope.google.map, 0);
            }

            // Removes the markers from the map, but keeps them in the array.
            $scope.clearMarkers = function () {
                $scope.setMapOnAll(null, $scope.braches.length);
            }

            // Deletes all markers in the array by removing references to them.
            $scope.deleteMarkers = function () {
                $scope.clearMarkers();
                //  $scope.google.markers = [];
                $scope.google.markers = $scope.google.markers.slice($scope.google.markers.length - $scope.braches.length, $scope.google.markers.length);
            }

            $scope.initMap(31.972907, 35.9092202);
            $scope.GetActiveBranches();

            //#endregion



        }]);
})(angular.module("iDeliver"));