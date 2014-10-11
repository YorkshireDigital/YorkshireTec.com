(function () {
    'use strict';

    window.app
        .controller('mailinglistController', ['$scope', 'serviceHelperSvc', mailinglistcontroller]);

    function mailinglistcontroller($scope, serviceHelperSvc) {
        $scope.title = 'mailinglistController';
        serviceHelperSvc.Archives.query(function(result) {
            $scope.archives = result.archives;
        });
        activate();

        function activate() { }
    }
})();
