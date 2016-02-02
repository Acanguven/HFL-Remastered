'use strict';
/**
 * @ngdoc function
 * @name sbAdminApp.controller:smurfs
 * @description
 * # MainCtrl
 * Controller of the sbAdminApp
 */
angular.module('sbAdminApp').controller('smurfs', function($scope,$http,$rootScope, Websocket) {
	if(!$scope.user){
		return false;
	}

	$http.get("http://localhost:3000/api/getSmurfs").then(function(res){
		$scope.user.userData.smurfs = res.data.smurfs;
		$scope.user.userData.groups = res.data.groups;
		$scope.user.userData.smurfs.forEach(function(smurf){
			if (smurf.group > -1 && smurf.group != null){
				smurf.group = smurf.group.toString();
			}
		});
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
		$scope.user.userData.smurfs.forEach(function(smurf){
			if(smurf.group){
				if(smurf.group == index){
					smurf.group = null;
				}else{
					if(smurf.group > index){
						smurf.group--;
					}
				}
				if (smurf.group > -1 && smurf.group != null){
					smurf.group = smurf.group.toString();
				}
			}
		});
		$scope.user.userData.groups.splice(index,1);
	}

	$scope.removeSmurf = function(sr){
		var srindex = $scope.user.userData.smurfs.indexOf(sr);
		$scope.user.userData.smurfs.splice(srindex,1);
	}

	$scope.$watch(function(){
		return $scope.user.userData.smurfs;
	}, function(){
		$scope.user.userData.groups.forEach(function(group){
			group.smurfs = [];
		});
		$scope.user.userData.smurfs.forEach(function(smurf){
			if(smurf.group > -1 && smurf.group != null){
				$scope.user.userData.groups[smurf.group].smurfs.push(smurf);
			}
		});
	},true);


	$scope.$watch(function(){
		return $scope.user.userData
	}, function(){
		$scope.user.userData.smurfs.forEach(function(smurf){
			if (smurf.group > -1 && smurf.group != null){
				smurf.group = smurf.group.toString();
			}
		});
		$http.post("http://localhost:3000/api/updateSmurfs", {userData:$scope.user.userData}).then(function(res){
			//console.log(res.data);
		});
	},true);

	$scope.showSmurfAction = function(smurf,type){
		if($rootScope.controller){
			if(type == "play"){
				if($rootScope.liveData.smurfs[smurf.username]){
					return !$rootScope.liveData.smurfs[smurf.username].isRunning;
				}else{
					return true;
				}
			}else if(type == "stop"){
				if($rootScope.liveData.smurfs[smurf.username]){
					return $rootScope.liveData.smurfs[smurf.username].isRunning;
				}else{
					return false;
				}
			}
		}else{
			return false;
		}
	}

	$scope.disableEditorLiveSmurf = function(smurf){
		if ($rootScope.liveData.smurfs[smurf.username]){
			return $rootScope.liveData.smurfs[smurf.username].isRunning;
		}else{
			return false;
		}
	}

	$scope.stopSmurf = function(smurf){
		Websocket.send({type:"smurf",smurf:smurf,action:"stop"});
	}

	$scope.startSmurf = function(smurf){
		Websocket.send({type:"smurf",smurf:smurf,action:"start"});
	}

	$scope.stopGroup = function(group){
		Websocket.send({type:"group",group:group,action:"stop"});
	}

	$scope.startGroup = function(group){
		Websocket.send({type:"group",group:group,action:"stop"});
	}

	$scope.refresher();
});