'use strict';
/**
 * @ngdoc function
 * @name sbAdminApp.controller:importExport
 * @description
 * # MainCtrl
 * Controller of the sbAdminApp
 */

angular.module('sbAdminApp').controller('importExport', function($scope,$http,$stateParams) {
	$scope.exportedSettings = "testingasd";
	$scope.copyStatus = "Copy clicking this button";
	$scope.copied = function(){
		$scope.copyStatus = "Copied successfully!"	
	}
	$scope.copyfail = function(err){
		$scope.copyStatus = err;
	}
});