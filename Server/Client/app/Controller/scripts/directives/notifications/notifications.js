'use strict';

/**
 * @ngdoc directive
 * @name izzyposWebApp.directive:adminPosHeader
 * @description
 * # adminPosHeader
 */
angular.module('controller')
	.directive('controller',function(){
		return {
        templateUrl:'scripts/directives/notifications/notifications.html',
        restrict: 'E',
        replace: true,
    	}
	});


