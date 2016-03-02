var express = require('express');
var router = express.Router();
var mongoose = require('mongoose');
var jwt = require('jsonwebtoken');
var querystring = require('querystring');
var request = require('request');
var fs = require("fs");
var js2lua = require('js2lua');
var net = require('net');
var Table = require('cli-table2');
var VERSION = "1.2"
mongoose.connect("mongodb://127.0.0.1:27017/hflRest");
var db = mongoose.connection;
var fs = require("fs");
var Schema = mongoose.Schema;
var TOKEN_KEY = "Oh!my_ohMibod.?;";
db.on('error', console.error.bind(console, 'connection error:'));

db.once('open', function(callback) {
    console.log("connection to db open")
    //db.db.dropDatabase(); //Db refresher
});

var User = require("../models/user.js");

router.get('/ts', function(req, res, next) {
    res.json({
        ts: Date.now()
    });
});

router.get('/ping', verifyTokenDetectUser, function(req, res, next) {
    res.end("pong");
});

router.post("/remotelogin", function(req, res, next) {
    loginForumBridge(req.body.username, req.body.password, function(login) {
        var resultResponse = {
            forumData: {},
            userData: {},
            token: ""
        };
        if (login.uid) {
            resultResponse.forumData = login;
            User.findOne({
                uid: login.uid
            }, {
                logs: 0,
                ai:0
            }, function(err, user) {
                if (err) {
                    res.end("err");
                } else {
                    if (user) {
                        if (req.body.hwid) {
                            if (user.type === 0) {
                                user.testHwid(req.body.hwid, function(r) {
                                    if (r) {
                                        resultResponse.userData = user;
                                        resultResponse.date = Date.now();
                                        resultResponse.token = jwt.sign({
                                            joindate: login.joindate,
                                            uid: login.uid,
                                            removeTime: Date.now() + 1000 * 60 * 30
                                        }, TOKEN_KEY);
                                        res.json(resultResponse);
                                    } else {
                                        res.json({
                                            err: "You have to wait some more to reset your hwid."
                                        });
                                    }
                                });
                            } else {
                                user.testHwid(req.body.hwid, function(r) {
                                    if (r) {
                                        resultResponse.userData = user;
                                        resultResponse.date = Date.now();
                                        resultResponse.token = jwt.sign({
                                            joindate: login.joindate,
                                            uid: login.uid,
                                            removeTime: Date.now() + 1000 * 60 * 30
                                        }, TOKEN_KEY);
                                        res.json(resultResponse);
                                    } else {
                                        res.json({
                                            err: "You have to wait some more to reset your hwid."
                                        });
                                    }
                                });
                            }
                        } else {
                            resultResponse.userData = user;
                            resultResponse.date = Date.now();
                            resultResponse.token = jwt.sign({
                                joindate: login.joindate,
                                uid: login.uid,
                                removeTime: Date.now() + 1000 * 60 * 30
                            }, TOKEN_KEY);
                            res.json(resultResponse);
                        }
                    } else {
                        var newUser = new User({
                            uid: login.uid,
                        });
                        newUser.save(function(err, newUserAppended) {
                            resultResponse.userData = newUserAppended;
                            resultResponse.date = Date.now();
                            resultResponse.token = jwt.sign({
                                joindate: login.joindate,
                                uid: login.uid,
                                removeTime: Date.now() + 1000 * 60 * 30
                            }, TOKEN_KEY);

                            if (req.body.hwid) {
                                User.findOne({
                                    hwid: req.body.hwid
                                }, function(err, hwidDup) {
                                    if (err) {
                                        //console.log(err);
                                    } else {
                                        if (user) {
                                            res.json({
                                                err: "This hwid is belongs to another account."
                                            });
                                        } else {
                                            newUserAppended.testHwid(req.body.hwid, function(r) {
                                                if (r) {
                                                    res.json(resultResponse);
                                                } else {
                                                    res.json({
                                                        err: "You have to wait some more to reset your hwid."
                                                    });
                                                }
                                            });
                                        }
                                    }
                                });
                            } else {
                                res.json(resultResponse);
                            }
                        });
                    }
                }
            })
        } else {
            res.json(login)
        }
    });
});

