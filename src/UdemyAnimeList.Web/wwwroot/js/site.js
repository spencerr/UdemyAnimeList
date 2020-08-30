// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
toastr.options = {
    positionClass: 'toast-bottom-center',
    timeOut: 0
};

$(document).on('error', 'img', function () {
    if (!$(this).attr('alt')) {
        $(this).attr('src', $(this).attr('data-default-src') || '/images/no-icon.svg');
    }
});

var redirect = function (data) {
    if (data.redirect) {
        window.location = data.redirect;
    } else {
        window.scrollTo(0, 0);
        window.location.reload();
    }
};

var createdAt = function (data, status, xhr) {
    const redirectTo = new URL(xhr.getResponseHeader('location'));
    redirectTo.searchParams.append("id", data.id);
    window.location = redirectTo.href;
};

var showAjaxSummary = function (xhr) {
    $validator = $('form').validate();
    const response = JSON.parse(xhr.responseText);
    for (const entry in response) {
        for (const error of response[entry].Errors) {
            $validator.showErrors({ [entry]: error.ErrorMessage });
        }
    }
};

var showServerError = function (xhr) {
    toastr.error('An error has occured, please try again later.', 'Server Error!');
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
    $(window).unbind();

    //$this.find(':input').removeClass('is-invalid is-valid');

    $.ajax({
        url: $this.attr('action'),
        type: 'post',
        data: formData,
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        dataType: 'json',
        statusCode: {
            200: redirect,
            201: createdAt,
            400: showAjaxSummary,
            500: showServerError
        },
        complete: function () {
            submitBtn.prop('disabled', false);
        }
    });

    return false;
});