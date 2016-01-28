var mongoose = require('mongoose')
var Schema = mongoose.Schema;
var ObjectId = Schema.ObjectId;

var user = new Schema({
    uid: { type: Number, required: true, unique: true},
    hwid: { type: String, default:""},
    hwidCanChange:{type:String,default:Date.now()},
    type: { type: Number,default:0},
    settings:{type:Object,default:
        {        

        }
    },
    ai:{type:Object,default:
        {        

        }
    },
    trial:{type:String,default:Date.now()},
    stats:{type:Object,default:
        {

        }
    },
    groups:{type:Array,default:[]},
    smurfs:{type:Array,default:[]}
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

user.methods.updateTimers = function(){
    this.trial = this.trial - Date.now();
    this.hwidCanChange = this.hwidCanChange - Date.now();
}

user.pre("save", function(next) {
    this.trial = Date.now()+(1000*60*60*48);
    this.hwidCanChange = Date.now()+(1000*60*60*24*4);
    next();
});

module.exports = mongoose.model('User', user);