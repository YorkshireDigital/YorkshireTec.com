(function () {
    $(function () {
        $("#showFeedbackPanel").on('click', showFeedbackPanel);
        $("#hideFeedbackPanel").on('click', hideFeedbackPanel);
        $("#showTrelloForm").on('click', showTrelloForm);
        $("#showSlackForm").on('click', showSlackForm);
        $("#raiseOnSlack").on('click', raiseOnSlack);
    });

    var showFeedbackPanel = function () {
        $('.notification__title').animate({
          opacity: 0
        });
        $('.feedback-panel').slideDown();
    };
    var hideFeedbackPanel = function () {
        $('.notification__title').animate({
          opacity: 1
        });
        $('.feedback-panel').slideUp();
    };
    var showTrelloForm = function () {
        $('#raise-on-slack').slideUp();
        $('#raise-on-trello').slideDown();
    };
    var showSlackForm = function () {
        $('#raise-on-trello').slideUp();
        $('#raise-on-slack').slideDown();
    };
    var raiseOnSlack = function () {

        var issue = {
            name: $('#raise-on-slack-name').val(),
            contact: $('#raise-on-slack-contact').val(),
            details: $('#raise-on-slack-details').val(),
            site: document.URL
        };

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
            $.ajax({
                url: "/feedback/raise",
                type: "POST",
                data: issue
            })
            .done(function (data) {
                $('.notification__title').after('<span class="notification__feedback">Thank you for your feedback ' + issue.name + '. We\'ll be in touch soon.</span>');
                $('.feedback-panel').slideUp();

                setTimeout(function () {
                    $('.notification__feedback').fadeOut(1000);
                    setTimeout(function () {
                        $('.notification__title').fadeIn();
                    }, 1000);
                }, 2000);
            });
        }
    };
}());
