'use strict';
/**
 * @ngdoc function
 * @name sbAdminApp.controller:remote
 * @description
 * # MainCtrl
 * Controller of the sbAdminApp
 */
angular.module('sbAdminApp').controller('remote', function($rootScope, $scope, $http, Websocket) {
    if(!$scope.user){
    	return false;
    }
    $scope.sendChatText = "";

    $scope.selectedRemote = false;

    $scope.selectR = function(id){
    	var found = false;
    	for(var x = 0; x < $rootScope.liveData.remote.length; x++){
            if($rootScope.liveData.remote[x].gameid == id){
                found = x;
            }
        }
        if(found !== false){
        	$scope.selectedRemote = $rootScope.liveData.remote[found];
        }
    }

    $scope.secondsConverter = function(time){
		var minutes = Math.floor(time / 60);
		time -= minutes * 60;

		var seconds = parseInt(time % 60, 10);

		return (minutes < 10 ? '0' + minutes : minutes) + ':' + (seconds < 10 ? '0' + seconds : seconds);
    }

    $scope.floor = function(val){
    	return ~~val;
    }

    $scope.setMap = function(){
    	var map = $scope.selectedRemote.data[9];
    	if(map == "summonerRift"){
            var mapX = $("#currentmap").width()*$scope.selectedRemote.data[7]/14716;
            var mapZ = $("#currentmap").height()*$scope.selectedRemote.data[8]/14824;
            $("#heroMarker").css({top:($("#currentmap").height()-mapZ)-$("#heroMarker").height()/2, left: mapX-$("#heroMarker").width()/2});
        }else if(map == "howlingAbyss"){
 			var mapX = $("#currentmap").width()*$scope.selectedRemote.data[7]/12839;
            var mapZ = $("#currentmap").height()*$scope.selectedRemote.data[8]/12820;
            $("#heroMarker").css({top:($("#currentmap").height()-mapZ)-$("#heroMarker").height()/2, left: mapX-$("#heroMarker").width()/2});
        }
    }

    $scope.$watch("selectedRemote", function(){
    	if($scope.selectedRemote){
    		$scope.setMap();

            setTimeout(function(){
                var objDiv = document.getElementById("chatscroller");
                objDiv.scrollTop = objDiv.scrollHeight;
            },100);
    	}
    },true)

    
    $scope.sendText = function(text){
        Websocket.send({type:"submitChat",gameid:$scope.selectedRemote.gameid,text:text});
        $scope.sendChatText = "";
        text = "";
        document.getElementById("inputforchat").value = "";
    }
});