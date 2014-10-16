(function () {
    'use strict';

    window.app.controller('homeController', ['$scope', '$sce', 'calendarService', '$routeParams', '$location', homeController]);

    function homeController($scope, $sce, calendarService, $routeParams, $location) {

        init();

        $scope.title = 'homeController';

        if ($routeParams.eventName) {
            $scope.loadEvent($routeParams.eventName);
        }

        var from = moment().date(1).subtract(1, 'M').format('DD/MM/YYYY');
        var to = moment().date(1).add(2, 'M').format('DD/MM/YYYY');

        $scope.loadEvents(from, to, function (events) {
            $scope.events = events;
            $scope.populateFilters(events);
        });

        function init() {
            $scope.to_trusted = function (html_code) {
                return $sce.trustAsHtml(html_code);
            };
            $scope.clndrNextMonth = function () {
                console.log('clndrNextMonth');
                $scope.clndr.forward();
                var from = moment($scope.clndr.month._d).date(1).add(1, 'M').format('DD/MM/YYYY');
                var to = moment($scope.clndr.month._d).date(1).add(2, 'M').format('DD/MM/YYYY');
                $scope.loadEvents(from, to, function (events) {
                    $scope.addNewEvents(events);
                });
            };
            $scope.clndrPreviousMonth = function () {
                console.log('clndrPreviousMonth');
                $scope.clndr.back();
                var from = moment($scope.clndr.month._d).date(1).subtract(1, 'M').format('DD/MM/YYYY');
                var to = moment($scope.clndr.month._d).date(1).format('DD/MM/YYYY');
                $scope.loadEvents(from, to, function (events) {
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
            $scope.loadEvent = function (eventName) {
                calendarService.Events.get({ eventId: eventName }, function(activeEvent) {
                    $scope.activeEvent = activeEvent;
                    $location.path('/event/' + activeEvent.id, false);
                });
            };
            $scope.loadEvents = function (from, to, callback) {
                calendarService.Calendar.query({ from: from, to: to }, callback);
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
        };
    }
})();