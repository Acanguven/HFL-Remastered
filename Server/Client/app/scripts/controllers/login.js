'use strict';
/**
 * @ngdoc function
 * @name sbAdminApp.controller:login
 * @description
 * # MainCtrl
 * Controller of the sbAdminApp
 */
angular.module('sbAdminApp').controller('login', function($scope,$http,$state) {
	$scope.errMsg = "";
	$scope.login = function(username,password){
		if(username.length > 3 && password.length > 3){
			$http.post("http://localhost:3000/api/remotelogin",{username:username,password:password}).then(function(res){
				if(res.data.message){
					$scope.errMsg = res.data.message;
				}else{
					$state.go('dashboard.home',{user:res.data});
				}
			});
		}
	}
});