window.app.filter('clndrCurrentMonthFilter', function () {
    return function (events, clndr) {
        var currentMonth = moment(clndr.month);
        var result = [];
    	for (var i = events.length - 1; i >= 0; i--) {
        	var eventStart = moment(events[i].start);
        	if (currentMonth.month() === eventStart.month()) {
        		if (currentMonth.year() === eventStart.year()) {
                	result.push(events[i]);
        		};
        	};
    	};
        return result;
    };
});