﻿(function () {
    'use strict';

    window.app
        .controller('homeController', ['$scope', 'calendarService', homeController]);

    function homeController($scope, calendarService) {
        $scope.title = 'homeController';

        calendarService.Events.query(function (events) {
            $scope.events = events;
            var interests = [];
            var locations = [];
            for (var i = events.length - 1; i >= 0; i--) {
                for (var j = events[i].interests.length - 1; j >= 0; j--) {
                    if ($.inArray(events[i].interests[j], interests) === -1) {
                        interests.push(events[i].interests[j]);
                    }
                }
                if ($.inArray(events[i].location, locations) === -1) {
                    locations.push(events[i].location);
                }
            };
            $scope.interests = interests;
            $scope.locations = locations;
            $scope.search = {
                interests: '',
                location: ''
            };
        });
    }
})();
