'use strict';
/**
 * @ngdoc function
 * @name sbAdminApp.controller:Items
 * @description
 * # MainCtrl
 * Controller of the sbAdminApp
 */
angular.module('sbAdminApp').controller('Items', function($scope,$http) {
    $http.get("champions.json").then(function(data){
    	$scope.heroList = data.data;
    })
});