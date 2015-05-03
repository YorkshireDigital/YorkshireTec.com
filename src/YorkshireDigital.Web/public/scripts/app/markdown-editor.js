$(function() {
    $('.markdown-editor').each(function() {
        $(this).append('<a href="#" class="btn-preview-markdown">Preview</a>');
        $(this).append('<a href="https://github.com/adam-p/markdown-here/wiki/Markdown-Cheatsheet" target="_black" style="float: right;">Markdown Help</a>');

        $('textarea', this).css('height', '350px');

        var $preview = $('<div />', {
            'class': 'markdown-preview',
            css: {
                'display': 'none',
                'padding': '.5em',
                'border': '1px solid #ccc',
                '-moz-border-radius': '3px',
                '-webkit-border-radius': '3px',
                'border-radius': '3px',
                'height': '350px',
                'overflow-y': 'scroll'
            }
        });
        $preview.html(marked($('textarea', this).val()));

        $('textarea', this).after($preview);
    });

    $(document).on('click', '.btn-preview-markdown', function(e) {
        e.preventDefault();

        var $editor = $(this).parent('.markdown-editor');

        if ($(this).text() === 'Preview') {
            $('textarea', $editor).hide();
            $('.markdown-preview', $editor).show();
            $(this).text('Edit');
        } else {
            $('textarea', $editor).show();
            $('.markdown-preview', $editor).hide();
            $(this).text('Preview');
        }

    });
    $('textarea', '.markdown-editor').change(function() {
        var $editor = $(this).parent('.markdown-editor');
        var $preview = $('.markdown-preview', $editor);

        $preview.html(marked($(this).val()));
    });
});