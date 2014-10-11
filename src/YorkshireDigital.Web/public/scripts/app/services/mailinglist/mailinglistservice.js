app.factory('serviceHelperSvc', [
    '$http', '$resource', function($http, $resource) {
        var baseUrl = config.apiurl;
        var buildUrl = function (resourceUrl) {
            return baseUrl + resourceUrl;
        };
        return {
            Archives: $resource(buildUrl('mailinglist/archive'), null, {
                'query': {method:'GET', isArray: false},
            })
        };
    }
]);