$.validator.addMethod('mustbetrue', function (value, element, params) {
    if (element.type && element.type === 'checkbox') {
        return element.checked;
    }
    return value == true;
});

$.validator.unobtrusive.adapters.addBool('mustbetrue');

//$(document).ready(function () {
//    // Disable jQuery validation for input[type=email] when a regex expression is used
//    $("input[type=email][data-val-regex]").rules("add", {
//        email: false
//    });
//});