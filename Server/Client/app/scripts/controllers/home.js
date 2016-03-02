'use strict';
/**
 * @ngdoc function
 * @name sbAdminApp.controller:Home
 * @description
 * # MainCtrl
 * Controller of the sbAdminApp
 */
angular.module('sbAdminApp').controller('Home', function(Websocket ,$scope, $position, $stateParams, $state, $http) {
	if(!$scope.user){
		return false;
	}

	$scope.trialRemains = function(){
		var remaining = $scope.user.userData.trial - $scope.user.date;
		return Math.round(remaining / 60000) + " minutes remain";
	}

	$scope.hwidRemains = function(){
		var remaining = $scope.user.userData.hwidCanChange - $scope.user.date;
		return "Available in " + Math.round(remaining / 60000) + " minutes";
	}

	$scope.bolUsername = function(){
		if ($scope.user.userData.bol && scope.user.userData.bol.length > 1){
			return $scope.user.userData.bol;
		}else{
			return false;
		}
	}

	$scope.timeLineLogs = [];

	$http.get("http://handsfreeleveler.com:4446/api/getLogs").then(function(res){
		$scope.user.userData.logs = res.data.logs;
		$scope.buildTimelineLogs(res.data.logs)
	});

	$scope.buildTimelineLogs = function(logs){
		var d = new Date();
		var today = [('0' + (d.getDate())).slice(-2),('0' + (d.getMonth() +1)).slice(-2),d.getFullYear()].join(".")
		
		logs.forEach(function(log){
			if(log.date.split(" ")[0] == today && log.code != "info" && log.smurf){
				console.log(log.code)
				$scope.timeLineLogs.push(log);
			}
		});
	}

	function parseDate(input) {
	  	var parts = input.match(/(\d+)/g);
	  	return new Date(parts[0], parts[1]-1, parts[2]);
	}
});
