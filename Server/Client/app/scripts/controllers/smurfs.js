'use strict';
/**
 * @ngdoc function
 * @name sbAdminApp.controller:smurfs
 * @description
 * # MainCtrl
 * Controller of the sbAdminApp
 */
angular.module('sbAdminApp').controller('smurfs', function($scope,$http) {
	$http.get("http://localhost:3000/api/getSmurfs").then(function(res){
		$scope.user.userData.smurfs = res.data;
	});

	$scope.defaultOptions = [
	    { name: '1', value: "1" }, 
	    { name: 'Summoners Rift', value: 2 }, 
	    { name: 'EUW', value: 'EUW' },
    ];

    $scope.levelOptions = [];
    for(var x = 1; x < 32; x++){
    	if(x == 31){
    		$scope.levelOptions.push({value:x,name:"Unlimited"});
    	}else{
    		$scope.levelOptions.push({value:x,name:x});
    	}
    }

    //$scope.levelOptions = [];


	$scope.refresher = function(){
		$scope.newSmurf = {
			desiredLevel : $scope.defaultOptions[0].value,
			queue : $scope.defaultOptions[1].value,
			region : $scope.defaultOptions[2].value,
		};
	}

	$scope.add = function(smurf){
		var newSmurf = angular.copy(smurf);
		$scope.user.userData.smurfs.push(newSmurf);
		$scope.refresher();
	}

	$scope.addGroup = function(){
		$scope.user.userData.groups.push({name:"New Group",smurfs:[],queue:31})
	}

	$scope.removeGroup = function(gr){
		var index = $scope.user.userData.groups.indexOf(gr);
		$scope.user.userData.groups.splice(index,1);
	}

	$scope.$watch(function(){
		return $scope.user.userData.smurfs;
	}, function(){
		$scope.user.userData.groups.forEach(function(group){
			group.smurfs = [];
		});
		$scope.user.userData.smurfs.forEach(function(smurf){
			if(smurf.group){
				$scope.user.userData.groups[smurf.group].smurfs.push(smurf);
			}
		});
	},true);


	$scope.$watch(function(){
		return $scope.user.userData
	})
	$scope.refresher();
});