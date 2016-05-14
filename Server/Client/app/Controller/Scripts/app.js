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
    .module('controller', [
        'oc.lazyLoad',
        'ui.router',
        'ui.bootstrap',
        'angular-loading-bar',
        'angular-clipboard',
        'ngWebSocket'
    ])
    .factory('httpRequestInterceptor', ['$rootScope', function ($rootScope) {
        return {
            request: function ($config) {
                if ($rootScope.token) {
                    $config.headers['Authorization'] = $rootScope.token;
                }
                return $config;
            },

            requestError: function (config) {
                return config;
            },

            response: function (response) {
                if (response.headers('Authorization')) {
                    $rootScope.token = response.headers('Authorization');
                }
                if (response.data && response.data.type && response.data.type == "dead") {
                    location.reload();
                }
                return response;
            }
        };
    }])
    .filter('range', function () {
        return function (input, total) {
            total = parseInt(total);

            for (var i = 0; i < total; i++) {
                input.push(i);
            }

            return input;
        };
    }).directive('onFinishRender', function ($timeout) {
        return {
            restrict: 'A',
            link: function (scope, element, attr) {
                if (scope.$last === true) {
                    $timeout(function () {
                        scope.$emit('ngRepeatFinished');
                    });
                }
            }
        }
    })
    .directive('onSmurfDropped', function ($timeout) {
        return {
            restrict: 'A',
            link: function (scope, element, attr) {
                $(element).droppable({
                    accept: ".smurfDrag",
                    drop: function (event, ui) {
                        scope.$emit('smurfDropped', { event: event, ui: ui });
                    }
                });
            }
        }
    })
    .directive('rotationDropped', function ($timeout) {
        return {
            restrict: 'A',
            link: function (scope, element, attr) {
                $(element).droppable({
                    accept: ".smurfDrag",
                    drop: function (event, ui) {
                        scope.$emit('smurfDropped', { event: event, ui: ui });
                    }
                });
            }
        }
    })
    .directive('rotationJobDraggable', function ($timeout) {
        return {
            restrict: 'A',
            link: function (scope, element, attr) {
                $(element).draggable({
                    containment: '.container',
                    cursor: 'move',
                    helper: 'clone',
                    zIndex: 100000,
                    scroll: false,
                    start: function () { },
                    stop: function (event, ui) { }
                }).mousedown(function () { });
            }
        }
    })
    .directive('ngEnter', function () {
        return function (scope, element, attrs) {
            element.bind("keydown keypress", function (event) {
                if (event.which === 13) {
                    scope.$apply(function () {
                        scope.$eval(attrs.ngEnter);
                    });

                    event.preventDefault();
                }
            });
        };
    })
    .directive('qTip', function ($rootScope) {
        return {
            restrict: 'A',
            link: function (scope, element, attr) {
                var videoID = $rootScope.helpVideo.match(/(youtu\.be\/|&*v=|\/v\/|\/embed\/)+([A-Za-z0-9\-_]{5,11})/);
                if (!videoID || videoID.length < 1) { return; }
                var _api = null;
                videoID = videoID[2] + $.fn.qtip.nextid;
                $(element).qtip({
                    id: videoID,
                    content: $('<div />', { id: videoID }),
                    position: {
                        at: 'top left',
                        my: 'right bottom',
                        swap: true
                    },
                    show: {
                        event: 'click',
                        effect: function () {
                            var style = this[0].style;
                            style.display = 'none';
                            setTimeout(function () { style.display = 'block'; }, 1);
                        }
                    },
                    hide: 'unfocus',
                    style: {
                        classes: 'qtip-youtube',
                        width: 500,
                    },
                    events: {
                        render: function (event, api) {
                            new YT.Player(videoID, {
                                playerVars: {
                                    autoplay: 1,
                                    enablejsapi: 1,
                                    origin: document.location.host
                                },
                                origin: document.location.host,
                                height: 300,
                                width: 480,
                                videoId: videoID.substr(0, 11),
                                events: {
                                    'onReady': function (e) {
                                        api.player = e.target;
                                        _api = api;
                                    },
                                }
                            });
                        },
                        hide: function (event, api) {
                            api.player && api.player.stopVideo();
                        }
                    }
                });
                scope.$on('$destroy', function () {
                    _api && _api.player && _api.destroy(true);
                });
            }
        };
    })
    .filter('reverse', function () {
        return function (items) {
            if (items) {
                return items.slice().reverse();
            }
        };
    })
    .factory('alertService', function ($rootScope, $timeout) {
        var alertService = {};

        // create an array of alerts available globally
        $rootScope.alerts = [];

        alertService.add = function (type, msg) {
            var num = Math.floor((Math.random() * 10000) + 1);
            $rootScope.alerts.push({ type: type, msg: msg, num: num });
            $timeout(function () {
                var index = -1;
                for (var x = 0; x < $rootScope.alerts.length; x++) {
                    if ($rootScope.alerts[x].num == num) {
                        index = x;
                    }
                }
                if (index > -1) {
                    $rootScope.alerts.splice(index, 1)
                }
            }, 2000);
        };

        alertService.closeAlert = function (index) {
            $rootScope.alerts.splice(index, 1);
        };
        $rootScope.closeAlert = alertService.closeAlert;

        return alertService;
    })
    .config(['$stateProvider', '$urlRouterProvider', '$ocLazyLoadProvider', "$httpProvider", function ($stateProvider, $urlRouterProvider, $ocLazyLoadProvider, $httpProvider) {

        $httpProvider.interceptors.push('httpRequestInterceptor');

        $ocLazyLoadProvider.config({
            debug: false,
            events: false,
        });

        $urlRouterProvider.otherwise('/login');

        $stateProvider.state('login', {
            templateUrl: 'views/login.html',
            controller: 'login',
            url: '/login',
            params: {
                redirect: true
            },
            resolve: {
                loadMyFile: function ($ocLazyLoad) {
                    return $ocLazyLoad.load({
                        name: 'controller',
                        files: [
                            'scripts/controllers/login.js',
                        ]
                    })
                }
            }
        }).state('home', {
            templateUrl: 'views/home.html',
            controller: 'home',
            url: '/home',
            params: {
                redirect: true
            },
            resolve: {
                loadMyFile: function ($ocLazyLoad) {
                    return $ocLazyLoad.load({
                        name: 'controller',
                        files: [
                            'scripts/controllers/home.js',
                            'scripts/directives/dashboard/stats/stats.js',
                        ]
                    })
                }
            }
        }).state('settings', {
            templateUrl: 'views/settings.html',
            controller: 'settings',
            url: '/settings',
            params: {
                redirect: true
            },
            resolve: {
                loadMyFile: function ($ocLazyLoad) {
                    return $ocLazyLoad.load({
                        name: 'controller',
                        files: [
                            'scripts/controllers/settings.js',
                        ]
                    })
                }
            }
        }).state('smurfs', {
            templateUrl: 'views/smurfs.html',
            controller: 'smurfs',
            url: '/smurfs',
            params: {
                redirect: true
            },
            resolve: {
                loadMyFile: function ($ocLazyLoad) {
                    return $ocLazyLoad.load({
                        name: 'controller',
                        files: [
                            'scripts/controllers/smurfs.js',
                        ]
                    })
                }
            }
        }).state('rotations', {
            templateUrl: 'views/rotations.html',
            controller: 'rotations',
            url: '/rotations',
            params: {
                redirect: true
            },
            resolve: {
                loadMyFile: function ($ocLazyLoad) {
                    return $ocLazyLoad.load({
                        name: 'controller',
                        files: [
                            'scripts/controllers/rotations.js',
                        ]
                    })
                }
            }
        }).state('gpudisabler', {
            templateUrl: 'views/gpudisabler.html',
            controller: 'gpudisabler',
            url: '/gpudisabler',
            params: {
                redirect: true
            },
            resolve: {
                loadMyFile: function ($ocLazyLoad) {
                    return $ocLazyLoad.load({
                        name: 'controller',
                        files: [
                            'scripts/controllers/gpudisabler.js',
                        ]
                    })
                }
            }
        }).state('groups', {
            templateUrl: 'views/groups.html',
            controller: 'groups',
            url: '/groups',
            params: {
                redirect: true
            },
            resolve: {
                loadMyFile: function ($ocLazyLoad) {
                    return $ocLazyLoad.load({
                        name: 'controller',
                        files: [
                            'scripts/controllers/groups.js',
                        ]
                    })
                }
            }
        }).state('sessions', {
            templateUrl: 'views/sessions.html',
            controller: 'sessions',
            url: '/sessions',
            params: {
                redirect: true
            },
            resolve: {
                loadMyFile: function ($ocLazyLoad) {
                    return $ocLazyLoad.load({
                        name: 'controller',
                        files: [
                            'scripts/controllers/sessions.js',
                        ]
                    })
                }
            }
        })
    }]).run(function () {
        NET.ready();
    });