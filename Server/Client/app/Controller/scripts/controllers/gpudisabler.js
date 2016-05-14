'use strict';
/**
 * @ngdoc function
 * @name sbAdminApp.controller:login
 * @description
 * # MainCtrl
 * Controller of the sbAdminApp
 */
angular.module('controller').controller('gpudisabler', function (Websocket, $scope, $http, $state, $stateParams, $rootScope, $interval) {
    if (!$rootScope.authenticated) {
        $state.go("login");
        return;
    }

    $scope.games = [];
    $scope.activateGame = false;

    var refreshInterval;
    NET.loadMaskedGames(function (games) {
        $scope.games = games;

        if (angular.isDefined(refreshInterval)) return;
        refreshInterval = $interval($scope.systemRefresher, 1000);
    });

    $scope.showGame = function (game) {
        NET.ClearDW();
        $scope.activateGame = angular.copy(game);
        var element = $("#imgDW");
        NET.showMaskGoOn($scope.activateGame.processId, $(element).offset().left, $(element).offset().top, $(element).width(), $(element).height());
    }

    $scope.renderOpt = function (type) {
        NET.renderOpt($scope.activateGame.processId, type, function (val) {
            $scope.activateGame.masked = val;
        });
    }

    $scope.systemRefresher = function () {
        NET.loadMaskedGames(function (games) {
            $scope.games = games;
            if ($scope.activateGame != false) {
                var found = false;
                for (var x = 0; x < $scope.games.length; x++) {
                    if ($scope.games[x].processId == $scope.activateGame.processId) {
                        found = true;
                    }
                }
                if (!found) {
                    NET.ClearDW();
                    $scope.activateGame = false;
                }
            }
        });
    }

    $scope.stopInterval = function () {
        if (angular.isDefined(refreshInterval)) {
            $interval.cancel(refreshInterval);
            refreshInterval = undefined;
        }
    };

    $scope.$on("$destroy", function () {
        $scope.stopInterval();
        NET.ClearDW();
    });
});