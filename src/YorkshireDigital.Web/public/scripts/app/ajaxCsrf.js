(function($) {
    $.fn.ajaxCsrf = function (options) {

        options.data.csrfToken = $('input[name="NCSRF"][type="hidden"]', this).val();

        return $.ajax(options);
    };
})(jQuery);