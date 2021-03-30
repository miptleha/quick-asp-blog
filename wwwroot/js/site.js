// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$('#save').click(function () {
    window.console.log('save');
    $.get("/Entry", { caption: $('#caption').text(), text: $('#text').text() })
        .fail(function (xhr) {
            console.log('fail: ' + xhr);
            $('#status').text("Error: " + xhr.statusText);
        })
        .done(function (xhr) {
            console.log('done: ' + xhr);
            $('#status').text("Success: " + xhr.statusText);
        });
});