router.get("/getLogs", verifyTokenDetectUser, function(req, res, next) {
    res.json(req.user);
});

router.get("/getSmurfs", verifyTokenDetectUser, function(req, res, next) {
    res.json(req.user);
});

router.post("/updateSettings", verifyTokenDetectUser, function(req, res, next) {
    req.user.settings = req.body.settings;
    req.user.save();
    res.end();
});

router.post("/importAI", verifyTokenDetectUser, function(req,res,next){
    if(req.body.ai){
        req.user.ai = JSON.parse(req.body.ai);
        req.user.markModified("ai");
        req.user.save(function(){
            res.end("Done");
        });
    }
});

router.post("/defaultAI", verifyTokenDetectUser, function(req,res,next){
    fs.readFile('./defaultbuild.json', 'utf8', function (err, data) {
        if (err) throw err;
        var obj = JSON.parse(data);
        req.user.ai = obj;
        req.user.markModified("ai");
        req.user.save(function(){
            res.end("Done");
        })
    });
});

router.get("/getAI", verifyTokenDetectUser, function(req,res,next){
    res.json(req.user.ai)
});

router.get("/getItems/:hero", verifyTokenDetectUser, function(req,res,next){
    if(!req.user.ai.items){
        req.user.ai.items = {};
    }
    if(!req.user.ai.items[req.params.hero]){
        req.user.ai.items[req.params.hero] = {
            11:[],
            12:[]
        }
    }
    res.json(req.user.ai.items[req.params.hero])
});

router.post("/updateItems", verifyTokenDetectUser, function(req,res,next){
    if(!req.user.ai.items){
        req.user.ai.items = {};
    }
    if(!req.user.ai.items[req.body.hero]){
        req.user.ai.items[req.body.hero] = {
            11:[],
            12:[]
        }
    }
    if(!req.user.ai.items[req.body.hero][req.body.map]){
        req.user.ai.items[req.body.hero][req.body.map] = [];
    }
    req.user.ai.items[req.body.hero][req.body.map] = req.body.build;

    req.user.markModified("ai");
    req.user.save();
    res.end();
});

router.post("/updateBol", verifyTokenDetectUser, function(req, res, next) {
    req.user.bol = req.body.bol;
    req.user.markModified("bol")
    console.log(req.user.bol)
    req.user.save();
    res.end();
});

router.post("/updateSmurfs", verifyTokenDetectUser, function(req, res, next) {
    req.user.smurfs = req.body.userData.smurfs;
    req.user.groups = req.body.userData.groups;
    req.user.markModified('smurfs');
    req.user.markModified('groups');
    req.user.save(function(err, updated) {
        //console.log(updated);
        res.json(updated);
    });
});

router.get("/getStaticVersions", function(req,res,next){
    request("http://ddragon.leagueoflegends.com/api/versions.json", function(err, resp, body){
        res.end(body);
    });
});

router.get("/staticItemsVersion/:v", function(req,res,next){
    request("http://ddragon.leagueoflegends.com/cdn/"+req.params.v+"/data/en_US/item.json", function(err, resp, body){
        res.end(js2lua.convert(JSON.parse(body)));
    });
});

router.get("/staticMasteryVersion/:v", function(req,res,next){
    request("http://ddragon.leagueoflegends.com/cdn/"+req.params.v+"/data/en_US/mastery.json", function(err, resp, body){
        res.end(js2lua.convert(JSON.parse(body)));
    });
});

router.get("/staticRuneVersion/:v", function(req,res,next){
    request("http://ddragon.leagueoflegends.com/cdn/"+req.params.v+"/data/en_US/rune.json", function(err, resp, body){
        res.end(js2lua.convert(JSON.parse(body)));
    });
});

router.get("/staticSummonerVersion/:v", function(req,res,next){
    request("http://ddragon.leagueoflegends.com/cdn/"+req.params.v+"/data/en_US/summoner.json", function(err, resp, body){
        res.end(js2lua.convert(JSON.parse(body)));
    });
});

