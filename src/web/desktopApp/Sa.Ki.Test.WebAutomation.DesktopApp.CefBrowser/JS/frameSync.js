(async function () {
    await CefSharp.BindObjectAsync("boundAsync");
    var r = boundAsync.div(16, 2)
        .then(function (res) {
            console.log(res);
        });
})();
