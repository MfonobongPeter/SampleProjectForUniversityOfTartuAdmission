module.exports = {
    layoutAdmin: {
        options: {
            config: '<%= pathAssetsAdminTpl %>/config/config.rb',
            sassDir: '<%= pathAssetsAdminTpl %>/sass',
            cssDir: '<%= pathAssetsAdminTpl %>/css',
            outputStyle: 'nested',
            linecomments: false
        }
    },
    layoutAdminRtl: {
        options: {
            config: '<%= pathAssetsAdminRtlTpl %>/config/config.rb',
            sassDir: '<%= pathAssetsAdminRtlTpl %>/sass',
            cssDir: '<%= pathAssetsAdminRtlTpl %>/css',
            outputStyle: 'nested',
            linecomments: false
        }
    }
};
