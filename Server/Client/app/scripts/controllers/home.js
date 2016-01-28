'use strict';
/**
 * @ngdoc function
 * @name sbAdminApp.controller:Home
 * @description
 * # MainCtrl
 * Controller of the sbAdminApp
 */
angular.module('sbAdminApp').controller('Home', function($scope, $position, $stateParams, $state, $http) {
	$scope.trialRemains = function(){
		return Math.round($scope.user.userData.trial / 60000) + " minutes remain";
	}

	$scope.hwidRemains = function(){
		return "Available in " + Math.round($scope.user.userData.hwidCanChange / 60000) + " minutes";
	}
});
