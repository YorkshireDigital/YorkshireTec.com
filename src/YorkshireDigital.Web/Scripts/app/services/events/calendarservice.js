app.factory('calendarService', [
    '$http', '$resource', function ($http, $resource) {
        var baseUrl = config.apiurl;
        var buildUrl = function (resourceUrl) {
            return baseUrl + resourceUrl;
        };
        return {
            Events: $resource(buildUrl('events/calendar'), null, {
                'query': { method: 'GET', isArray: true },
            })
        };
    }
]);