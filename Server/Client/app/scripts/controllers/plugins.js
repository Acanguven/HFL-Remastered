'use strict';
/**
 * @ngdoc function
 * @name sbAdminApp.controller:heroItems
 * @description
 * # MainCtrl
 * Controller of the sbAdminApp
 */
angular.module('sbAdminApp').controller('plugins', function($scope,$http,$stateParams) {
    if(!$scope.user){
		return false;
	}

    $scope.hero = $stateParams.heroName;
    $scope.staticversion = "?";
    $scope.copyStatus = "Copy clicking this button";
    $scope.importStatus = "Your plugin will be deleted";


	$http.get("http://handsfreeleveler.com:4446/api/getStaticVersions").then(function(res){
		$scope.staticversion = res.data[0];
		var heroUpper = $scope.hero.charAt(0).toUpperCase() + $scope.hero.slice(1)
		if(heroUpper == "Drmundo"){
			heroUpper = "DrMundo";
		}
		if(heroUpper == "Fiddlesticks"){
			heroUpper = "FiddleSticks";
		}
		if(heroUpper == "Wukong"){
			heroUpper = "MonkeyKing";
		}
		if(heroUpper == "Twistedfate"){
			heroUpper = "TwistedFate";
		}
		if(heroUpper == "Tahmkench"){
			heroUpper = "TahmKench";
		}
		if(heroUpper == "Missfortune"){
			heroUpper = "MissFortune";
		}
		if(heroUpper == "Masteryi"){
			heroUpper = "MasterYi";
		}
		if(heroUpper == "Kogmaw"){
			heroUpper = "KogMaw";
		}
		if(heroUpper == "Leesin"){
			heroUpper = "LeeSin";
		}
		if(heroUpper == "Jarvaniv"){
			heroUpper = "JarvanIV";
		}
		if(heroUpper == "Reksai"){
			heroUpper = "RekSai";
		}
		if(heroUpper == "Xinzhao"){
			heroUpper = "XinZhao";
		}
		
		$http.get("http://handsfreeleveler.com:4446/api/staticChampion/"+$scope.staticversion+"/"+heroUpper).then(function(res2){
			$scope.heroStatic = res2.data.data[heroUpper];
		});
    });

	$scope.exportedSettings = "";
    $scope.plugin = {
    	baron: true,
		buyPotions: true,
		contBuild: true,
		dragon: true,
		goldCap: 2000,
		lanePrompt: false,
		laneSelect: "auto",
		name: "Default",
		notes: "",
		support: false,
		teamDefend: true,
		teamPush: true,
		towerDive: false,
		version: 0.1,
		warding: true,
		combat:{
			0:{
				chance: 0,
				enabled: true,
				managt: 0,
				onlykill: "false",
				targeting: "target",
			},
			1:{
				chance: 0,
				enabled: true,
				managt: 0,
				onlykill: "false",
				targeting: "target",
			},
			2:{
				chance: 0,
				enabled: true,
				managt: 0,
				onlykill: "false",
				targeting: "target",
			},
			3:{
				chance: 0,
				enabled: true,
				managt: 0,
				onlykill: "false",
				targeting: "target",
			},
		},
		survive:{
			0:{
				chance: 0,
				enabled: true,
				managt: 0,
				onlykill: "false",
				targeting: "target",
			},
			1:{
				chance: 0,
				enabled: true,
				managt: 0,
				onlykill: "false",
				targeting: "target",
			},
			2:{
				chance: 0,
				enabled: true,
				managt: 0,
				onlykill: "false",
				targeting: "target",
			},
			3:{
				chance: 0,
				enabled: true,
				managt: 0,
				onlykill: "false",
				targeting: "target",
			},
		},
		farmpush:{
			0:{
				chance: 0,
				enabled: true,
				managt: 0,
				onlykill: "false",
				targeting: "target",
			},
			1:{
				chance: 0,
				enabled: true,
				managt: 0,
				onlykill: "false",
				targeting: "target",
			},
			2:{
				chance: 0,
				enabled: true,
				managt: 0,
				onlykill: "false",
				targeting: "target",
			},
			3:{
				chance: 0,
				enabled: true,
				managt: 0,
				onlykill: "false",
				targeting: "target",
			},
		},
		farmsafe:{
			0:{
				chance: 0,
				enabled: true,
				managt: 0,
				onlykill: "false",
				targeting: "target",
			},
			1:{
				chance: 0,
				enabled: true,
				managt: 0,
				onlykill: "false",
				targeting: "target",
			},
			2:{
				chance: 0,
				enabled: true,
				managt: 0,
				onlykill: "false",
				targeting: "target",
			},
			3:{
				chance: 0,
				enabled: true,
				managt: 0,
				onlykill: "false",
				targeting: "target",
			},
		},
		harass:{
			0:{
				chance: 0,
				enabled: true,
				managt: 0,
				onlykill: "false",
				targeting: "target",
			},
			1:{
				chance: 0,
				enabled: true,
				managt: 0,
				onlykill: "false",
				targeting: "target",
			},
			2:{
				chance: 0,
				enabled: true,
				managt: 0,
				onlykill: "false",
				targeting: "target",
			},
			3:{
				chance: 0,
				enabled: true,
				managt: 0,
				onlykill: "false",
				targeting: "target",
			},
		},
    }

    $scope.exportedSettings = JSON.stringify($scope.plugin);

    $http.get("http://handsfreeleveler.com:4446/api/getPlugin/"+$scope.hero).then(function(res){
        if(!res.data.emptyPlugin){
			$scope.plugin = angular.copy(res.data)
        	$scope.exportedSettings = JSON.stringify($scope.plugin);
        }
    });

    $scope.save = function(map){
        var pluginSave = angular.copy($scope.plugin)
    	$http.post("http://handsfreeleveler.com:4446/api/updatePlugin/"+$scope.hero,{plugin:pluginSave}).then(function(res){
    		$scope.saveStatus = "Saved"
    	});
    }

    $scope.$watch("plugin", function(){
    	$scope.exportedSettings = JSON.stringify($scope.plugin);
    },true);

	$scope.copied = function(){
		$scope.copyStatus = "Copied successfully!"	
	}
	$scope.copyfail = function(err){
		$scope.copyStatus = err;
	}

	$scope.import = function(json){
		$scope.plugin = JSON.parse(json);
	}
});