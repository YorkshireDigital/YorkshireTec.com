(function() {
    $(function () {
        $(document).on('click', '.tab-list__link', function (e) {
            e.preventDefault();
            $(this).parents('.tab-list__item').siblings().removeClass('tab-list__item--current');
            $(this).parents('.tab-list__item').addClass('tab-list__item--current');

            var $tab = $($(this).attr('for'), $(this).closest('.tab-container'));
            $tab.siblings().removeClass('tab-pane--current');
            $tab.addClass('tab-pane--current');
        });
    });
}())