var Animation = function () {
    //function to initiate CSS3 Animation 
    var runAnimation = function () {
        /* ENTRANCES */
        $('.animation-buttons .btn').click(function (e) {
            e.preventDefault();
        });
        $('#slideUpBtn').click(function () {
            $(this).each(function () {
                $('#object').removeClass();
                $('#object').addClass("animated slideInUp");
            });
        });
        $('#slideDownBtn').click(function () {
            $(this).each(function () {
                $('#object').removeClass();
                $('#object').addClass("animated slideInDown");
            });
        });
        $('#slideLeftBtn').click(function () {
            $(this).each(function () {
                $('#object').removeClass();
                $('#object').addClass("animated slideInLeft");
            });
        });
        $('#slideRightBtn').click(function () {
            $(this).each(function () {
                $('#object').removeClass();
                $('#object').addClass("animated slideInRight");
            });
        });
        $('#expandUpBtn').click(function () {
            $(this).each(function () {
                $('#object').removeClass();
                $('#object').addClass("animated zoomInUp");
            });
        });
        $('#fadeInBtn').click(function () {
            $(this).each(function () {
                $('#object').removeClass();
                $('#object').addClass("animated fadeIn");
            });
        });
        $('#expandOpenBtn').click(function () {
            $(this).each(function () {
                $('#object').removeClass();
                $('#object').addClass("animated zoomIn");
            });
        });
        $('#bigEntranceBtn').click(function () {
            $(this).each(function () {
                $('#object').removeClass();
                $('#object').addClass("animated zoomIn");
            });
        });
        $('#flipBtn').click(function () {
            $(this).each(function () {
                $('#object').removeClass();
                $('#object').addClass("animated flip");
            });
        });

        /* MISC */
        $('#bounceBtn').click(function () {
            $(this).each(function () {
                $('#object').removeClass();
                $('#object').addClass("animated bounce");
            });
        });
        $('#pulseBtn').click(function () {
            $(this).each(function () {
                $('#object').removeClass();
                $('#object').addClass("animated pulse");
            });
        });
        $('#shakeBtn').click(function () {
            $(this).each(function () {
                $('#object').removeClass();
                $('#object').addClass("animated shake");
            });
        });
        $('#wobbleBtn').click(function () {
            $(this).each(function () {
                $('#object').removeClass();
                $('#object').addClass("animated wobble");
            });
        });
        $('#bounceInUpBtn').click(function () {
            $(this).each(function () {
                $('#object').removeClass();
                $('#object').addClass("animated bounceInUp");
            });
        });
        $('#bounceInDownBtn').click(function () {
            $(this).each(function () {
                $('#object').removeClass();
                $('#object').addClass("animated bounceInDown");
            });
        });
    };
    return {
        init: function () {
            runAnimation();
        }
    };
}();