$(function () {
    function subscribeToMailingList(subscribe) {
        $('p.error-message', '#mailchimp-wrapper').remove();
        $('p.success-message', '#mailchimp-wrapper').remove();
        var email = $('#email').val();

        if (email !== '') {
            var url = subscribe ? "mailinglist/subscribe" : "mailinglist/unsubscribe";

            $('#joinMailingList').addClass('loading');

            $('#mailchimp-wrapper').ajaxCsrf({
                url: url,
                type: "POST",
                data: { email: email },
                headers: {
                    Accept: 'application/json'
                }
            }).done(function (response) {
                $('form').removeClass('loading');
                $('#mailchimp-wrapper').append('<p class="success success-message">Success: ' + response.message + '</p>');

                if (subscribe) {
                    $('#mc-subscribe').text('Resend mailing list confirmation');
                    $('#mc-subscribe').removeClass('btn--primary');
                    $('#mc-subscribe').addClass('btn--secondary');
                } else {
                    $('#mc-unsubscribe').text('Resend Unsubscribe mailing list confirmation');
                }

            }).fail(function (response) {
                $('p.error-message', '#mailchimp-wrapper').remove();
                $('p.success-message', '#mailchimp-wrapper').remove();
                $('form').removeClass('loading');
                if (response.responseJSON) {
                    $('#mailchimp-wrapper').append('<p class="error error-message">Error: ' + response.responseJSON.message + '</p>');
                }
            });

        } else {
            $('#email').closest('.field').addClass('error');
        }
    };
    $(document).on('click', '#mc-subscribe', function () {
        subscribeToMailingList(true);
    });
    $(document).on('click', '#mc-unsubscribe', function () {
        subscribeToMailingList(false);
    });
});