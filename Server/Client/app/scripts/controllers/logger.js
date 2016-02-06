'use strict';
/**
 * @ngdoc function
 * @name sbAdminApp.controller:MainCtrl
 * @description
 * # MainCtrl
 * Controller of the sbAdminApp
 */
angular.module('sbAdminApp').controller('logger', function($scope, $http, $rootScope, Websocket) {
	if(!$scope.user){
		return false;
	}
	$scope.logSmurf = false;

	$http.get("http://handsfreeleveler.com:4446/api/getLogs").then(function(res){
		$scope.user.userData.logs = res.data.logs;
	});

	$scope.smurfsFromLogs = function(){
		var smurfs = [];
		$scope.user.userData.logs.forEach(function(log){
			if(log.smurf){
				var index = smurfs.indexOf(log.smurf);
				if(index < 0){
					smurfs.push(log.smurf);
				}
			}
		});
		return smurfs;
	}

	$scope.systemLogs = function(){
		var logs = [];
		$scope.user.userData.logs.forEach(function(log){
			if(!log.smurf){
				logs.push(log);	
			}
		});
		return logs;
	}

	$scope.smurfLogs = function(){
		var logs = [];
		$scope.user.userData.logs.forEach(function(log){
			if(log.smurf && log.smurf == $scope.logSmurf){
				logs.push(log);	
			}
		});
		return logs;
	}

	$scope.show = function(smurf){
		$scope.logSmurf = smurf;
	}

	Websocket.on("log",function(data){
		$scope.user.userData.logs.push(data.log);
	},true);
});