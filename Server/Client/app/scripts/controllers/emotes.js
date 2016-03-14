'use strict';
/**
 * @ngdoc function
 * @name sbAdminApp.controller:MainCtrl
 * @description
 * # MainCtrl
 * Controller of the sbAdminApp
 */
angular.module('sbAdminApp').controller('emotes', function($scope, $http, $rootScope, Websocket) {
	if(!$scope.user){
		return false;
	}

	$scope.emotes = {
		onkill:false,
		betterstate:false,
		spellmiss:false
	};

	
	$scope.save = function(){
        var emoteSave = angular.copy($scope.emotes)
    	$http.post("http://handsfreeleveler.com:4446/api/updateEmotes",{emotes:emoteSave}).then(function(res){
    		$scope.saveStatus = "Saved"
    	});
    }

    $http.get("http://handsfreeleveler.com:4446/api/getEmotes").then(function(res){
        $scope.emotes = res.data;
    });
	
});