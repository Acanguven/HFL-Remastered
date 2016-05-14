'use strict';
/**
 * @ngdoc function
 * @name sbAdminApp.controller:login
 * @description
 * # MainCtrl
 * Controller of the sbAdminApp
 */
angular.module('controller').controller('settings', function (Websocket, $scope, $http, $state, $stateParams, $rootScope) {
    /*if (!$rootScope.authenticated) {
        $state.go("login");
        return;
    }*/
    $scope.mySelect = NET.loadSettings();
    $scope.currentTemplate = $scope.mySelect["theme"];
    $scope.templateList = templateList;
    $("body").addClass("loggedIn");
    $scope.initDone = false;

    $scope.updateTheme = function () {
        changeTemplate($scope.mySelect["theme"]);
    }

    $scope.$watch("mySelect", function () {
        if ($scope.initDone) {
            NET.saveSettings($scope.mySelect["theme"], $scope.mySelect["language"], $scope.mySelect["cPacketSearch"], $scope.mySelect["buyBoost"], $scope.mySelect["reconnect"], $scope.mySelect["disableGpu"], $scope.mySelect["injection"]);
        } else {
            $scope.initDone = true;
        }
    }, true);
});