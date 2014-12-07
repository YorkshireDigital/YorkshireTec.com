(function () {
    'use strict';

    window.app.controller('homeController', ['$scope', '$sce', 'calendarService', 'feedbackService', 'modal', '$routeParams', '$location', homeController]);

    function homeController($scope, $sce, calendarService, feedbackService, modal, $routeParams, $location) {
        if (!$scope.events) {
            init();

            var from = moment().date(1).subtract(1, 'M').format('DD/MM/YYYY');
            var to = moment().date(1).add(2, 'M').format('DD/MM/YYYY');

            $scope.loadEvents(from, to, function (events) {
                $scope.events = events;
                $scope.populateFilters(events);
            });
        }

        if ($routeParams.eventName) {
            $scope.loadEvent($routeParams.eventName);
        }
        else if ($scope.activeEvent){
            $scope.closeEvent();
        }

        function init() {

            if (config.isBeta) {
                $scope.beta = true;
                $scope.reportedIssue = { site: document.URL };
            }

            $scope.to_trusted = function(html_code) {
                return $sce.trustAsHtml(html_code);
            };
            $scope.clndrNextMonth = function() {
                $scope.clndr.forward();
                var from = moment($scope.clndr.month._d).date(1).add(1, 'M').format('DD/MM/YYYY');
                var to = moment($scope.clndr.month._d).date(1).add(2, 'M').format('DD/MM/YYYY');
                $scope.loadEvents(from, to, function(events) {
                    $scope.addNewEvents(events);
                });
            };
            $scope.clndrPreviousMonth = function() {
                $scope.clndr.back();
                var from = moment($scope.clndr.month._d).date(1).subtract(1, 'M').format('DD/MM/YYYY');
                var to = moment($scope.clndr.month._d).date(1).format('DD/MM/YYYY');
                $scope.loadEvents(from, to, function(events) {
                    $scope.addNewEvents(events);
                });
            };
            $scope.addNewEvents = function(events) {
                var newEvents = [];
                for (var e = events.length - 1; e >= 0; e--) {
                    var found = false;
                    for (var k = 0; k < $scope.events.length; k++) {
                        if (events[e].uniqueName == $scope.events[k].uniqueName) {
                            found = true;
                        }
                    }
                    if (found === false) {
                        newEvents.push(events[e]);
                        $scope.events.push(events[e]);
                    }
                };
                $scope.populateFilters(newEvents);
                $scope.clndr.addEvents(newEvents);
                return newEvents;
            };
            $scope.goToEvent = function(eventName) {
                $location.path('event/'+eventName, false);
                $scope.scrollTop = $(window).scrollTop();
                $scope.loadEvent(eventName);
            };
            $scope.loadEvent = function(eventName) {
                calendarService.Events.get({ eventId: eventName }, function(activeEvent) {
                    $scope.activeEvent = activeEvent;
                    $scope.activeEvent.startFormat = $sce.trustAsHtml(activeEvent.startFormat);
                    $('body').addClass('no-scroll');
                });
            };
            $scope.closeEvent = function () {
                if ($scope.activeEvent) {
                    $scope.closedEvent = $scope.activeEvent.uniqueName;
                };
                $scope.activeEvent = null;
                $location.path('/', false);
                $('body').removeClass('no-scroll');
                if ($scope.scrollTop) {
                    $(window).scrollTop($scope.scrollTop);
                    $scope.scrollTop = null;
                };
            };
            $scope.loadEvents = function(from, to, callback) {
                calendarService.Calendar.query({ from: from, to: to }, callback);
            };
            $scope.populateFilters = function(events) {
                var interests = $scope.interests || [];
                var locations = $scope.locations || [];
                for (var i = events.length - 1; i >= 0; i--) {
                    for (var j = events[i].interests.length - 1; j >= 0; j--) {
                        if ($.inArray(events[i].interests[j], interests) === -1) {
                            interests.push(events[i].interests[j]);
                        }
                    }
                    if ($.inArray(events[i].region, locations) === -1) {
                        locations.push(events[i].region);
                    }
                };
                $scope.interests = interests;
                $scope.locations = locations;
                $scope.search = $scope.search || {
                    interests: '',
                    location: ''
                };
            };
            $scope.showFeedbackPanel = function () {
                $('.notification__title').fadeOut();
                $('.feedback-panel').slideDown();
            };
            $scope.hideFeedbackPanel = function () {
                $('.notification__title').fadeIn();
                $('.feedback-panel').slideUp();
            };
            $scope.showTrelloForm = function () {
                $('#raise-on-slack').slideUp();
                $('#raise-on-trello').slideDown();
            };
            $scope.showSlackForm = function () {
                $('#raise-on-trello').slideUp();
                $('#raise-on-slack').slideDown();
            };
            $scope.raiseOnSlack = function (issue) {
                $('#js-slack-feedback').empty();
                var valid = true;
                if (!issue.name) {
                    $('#js-slack-feedback').append($('<li />', { text: "Please provide your name" }));
                    valid = false;
                }
                if (!issue.contact) {
                    $('#js-slack-feedback').append($('<li />', { text: "Please provide contact details" }));
                    valid = false;
                }
                if (!issue.details) {
                    $('#js-slack-feedback').append($('<li />', { text: "Please provide details of the issue" }));
                    valid = false;
                }
                if (valid) {        
                    feedbackService.Raise.save(issue);
                    $scope.modal.hide();

                    $('.notification__title').after('<span class="notification__feedback">Thank you for your feedback ' + issue.name + '. We\'ll be in touch soon.</span>');

                    $('.feedback-panel').slideUp();

                    setTimeout(function () {
                        $('.notification__feedback').fadeOut(1000);
                        setTimeout(function () {
                            $('.notification__title').fadeIn();
                        }, 1000);
                    }, 2000);
                }
            };
            $scope.formatDay = function (date, format) {
                return moment(date).format(format);
            };
        };
    }
})();