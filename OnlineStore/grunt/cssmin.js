module.exports = {
    layoutAdmin: {
        files: [{
            expand: true,
            cwd: '<%= pathAssetsAdminTpl %>/css',
            src: ['**/*.css', '!**/*.min.css'],
            dest: '<%= pathAssetsAdminTpl %>/css',
            ext: '.min.css'
        },
        {
            '<%= pathAssetsAdminTpl %>/fonts/clip-font.min.css': ['<%= pathAssetsAdminTpl %>/fonts/clip-font.css']
        }]        
    },
    layoutAdminRtl: {
        files: [{
            expand: true,
            cwd: '<%= pathAssetsAdminRtlTpl %>/css',
            src: ['**/*.css', '!**/*.min.css'],
            dest: '<%= pathAssetsAdminRtlTpl %>/css',
            ext: '.min.css'
        },
        {
            '<%= pathAssetsAdminRtlTpl %>/fonts/clip-font.min.css': ['<%= pathAssetsAdminRtlTpl %>/fonts/clip-font.css']
        }]
    },
    libs: {
        files: {
            '<%= pathAssetsAdminTpl %>/plugin/jquery-ui.custom.min.css': ['<%= pathAssetsAdminTpl %>/plugin/jquery-ui.custom.css']
        }
    },
    libsRtl: {
        files: {
            '<%= pathAssetsAdminRtlTpl %>/plugin/jquery-ui.custom.min.css': ['<%= pathAssetsAdminRtlTpl %>/plugin/jquery-ui.custom.css']
        }
    }
};