router.get("/staticChampionVersion/:v", function(req,res,next){
    request("http://ddragon.leagueoflegends.com/cdn/"+req.params.v+"/data/en_US/champion.json", function(err, resp, body){
        res.end(js2lua.convert(JSON.parse(body)));
    });
});

function loginForumBridge(username, password, callback) {

    var headers = {
        'Content-Type': 'application/x-www-form-urlencoded'
    }

    var options = {
        url: 'http://127.0.0.1:4444/api/ns/login',
        method: 'POST',
        headers: headers,
        form: {
            'username': username,
            'password': password
        }
    }


    request(options, function(error, response, body) {
        if (!error) {
            callback(JSON.parse(body))
        }else{
            console.log(body,error)
        }
    })
}

function verifyTokenDetectUser(req, res, next) {
    jwt.verify(req.headers.authorization, TOKEN_KEY, function(err, decoded) {
        if (err) {
            res.json({
                type: "dead",
                message: "Token error"
            });
            return false;
        }
        if (!decoded) {
            res.json({
                type: "dead",
                message: "Token decoding error"
            });
            return false;
        }
        if (decoded.removeTime < Date.now()) {
            res.json({
                type: "dead",
                message: "Token expire error"
            });
            return false;
        } else {
            User.findOne({
                uid: decoded.uid
            }, function(err, user) {
                if (err || !user) {
                    res.json({
                        type: "dead",
                        message: "Token user does not exist"
                    });
                    return false;
                } else {
                    req.user = user;
                    decoded.removeTime = Date.now() + 1000 * 60 * 20;
                    var newToken = jwt.sign(decoded, TOKEN_KEY);
                    res.header("Authorization", newToken);
                    next();
                }
            });
        }
    });
}

function verifyTokenDetectUserSocket(token, cb) {
    jwt.verify(token, TOKEN_KEY, function(err, decoded) {
        if (err) {
            cb(false);
        }
        if (!decoded) {
            cb(false);
        }
        if (decoded.removeTime < Date.now()) {
            cb(false);
        } else {
            User.findOne({
                uid: decoded.uid
            }, function(err, user) {
                if (err || !user) {
                    cb(false);
                } else {
                    cb(user);
                }
            });
        }
    });
}

function SocketMap() {
    this.groupList = [];

    this.addSocket = function(ws, type, user) {
        if (!this.groupList[user.uid]) {
            this.groupList[user.uid] = new Group(user);
        } else {
            this.groupList[user.uid].user = user;
        }
        if (type == "controller") {
            this.groupList[user.uid].addController(ws);
        } else if (type == "remote") {
            this.groupList[user.uid].addRemote(ws);
        }
    }

    this.removeSocket = function(ws, type, user) {
        if (this.groupList[user.uid]) {
            if (type == "controller") {
                this.groupList[user.uid].removeController(ws);
            } else if (type == "remote") {
                this.groupList[user.uid].removeRemote(ws);
            }
            if (this.groupList[user.uid].getRemoteCount() + this.groupList[user.uid].getControllerCount() === 0) {
                delete this.groupList[user.uid];
            }
        }
    }

    this.cmdLog = function(ws, data) {
        if (this.groupList[ws.user.uid]) {
            this.groupList[ws.user.uid].cmdOutput(data);
        }
    }

    this.cmdWrite = function(ws, data) {
        if (this.groupList[ws.user.uid]) {
            this.groupList[ws.user.uid].cmdWrite(data);
        }
    }

    this.updateSettings = function(ws, settings) {
        if (this.groupList[ws.user.uid]) {
            this.groupList[ws.user.uid].updateSettings(settings);
        }
    }

    this.pushLog = function(ws, log) {
        if (this.groupList[ws.user.uid]) {
            this.groupList[ws.user.uid].pushLog(log);
        }
    }

    this.event = function(ws, event) {
        if (this.groupList[ws.user.uid]) {
            this.groupList[ws.user.uid].event(event);
        }
    }

    this.updateSmurfs = function(ws, update) {
        if (this.groupList[ws.user.uid]) {
            this.groupList[ws.user.uid].updateSmurfs(update);
        }
    }

    //Script Module
    this.addScript = function(uid, socket, gameid, champion){
        if (this.groupList[uid]) {
            this.groupList[uid].addScript(socket,gameid,champion)
        }
    }

    this.scriptPing = function(uid,data){
        if (this.groupList[uid]) {
            this.groupList[uid].scriptPing(data)
        }
    }

    this.removeScript = function(socket){
        if(socket.uid && socket.gameid){
            if (this.groupList[socket.uid]) {
                this.groupList[socket.uid].removeScript(socket.gameid)
            }
        }
    }
}

