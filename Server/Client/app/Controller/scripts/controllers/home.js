'use strict';
/**
 * @ngdoc function
 * @name sbAdminApp.controller:login
 * @description
 * # MainCtrl
 * Controller of the sbAdminApp
 */
angular.module('controller').controller('home', function (Websocket, $scope, $http, $state, $stateParams, $rootScope, $interval) {
    if (!$rootScope.authenticated) {
        $state.go("login");
        return;
    }
    $scope._repeatTimes = 1;
    $scope.repeatTimes = 1;
    NET.resize(1160, 750);
    if (!$rootScope.centered) {
        NET.centerScreen();
        $rootScope.centered = true;
    }
    $scope.user = $rootScope.user;
    $("body").addClass("loggedIn");
    

    $scope.action = function (t) {
        switch (t) {
            case 0:
                alert("pause");
                break;
            case 1:
                alert("stop");
                break;
            case 2:
                alert("start");
                break;
            case 3:
                alert("resume");
                break;
        }
    }

    $scope.$watch("repeatTimes", function () {
        var num = Number($scope.repeatTimes);
        if (!isNaN(num) && $scope.repeatTimes != null && $scope.repeatTimes != 0) {
            $scope._repeatTimes = num;
        } else {
            $scope.repeatTimes = $scope._repeatTimes;
        }
    });

    $scope.action = function (t) {
        NET.action(t, $scope._repeatTimes);
    }

    $scope.showGames = function () {
        $state.go("gpudisabler");
    }

    $scope.showSessions = function () {
        $state.go("sessions");
    }
});