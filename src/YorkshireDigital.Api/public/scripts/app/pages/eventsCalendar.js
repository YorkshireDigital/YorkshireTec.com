(function () {
    $(function () {
        initialiseClndr();
    });
    var initialiseClndr = function() {
        var from = moment().date(1).subtract(1, 'M').format('DD/MM/YYYY');
        var to = moment().date(1).add(2, 'M').format('DD/MM/YYYY');

        $.ajax({
            url: "/events/calendar",
            type: "GET",
            data: {
                from: from,
                to: to
            }
        })
            .done(function (events) {
                populateClndr(events);
            });
    };
    var populateClndr = function (events) {
        $('.events-calendar').clndr({
            template: $('#template-calendar').html(),
            forceSixRows: true,
            events: events,
            multiDayEvents: {
                startDate: 'start',
                endDate: 'end'
            },
            startWithMonth: moment(),
            daysOfTheWeek: [
                '<span class="header-day__sm">S</span><span class="header-day__md">Sun</span><span class="header-day__lg">Sunday</span>',
                '<span class="header-day__sm">M</span><span class="header-day__md">Mon</span><span class="header-day__lg">Monday</span>',
                '<span class="header-day__sm">T</span><span class="header-day__md">Tue</span><span class="header-day__lg">Tuesday</span>',
                '<span class="header-day__sm">W</span><span class="header-day__md">Wed</span><span class="header-day__lg">Wednesday</span>',
                '<span class="header-day__sm">T</span><span class="header-day__md">Thu</span><span class="header-day__lg">Thursday</span>',
                '<span class="header-day__sm">F</span><span class="header-day__md">Fri</span><span class="header-day__lg">Friday</span>',
                '<span class="header-day__sm">S</span><span class="header-day__md">Sat</span><span class="header-day__lg">Saturday</span>'
            ]
        });
    };
}());