function Group(user) {
    this.user = user;
    this.controller = [];
    this.remote = [];
    this.scripts = [];

    this.getMembers = function() {
        var members = [];
        this.remote.forEach(function(rt) {
            members.push(rt);
        });
        this.controller.forEach(function(ct) {
            members.push(ct);
        });
        return members;
    }

    this.getRemoteCount = function() {
        return this.remote.length;
    }

    this.getControllerCount = function() {
        return this.controller.length;
    }

    this.getScriptCount = function() {
        return this.scripts.length;
    }

    this.removeRemote = function(ws) {
        var found = false;
        this.remote.forEach(function(rmt) {
            if (rmt.socket == ws) {
                found = rmt;
            }
        });
        if (found) {
            var index = this.remote.indexOf(found);
            this.remote.splice(index, 1);
            //console.log("User " + this.user.uid + " left as Remote");
            this.updateStatus();
        }
    }

    this.removeController = function(ws) {
        if (this.controller[0].socket == ws) {
            //console.log("User " + this.user.uid + " left as Controller");
            this.controller = [];
            this.updateStatus();
        }
    }

    this.addRemote = function(ws) {
        var found = false;
        this.remote.forEach(function(rmt) {
            if (rmt.socket == ws) {
                found = true;
            }
        });
        if (!found) {
            //console.log("User " + this.user.uid + " logged in as Remote");
            this.remote.push({
                ip: ws.upgradeReq.connection.remoteAddress,
                socket: ws
            })
            this.updateStatus();
        }
    }

    this.addController = function(ws) {
        //console.log("User " + this.user.uid + " logged in as Controller");
        this.controller[0] = {
            ip: ws.upgradeReq.connection.remoteAddress,
            socket: ws
        };
        this.updateStatus();
    }

    this.updateStatus = function() {
        var status = {
            remoteLength: this.remote.length,
            controller: false,
            type: "status"
        }
        if (this.controller.length > 0) {
            status.controller = {
                ip: this.controller[0].ip
            }
        }
        this.getMembers().forEach(function(member) {
            member.socket.send(JSON.stringify(status));
        });
    }

    this.cmdOutput = function(text) {
        this.remote.forEach(function(rmt) {
            if (rmt) {
                rmt.socket.send(JSON.stringify({
                    type: "cmdLog",
                    text: text
                }));
            }
        });
    }

    this.cmdWrite = function(text) {
        if (this.controller[0]) {
            this.controller[0].socket.send(JSON.stringify({
                type: "cmdWrite",
                text: text
            }));
        }
    }

    this.updateSettings = function(settings) {
        if (this.controller[0]) {
            this.controller[0].socket.send(JSON.stringify({
                type: "updateSettings",
                settings: settings
            }));
        }
    }

    this.pushLog = function(log) {
        this.remote.forEach(function(rmt) {
            if (rmt) {
                rmt.socket.send(JSON.stringify({
                    type: "log",
                    log: log
                }));
            }
        });
    }

    this.event = function(event) {
        if (this.controller[0]) {
            this.controller[0].socket.send(JSON.stringify(event));
        }
    }

    this.updateSmurfs = function(update) {
        this.remote.forEach(function(rmt) {
            if (rmt) {
                rmt.socket.send(JSON.stringify({
                    type: "liveData",
                    update: update
                }));
            }
        });
    }

    this.addScript = function(socket,gameid,champion){
        var found = false;
        this.scripts.forEach(function (script){
            if (script.gameid == gameid){
                script.socket = socket;
                found = true;
            }
        })
        if(!found){
            var script = {
                socket: socket,
                gameid: gameid,
                champion: champion
            }
            this.scripts.push(script);
        }
    }

    this.scriptPing = function(data){
        this.remote.forEach(function(rmt) {
            if (rmt) {
                rmt.socket.send(JSON.stringify({
                    type: "scriptPing",
                    info: data
                }));
            }
        });
    }

    this.removeScript = function(gameid){
        var found = false;
        for(var x = 0; x < this.scripts.length; x++){
            if(this.scripts[x].gameid == gameid){
                found = x;
            }
        }
        if(found !== false){
            this.scripts.splice(found,1);
            this.remote.forEach(function(rmt) {
                rmt.socket.send(JSON.stringify({
                    type: "removeScript",
                    gameid: gameid
                }));
            });
        }
    }
}

