'use strict';
/**
 * @ngdoc function
 * @name sbAdminApp.controller:login
 * @description
 * # MainCtrl
 * Controller of the sbAdminApp
 */
angular.module('sbAdminApp').controller('login', function(Websocket,$scope,$http,$state,$stateParams) {
	if(!$stateParams.redirect){
		top.location.href = "/";
	}
	$scope.errMsg = "";
	$scope.login = function(username,password){
		if(username.length > 2 && password.length > 2){
			$http.post("http://handsfreeleveler.com:4446/api/remotelogin",{username:username,password:password}).then(function(res){
				if(res.data.message){
					$scope.errMsg = res.data.message;
				}else{
					Websocket.send({type:"login",token:res.data.token});
					$state.go('dashboard.home',{user:res.data});
				}
			});
		}
	}
});