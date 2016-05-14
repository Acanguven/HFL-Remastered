'use strict';
/**
 * @ngdoc function
 * @name sbAdminApp.controller:sessions
 * @description
 * # MainCtrl
 * Controller of the sbAdminApp
 */
angular.module('controller').controller('sessions', function (Websocket, $scope, $http, $state, $stateParams, $rootScope, $timeout) {
    if (!$rootScope.authenticated) {
        $state.go("login");
        return;
    }
    
    NET.getSessions(function (sessions) {
        $scope.sessions = sessions;
    })

    $scope.selectSession = function (session) {
        $scope.selectedSession = session;
    }

    $scope.selectedSession = false;
});