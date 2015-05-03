$(function () {
    $(document).on('click', '#delete-event', function (e) {
        e.preventDefault();
        if (confirm('Are you sure?')) {
            var id = $(this).data('event-id');
            $.ajax({
                type: 'DELETE',
                url: '/admin/event/' + id
            }).done(function () {
                window.location.href = '/admin';
            });
        }
    });

    $(document).on('click', '.js-add-talk', function (e) {
        e.preventDefault();

        var $template = $('.js-talk-template').clone();
        $template.removeClass('js-talk-template');
        $template.removeClass('is-hidden');
        $template.addClass('js-talk-wrapper');

        var index = $('.js-talk-wrapper', '.js-talks-wrapper').length;

        $template.html($template.html().replace(/\[index\]/g, '[' + index + ']'));

        $('.js-talks-wrapper').append($template);
    });
    $(document).on('click', '.js-remove-talk', function (e) {
        e.preventDefault();
        if (confirm('Are you sure?')) {
            $(this).closest('.js-talk-wrapper').remove();

            $('.js-talk-wrapper', '.js-talks-wrapper').each(function (i) {
                $(this).html($(this).html().replace(/\[\d+\]/g, '[' + i + ']'));
            });
        }
    });
    $(".chosen-select").chosen({ disable_search_threshold: 10 });
});