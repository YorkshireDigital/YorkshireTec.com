(function () {
    'use strict';

    window.app
        .controller('homeController', ['$scope', '$sce', 'calendarService', homeController]);

    function homeController($scope, $sce, calendarService) {
        $scope.title = 'homeController';

        $scope.to_trusted = function(html_code) {
            return $sce.trustAsHtml(html_code);
        };
        $scope.clndrNextMonth = function() {
            console.log('clndrNextMonth');
            $scope.clndr.forward();
            $scope.loadEvents(moment($scope.clndr.month._d), function (events) {
                $scope.addNewEvents(events);
            });
        };
        $scope.clndrPreviousMonth = function() {
            console.log('clndrPreviousMonth');
            $scope.clndr.back();
            $scope.loadEvents(moment($scope.clndr.month._d), function (events) {
                $scope.addNewEvents(events);
            });
        };
        $scope.addNewEvents = function (events) {
            var newEvents = [];
            for (var e = events.length - 1; e >= 0; e--) {
                var found = false;
                for (var k = 0; k < $scope.events.length; k++) {
                    if (events[e].uniqueName == $scope.events[k].uniqueName) {
                        found = true;
                    }
                }
                if (found === false) {
                    newEvents.push(events[e]);
                    $scope.events.push(events[e]);
                }
            };
            $scope.populateFilters(newEvents);
            $scope.clndr.addEvents(newEvents);
            return newEvents;
        };
        $scope.loadEvents = function (month, callback) {
            var from = month.date(1).subtract(1, 'M').format('DD/MM/YYYY');
            var to = month.date(1).add(3, 'M').format('DD/MM/YYYY');

            calendarService.Events.query({ from: from, to: to }, callback);
        };

        $scope.populateFilters = function (events) {
            var interests = $scope.interests || [];
            var locations = $scope.locations || [];
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
            $scope.search = $scope.search || {
                interests: '',
                location: ''
            };
        };

        $scope.loadEvents(moment(), function (events) {
            $scope.events = events;
            $scope.populateFilters(events);
        });
    }
})();