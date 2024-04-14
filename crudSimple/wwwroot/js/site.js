// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


$(document).ready(function () {
    $(document).on('click', '#JQbtn', function () {
        var formData = [];

        $('input.form-control').each(function () {
            formData.push($(this).val());
        });

        var ajax1 = $.ajax({
            url: '/Emp/insertEmployee2',
            type: 'post',
            data: {
                EmpName: $('#en').val(),
                EmpEmail: $('#ee').val(),
                EmpSalary: $('#es').val(),
            }
        });
        $.when(ajax1).done(function (response1) {
            alert("data sent successfully");
        });
    });
});
