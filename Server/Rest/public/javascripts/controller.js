var app = angular.module("panel", []);

app.controller("main",function($scope, $http){
	var panelKey = getCookie("pass");
	if(!panelKey | panelKey.length < 1){
		panelKey = prompt("Give me something");
	}
	createCookie("pass",panelKey,10);

	$scope.userList = [];

	$scope.update = function(){
		$http.post("http://handsfreeleveler.com:4446/api/admin/getAccounts", {lawPass:panelKey}).then(function(msg){
			$scope.userList = msg.data;
		});
	}

	$scope.update();

	$scope.trialRemains = function(number){
		var remaining = number - Date.now();
		return Math.round(remaining / 60000);
	}
	$scope.act = function(user,action){
		var amount = 1000*60*60*30;
		if(action === 3){
			amount = prompt("Minutes?",1000*60*60*30);
		}
		$http.post("http://handsfreeleveler.com:4446/api/admin/act", {lawPass:panelKey,act:action,id:user._id,amount:amount}).then(function(msg){
			$scope.update();
		});
	}
});


function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for(var i=0; i<ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0)==' ') c = c.substring(1);
        if (c.indexOf(name) == 0) return c.substring(name.length,c.length);
    }
    return "";
}

function createCookie(name,value,days) {
    if (days) {
        var date = new Date();
        date.setTime(date.getTime()+(days*24*60*60*1000));
        var expires = "; expires="+date.toGMTString();
    }
    else var expires = "";
    document.cookie = name+"="+value+expires+"; path=/";
}