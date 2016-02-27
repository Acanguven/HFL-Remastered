'use strict';
/**
 * @ngdoc overview
 * @name sbAdminApp
 * @description
 * # sbAdminApp
 *
 * Main module of the application.
 */
angular
    .module('sbAdminApp', [
        'oc.lazyLoad',
        'ui.router',
        'ui.bootstrap',
        'angular-loading-bar',
        'angular-clipboard',
        'ngWebSocket'
    ])
    .factory('httpRequestInterceptor',['$rootScope', function($rootScope){
        return {
            request: function($config) {
                if( $rootScope.token ) {
                    $config.headers['Authorization'] = $rootScope.token;
                }
                return $config;
            },

            requestError: function(config) {
                return config;
            },

            response: function(response){
                if(response.headers('Authorization')){
                    $rootScope.token = response.headers('Authorization');
                }
                if(response.data && response.data.type && response.data.type == "dead"){
                    location.reload();
                }
                return response;
            }
        };
    }])
    .filter('range', function() {
        return function(input, total) {
            total = parseInt(total);

            for (var i=0; i<total; i++) {
              input.push(i);
            }

            return input;
        };
    })
    .directive('ngEnter', function () {
        return function (scope, element, attrs) {
            element.bind("keydown keypress", function (event) {
                if(event.which === 13) {
                    scope.$apply(function (){
                        scope.$eval(attrs.ngEnter);
                    });
     
                    event.preventDefault();
                }
            });
        };
    })
    .filter('reverse', function() {
        return function(items) {
            return items.slice().reverse();
        };
    })
    .config(['$stateProvider', '$urlRouterProvider', '$ocLazyLoadProvider', "$httpProvider" , function($stateProvider, $urlRouterProvider, $ocLazyLoadProvider, $httpProvider) {
        
        $httpProvider.interceptors.push('httpRequestInterceptor');

        $ocLazyLoadProvider.config({
            debug: false,
            events: false,
        });

        $urlRouterProvider.otherwise('/login');

        $stateProvider
            .state('dashboard', {
                url: '/dashboard',
                controller: 'MainCtrl',
                templateUrl: 'views/dashboard/main.html',
                params: {
                    user: null
                },
                resolve: {
                    loadMyDirectives: function($ocLazyLoad) {
                        return $ocLazyLoad.load({
                                name: 'sbAdminApp',
                                files: [
                                    'scripts/controllers/main.js',
                                    'scripts/directives/header/header.js',
                                    'scripts/directives/header/header-notification/header-notification.js',
                                    'scripts/directives/sidebar/sidebar.js',
                                    'scripts/directives/sidebar/sidebar-search/sidebar-search.js'
                                ]
                            }),
                            $ocLazyLoad.load({
                                name: 'toggle-switch',
                                files: ["bower_components/angular-toggle-switch/angular-toggle-switch.min.js",
                                    "bower_components/angular-toggle-switch/angular-toggle-switch.css"
                                ]
                            }),
                            $ocLazyLoad.load({
                                name: 'ngAnimate',
                                files: ['bower_components/angular-animate/angular-animate.js']
                            })
                        $ocLazyLoad.load({
                            name: 'ngCookies',
                            files: ['bower_components/angular-cookies/angular-cookies.js']
                        })
                        $ocLazyLoad.load({
                            name: 'ngResource',
                            files: ['bower_components/angular-resource/angular-resource.js']
                        })
                        $ocLazyLoad.load({
                            name: 'ngSanitize',
                            files: ['bower_components/angular-sanitize/angular-sanitize.js']
                        })
                        $ocLazyLoad.load({
                            name: 'ngTouch',
                            files: ['bower_components/angular-touch/angular-touch.js']
                        })
                    }
                }
            })
            .state('dashboard.home', {
                url: '/home',
                templateUrl: 'views/dashboard/home.html',
                controller: 'Home',
                resolve: {
                    loadMyFiles: function($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            name: 'sbAdminApp',
                            files: [
                                'scripts/directives/timeline/timeline.js',
                                'scripts/controllers/home.js',
                                'scripts/directives/notifications/notifications.js',
                                'scripts/directives/chat/chat.js',
                                'scripts/directives/dashboard/stats/stats.js'
                            ]
                        })
                    }
                }
            })
            .state('dashboard.form', {
                templateUrl: 'views/form.html',
                url: '/form'
            })
            .state('dashboard.blank', {
                templateUrl: 'views/pages/blank.html',
                url: '/blank'
            })
            .state('login', {
                templateUrl: 'views/pages/login.html',
                controller: 'login',
                url: '/login',
                params: {
                    redirect: true
                },
                resolve: {
                    loadMyFile: function($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            name: 'sbAdminApp',
                            files: [
                                'scripts/controllers/login.js',
                            ]
                        })
                    }
                }
            })
            .state('dashboard.chart', {
                templateUrl: 'views/chart.html',
                url: '/chart',
                controller: 'ChartCtrl',
                resolve: {
                    loadMyFile: function($ocLazyLoad) {
                        return $ocLazyLoad.load({
                                name: 'chart.js',
                                files: [
                                    'bower_components/angular-chart.js/dist/angular-chart.min.js',
                                    'bower_components/angular-chart.js/dist/angular-chart.css'
                                ]
                            }),
                            $ocLazyLoad.load({
                                name: 'sbAdminApp',
                                files: ['scripts/controllers/chartContoller.js']
                            })
                    }
                }
            })
            .state('dashboard.table', {
                templateUrl: 'views/table.html',
                url: '/table'
            })
            .state('dashboard.panels-wells', {
                templateUrl: 'views/ui-elements/panels-wells.html',
                url: '/panels-wells'
            })
            .state('dashboard.buttons', {
                templateUrl: 'views/ui-elements/buttons.html',
                url: '/buttons'
            })
            .state('dashboard.notifications', {
                templateUrl: 'views/ui-elements/notifications.html',
                url: '/notifications'
            })
            .state('dashboard.typography', {
                templateUrl: 'views/ui-elements/typography.html',
                url: '/typography'
            })
            .state('dashboard.icons', {
                templateUrl: 'views/ui-elements/icons.html',
                url: '/icons'
            })
            .state('dashboard.grid', {
                templateUrl: 'views/ui-elements/grid.html',
                url: '/grid'
            })
            .state('dashboard.stats', {
                templateUrl: 'views/ui-elements/stats.html',
                url: '/home',
                resolve: {
                    loadMyFile: function($ocLazyLoad) {
                        return $ocLazyLoad.load({
                                name: 'chart.js',
                                files: [
                                    'bower_components/angular-chart.js/dist/angular-chart.min.js',
                                    'bower_components/angular-chart.js/dist/angular-chart.css'
                                ]
                            }),
                            $ocLazyLoad.load({
                                name: 'sbAdminApp',
                                files: ['scripts/controllers/chartContoller.js']
                            })
                    }
                }
            })
            .state('dashboard.smurfs', {
                controller: "smurfs",
                templateUrl: 'views/ui-elements/smurfs.html',
                url: '/smurfs',
                resolve: {
                    loadMyFiles: function($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            name: 'sbAdminApp',
                            files: [
                                'scripts/controllers/smurfs.js',
                            ]
                        })
                    }
                }
            })
            .state('dashboard.shell', {
                controller: "shell",
                templateUrl: 'views/ui-elements/shell.html',
                url: '/shell',
                resolve: {
                    loadMyFiles: function($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            name: 'sbAdminApp',
                            files: [
                                'scripts/controllers/shell.js',
                            ]
                        })
                    }
                }
            })
            .state('dashboard.subscription', {
                controller: "subscription",
                templateUrl: 'views/ui-elements/subscription.html',
                url: '/subscription',
                resolve: {
                    loadMyFiles: function($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            name: 'sbAdminApp',
                            files: [
                                'scripts/controllers/subscription.js',
                            ]
                        })
                    }
                }
            })
            .state('dashboard.bol', {
                controller: "bol",
                templateUrl: 'views/ui-elements/bol.html',
                url: '/bol',
                resolve: {
                    loadMyFiles: function($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            name: 'sbAdminApp',
                            files: [
                                'scripts/controllers/bol.js',
                            ]
                        })
                    }
                }
            })
            .state('dashboard.banners', {
                controller: "banners",
                templateUrl: 'views/ui-elements/banners.html',
                url: '/banners',
                resolve: {
                    loadMyFiles: function($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            name: 'sbAdminApp',
                            files: [
                                'scripts/controllers/banners.js',
                            ]
                        })
                    }
                }
            })
            .state('dashboard.settings', {
                templateUrl: 'views/ui-elements/settings.html',
                controller: 'settings',
                url: '/settings',
                resolve: {
                    loadMyFiles: function($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            name: 'sbAdminApp',
                            files: [
                                'scripts/controllers/settings.js',
                            ]
                        })
                    }
                }
            })
            .state('dashboard.chat', {
                templateUrl: 'views/ui-elements/chat.html',
                url: '/chat',
                resolve: {
                    loadMyFiles: function($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            name: 'sbAdminApp',
                            files: [
                                'scripts/directives/chat/chat.js',
                            ]
                        })
                    }
                }
            })
            .state('dashboard.remote', {
                templateUrl: 'views/ui-elements/remote.html',
                url: '/remote',
                controller: 'remote',
                resolve: {
                    loadMyFiles: function($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            name: 'sbAdminApp',
                            files: [
                                'scripts/controllers/remote.js',
                                'scripts/directives/chat/chat.js',
                                'scripts/directives/game/herobox.js',
                                'scripts/directives/game/item.js',
                            ]
                        })
                    }
                }
            }).state('dashboard.logs', {
                templateUrl: 'views/ui-elements/logs.html',
                url: '/logs',
                resolve: {
                    loadMyFiles: function($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            name: 'sbAdminApp',
                            files: [
                                'scripts/controllers/logger.js',
                            ]
                        })
                    }
                },
                controller : 'logger'
            }).state('dashboard.items', {
                resolve: {
                    loadMyFile: function($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            name: 'sbAdminApp',
                            files: [
                                'scripts/directives/game/herobox.js',
                                'scripts/controllers/items.js'
                            ]
                        })
                    },
                },
                templateUrl: 'views/ui-elements/champs.html',
                url: '/items',
                controller: "Items"
            }).state('dashboard.championItems', {
                templateUrl: 'views/ui-elements/champItems.html',
                url: '/items/:heroName',
                controller: "heroItems",
                resolve: {
                    loadMyFile: function($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            name: 'sbAdminApp',
                            files: [
                                'scripts/controllers/heroItems.js',
                                'scripts/directives/game/herobox.js',
                                'scripts/directives/game/item.js'
                            ]
                        })
                    }
                }
            }).state("dashboard.impexp", {
                templateUrl: 'views/ui-elements/impexp.html',
                url: '/ImportAndExport/',
                controller: "importExport",
                resolve: {
                    loadMyFile: function($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            name: 'sbAdminApp',
                            files: [
                                'scripts/controllers/importExport.js',
                            ]
                        })
                    }
                }
            });
    }]);

