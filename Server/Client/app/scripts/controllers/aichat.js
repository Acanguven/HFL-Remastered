'use strict';
/**
 * @ngdoc function
 * @name sbAdminApp.controller:MainCtrl
 * @description
 * # MainCtrl
 * Controller of the sbAdminApp
 */
angular.module('sbAdminApp').controller('aichat', function($scope, $http, $rootScope, Websocket) {
	if(!$scope.user){
		return false;
	}

	$scope.selectedCase = "GAME START";

	$scope.cases = [
		"GAME START",
	    "ON DEAD",
	    "ON KILL",
	    "ON ASSIST",
	    "ALLY TOWER DESTROYED",
	    "ENEMY TOWER DESTROYED",
	    "ALLY CHAMPION KILLS 3 IN A ROW",
	    "ALLY CHAMPION KILLS 4 IN A ROW",
	    "ALLY CHAMPION KILLS 5 IN A ROW",
	    "ENEMY CHAMPION KILLS 3 IN A ROW",
	    "ENEMY CHAMPION KILLS 4 IN A ROW",
	    "ENEMY CHAMPION KILLS 5 IN A ROW",
	    "GAME LOST",
	    "GAME WON",
	    "SOMEONE CALLED YOU ON CHAT"
    ]

	$scope.aichat = {};

	$scope.cases.forEach(function(chatCase){
		$scope.aichat[chatCase] = [];
	});

	$scope.add = function(text,cas,perc){
		if(perc){
			$scope.aichat[cas].push({
				text:text,
				perc:perc
			});
		}
	}

	$scope.remove = function(msg,cas){
		var index = cas.indexOf(msg);
		cas.splice(index,1);
	}

	$scope.save = function(map){
        var chatSave = angular.copy($scope.aichat)
    	$http.post("http://handsfreeleveler.com:4446/api/updateChat",{chat:chatSave}).then(function(res){
    		$scope.saveStatus = "Saved"
    	});
    }

    $http.get("http://handsfreeleveler.com:4446/api/getChat").then(function(res){
        if(res.data[$scope.cases[0]]){
        	$scope.aichat = res.data;
        }
    });
});