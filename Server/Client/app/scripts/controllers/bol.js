'use strict';
/**
 * @ngdoc function
 * @name sbAdminApp.controller:bol
 * @description
 * # MainCtrl
 * Controller of the sbAdminApp
 */
angular.module('sbAdminApp').controller('bol', function($rootScope,$scope, $http) {
    if(!$scope.user){
    	return false;
    }
    $scope.tempName = angular.copy($scope.user.userData.bol);
    console.log($scope.user)

    $scope.save = function(name){
        $scope.user.userData.bol = name;
        $http.post("http://handsfreeleveler.com:4446/api/updateBol",{bol:name});
    }
});