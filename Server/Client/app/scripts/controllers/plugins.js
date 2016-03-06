'use strict';
/**
 * @ngdoc function
 * @name sbAdminApp.controller:heroItems
 * @description
 * # MainCtrl
 * Controller of the sbAdminApp
 */
angular.module('sbAdminApp').controller('plugins', function($scope,$http,$stateParams) {
    $scope.hero = $stateParams.heroName;

	$http.get("champions.json").then(function(data){
    	$scope.heroes = data.data;
    });

    alert("Engines running")
});