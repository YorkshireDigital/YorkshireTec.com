/**
 * Created by Macs on 03/12/2014.
 */
(function () {
    'use strict';

    window.app.controller('betaController', ['$scope', betaController]);

    function betaController($scope) {
        function activate() {

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
        }

        activate();
    }
});