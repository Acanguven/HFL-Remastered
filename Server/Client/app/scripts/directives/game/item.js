'use strict';

/**
 * @ngdoc directive
 * @name izzyposWebApp.directive:adminPosHeader
 * @description
 * # adminPosHeader
 */
angular.module('sbAdminApp')
	.directive('item', function() {
    return {
        restrict: 'E',
        compile: function(element, attrs) {
            var htmlText = '<img class="itembox" src="/img/items/'+encodeURIComponent(attrs.name)+'.jpg"/>';
            element.replaceWith(htmlText);
        }
    };
})


