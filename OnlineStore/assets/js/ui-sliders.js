var UISliders = function () {
    //function to initiate rangeSlider
    var runRangeSliders = function () {
        
        $("#slider_example_1").ionRangeSlider();
        
        $("#slider_example_3").ionRangeSlider({
            type: "double",
            min: 0,
            max: 10000,
            grid: true,
            grid_num: 10
        });

        $("#slider_example_4").ionRangeSlider({
            type: "double",
            min: 0,
            max: 10000,
            step: 500,
            grid: true,
            grid_snap: true
        });

        $("#slider_example_5").ionRangeSlider({
            type: "double",
            min: 0,
            max: 100,
            from: 20,
            from_min: 10,
            from_max: 30,
            from_shadow: true,
            to: 80,
            to_min: 70,
            to_max: 90,
            to_shadow: true,
            grid: true,
            grid_num: 10
        });

        $("#slider_example_6").ionRangeSlider({
            min: +moment().subtract(12, "hours").format("X"),
            max: +moment().format("X"),
            from: +moment().subtract(6, "hours").format("X"),
            grid: true,
            force_edges: true,
            prettify: function (num) {
                var m = moment(num, "X").locale("en");
                return m.format("Do MMMM, HH:mm");
            }
        });

    };
    //function to initiate jQuery Knob
    var runKnob = function () {
        $(".knob").knob({
            draw: function () {
                // "tron" case
                if (this.$.data('skin') == 'tron') {
                    var a = this.angle(this.cv) // Angle
                        ,
                        sa = this.startAngle // Previous start angle
                        ,
                        sat = this.startAngle // Start angle
                        ,
                        ea // Previous end angle
                        , eat = sat + a // End angle
                        ,
                        r = true;
                    this.g.lineWidth = this.lineWidth;
                    this.o.cursor && (sat = eat - 0.3) && (eat = eat + 0.3);
                    if (this.o.displayPrevious) {
                        ea = this.startAngle + this.angle(this.value);
                        this.o.cursor && (sa = ea - 0.3) && (ea = ea + 0.3);
                        this.g.beginPath();
                        this.g.strokeStyle = this.previousColor;
                        this.g.arc(this.xy, this.xy, this.radius - this.lineWidth, sa, ea, false);
                        this.g.stroke();
                    }
                    this.g.beginPath();
                    this.g.strokeStyle = r ? this.o.fgColor : this.fgColor;
                    this.g.arc(this.xy, this.xy, this.radius - this.lineWidth, sat, eat, false);
                    this.g.stroke();
                    this.g.lineWidth = 2;
                    this.g.beginPath();
                    this.g.strokeStyle = this.o.fgColor;
                    this.g.arc(this.xy, this.xy, this.radius - this.lineWidth + 1 + this.lineWidth * 2 / 3, 0, 2 * Math.PI, false);
                    this.g.stroke();
                    return false;
                }
            }
        });
        // Example of infinite knob, iPod click wheel
        var v, up = 0,
            down = 0,
            i = 0,
            $idir = $("div.idir"),
            $ival = $("div.ival"),
            incr = function () {
                i++;
                $idir.show().html("+").fadeOut();
                $ival.html(i);
            }, decr = function () {
                i--;
                $idir.show().html("-").fadeOut();
                $ival.html(i);
            };
        $("input.infinite").knob({
            min: 0,
            max: 20,
            stopper: false,
            change: function () {
                if (v > this.cv) {
                    if (up) {
                        decr();
                        up = 0;
                    } else {
                        up = 1;
                        down = 0;
                    }
                } else {
                    if (v < this.cv) {
                        if (down) {
                            incr();
                            down = 0;
                        } else {
                            down = 1;
                            up = 0;
                        }
                    }
                }
                v = this.cv;
            }
        });
    };
    return {
        init: function () {
            runRangeSliders();
            runKnob();
        }
    };
}();