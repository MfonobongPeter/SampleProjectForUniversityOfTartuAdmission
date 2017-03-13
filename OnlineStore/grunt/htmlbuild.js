module.exports = {
    admin: {
        src: [
            '<%= pathAssetsAdminTpl %>/base/*.html',
            '!_base.template.html',
        ],
        dest: 'admin-v2/clip-one/',
        options: {
            beautify: true,
            relative: true,
            scripts: {
                bundle: [
                        '<%= pathBowerComponents %>/jquery-ui/jquery-ui.min.js',
                        '<%= pathBowerComponents %>/bootstrap/dist/js/bootstrap.min.js',
                        '<%= pathBowerComponents %>/bootstrap-hover-dropdown/bootstrap-hover-dropdown.min.js',
                        '<%= pathBowerComponents %>/blockUI/jquery.blockUI.js',
                        '<%= pathBowerComponents %>/iCheck/icheck.min.js',
                        '<%= pathBowerComponents %>/perfect-scrollbar/js/min/perfect-scrollbar.jquery.min.js',
                        '<%= pathBowerComponents %>/jquery.cookie/jquery.cookie.js',
                        '<%= pathBowerComponents %>/sweetalert/dist/sweetalert.min.js',
                ],
                scriptIE: [
                    '<%= pathBowerComponents %>/respond/dest/respond.min.js',
                    '<%= pathBowerComponents %>/Flot/excanvas.min.js',
                    '<%= pathBowerComponents %>/jquery-1.x/dist/jquery.min.js',
                ],
                resurces: {
                    jQuery: '<%= pathBowerComponents %>/jquery/dist/jquery.min.js'
                },
                main: '<%= pathAssetsAdminTpl %>/js/min/main.min.js'
            },
            styles: {
                bundle: [
                   '<%= pathBowerComponents %>/bootstrap/dist/css/bootstrap.min.css',
                   '<%= pathBowerComponents %>/font-awesome/css/font-awesome.min.css',
                   '<%= pathAssetsAdminTpl %>/fonts/clip-font.min.css',
                   '<%= pathBowerComponents %>/iCheck/skins/all.css',
                   '<%= pathBowerComponents %>/perfect-scrollbar/css/perfect-scrollbar.min.css',
                   '<%= pathBowerComponents %>/sweetalert/dist/sweetalert.css',
                ],
                theme: {
                    light: '<%= pathAssetsAdminTpl %>/css/theme/light.min.css',
                },
                googleFonts: '//fonts.googleapis.com/css?family=Open+Sans:400,300,600,700|Raleway:400,100,200,300,500,600,700,800,900',
                main: '<%= pathAssetsAdminTpl %>/css/main.min.css',
                mainResponsive: '<%= pathAssetsAdminTpl %>/css/main-responsive.min.css',
                print: '<%= pathAssetsAdminTpl %>/css/print.min.css',
            },
            sections: {
                layout: {
                    header: '<%= pathAssetsAdminTpl %>/base/layout/header.html',
                    footer: '<%= pathAssetsAdminTpl %>/base/layout/footer.html',
                    meta: '<%= pathAssetsAdminTpl %>/base/layout/_meta.html',
                    navbar: '<%= pathAssetsAdminTpl %>/base/layout/navbar.html',
                    sidebar: '<%= pathAssetsAdminTpl %>/base/layout/sidebar.html',
                    styleSelector: '<%= pathAssetsAdminTpl %>/base/layout/style-selector.html',
                    panelConfiguration: '<%= pathAssetsAdminTpl %>/base/layout/panel-configuration.html',
                    eventManagement: '<%= pathAssetsAdminTpl %>/base/layout/event-management.html',
                    horizzontalMenu: '<%= pathAssetsAdminTpl %>/base/layout/horizzontal-menu.html'
                }
            },
            data: {
                version: "2.0.0",
                title: "Clip-One - Responsive Admin Template",
                author: "ClipTheme",
                description: "Responsive Admin Template build with Twitter Bootstrap and jQuery",
                pathFavicon: "favicon.ico"
            }
        }
    },
    adminRtl: {
        src: [
            '<%= pathAssetsAdminRtlTpl %>/base/*.html',
            '!_base.template.html',
        ],
        dest: 'admin-v2/clip-one-rtl/',
        options: {
            beautify: true,
            relative: true,
            scripts: {
                bundle: [
                        '<%= pathBowerComponents %>/jquery-ui/jquery-ui.min.js',
                        '<%= pathBowerComponents %>/bootstrap/dist/js/bootstrap.min.js',
                        '<%= pathBowerComponents %>/bootstrap-hover-dropdown/bootstrap-hover-dropdown.min.js',
                        '<%= pathBowerComponents %>/blockUI/jquery.blockUI.js',
                        '<%= pathBowerComponents %>/iCheck/icheck.min.js',
                        '<%= pathBowerComponents %>/perfect-scrollbar/js/min/perfect-scrollbar.jquery.min.js',
                        '<%= pathBowerComponents %>/jquery.cookie/jquery.cookie.js',
                        '<%= pathBowerComponents %>/sweetalert/dist/sweetalert.min.js',
                ],
                scriptIE: [
                    '<%= pathBowerComponents %>/respond/dest/respond.min.js',
                    '<%= pathBowerComponents %>/Flot/excanvas.min.js',
                    '<%= pathBowerComponents %>/jquery-1.x/dist/jquery.min.js',
                ],
                resurces: {
                    jQuery: '<%= pathBowerComponents %>/jquery/dist/jquery.min.js'
                },
                main: '<%= pathAssetsAdminRtlTpl %>/js/min/main.min.js'
            },
            styles: {
                bundle: [
                   '<%= pathBowerComponents %>/bootstrap/dist/css/bootstrap.min.css',
                   '<%= pathBowerComponents %>/font-awesome/css/font-awesome.min.css',
                   '<%= pathAssetsAdminRtlTpl %>/fonts/clip-font.min.css',
                   '<%= pathBowerComponents %>/iCheck/skins/all.css',
                   '<%= pathBowerComponents %>/perfect-scrollbar/css/perfect-scrollbar.min.css',
                   '<%= pathBowerComponents %>/sweetalert/dist/sweetalert.css',
                ],
                theme: {
                    light: '<%= pathAssetsAdminRtlTpl %>/css/theme/light.min.css',
                },
                googleFonts: '//fonts.googleapis.com/css?family=Open+Sans:400,300,600,700|Raleway:400,100,200,300,500,600,700,800,900',
                main: [
                    '<%= pathAssetsAdminRtlTpl %>/css/main.min.css',
                    '<%= pathAssetsAdminRtlTpl %>/css/rtl-version.min.css'
                ],
                mainResponsive: '<%= pathAssetsAdminRtlTpl %>/css/main-responsive.min.css',
                print: '<%= pathAssetsAdminRtlTpl %>/css/print.min.css',
            },
            sections: {
                layout: {
                    header: '<%= pathAssetsAdminRtlTpl %>/base/layout/header.html',
                    footer: '<%= pathAssetsAdminRtlTpl %>/base/layout/footer.html',
                    meta: '<%= pathAssetsAdminRtlTpl %>/base/layout/_meta.html',
                    navbar: '<%= pathAssetsAdminRtlTpl %>/base/layout/navbar.html',
                    sidebar: '<%= pathAssetsAdminRtlTpl %>/base/layout/sidebar.html',
                    styleSelector: '<%= pathAssetsAdminRtlTpl %>/base/layout/style-selector.html',
                    panelConfiguration: '<%= pathAssetsAdminRtlTpl %>/base/layout/panel-configuration.html',
                    eventManagement: '<%= pathAssetsAdminRtlTpl %>/base/layout/event-management.html',
                    horizzontalMenu: '<%= pathAssetsAdminRtlTpl %>/base/layout/horizzontal-menu.html'
                }
            },
            data: {
                version: "2.0.0",
                title: "Clip-One - Responsive Admin Template",
                author: "ClipTheme",
                description: "Responsive Admin Template build with Twitter Bootstrap and jQuery",
                pathFavicon: "favicon.ico"
            }
        }
    }
}