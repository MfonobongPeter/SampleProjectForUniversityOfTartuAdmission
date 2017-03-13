module.exports = function (grunt) {

    //It returns the time of each task
    require('time-grunt')(grunt);


    require('load-grunt-tasks')(grunt);
    require('load-grunt-config')(grunt, {
        // data passed into config.  Can use with <%= pathAssetsAdminTpl %>
        data: {
            pathAssetsAdminTpl: "admin-v2/clip-one/assets",
            pathAssetsAdminRtlTpl: "admin-v2/clip-one-rtl/assets",
            pathBowerComponents: "bower_components"
        }
    });

    // LTR version
    grunt.registerTask('buildadmin', ['compass:layoutAdmin', 'cssmin:layoutAdmin', 'uglify:layoutAdmin', 'htmlbuild:admin']);
    grunt.registerTask('buildpageadmin', ['prompt:setNameNewPageAdmin', 'copy:newPageAdmin', 'htmlbuild:admin']);
    grunt.registerTask('buildlibs', ['concat:libs', 'uglify:libs', 'cssmin:libs', 'copy:libs', 'less:libs', 'less:libsCompress']);

    // RTL version
    grunt.registerTask('buildadminrtl', ['compass:layoutAdminRtl', 'cssmin:layoutAdminRtl', 'uglify:layoutAdminRtl', 'htmlbuild:adminRtl']);
    grunt.registerTask('buildpageadminrtl', ['prompt:setNameNewPageAdmin', 'copy:newPageAdminRtl', 'htmlbuild:adminRtl']);
    grunt.registerTask('buildlibsrtl', ['concat:libsRtl', 'uglify:libsRtl', 'cssmin:libsRtl', 'copy:libsRtl', 'less:libsRtl', 'less:libsCompressRtl']);
};
