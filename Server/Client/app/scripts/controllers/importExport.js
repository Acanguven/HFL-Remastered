'use strict';
/**
 * @ngdoc function
 * @name sbAdminApp.controller:importExport
 * @description
 * # MainCtrl
 * Controller of the sbAdminApp
 */

angular.module('sbAdminApp').controller('importExport', function($scope,$http,$stateParams) {

	if(!$scope.user){
		return false;
	}

	$http.get("http://handsfreeleveler.com:4446/api/getAI").then(function(res){
		$scope.exportedSettings = JSON.stringify(res.data);
	});

	$scope.exportedSettings = "";
	$scope.copyStatus = "Copy clicking this button";




	$scope.copied = function(){
		$scope.copyStatus = "Copied successfully!"	
	}
	$scope.copyfail = function(err){
		$scope.copyStatus = err;
	}

	$scope.import = function(json){
		$http.post("http://handsfreeleveler.com:4446/api/importAI", {ai:json}).then(function(res){
			$scope.importStatus = "Settings imported, please refresh page";
		});
	}

	$scope.importDefault = function(){
		$http.post("http://handsfreeleveler.com:4446/api/defaultAI").then(function(res){
			$scope.importStatus = "Default settings imported, please refresh page";
		});
	}

	$scope.importStatus = "Your settings will be deleted.";
});