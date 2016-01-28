var express = require('express');
var router = express.Router();
var mongoose = require('mongoose');
var jwt = require('jsonwebtoken');
var ipn = require('paypal-ipn');
var querystring = require('querystring');
var http = require('http');

mongoose.connect("mongodb://localhost:27017/hflRest");
var db = mongoose.connection;
var fs = require("fs");
var Schema = mongoose.Schema;
var TOKEN_KEY = "Oh!my_ohMibod.?;";
db.on('error', console.error.bind(console, 'connection error:'));
db.once('open', function (callback) {
  console.log("connection to db open")
  //db.db.dropDatabase(); //Db refresher
});
var User = require("../models/user.js");


router.get('/ts', function(req, res, next) {
	res.json({ts:Date.now()});
});

router.post('/ipn', function(req, res, next) {
	ipn.verify(req.body, {'allow_sandbox': true}, function callback(err, mes) {
	  if (err) {
	    console.error(err);
	  } else {
	    if (req.body.payment_status == 'Completed') {
	      // Payment has been confirmed as completed 
	    }
	  }
	});
});

router.post("/remotelogin", function(req,res,next){
	loginForumBridge(req.body.username,req.body.password,function(login){
		var resultResponse = {
			forumData : {},
			userData: {},
			token:""
		};
		if(login.uid){
			resultResponse.forumData = login;
			User.findOne({uid:login.uid}, function(err,user){
				if(err){
					console.log(err);
				}else{
					if(user){
						user.updateTimers();
						resultResponse.userData = user;
						resultResponse.token = jwt.sign({
							joindate: login.joindate,
							uid: login.uid,
							removeTime: Date.now() + 1000 * 60 * 1
						},TOKEN_KEY);
						res.json(resultResponse);
					}else{
						var newUser = new User({
							uid:login.uid,
						});
						newUser.save(function(err,newUserAppended){
							newUserAppended.updateTimers();
							resultResponse.userData = newUserAppended;
							resultResponse.token = jwt.sign({
								joindate: login.joindate,
								uid: login.uid,
								removeTime: Date.now() + 1000 * 60 * 1
							},TOKEN_KEY);
							res.json(resultResponse);
						});
					}
				}
			})
		}else{
			res.json(login)
		}
	});
});

router.get("/getSmurfs", verifyTokenDetectUser, function(req,res,next){
	res.json(req.user.smurfs);
});


function loginForumBridge(username,password,callback){
	var data = querystring.stringify({
    username: username,
    password: password
  });

	var options = {
    host: 'forum.handsfreeleveler.com',
    port: 80,
    path: '/api/ns/login',
    method: 'POST',
    headers: {
        'Content-Type': 'application/x-www-form-urlencoded',
        'Content-Length': Buffer.byteLength(data)
    }
	};

	var req = http.request(options, function(res) {
    res.setEncoding('utf8');
    res.on('data', function (chunk) {
      callback(JSON.parse(chunk));
    });
	});

	req.write(data);
	req.end();
}

function verifyTokenDetectUser(req,res,next){
	jwt.verify(req.headers.authorization, TOKEN_KEY, function (err, decoded) {
		if (err) {
			res.json({ type: "dead", message: "Token error" });
			return false;
		}
		if (!decoded) {
			res.json({ type: "dead", message: "Token decoding error" });
			return false;
		}
		if (decoded.removeTime < Date.now()) {
			res.json({ type: "dead", message: "Token expire error" });
			return false;
		} else {
			User.findOne({uid:decoded.uid}, function(err,user){
				if(err || !user){
					res.json({ type: "dead", message: "Token user does not exist" });
					return false;
				}else{
					req.user = user;
					next();
				}
			});
		}
	});
}

module.exports = router;
