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

    $scope.$watch("search.map", function(){
        $scope.calculateInventory();
    })

    $http.get("items.json").then(function(res){
    	$scope.itemList = [];
    	for (var key in res.data.data){
            if(res.data.data[key].name.indexOf("Potion") == -1){
                console.log(res.data.data[key].name)
                delete res.data.data[key].description;
                delete res.data.data[key].colloq;
                delete res.data.data[key].plaintext;
                delete res.data.data[key].tags;
                delete res.data.data[key].stats;
                var obj = res.data.data[key];
                obj.id = key;
                $scope.itemList.push(obj);
            }
    	}

        $http.get("http://handsfreeleveler.com:4446/api/getItems/"+$scope.hero).then(function(res){
            $scope.matchUpdatedQueue(angular.copy(res.data))
        });
    });

    $scope.matchUpdatedQueue = function(queue){
        for(var mapNum in queue){
            queue[mapNum].forEach(function(transaction){
                $scope.itemList.forEach(function(sItem){
                    if (transaction.item == sItem.name){
                        transaction.item = sItem;
                    }
                });
            });
            $scope.heroBuild[mapNum].queue = queue[mapNum];
        }
        $scope.calculateInventory();
    }

    $scope.popLast = function(){
        $scope.heroBuild[$scope.search.map].queue.pop();
        $scope.calculateInventory();
    }

    $scope.getItemById = function(id){
        var itemFound = false;
        $scope.itemList.forEach(function(item){
            if (id == item.id){
                itemFound = item;
            }
        })
        return itemFound;
    }

    $scope.out = function(item){
        $(".itemhover").remove();
    }

    $scope.hover = function(item){
        if(item.from){
            if($("#item_"+item.id).parent().children(".itemhover").length == 0){
                $("#item_"+item.id).parent().append("<div class='itemhover'></div>")
                item.from.forEach(function(item){
                    var itemObject = $scope.getItemById(item);
                    var $itemNeeded = $('#item_'+item).parent().clone();

                    $($itemNeeded).children().attr("id",'#itemclone_'+item);
                    if(itemObject.from){
                        var neededItems = itemObject.from;
                        $($itemNeeded).mouseover(function(){
                            if($(this).children(".hovernext").length == 0){
                                if($(".hovernext").length > 0){
                                    $(".hovernext").remove();
                                }
                                $(this).append("<div class='hovernext'></div>")
                                neededItems.forEach(function(item2){
                                    var $n2 = $('#item_'+item2).parent().clone();
                                    $($n2).children().attr("id",'#itemclone_'+item2);
                                    $($n2).click(function(){
                                        $('#item_'+item2).click();
                                        return false;
                                    });
                                    $('.hovernext').append($n2)
                                });
                                $('.hovernext a').removeClass("ng-hide")
                            }
                        });

                        $($itemNeeded).parent().mouseout(function(){
                            $(this).children().children(".hovernext").remove();
                        });
                    }
                    $($itemNeeded).click(function(){
                        $('#item_'+item).click();
                    });
                    $('.itemhover').append($itemNeeded);
                })
                $('.itemhover a').removeClass("ng-hide")
            }
        }
    }

    $scope.addItem = function(item){
    	$scope.heroBuild[$scope.search.map].queue.push({item:item,buy:true});
    	$scope.calculateInventory();
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
	    			needToFind = 0;
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
        var heroSave = angular.copy($scope.heroBuild[$scope.search.map]).queue
        heroSave.forEach(function(transaction){
            transaction.item = transaction.item.name;
        });
    	$http.post("http://handsfreeleveler.com:4446/api/updateItems",{
            build:heroSave,
            map:map,
            hero:$scope.hero
        });
    }

    $scope.copyFrom = function(from,to){
        var heroSave = angular.copy($scope.heroBuild[from]).queue
        $scope.heroBuild[to].queue = heroSave
    }
});