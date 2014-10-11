(function () {
    'use strict';

    window.app
        .controller('homeController', ['$scope', 'calendarService', homeController]);

    function homeController($scope, calendarService) {
        $scope.title = 'homeController';

        calendarService.Events.query(function (events) {
            $scope.events = events;
        });

        init();

        function init() { 

        // var thisMonth = moment().format('YYYY-MM');

        // var eventArray = [
        
        //     // startDate: "{{ event.startDate }}",
        //     // endDate: "{{ event.endDate }}",
        //     // title: "{{ event.title }}",
        //     // shortTitle: "{{ event.shortTitle }}",
        //     // strapline: "{{ event.strapline }}",
        //     // description: "{{ event.description }}", 
        //     // brandingAccent: "{{ event.brandingAccent }}"
        // ]

        // var calendar = $('.calendar').clndr({
        //     template: $('#template-calendar').html(),
        //     events: eventArray,
        //     multiDayEvents: {
        //       startDate: 'startDate',
        //       endDate: 'endDate'
        //     },
        //     startWithMonth: moment(),
        //     clickEvents: {
        //       click: function(target) {
        //         console.log(target);
        //       }
        //     },
        //     daysOfTheWeek: [
        //     '<span class="header-day__sm">S</span><span class="header-day__md">Sun</span><span class="header-day__lg">Sunday</span>',
        //     '<span class="header-day__sm">M</span><span class="header-day__md">Mon</span><span class="header-day__lg">Monday</span>',
        //     '<span class="header-day__sm">T</span><span class="header-day__md">Tue</span><span class="header-day__lg">Tuesday</span>',
        //     '<span class="header-day__sm">W</span><span class="header-day__md">Wed</span><span class="header-day__lg">Wednesday</span>',
        //     '<span class="header-day__sm">T</span><span class="header-day__md">Thu</span><span class="header-day__lg">Thursday</span>',
        //     '<span class="header-day__sm">F</span><span class="header-day__md">Fri</span><span class="header-day__lg">Friday</span>',
        //     '<span class="header-day__sm">S</span><span class="header-day__md">Sat</span><span class="header-day__lg">Saturday</span>'
        //     ],
        //     forceSixRows: true
        //   });
        }
    }
})();
