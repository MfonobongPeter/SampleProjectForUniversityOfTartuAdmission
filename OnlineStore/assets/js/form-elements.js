var FormElements = function () {
    //function to initiate jquery.inputlimiter
    var runInputLimiter = function () {
        $('.limited').maxlength({
            threshold: 50,
            warningClass: "label label-info",
            limitReachedClass: "label label-warning",
            message: 'used %charsTyped% of %charsTotal% chars.'
        });
    };
    //function to initiate query.autosize    
    var runAutosize = function () {
        //$(".autosize").autosize();
        autosize($('.autosize'));
    };
    //function to initiate Select2
    var runSelect2 = function () {
        $(".search-select").select2({
            placeholder: "Select a State",
            allowClear: true
        });
    };
    //function to initiate jquery.maskedinput
    var runMaskInput = function () {
        $.mask.definitions['~'] = '[+-]';
        $('.input-mask-date').mask('99/99/9999');
        $('.input-mask-phone').mask('(999) 999-9999');
        $('.input-mask-eyescript').mask('~9.99 ~9.99 999');
        $(".input-mask-product").mask("a*-999-a999", {
            placeholder: " ",
            completed: function () {
                alert("You typed the following: " + this.val());
            }
        });
    };
    var runMaskMoney = function () {
        $(".currency").maskMoney();
    };
    //function to initiate bootstrap-datepicker
    var runDatePicker = function () {
        $('.date-picker').datepicker({
            autoclose: true,
            container: '#picker-container'
        });
    };
    //function to initiate bootstrap-timepicker
    var runTimePicker = function () {
        $('.time-picker').timepicker();
    };
    //function to initiate daterangepicker
    var runDateRangePicker = function () {
        $('.date-range').daterangepicker();
        $('.date-time-range').daterangepicker({
            timePicker: true,
            timePickerIncrement: 30,
            locale: {
                format: 'MM/DD/YYYY h:mm A'
            }
        });
    };
    //function to initiate bootstrap-colorpicker
    var runColorPicker = function () {
        $('.color-picker').colorpicker({
            format: 'hex'
        });
        $('.color-picker-rgba').colorpicker({
            format: 'rgba'
        });
        $('.colorpicker-component').colorpicker();
    };
    //function to initiate jquery.tagsinput
    var runTagsInput = function () {
        $('#tags_1').tagsInput({
            width: 'auto'
        });
    };
    //function to initiate summernote
    var runSummerNote = function () {
        $('.summernote').summernote({
            height: 300,
            tabsize: 2
        });
    };
    //function to initiate ckeditor
    var runCKEditor = function () {
        CKEDITOR.disableAutoInline = true;
        $('textarea.ckeditor').ckeditor();
    };
    //function to initiate fileinput
    var runFileInput = function () {
        $("#input-simple").fileinput();
        $("#input-preview").fileinput();
        var btnCust = '<button type="button" class="btn btn-default" title="Add picture tags" ' +
        'onclick="alert(\'Call your custom code here.\')">' +
        '<i class="glyphicon glyphicon-tag"></i>' +
        '</button>';
        $("#avatar").fileinput({
            overwriteInitial: true,
            maxFileSize: 2000,
            showClose: false,
            showCaption: false,
            browseLabel: '',
            removeLabel: '',
            browseIcon: '<i class="glyphicon glyphicon-folder-open"></i>',
            removeIcon: '<i class="glyphicon glyphicon-remove"></i>',
            removeTitle: 'Cancel or reset changes',
            elErrorContainer: '#kv-avatar-errors',
            msgErrorClass: 'alert alert-block alert-danger',
            defaultPreviewContent: '<img src="http://www.placehold.it/160x160/EFEFEF/AAAAAA?text=no+image" alt="Your Avatar" >',
            layoutTemplates: { main2: '{preview} {remove} {browse}' },
            allowedFileExtensions: ["jpg", "png", "gif"]
        });
    };
    return {
        //main function to initiate template pages
        init: function () {
            runInputLimiter();
            runAutosize();
            runSelect2();
            runMaskInput();
            runMaskMoney();
            runDatePicker();
            runTimePicker();
            runDateRangePicker();
            runColorPicker();
            runTagsInput();
            runSummerNote();
            runCKEditor();
            runFileInput();
        }
    };
}();