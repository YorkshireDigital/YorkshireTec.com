(function () {
    'use strict';

    window.app = angular.module('yorkshireDigitalApp', [
        'ngRoute',
        'ngResource'
    ]);

    app.config([
        '$routeProvider', '$locationProvider', '$httpProvider', function($routeProvider, $locationProvider, $httpProvider) {
            $httpProvider.defaults.useXDomain = true;
            delete $httpProvider.defaults.headers.common['X-Requested-With'];
            $httpProvider.defaults.useXDomain = true;

            $routeProvider
                .when('/', { templateUrl: '/Scripts/app/views/home/Home.html', controller: 'homeController' })
                .when('/Archive/Newsletter', { templateUrl: '/Scripts/app/views/mailinglist/Archive.html', controller: 'mailinglistController' })
                .when('/Error', { templateUrl: '/Scripts/app/views/shared/Error.html' })
                .when('/404', { templateUrl: '/Scripts/app/views/shared/404.html' })


                .otherwise({
                    redirectTo: '/404'
                });

            $locationProvider.html5Mode(true);
        }
    ]);
})();