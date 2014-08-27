app.factory('calendarService', [
    '$http', '$resource', function ($http, $resource) {
        var baseUrl = config.apiurl;
        var buildUrl = function (resourceUrl) {
            return baseUrl + resourceUrl;
        };
        return {
            Events: $resource(buildUrl('events'), null, {
                'query': { method: 'GET', isArray: false },
            })
        };
    }
]);