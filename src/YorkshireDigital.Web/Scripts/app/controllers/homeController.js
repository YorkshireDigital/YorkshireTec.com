(function () {
    'use strict';

    window.app
        .controller('homeController', ['$scope', 'calendarService', homeController]);

    function homeController($scope, calendarService) {
        $scope.title = 'homeController';

        calendarService.Events.query(function (events) {
            $scope.events = events;
        });

        init();

        function init() { }
    }
})();
