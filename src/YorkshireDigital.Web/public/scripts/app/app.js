﻿(function () {
    'use strict';

    window.app = angular.module('yorkshireDigitalApp', [
        'ngRoute',
        'ngResource',
        'tien.clndr',
        'ui.utils'
    ]);

    app.config([
        '$routeProvider', '$locationProvider', '$httpProvider', function($routeProvider, $locationProvider, $httpProvider) {
            $httpProvider.defaults.useXDomain = true;
            delete $httpProvider.defaults.headers.common['X-Requested-With'];
            $httpProvider.defaults.useXDomain = true;

            $routeProvider
                .when('/', { templateUrl: '/public/views/home/Home.html', controller: 'homeController' })
                .when('/event/:eventName?', { templateUrl: '/public/views/home/Home.html', controller: 'homeController' })
                .when('/Archive/Newsletter', { templateUrl: '/public/views/mailinglist/Archive.html', controller: 'mailinglistController' })
                .when('/Error', { templateUrl: '/public/views/shared/Error.html' })
                .when('/404', { templateUrl: '/public/views/shared/404.html' })

                .otherwise({
                    redirectTo: '/404'
                });

            $locationProvider.html5Mode(true);
        }
    ]);
})();