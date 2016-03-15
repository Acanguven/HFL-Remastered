angular.module('sbAdminApp').factory('Websocket', function ($websocket, $rootScope) {
  var ws = $websocket("ws://handsfreeleveler.com:4447/");
  var reconnecting = false;
  injectStuff();

  function injectStuff(){
	  ws.onMessage(function (event) {
	  	//console.log('Recieved Message', event);
	    var response;
	    try {
	        response = angular.fromJson(event.data);
	        registery.forEach(function(fn){
	        	if(fn.type === response.type){
	        		fn.cb(response);
	        	}
	        });
	    } catch (e) {

	    }
	  });
	  ws.onError(function (event) {
	    console.log('connection Error', event);
	  });
	  ws.onClose(function (event) {
	    console.log('connection closed', event);
	    if(!reconnecting){
		    reconnecting = true;
		    reconnector = setInterval(function(){
		    	ws.onErrorCallbacks = [];
		    	ws.onOpenCallbacks = [];
		    	ws.onCloseCallbacks = [];
		      ws = $websocket("ws://handsfreeleveler.com:4447/");
		      injectStuff();
		    },500)
	    }
	  });
	  ws.onOpen(function (event) {
	  	clearInterval(reconnector);
	  	reconnecting = false;
	    console.log('connection open', event);
	    if($rootScope.token){
	    	ws.send({type:"login",token:$rootScope.token});
	    }
	  });
  }

  var reconnector;
  var registery = [];

  return {
    ws:ws,
    status: function () {
      return ws.readyState;
    },
    send: function (message) {
      if (angular.isString(message)) {
        ws.send(message);
      }
      else if (angular.isObject(message)) {
        ws.send(JSON.stringify(message));
      }
    },
    on: function(type,cb,overWrite){
    	if(overWrite === true){
    		var regFound = false;
	    	registery.forEach(function(reg){
	    		if(reg && reg.type === type){
	    			regFound = true;
	    			reg = {type:type,cb:cb}; 
	    			console.debug("OverWrited Callback", type);
	    		}
	    	})
	    	if(!regFound){
    			registery.push({type:type,cb:cb});
  				console.debug("Registered callback", type);
	    	}
    	}else{
    		registery.push({type:type,cb:cb});
  			console.debug("Registered callback", type);
    	}
    }
  };
})
.run(function(Websocket,$rootScope,$interval){
	$rootScope.liveData = {
		groups:[],
		smurfs:[],
		remote:[]
	}

	$interval(function(){
		$rootScope.liveData.remote.forEach(function(script, index){
			if(script.lastPing + 7500 < Date.now()){
				$rootScope.liveData.remote.splice(index,1);
			}
		})
	},2000)

	Websocket.on('status', function (data) {
		if(data.controller){
			$rootScope.controller = {
				ip:data.controller.ip
			};
		}else{
			$rootScope.controller = false;
		}
  	});
  	Websocket.on('liveData', function (data) {
		$rootScope.liveData.smurfs = [];

		data.update.smurfs.forEach(function(smrf){
			$rootScope.liveData.smurfs[smrf.username] = smrf;
		})

		$rootScope.liveData.groups = [];
		data.update.groups.forEach(function(grp){
			$rootScope.liveData.groups[grp.name] = grp;
		})
  	});
  	Websocket.on('scriptPing' , function(data){
  		var found = false;
        for(var x = 0; x < $rootScope.liveData.remote.length; x++){
            if($rootScope.liveData.remote[x].gameid == data.info.gameid){
            	if (!$rootScope.liveData.remote[x].chat){
            		$rootScope.liveData.remote[x].chat = [];
            	}
                $rootScope.liveData.remote[x].data = data.info.data;
                $rootScope.liveData.remote[x].lastPing = Date.now();
                found = true;
            }
        }
        if(!found){
        	data.lastPing = Date.now();
        	$rootScope.liveData.remote.push(data.info);
        }
  	});
  	Websocket.on('removeScript' , function(data){
  		var found = false;
        for(var x = 0; x < $rootScope.liveData.remote.length; x++){
            if($rootScope.liveData.remote[x].gameid == data.gameid){
                found = x;
            }
        }
        if(found !== false){
        	$rootScope.liveData.remote.splice(x,1);
        }
  	});
  	Websocket.on('chatUpdate', function(data){
  		var found = false;
        for(var x = 0; x < $rootScope.liveData.remote.length; x++){
            if($rootScope.liveData.remote[x].gameid == data.gameid){
                found = x;
            }
        }
        if (found !== false){
        	$rootScope.liveData.remote[found].chat = data.chats;
        }
  	});
})
    