'use strict';
/**
 * @ngdoc function
 * @name sbAdminApp.controller:Home
 * @description
 * # MainCtrl
 * Controller of the sbAdminApp
 */
angular.module('sbAdminApp').controller('Home', function(Websocket ,$scope, $position, $stateParams, $state, $http) {
	$scope.trialRemains = function(){
		var remaining = $scope.user.userData.trial - $scope.user.date;
		return Math.round(remaining / 60000) + " minutes remain";
	}

	$scope.hwidRemains = function(){
		var remaining = $scope.user.userData.hwidCanChange - $scope.user.date;
		return "Available in " + Math.round(remaining / 60000) + " minutes";
	}
});
