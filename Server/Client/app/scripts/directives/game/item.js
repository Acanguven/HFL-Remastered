'use strict';

/**
 * @ngdoc directive
 * @name izzyposWebApp.directive:adminPosHeader
 * @description
 * # adminPosHeader
 */
angular.module('sbAdminApp')
	.directive('itemscope', function() {
    return {
        restrict: 'E',
        scope:{
        	item: "=",
        	buy: "="
        },
        template: '<img class="itembox" style="border:{{buy?\'2px solid green\':\'2px solid red\'}}" src="http://ddragon.leagueoflegends.com/cdn/6.7.1/img/item/{{item.id}}.png"/>'
    };
})

angular.module('sbAdminApp')
	.directive('iteminventory', function() {
    return {
        restrict: 'E',
        scope:{
        	item: "=",
            action: "=",
            slot: "="
        },
        template: '<img ng-click="action(slot)" class="itembox" src="{{item?\'http://ddragon.leagueoflegends.com/cdn/6.7.1/img/item/\'+item.id+\'.png\':\'/img/items/empty.png\'}}" name="{{item.name}}"/>'
    };
})

angular.module('sbAdminApp')
    .directive('itemlist', function() {
    return {
        restrict : 'E',
        template : '<a ng-repeat="(key,value) in list | filtermap:search.map"  ng-mouseover="hover(value)" ng-mouseleave="out(value)" ng-show="value.name.toLowerCase().indexOf(search.name.toLowerCase()) != -1"><img title="{{value.name}}" id="item_{{value.id}}" class="img-circle" src="http://ddragon.leagueoflegends.com/cdn/6.7.1/img/item/{{value.image.full}}" style="margin-bottom:10px;cursor:pointer;margin-right:11px;" height="50px" ng-click="action(value)"/></a>',
        scope: {
            list: "=",
            search: "=",
            action: "=",
            hover:"=",
            out:"=",
        },
        replace:true
    }
})


angular.module('sbAdminApp')
   .filter( 'filtermap', function() {
		return function( items ,mapS) {
			var map = parseInt(mapS);
			var tempitems = [];

            angular.forEach(items, function (item) {
                if(item.maps[map] == true){
                	tempitems.push(item);
                }
            });

       		return tempitems;
		}
});
