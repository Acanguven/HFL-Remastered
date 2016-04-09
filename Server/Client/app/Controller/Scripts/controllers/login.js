'use strict';
/**
 * @ngdoc function
 * @name sbAdminApp.controller:login
 * @description
 * # MainCtrl
 * Controller of the sbAdminApp
 */
angular.module('controller').controller('login', function(Websocket,$scope,$http,$state,$stateParams) {
	if(NET.isAuthenticated()){

	}

	$scope.login = function(username,password){
		NET.login(username,password, function(res){
			alert(res);
		});
	}

	$scope.openRegister = function(){
		//NET.openRegister();
		NET.resize(1024,768);
	}
});