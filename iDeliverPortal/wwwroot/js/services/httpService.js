(function (app) {
    'use strict';
    app.factory('httpService', ['$http', '$log', 'appsettings', function ($http, $log, appsettings) {
        this.httpPost = function (url, data, headers) {
            let response = $http({
                url: appsettings.apiBaseUrl + url,
                method: 'POST',
                data: data,
                headers: headers
            });

            return response;
        };

        this.httpGet = function (url, params, headers) {
            let response = $http({
                url: appsettings.apiBaseUrl + url,
                method: 'GET',
                params: params,
                headers: headers
            });

            return response;
        };

        return {
            httpPost: this.httpPost,
            httpGet: this.httpGet
        };
    }]);
})(angular.module("iDeliver"));