var cliTable = new Table({
    head: ['#', 'User Id', 'Controller', 'Script', 'Remote', 'Account Type', 'Trial'],
    colWidths: [3, 35, 15, 15, 15, 20, 20]
});

var updateTable = function() {
    console.log('\033c');
    var x = 1;
    cliTable.splice(0, cliTable.length)
    for (var gr in socketMap.groupList) {
        if (gr) {
            cliTable.push([x, socketMap.groupList[gr].user.id, socketMap.groupList[gr].getControllerCount(), socketMap.groupList[gr].getScriptCount(), socketMap.groupList[gr].getRemoteCount(), socketMap.groupList[gr].user.type, socketMap.groupList[gr].user.trial]);
            x++;
        }
    }
    console.log(cliTable.toString());
}

function handleMessage(data, ws) {
    switch (data.type) {
        case 'login':
            if (data.token) {
                verifyTokenDetectUserSocket(data.token, function(user) {
                    if (user) {
                        if (data.hwid) {
                            ws.user = user;
                            ws.rType = "controller";
                            socketMap.addSocket(ws, "controller", user)
                        } else {
                            ws.user = user;
                            ws.rType = "remote";
                            socketMap.addSocket(ws, "remote", user)
                        }
                        updateTable();
                    }
                });
            }
            break;

        case 'cmdLog':
            if (ws.user && ws.user.uid) {
                socketMap.cmdLog(ws, data.text);
            }
            break;

        case 'cmdWrite':
            if (ws.user && ws.user.uid) {
                socketMap.cmdWrite(ws, data.text);
            }
            break;

        case 'updateSettings':
            if (ws.user && ws.user.uid) {
                socketMap.updateSettings(ws, data.settings);
            }
            break;

        case 'log':
            if (ws.user && ws.user.uid) {
                var log = {
                    text: data.text,
                    date: data.date,
                    code: data.code,
                    smurf: data.smurf
                }
                ws.user.logs.push(log);
                ws.user.save();
                socketMap.pushLog(ws, log);
            }
            break;

        case 'smurf':
            if (ws.user && ws.user.uid) {
                socketMap.event(ws, data);
            }
            break;

        case 'group':
            if (ws.user && ws.user.uid) {
                socketMap.event(ws, data);
            }
            break;

        case 'smurfupdate':
            if (ws.user && ws.user.uid) {
                socketMap.updateSmurfs(ws, data);
            }
            break;
        case 'smurfdbupdate':
        	if (ws.user && ws.user.uid) {
                User.findOne( {uid:ws.user.uid}, function(err,acc){
                    if(!err && acc){
                        for(var i in acc.smurfs){
                            var smurf = acc.smurfs[i];
                            if (smurf.username == data.smurf.username){
                                smurf.currentLevel = data.smurf.currentLevel;
                                smurf.currentip = data.smurf.currentip;
                                smurf.currentrp = data.smurf.currentrp;
                            }
                        }
                        acc.markModified('smurfs');
                        acc.save();
                    }
                });
            }
        	break;
    }
}

