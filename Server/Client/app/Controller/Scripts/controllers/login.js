'use strict';
/**
 * @ngdoc function
 * @name sbAdminApp.controller:login
 * @description
 * # MainCtrl
 * Controller of the sbAdminApp
 */
angular.module('controller').controller('login', function (Websocket, $scope, $http, $state, $stateParams, $rootScope, $interval) {

    $scope.pendingAnswer = false;

    if (NET.isAuthenticated()) {

    }

    NET.requestLogin(function (username, password) {
        $scope.username = username;
        $scope.password = password;
        $scope.login(username, password);
    });


    $scope.login = function (username, password) {
        if (username.length > 0) {
            $scope.pendingAnswer = true;
            NET.login(username, password, function (res) {
                $rootScope.status = {};
                $rootScope.updateStatus = function (status) {
                    $rootScope.status = status;
                }
                NET.registerHomeCb($scope.updateStatus);
                NET.getBolStatus(function (val) {
                    $rootScope.bolRunning = val;
                });
                $rootScope.sessionTime = "00:00:00";
                $interval(function () {
                    if ($rootScope.status && $rootScope.status.running) {
                        NET.getSessionTime(function (val) {
                            $rootScope.sessionTime = val;
                        });
                    }
                }, 500);
                $scope.pendingAnswer = false;
                if (res !== false) {
                    $rootScope.authenticated = true;
                    $rootScope.user = JSON.parse(res);
                    $("body").addClass("loggedIn");
                    $state.go('home');
                }
            });
        }
    }

    $scope.openRegister = function () {
        //NET.openRegister();
        NET.updateNotificationCount(44);
        NET.resize(1024, 768);
    }
});