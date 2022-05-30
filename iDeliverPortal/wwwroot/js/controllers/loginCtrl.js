(function (app) {
    'use strict';

    app.controller('loginCtrl', ['$scope', '$rootScope', '$log', 'httpService', 'commonService',
        function ($scope, $rootScope, $log, httpService, commonService) {
            $scope.form = {
                fields: {
                    username: '',
                    password: '',
                    show: {
                        password: false
                    }
                },
                authenticated: null,
                status: null,
                message: null
            };

            $scope.submit = function (event) {
                $rootScope.page.loaded = false;

                let promise = httpService.httpPost('Authentication/login', {
                    username: $scope.form.fields.username,
                    password: $scope.form.fields.password
                }, { 'Content-Type': 'application/json' });

                promise.then(function (res) {
                    $scope.form.status = res.status;
                    switch (res.status) {
                        case 200:
                            $scope.form.authenticated = true;
                            sessionStorage.setItem('token', res.data.token);
                            commonService.redirect('home/index');

                            break;
                        default:
                            $scope.form.authenticated = false;
                            $scope.form.message = res.data;
                            break;
                    }
                    $rootScope.page.loaded = true;
                }, function (res) {
                    $scope.form.status = res.status;
                    $scope.form.authenticated = false;
                    $scope.form.message = res.data;
                    $rootScope.page.loaded = true;
                });
            }

            $scope.init = function () {
                $rootScope.page.loaded = true;

                var token = sessionStorage.getItem('token');

                if (token)
                    commonService.redirect('home/index');
            };

            $scope.init();
        }]);

})(angular.module("iDeliver"));