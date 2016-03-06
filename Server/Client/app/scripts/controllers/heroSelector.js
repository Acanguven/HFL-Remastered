'use strict';
/**
 * @ngdoc function
 * @name sbAdminApp.controller:Items
 * @description
 * # MainCtrl
 * Controller of the sbAdminApp
 */
angular.module('sbAdminApp').controller('heroSelector', function($scope,$http,$stateParams) {
    $http.get("champions.json").then(function(data){
    	$scope.heroList = data.data;
    })
    $scope.navigate = $stateParams.navigate
});