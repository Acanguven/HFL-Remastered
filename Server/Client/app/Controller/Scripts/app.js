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
    .factory('alertService', function($rootScope,$timeout) {
        var alertService = {};

        // create an array of alerts available globally
        $rootScope.alerts = [];

        alertService.add = function(type, msg) {
            var num = Math.floor((Math.random() * 10000) + 1);
            $rootScope.alerts.push({type: type, msg: msg,num:num});
            $timeout(function() {
                var index = -1;
                for(var x = 0; x < $rootScope.alerts.length; x++){
                    if($rootScope.alerts[x].num == num){
                        index = x;
                    }
                }
                if(index > -1){
                    $rootScope.alerts.splice(index,1)
                }
            }, 2000);
        };

        alertService.closeAlert = function(index) {
            $rootScope.alerts.splice(index, 1);
        };
        $rootScope.closeAlert = alertService.closeAlert;

        return alertService;
    })
    .config(['$stateProvider', '$urlRouterProvider', '$ocLazyLoadProvider', "$httpProvider" , function($stateProvider, $urlRouterProvider, $ocLazyLoadProvider, $httpProvider) {
        
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
                    loadMyFile: function($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            name: 'controller',
                            files: [
                                'scripts/controllers/login.js',
                            ]
                        })
                    }
                }
            })
    }]);

