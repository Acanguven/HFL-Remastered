'use strict';

/**
 * @ngdoc directive
 * @name izzyposWebApp.directive:adminPosHeader
 * @description
 * # adminPosHeader
 */
angular.module('sbAdminApp')
	.directive('hero', function() {
    return {
        restrict: 'E',
        compile: function(element, attrs) {
            if(!attrs.name && attrs.name == ""){
                var htmlText = "";
            }else{
                if(attrs.minimap){
                    var htmlText = '<img class="img-circle" id="heroMarker" src="/img/champions/'+attrs.name.toLowerCase()+'.png" style="margin-bottom:10px;position:absolute;" height="25px;"/>';
                }else{
                    if(attrs.chat){
                        var htmlText = '<img class="img-circle" src="/img/champions/'+attrs.name.toLowerCase()+'.png" style="margin-bottom:10px;cursor:pointer;" height="50px"/>';
                    }else{
                        var htmlText = '<img class="img-circle" src="/img/champions/'+attrs.name.toLowerCase()+'.png" style="margin-bottom:10px;cursor:pointer;" height="100px"/>';
                    }
                    
                }
            }
            element.replaceWith(htmlText);
        }
    };
});

angular.module('sbAdminApp')
    .directive('chathero', function() {
    return {
        restrict : 'E',
        template : '<img class="img-circle" src="http://ddragon.leagueoflegends.com/cdn/6.5.1/img/champion/{{name}}.png" style="margin-bottom:10px;cursor:pointer;" height="50px"/>',
        scope: {
            name: "=",
        },
        replace:true
    }
});

angular.module('sbAdminApp')
    .directive('herolist', function() {
    return {
        restrict : 'E',
        template : '<a ng-repeat="hero in list | filter:search" href="#/dashboard/{{navigate}}/{{hero.toLowerCase()}}"><img class="img-circle" src="/img/champions/{{hero.toLowerCase()}}.png" style="margin-bottom:10px;cursor:pointer;margin-right:11px;" height="100px"/></a>',
        scope: {
            list: "=",
            search: "=",
            navigate: "="
        },
        replace:true
    }
})


angular.module('sbAdminApp')
    .directive('heroscope', function() {
    return {
        restrict : 'E',
        template : '<img class="img-circle" src="/img/champions/{{name.toLowerCase()}}.png" style="margin-bottom:10px;cursor:pointer;margin-right:11px;" height="100px"/>',
        scope: {
            name: "="
        },
        replace:true
    }
})