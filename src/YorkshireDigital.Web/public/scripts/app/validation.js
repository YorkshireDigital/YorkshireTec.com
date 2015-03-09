(function() {
    $(function() {
        $('.field-error').each(function () {
            var $fmGroup = $(this).closest('.form-group');

            $fmGroup.addClass('error');
            $('.field', $fmGroup).addClass('error');
        });

        if ($('.field-error').length > 0) {
            $('html, body').scrollTop($('.field-error').offset().top);
        }

    });
})();