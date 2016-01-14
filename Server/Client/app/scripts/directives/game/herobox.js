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
            if(attrs.minimap){
                var htmlText = '<img class="img-circle" src="/img/champions/'+attrs.name+'.png" style="margin-bottom:10px;position:absolute;" height="25px;"/>';
            }else{
                if(attrs.chat){
                    var htmlText = '<img class="img-circle" src="/img/champions/'+attrs.name+'.png" style="margin-bottom:10px;cursor:pointer;" height="50px"/>';
                }else{
                    var htmlText = '<img class="img-circle" src="/img/champions/'+attrs.name+'.png" style="margin-bottom:10px;cursor:pointer;" height="100px"/>';
                }
                
            }
            element.replaceWith(htmlText);
        }
    };
})


