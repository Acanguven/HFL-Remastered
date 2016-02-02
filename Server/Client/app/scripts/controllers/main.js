'use strict';
/**
 * @ngdoc function
 * @name sbAdminApp.controller:MainCtrl
 * @description
 * # MainCtrl
 * Controller of the sbAdminApp
 */
angular.module('sbAdminApp').controller('MainCtrl', function($scope, $position, $stateParams,$state, $http, $rootScope, Websocket,$interval) {
	if(!$stateParams.user){
		$state.go('login');
	}else{
		$scope.user = angular.copy($stateParams.user);
		$rootScope.token = $scope.user.token
	}

	$rootScope.liveData = {
		groups:[],
		smurfs:[]
	}
	
	$interval(function(){
		if($rootScope.token){
			$http.get("http://localhost:3000/api/ping");
		}
	},1000 * 30)
});
