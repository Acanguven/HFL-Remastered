var mongoose = require('mongoose')
var fs = require('fs');
var Schema = mongoose.Schema;
var ObjectId = Schema.ObjectId;

var user = new Schema({
    uid: { type: Number, required: true, unique: true},
    hwid: { type: String, default:""},
    hwidCanChange:{type:Number},
    type: { type: Number,default:0},
    settings:{type:Object,default:
        {        
            packetSearch:0,
            buyBoost:0,
            reconnect:1,
            disableGpu:0,
            manualInjection:0
        }
    },
    ai:{type:Object,default:{}},
    trial:{type:Number},
    stats:{type:Object,default:
        {
            totalGames:[],
            totalWins:[]
        }
    },
    logs:{type:Array,default:[]},
    groups:{type:Array,default:[]},
    smurfs:{type:Array,default:[]},
    bol:{type:String,default:""}
});
user.methods.testHwid = function (hwid, cb) {
    if(this.hwid == hwid){
        cb(true);
    }else{
        if(Date.now() > this.hwidCanChange){
            this.hwid = hwid;
            this.hwidCanChange = Date.now()+(1000*60*60*24*4);
            this.save(function(err,nHa){
                cb(true);
            });
        }else{
            cb(false);
        }
    }
};

user.methods.testTrial = function (cb) {
    if(Date.now() > this.trial){
        cb(false);
    }else{
        cb(true);
    }
};

user.pre("save", function(next) {
    if(!this.trial && !this.hwidCanChange){
        var self = this;
        fs.readFile('./defaultbuild.json', 'utf8', function (err, data) {
            if (err) throw err;
            var obj = JSON.parse(data);
            self.ai = obj;
            self.trial = Date.now()+(1000*60*60*30); //500u 48 yap
            self.hwidCanChange = Date.now()-1000;
            next();
        });
    }else{
        next();
    }
});

module.exports = mongoose.model('User', user);