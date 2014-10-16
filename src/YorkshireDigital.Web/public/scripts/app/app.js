(function () {
    'use strict';

    window.app = angular.module('yorkshireDigitalApp', [
        'ngRoute',
        'ngResource',
        'tien.clndr',
        'ui.utils',
        'ui.router'
    ]);

    app.config([
        '$routeProvider', '$locationProvider', '$httpProvider', '$urlRouterProvider', function($routeProvider, $locationProvider, $httpProvider, $urlRouterProvider) {
            $httpProvider.defaults.useXDomain = true;
            delete $httpProvider.defaults.headers.common['X-Requested-With'];
            $httpProvider.defaults.useXDomain = true;
            //$urlRouterProvider.deferIntercept();

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

    app.run(['$route', '$rootScope', '$location', function ($route, $rootScope, $location) {
        var original = $location.path;
        $location.path = function (path, reload) {
            if (reload === false) {
                var lastRoute = $route.current;
                var un = $rootScope.$on('$locationChangeSuccess', function () {
                    $route.current = lastRoute;
                });
            }
            return original.apply($location, [path]);
        };
    }])
})();