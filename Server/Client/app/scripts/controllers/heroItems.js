'use strict';
/**
 * @ngdoc function
 * @name sbAdminApp.controller:heroItems
 * @description
 * # MainCtrl
 * Controller of the sbAdminApp
 */
angular.module('sbAdminApp').controller('heroItems', function($scope,$http,$stateParams) {
    $scope.hero = $stateParams.heroName;

	$http.get("champions.json").then(function(data){
    	$scope.heroes = data.data;
    });

    $http.get("items.json").then(function(res){
    	$scope.itemList = [];
    	for (var key in res.data.data){
    		var obj = res.data.data[key];
    		obj.id = key;
    		$scope.itemList.push(obj);
    	}
    });

    $scope.heroBuild = {
    	11:{
    		queue:[],
    		endItems:{}
    	},
    	12:{
    		queue:[],
    		endItems:{}
    	}
    }

    $scope.addItem = function(item){
    	$scope.heroBuild[$scope.search.map].queue.push({item:item,buy:true});
    	$scope.calculateInventory();
    	$scope.search.name = "";
    }

    $scope.sellItem = function(slot){
    	$scope.heroBuild[$scope.search.map].queue.push({slot:slot,buy:false,item:$scope.heroBuild[$scope.search.map].endItems[slot]});
    	$scope.calculateInventory();
    	$scope.search.name = "";
    }

    $scope.calculateInventory = function(){
    	$scope.heroBuild[$scope.search.map].endItems = [];
    	var inventoryIndex = 0;
    	for(var x = 0; x < $scope.heroBuild[$scope.search.map].queue.length; x++){
    		var transaction = $scope.heroBuild[$scope.search.map].queue[x];
    		if(transaction.buy){
    			var synergy = $scope.checkSynergy(transaction.item);
    			if (synergy !== false){
    				for(var item in $scope.heroBuild[$scope.search.map].endItems){
    					if(synergy.indexOf($scope.heroBuild[$scope.search.map].endItems[item].id) > -1){
    						var synIndex = synergy.indexOf($scope.heroBuild[$scope.search.map].endItems[item].id);
    						delete $scope.heroBuild[$scope.search.map].endItems[item];
    						synergy.splice(synIndex,1)
    					}
    				}
    				inventoryIndex = $scope.calculateIndex();
    				$scope.heroBuild[$scope.search.map].endItems[inventoryIndex] = transaction.item;
    				inventoryIndex = $scope.calculateIndex();
    			}else{
	    			var inventoryLength = 0;
	    			for(var item in $scope.heroBuild[$scope.search.map].endItems){
	    				inventoryLength++;
	    			}
	    			if(inventoryLength == 6){
	    				$scope.heroBuild[$scope.search.map].queue.splice($scope.heroBuild[$scope.search.map].queue.length-1,1)
	    				return false;
	    			}else{
						$scope.heroBuild[$scope.search.map].endItems[inventoryIndex] = transaction.item;
						inventoryIndex = $scope.calculateIndex();
					}
    			}
    		}else{
    			delete $scope.heroBuild[$scope.search.map].endItems[transaction.slot];
    			inventoryIndex = $scope.calculateIndex();
    		}
    	}
    }

    $scope.checkSynergy = function(item){
    	if(!item.from || item.from.length == 0){
    		return false;
    	}else{
	    	var itemRequirements = angular.copy(item.from);
	    	var needToFind = itemRequirements.length;
	    	for(var itemz in $scope.heroBuild[$scope.search.map].endItems){
	    		var itemFound = itemRequirements.indexOf($scope.heroBuild[$scope.search.map].endItems[itemz].id);
	    		if(itemFound > -1){
	    			needToFind--;
	    		}
	    	}
	    	if (needToFind <= 0){
	    		return angular.copy(item.from);
	    	}else{
	    		return false;
	    	}
    	}
    }

    $scope.calculateIndex = function(){
    	var slot = -1;
    	for(var x = 0; x < 7 ; x++){
    		if(!$scope.heroBuild[$scope.search.map].endItems[x]){
    			slot = x;
    			break;
    		}
    	}
    	return slot;
    }

    $scope.clear = function(id){
    	$scope.heroBuild[id].endItems = {}
    	$scope.heroBuild[id].queue = [];
    }

    $scope.calcuteValue = function(){
    	var price = 0;
    	for(var item in $scope.heroBuild[$scope.search.map].endItems){
			price += $scope.heroBuild[$scope.search.map].endItems[item].gold.total;
		}
		return price;
    }

    $scope.save = function(map){
    	var heroJSON = JSON.stringify($scope.heroBuild[$scope.search.map]);
    	console.log(heroJSON);
    }
});