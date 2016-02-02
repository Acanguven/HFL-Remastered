'use strict';
/**
 * @ngdoc function
 * @name sbAdminApp.controller:login
 * @description
 * # MainCtrl
 * Controller of the sbAdminApp
 */
angular.module('sbAdminApp').controller('shell', function(Websocket,$scope,$state, $timeout,$rootScope) {
	if(!$rootScope.token){
		return false;
	}
	$scope.cmdInput = "";

	$scope.submit = function(text){
		$scope.cmdInput = "";
		Websocket.send({type:"cmdWrite",text:text});
	}

	if(!$rootScope.lastCommands){
		$rootScope.lastCommands = [];
	}
	$scope.waitingNewOutput = [];

	Websocket.on("cmdLog", function(data){
		$scope.waitingNewOutput.push(data.text);
		$timeout(function () {
		    if($scope.waitingNewOutput.length > 0){
		    	$scope.waitingNewOutput.forEach(function(output){
		    		$rootScope.lastCommands.push(output);
		    	})
		    	$scope.waitingNewOutput = [];
		    	$timeout(function () {
		    		$scope.updateDiv();
		    	},50)
		    }
		}, 50);
	},true)

	$scope.updateDiv = function(){
		var cmdUl = document.getElementById("cmdUl");
		cmdUl.scrollTop = cmdUl.scrollHeight;
	}

	$scope.lastCommands = $rootScope.lastCommands;
	$timeout(function () {
		$scope.updateDiv();
	},200)
});