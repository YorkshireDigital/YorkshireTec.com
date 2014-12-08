var calendarService = function ($http, $resource) {
        var baseUrl = config.apiurl;
        var buildUrl = function (resourceUrl) {
            return baseUrl + resourceUrl;
        };

        return {
            Events: $resource(buildUrl('events/:eventId'), {
                eventId: '@eventId',
            }, {
                'get': { method: 'GET', isArray: false}, 
                'add': { method: 'POST', isArray: false },
                'remove': { method: 'DELETE', isArray: false }
            }),
            Calendar: $resource(buildUrl('events/calendar?from=:from&to=:to'), {
                from: '@from',
                to: '@to'
            }, {
                'query': { method: 'GET', isArray: true }
            })
        };
    };