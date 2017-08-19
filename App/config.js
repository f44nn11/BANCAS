function config($stateProvider, $urlRouterProvider, $ocLazyLoadProvider, IdleProvider, KeepaliveProvider) {
    // Configure Idle settings
    IdleProvider.idle(5); // in seconds
    IdleProvider.timeout(120); // in seconds

    $urlRouterProvider.otherwise("/login");

    $ocLazyLoadProvider.config({
        // Set to true if you want to see what and when is dynamically loaded
        debug: false
    });

    $stateProvider
    .state('appmain', {
        url: "/appmain",
        //templateUrl: "./App/views/common/content.aspx",
        //controller: appmainControllers,
        views: {
            "@": {
                templateUrl: "./App/views/common/content.aspx",
                controller: appmainControllers
            }
        },
        data: {
            displayName: 'Home'
        }
    })
    .state('login', {
        url: "/login",
        views: {
            "@": {
                templateUrl: "./App/views/login.aspx",
                controller: loginControllers
            }
        },
        //templateUrl: "./App/views/login.aspx",
        //controller: loginControllers,
        data: { pageTitle: 'Login', specialClass: 'gray-bg' }
        //
    })
    .state('layouts', {
        url: "/layouts",
        templateUrl: "./App/views/layout.aspx",
        data: { pageTitle: 'Layouts' },
        controller: layoutControllers
    })
    .state('appmain.application', {
        //abstract: true,
        url: "/application",
        views: {
            "@appmain.application": {
                templateUrl: "./App/views/user/application.aspx"
            }
        }, 
        //templateUrl: "./App/views/user/application.aspx",
        data: {
            displayName: "Application"
        }
    })
    .state('appmain.application.user', {
        url: "/user",
        views: {
            "@appmain.application.user": {
                templateUrl: "./App/views/user/user.aspx"
                //controller: usermanageControllers
            }
        },
        data: {
            displayName: "User Management"
        }
    })
    .state('appmain.application.user.manage', {
        url: "/manage",
        views: {
            "@": {
                templateUrl: "./App/views/common/content.aspx",
                controller: appmainControllers
            },
            "@appmain.application.user.manage": {
                templateUrl: "./App/views/user/usermanagement.aspx",
                controller: usermanageControllers
            }
        },
        //controller: usermanageControllers,
        data: {
            pageTitle: 'User Management',
            displayName: 'Application of User Authorization'
            },
        resolve: {
            loadPlugin: function ($ocLazyLoad) {
                return $ocLazyLoad.load([
                    {
                        serie: true,
                        files: ['Scripts/plugins/dataTables/jquery.dataTables.js', 'Content/css/plugins/dataTables/dataTables.bootstrap.css']
                    },
                    {
                        serie: true,
                        files: ['Scripts/plugins/dataTables/dataTables.bootstrap.js']
                    },
                    {
                        name: 'datatables',
                        files: ['Scripts/plugins/dataTables/angular-datatables.min.js']
                    },
                    {
                        insertBefore: '#loadBefore',
                        name: 'toaster',
                        files: ['Scripts/plugins/toastr/toastr.min.js', 'Content/css/plugins/toastr/toastr.min.css']
                    },
                    
                    {
                        files: ['Content/css/plugins/iCheck/custom.css', 'Scripts/plugins/iCheck/icheck.min.js']
                    },
                    {
                        serie: true,
                        files: ['Scripts/plugins/footable/footable.all.min.js', 'Content/css/plugins/footable/footable.core.css']
                    },
                    {
                        name: 'ui.footable',
                        files: ['Scripts/plugins/footable/angular-footable.js']
                    },
                    {
                        files: ['Content/css/plugins/awesome-bootstrap-checkbox/awesome-bootstrap-checkbox.css']
                    },
                    {
                        insertBefore: '#loadBefore',
                        name: 'localytics.directives',
                        files: ['Content/css/plugins/chosen/chosen.css', 'Scripts/plugins/chosen/chosen.jquery.js', 'Scripts/angular/angular-chosen.min.js']
                    }
                ]);
            }
        }
        
    })
    .state('appmain.application.user.contact', {
        url: "/contact",
        views: {
            "@": {
                templateUrl: "./App/views/common/content.aspx",
                controller: appmainControllers
            },
            '@appmain.application.user.contact': {
                templateUrl: "./App/views/user/contact.aspx",
                controller: contactControllers
            }
        },
        //templateUrl: "./App/views/user/contact.aspx",
        //controller: contactControllers,
        data: {
            pageTitle: 'contact',
            displayName: 'Contact'
        },
        resolve: {
            loadPlugin: function ($ocLazyLoad) {
                return $ocLazyLoad.load([
                    {
                        files: ['Content/css/plugins/dropzone/basic.css', 'Content/css/plugins/dropzone/dropzone.css', 'Scripts/plugins/dropzone/dropzone.js']
                    }
                ]);
            }
        }
    })
    .state('application.user.password', {
        url: "/password",
        views: {
            '@application': {
                templateUrl: "./App/views/user/changepass.aspx",
                controller: changepassControllers
            }
        }
    })
    .state('application.user.password.change', {
        url: "/change",
        templateUrl: "./App/views/user/changepass.aspx",
        data: { pageTitle: 'Change Password' },
        controller: changepassControllers
    })
    .state('auxiliary', {
        abstract: true,
        url: "/auxiliary",
        templateUrl: "./App/views/common/content.aspx"
    })
    .state('auxiliary.holiday', {
        url: "/holiday",
        templateUrl: "./App/views/user/usermanagement.aspx",
        data: { pageTitle: 'Holiday' },
        resolve: {
            loadPlugin: function ($ocLazyLoad) {
                return $ocLazyLoad.load([
                    {
                        serie: true,
                        files: ['Scripts/plugins/dataTables/jquery.dataTables.js','Content/css/plugins/dataTables/dataTables.bootstrap.css']
                    },
                    {
                        serie: true,
                        files: ['Scripts/plugins/dataTables/dataTables.bootstrap.js']
                    },
                    {
                        name: 'datatables',
                        files: ['Scripts/plugins/dataTables/angular-datatables.min.js']
                    }
                ]);
            }
        }
        //,controller: usermanageController
    })
}

angular
    .module('WebApp')
    .config(config)
    .run(function ($rootScope, $state) {
        $rootScope.$state = $state;
    });
