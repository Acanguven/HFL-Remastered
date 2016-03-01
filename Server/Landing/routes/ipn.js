var express = require('express');
var router = express.Router();
var mongoose = require('mongoose');
mongoose.connect("mongodb://127.0.0.1:27017/hflRest");
var db = mongoose.connection;
var fs = require("fs");
var Schema = mongoose.Schema;
db.on('error', console.error.bind(console, 'connection error:'));

db.once('open', function(callback) {
    console.log("connection to db open")
});

var User = require("../models/user.js");

router.post('/gotPaymentHolyFuckYes', function(req,res,next){
    if(req.body.payment_status){
      if (req.body.payment_status == 'Completed') {
        if(req.body.custom){
          if(req.body.mc_gross == "50.00"){
            User.update({_id: req.body.custom}, {
                type: 2, 
            }, function(err, numberAffected, rawResponse) {
               console.log(err);
            })
          }
          if(req.body.mc_gross == "35.00"){
            User.update({_id: req.body.custom}, {
                type: 1, 
            }, function(err, numberAffected, rawResponse) {
               console.log(err);
            })
          }
          if(req.body.mc_gross == "15.00"){
            User.update({_id: req.body.custom}, {
                trial: Date.now()+2592000000, 
            }, function(err, numberAffected, rawResponse) {
               console.log(err);
            })
          }
        }
      }
    }
    res.sendStatus(200);
})


module.exports = router;