var socketMap = new SocketMap();

var WebSocketServer = require('ws').Server,
    wss = new WebSocketServer({
        port: 4447
    });

wss.on('connection', function(ws) {
    ws.on('error', function(err) {
        updateTable();
        return false;
    });

    ws.on('message', function(message) {
        var res = tryParseJSON(message);
        if (res != false) {
            handleMessage(res, ws);
        }
        updateTable();
    });

    ws.on('close', function() {
        if (ws.user) {
            socketMap.removeSocket(ws, ws.rType, ws.user);
        }
        updateTable();
    });
});

function tryParseJSON(jsonString) {
    try {
        var o = JSON.parse(jsonString);
        if (o && typeof o === "object" && o !== null && o.type) {
            return o;
        }
    } catch (e) {}
    return false;
};

function tryParseValidJSON(jsonString) {
    try {
        var o = JSON.parse(jsonString);

        // Handle non-exception-throwing cases:
        // Neither JSON.parse(false) or JSON.parse(1234) throw errors, hence the type-checking,
        // but... JSON.parse(null) returns 'null', and typeof null === "object", 
        // so we must check for that, too.
        if (o && typeof o === "object" && o !== null) {
            return o;
        }
    } catch (e) {}

    return false;
};

net.createServer(function(socket) {
	socket.write(scriptPacket("welcome",VERSION));
    socket.on('data', function(data) {
    	var textChunk = data.toString('utf8');
    	if (textChunk.indexOf("|") > -1){
    		scriptManager(textChunk.split("|"),socket);
    	}
    });

    socket.on('error', function(err) {
    	socketMap.removeScript(socket);
    });

    socket.on('end', function() {
        socketMap.removeScript(socket);
    });
}).listen(4450);

function scriptManager(cmd,socket){
	switch(cmd[0]){
		case "login":
			User.findOne({bol:cmd[1]},"type trial uid ai", function(err,user){
                if(err || !user){
                    socket.write(scriptPacket("login","Failed to authenticate "+cmd[1]+", you should set Bol Settings on Remote Controller"));
                }else{
                    if (user.type == 2 || user.type == 1){
                        var convertedCode = js2lua.convert(user.ai.items[cmd[3].toLowerCase()])
                        socket.write(scriptPacket("login","Successfuly logged in "+cmd[1],convertedCode));
                        socket.uid = user.uid;
                        socket.champion = cmd[3].toLowerCase();
                        socket.gameid = cmd[2];
                        socketMap.addScript(user.uid, socket, cmd[2], cmd[3])
                    }else{
                        var ts = Date.now()
                        if(user.trial - ts > 0){
                            var minutes = Math.round((user.trial - ts) / 60000)
                            var convertedCode = js2lua.convert(user.ai.items[cmd[3].toLowerCase()])
                            socket.write(scriptPacket("login","Successfuly logged in "+cmd[1]+", " + minutes + " minutes remain.",convertedCode));
                            socket.uid = user.uid;
                            socket.champion = cmd[3].toLowerCase();
                            socket.gameid = cmd[2];
                            socketMap.addScript(user.uid, socket, cmd[2], cmd[3])
                        }else{
                            socket.write(scriptPacket("login","Your trial is expired "+cmd[1]));
                        }
                    }
                }
            });
		break;
        case "ping":
            if(socket.uid && socket.champion && socket.gameid){
                var info = {};
                var cmdInfo = cmd;
                cmdInfo.splice(0,1);
                info['data'] = cmd;
                info['champion'] = socket.champion;
                info['uid'] = socket.uid;
                info['gameid'] = socket.gameid;
                socketMap.scriptPing(socket.uid,info)
                socket.write(scriptPacket("pong",Date.now()));
            }
        break;
	}
}

function scriptPacket(){
	var packet = [];
	for(var arg = 0; arg < arguments.length; ++ arg)
	{
	    packet.push(arguments[arg]);
	}
	return packet.join("|");
}


module.exports = router;