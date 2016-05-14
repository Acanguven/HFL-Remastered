'use strict';
/**
 * @ngdoc function
 * @name sbAdminApp.controller:login
 * @description
 * # MainCtrl
 * Controller of the sbAdminApp
 */
angular.module('controller').controller('smurfs', function (Websocket, $scope, $http, $state, $stateParams, $rootScope) {
    if (!$rootScope.authenticated) {
        $state.go("login");
        return;
    }
    
    NET.loadSmurfs(function (smurfs) {
        $scope.initDone = false;
        $scope.smurfs = smurfs;
    });

    if ($scope.smurfs == null) {
        $scope.smurfs = [];
    }
    $("body").addClass("loggedIn");

    $scope.defaultOptions = [
	    { name: '1', value: "1" },
	    { name: 'Summoners Rift', value: 2 },
	    { name: 'EUW', value: 'EUW' },
    ];

    $scope.refresher = function () {
        $scope.newSmurf = {
            desiredLevel: $scope.defaultOptions[0].value,
            queue: $scope.defaultOptions[1].value,
            region: $scope.defaultOptions[2].value,
        };
    }

    $scope.add = function (smurf) {
        var newSmurf = angular.copy(smurf);
        newSmurf.id = Date.now().toString();
        newSmurf.currentLevel = "?";
        newSmurf.currentrp = "?";
        newSmurf.currentip = "?";
        $scope.smurfs.push(newSmurf);
        $scope.refresher();
        $scope.save();
    }

    $scope.removeSmurf = function (sr) {
        var srindex = $scope.smurfs.indexOf(sr);
        $scope.smurfs.splice(srindex, 1);
        $scope.save();
    }

    $scope.save = function () {
        NET.saveSmurfs($scope.smurfs);
    }

    $scope.loadFromFile = function () {
        NET.loadFromFile(function (smurfsAsText) {
            var lineSmurfs = smurfsAsText.split("\n");
            for (var x = 0; x < lineSmurfs.length; x++) {
                var smurfArr = lineSmurfs[x].split("|");
                var smurf = {
                    username: smurfArr[0],
                    password: smurfArr[1],
                    region: smurfArr[2],
                    queue: smurfArr[3],
                    desiredLevel: Number(smurfArr[4]),
                }
                
                smurf.currentLevel = "?";
                smurf.currentrp = "?";
                smurf.currentip = "?";
                $scope.smurfs.push(smurf);
            }
        });
    }

    $scope.$watch("smurfs", function () {
        if ($scope.initDone) {
            $scope.save();
        } else {
            $scope.initDone = true;
        }
    },true);

    $scope.refresher();
});