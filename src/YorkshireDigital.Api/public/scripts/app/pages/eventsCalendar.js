(function () {
    var clndr;

    $(function () {
        initialiseClndr();
        $(document).on('click', '.js-clndrNextMonth', function() {
            console.log('js-clndrNextMonth');
            clndrNextMonth();
        });
        $(document).on('click', '.js-clndrPreviousMonth', clndrPreviousMonth);
    });
    var initialiseClndr = function () {
        var from = moment().date(1).subtract(1, 'M').format('DD/MM/YYYY');
        var to = moment().date(1).add(2, 'M').format('DD/MM/YYYY');
        $('#calendar-month').text(moment().format('MMMM'));
        renderClndr([], moment());
        loadEvents(from, to);
    };
    var loadEvents = function(from, to) {
        $.ajax({
                url: "/events/calendar",
                type: "GET",
                data: {
                    from: from,
                    to: to
                }
            })
            .done(function(events) {
                populateClndr(events);
            });
    };
    var renderClndr = function(month) {
        clndr=  $('.events-calendar').clndr({
            template: $('#template-calendar').html(),
            forceSixRows: true,
            events: [],
            multiDayEvents: {
                startDate: 'start',
                endDate: 'end'
            },
            startWithMonth: month,
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
    var populateClndr = function (events, emptyCalendar) {
        if (emptyCalendar) {
            clndr.setEvents(events);
        } else {
            addNewEvents(events);
        }
        var month = clndr.month;
        var eventsThisMonth = _.filter(clndr.eventsThisMonth, function (event) {
            return filterEventsByMonth(event, month);
        });
        $('#event-count').text(eventsThisMonth.length);
        $('#calendar-month').text(month.format('MMMM'));
        $('.loading-item__overlay').addClass('hide');
        $('.clndr-grid').removeClass('loading__item');
    };
    var addNewEvents = function (events) {
        var newEvents = [];
        for (var e = events.length - 1; e >= 0; e--) {
            var found = false;
            for (var k = 0; k < clndr.eventsThisMonth.length; k++) {
                if (events[e].uniqueName == clndr.eventsThisMonth[k].uniqueName) {
                    found = true;
                }
            }
            if (found === false) {
                newEvents.push(events[e]);
            }
        };
        //$scope.populateFilters(newEvents);
        clndr.addEvents(newEvents);
        return newEvents;
    };
    var filterEventsByMonth = function (event, currentMonth) {
        return currentMonth.isSame(event.start, 'month');
    };
    var clndrNextMonth = function () {
        clndr.forward();
        $('#calendar-month').text(clndr.month.format('MMMM'));
        var from = moment(clndr.month._d).date(1).add(1, 'M').format('DD/MM/YYYY');
        var to = moment(clndr.month._d).date(1).add(2, 'M').format('DD/MM/YYYY');
        loadEvents(from, to);
    };
    var clndrPreviousMonth = function () {
        clndr.back();
        $('#calendar-month').text(clndr.month.format('MMMM'));
        var from = moment(clndr.month._d).date(1).subtract(1, 'M').format('DD/MM/YYYY');
        var to = moment(clndr.month._d).date(1).format('DD/MM/YYYY');
        loadEvents(from, to);
    };
}());