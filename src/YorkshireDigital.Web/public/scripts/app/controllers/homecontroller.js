(function () {
    'use strict';

    window.app
        .controller('homeController', ['$scope', '$sce', 'calendarService', homeController]);

    function homeController($scope, $sce, calendarService) {
        $scope.title = 'homeController';

        $scope.to_trusted = function(html_code) {
            return $sce.trustAsHtml(html_code);
        };

        var to = moment().date(1).add(2, 'M').format('DD/MM/YYYY');

        var from = moment().date(1).subtract(1, 'M').format('DD/MM/YYYY');

        calendarService.Events.query({from: from, to: to}, function (events) {
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