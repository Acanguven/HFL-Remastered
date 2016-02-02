'use strict';
/**
 * @ngdoc function
 * @name sbAdminApp.controller:settings
 * @description
 * # MainCtrl
 * Controller of the sbAdminApp
 */
angular.module('sbAdminApp').controller('settings', function($scope,$http,$rootScope,Websocket) {
	if(!$scope.user){
		return false;
	}

	$scope.$watch(function(){
		return $scope.user.userData.settings;
	},function(){
		if($scope.controller){
			Websocket.send({type:"updateSettings",settings:$scope.user.userData.settings});
		}
		$http.post("http://localhost:3000/api/updateSettings",{settings:$scope.user.userData.settings});
	},true)
});