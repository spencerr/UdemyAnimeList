// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


$(document).ready(function () {
    $('img').on('error', function () {
        if (!$(this).attr('alt')) {
            $(this).attr('src', $(this).attr('data-default-src') || '/images/no-icon.svg');
        }
    });
});

var redirect = function (data) {
    if (data.redirect) {
        window.location = data.redirect;
    } else {
        window.scrollTo(0, 0);
        window.location.reload();
    }
};

var showAjaxSummary = function (xhr) {
    $validator = $('form').validate();
    let response = JSON.parse(xhr.responseText);
    for (let entry in response) {
        for (let error of response[entry].Errors) {
            $validator.showErrors({ [entry]: error.ErrorMessage });
        }
    }
};


$('form[method=post]').not('.no-ajax').on('submit', function (e) {
    e.preventDefault();
    const $this = $(this);
    const submitBtn = $this.find('[type="submit"]');
    const formData = $this.serialize();

    if (!$this.valid()) {
        return false;
    }

    submitBtn.prop('disabled', true);
    //$(window).unbind();

    $this.find(':input').removeClass('is-invalid is-valid');

    $.ajax({
        url: $this.attr('action'),
        type: 'post',
        data: formData,
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        dataType: 'json',
        statusCode: {
            200: redirect,
            400: showAjaxSummary
        },
        complete: function () {
            submitBtn.prop('disabled', false);
        }
    });

    return false;
});