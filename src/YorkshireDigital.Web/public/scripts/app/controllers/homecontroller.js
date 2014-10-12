(function () {
    'use strict';

    window.app
        .controller('homeController', ['$scope', 'calendarService', homeController]);

    function homeController($scope, calendarService) {
        $scope.title = 'homeController';

        calendarService.Events.query(function (events) {
            $scope.events = events;
            var interests = [];
            for (var i = events.length - 1; i >= 0; i--) {
                for (var j = events[i].interests.length - 1; j >= 0; j--) {
                    if ($.inArray(events[i].interests[j], interests) === -1) {
                        interests.push(events[i].interests[j]);
                    }
                }
            };
            $scope.interests = interests;
        });

        init($scope);

        function init($scope) { 
        }
    }
})();
