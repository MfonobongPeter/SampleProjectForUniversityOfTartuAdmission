var Error404 = function () {
    //function to initiate RainyDay
    var runRainyDay = function () {
        var image = document.getElementById('background');
        image.onload = function () {
            var engine = new RainyDay({
                image: this,
                blur: 20
            });
            engine.rain([[1, 2, 8000]]);
            engine.rain([[3, 3, 0.88], [5, 5, 0.9], [6, 2, 1]], 100);
        };
        image.crossOrigin = 'anonymous';
        image.src = 'assets/images/back-error-page.jpg';
    };
    return {
        //main function to initiate template pages
        init: function () {
            runRainyDay();
        }
    };
}();