(function () {
    "use strict";
    var app = angular.module("iDeliver", ["720kb.datepicker", 'ui.bootstrap', 'thatisuday.dropzone', 'ngSanitize', 'ngTable', 'ngResource']);

    app.constant("appsettings", {
        apiBaseUrl: "http://localhost:5117/api/",
        baseUrl: "http://localhost:5050/",
        cdnUrl: "http://localhost:5050/"
    });

    app.config(['$httpProvider', function ($httpProvider) {
        $httpProvider.interceptors.push('interceptor');
    }]);

    app.run(function ($http, $log, appsettings, $rootScope) {
        $rootScope.page = {
            loaded: false,
            baseUrl: appsettings.baseUrl
        };
    });
})();