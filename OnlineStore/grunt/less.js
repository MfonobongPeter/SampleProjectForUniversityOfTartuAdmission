module.exports = {
    libs: {
        files: {
            '<%= pathAssetsAdminTpl %>/plugin/bootstrap-timepicker.css': "<%= pathBowerComponents %>/bootstrap-timepicker/css/timepicker.less"
        }
    },
    libsRtl: {
        files: {
            '<%= pathAssetsAdminRtlTpl %>/plugin/bootstrap-timepicker.css': "<%= pathBowerComponents %>/bootstrap-timepicker/css/timepicker.less"
        }
    },
    libsCompress: {
        options: {
            compress: true
        },
        files: {
            '<%= pathAssetsAdminTpl %>/plugin/bootstrap-timepicker.min.css': "<%= pathBowerComponents %>/bootstrap-timepicker/css/timepicker.less"
        }
    },
    libsCompressRtl: {
        options: {
            compress: true
        },
        files: {
            '<%= pathAssetsAdminRtlTpl %>/plugin/bootstrap-timepicker.min.css': "<%= pathBowerComponents %>/bootstrap-timepicker/css/timepicker.less"
        }
    }
};