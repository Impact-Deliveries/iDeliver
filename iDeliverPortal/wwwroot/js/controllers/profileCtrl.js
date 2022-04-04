﻿(function (app) {
    'use strict';

    app.controller('profileCtrl', ['$scope', '$rootScope', '$log', 'httpService', 'commonService',
        function ($scope, $rootScope, $log, httpService, commonService) {
            $scope.profile = null;

            $scope.loggedin = function () {
                let promise = httpService.httpGet('Authentication/loggedin', null, { 'Content-Type': 'application/json' });

                promise.then(function (res) {
                    switch (res.status) {
                        case 200:
                            $scope.profile = res.data;
                            $rootScope.page.loaded = true;
                            break;
                        default:
                            commonService.redirect();
                            break;
                    }
                }, function (res) {
                    commonService.redirect();
                });
            };

            $scope.logout = function () {
                $rootScope.page.profile = null;
                commonService.redirect();
            };

            $scope.modules = [{
                id: 1,
                name: "dashboard",
                active: true,
                icon: "fa fa-tachometer-alt",
                child: []
            },
            {
                id: 2,
                name: "roles",
                active: false,
                icon: "fas fa-user-tag",
                child: [
                    {
                        id: 3,
                        name: "merchants",
                        active: false,
                        icon: "fas fa-store me-2"
                    },
                    {
                        id: 4,
                        name: "drivers",
                        active: false,
                        icon: "fas fa-car me-2"
                    }
                ]
            }];

            $scope.goto = function (id) {
                if (!id) return;

                let isExist = $scope.modules.some(v => v.id === id && v.active === true);
                if (!isExist) {
                    $scope.modules.filter(f => f.active === true)[0].active = false;
                    $scope.modules.filter(f => f.id === id)[0].active = true;
                }
            };



            $scope.init = function () {
                $rootScope.page.loaded = false;
                $scope.loggedin();
            };

            $scope.init();
        }]);

})(angular.module("iDeliver"));