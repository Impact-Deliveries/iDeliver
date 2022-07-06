(function () {
    "use strict";
    var app = angular.module("iDeliver", ["720kb.datepicker", 'ui.bootstrap', 'thatisuday.dropzone', 'ngSanitize', 'ngTable', 'ngResource']);

    app.constant("appsettings", {
       //apiBaseUrl: "http://localhost:5117/api/",
       //baseUrl: "http://localhost/iDeliverPortal/",
       //cdnUrl: "http://localhost/iDeliverPortal/"

        //apiBaseUrl: "http://10.0.0.234/iDeliveryService/api/",
        //baseUrl: "http://10.0.0.234/iDeliverPortal/",
        //cdnUrl: "http://10.0.0.234/iDeliverPortal/"

        //apiBaseUrl: "http://abdawwad1-001-site1.itempurl.com/api/",
        //baseUrl: "http://www.ideliveryapp.com/",
        //cdnUrl: "http://www.ideliveryapp.com/"

             apiBaseUrl: "http://localhost:5117/api/",
        baseUrl: "http://localhost/iDeliverPortal/",
        cdnUrl: "http://localhost/iDeliverPortal/"

    });


    app.config(['$httpProvider', function ($httpProvider) {
        $httpProvider.interceptors.push('interceptor');
    }]);

    app.run(function ($http, $log, appsettings, $rootScope) {
        $rootScope.page = {
            loaded: false,
            baseUrl: appsettings.baseUrl,
            PageID:0
        };
    });
})();