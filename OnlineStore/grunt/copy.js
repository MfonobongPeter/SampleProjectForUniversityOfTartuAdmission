module.exports = {
    newPageAdmin: {
        files: [
            {
                src: '<%= pathAssetsAdminTpl %>/base/templates/_base.template.html',
                dest: '<%= pathAssetsAdminTpl %>/base/<%= filename %>.html'
            }
        ]
    },
    newPageAdminRtl: {
        files: [
            {
                src: '<%= pathAssetsAdminRtlTpl %>/base/templates/_base.template.html',
                dest: '<%= pathAssetsAdminRtlTpl %>/base/<%= filename %>.html'
            }
        ]
    }
};
