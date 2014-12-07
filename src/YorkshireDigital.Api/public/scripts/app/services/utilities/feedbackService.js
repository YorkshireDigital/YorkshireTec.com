app.factory('feedbackService', [
    '$http', '$resource', function ($http, $resource) {
        var baseUrl = config.apiurl;
        var buildUrl = function (resourceUrl) {
            return baseUrl + resourceUrl;
        };

        return {
            Raise: $resource(buildUrl('feedback/raise'))
        };
    }
]);