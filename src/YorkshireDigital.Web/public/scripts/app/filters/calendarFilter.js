window.app.filter('calendarFilter', function () {
    return function (events, filter) {
        //console.log(filter);
        var result = [];
        for (var i = 0; i < events.length; i++) {
            var include = true;
            if (filter.location && events[i].location != filter.location) {
                console.log(events[i].title + ' is not in ' + filter.location);
                include = false;
            }
            if (filter.interests && events[i].interests.indexOf(filter.interests) == -1) {
                console.log(events[i].title + ' does not have ' + filter.interests);
                include = false;
            }
            if (filter.searchText && events[i].title.toLowerCase().indexOf(filter.searchText.toLowerCase()) == -1) {
                console.log(events[i].title + ' does not match ' + filter.searchText);
                include = false;
            }
            if (include) {
                result.push(events[i]);
            }
        }
        return result;
    };
});