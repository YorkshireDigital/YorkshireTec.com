(function () {
    'use strict';

    window.app
        .controller('homeController', ['$scope', homeController]);

    function homeController($scope) {
        $scope.title = 'homeController';

        init();

        function init() { }
    }
})();
