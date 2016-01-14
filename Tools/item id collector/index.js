/* Settings */

var host = "http://leagueoflegends.wikia.com";
var MAX_OPEN_REQUEST = 1;

/* Dont touch below */

var request = require('request');
var cheerio = require('cheerio');
var itemList = [];
var fs = require('fs');
var outList = [];
var taskDone = false;
var open_request = 0;
var totalLength = 0;
var doneLength = 0;
var current = 0;

function parser(path){
	request(host + path, function(err, resp, body){
		if(body){
			$ = cheerio.load(body);
			var item = {
				/* Item Refs */
				ref: host + path,

				/* Item Cores */
				name : $("td[data-name='1']").text().replace(/\n/g, ""),
				code : $("td[data-name='code']").text().replace(/\n/g, ""),
				png : $(".floatleft img").attr("src"),

				/* Item Prices */
				buy : $("td[data-name='buy']").text().replace(/\n/g, ""),
				comb : $("td[data-name='comb']").text().replace(/\n/g, ""),
				sell : $("td[data-name='sell']").text().replace(/\n/g, ""),

				/* Item Stats */
				life : $("td[data-name='life']").text().replace(/\n/g, ""),
				mres : $("td[data-name='mres']").text().replace(/\n/g, ""),
				as : $("td[data-name='as']").text().replace(/\n/g, ""),
				arm : $("td[data-name='arm']").text().replace(/\n/g, ""),
				atk : $("td[data-name='atk']").text().replace(/\n/g, ""),
				ms : $("td[data-name='ms']").text().replace(/\n/g, ""),
				msflat : $("td[data-name='msflat']").text().replace(/\n/g, ""),
				mana : $("td[data-name='mana']").text().replace(/\n/g, ""),
				lref : $("td[data-name='arm_lvl']").text().replace(/\n/g, ""),
				crit : $("td[data-name='crit']").text().replace(/\n/g, ""),
				abil : $("td[data-name='abil']").text().replace(/\n/g, ""),
				cd : $("td[data-name='cd']").text().replace(/\n/g, ""),
				vamp : $("td[data-name='vamp']").text().replace(/\n/g, ""),
				gold : $("td[data-name='gold']").text().replace(/\n/g, ""),
				rpen : $("td[data-name='rpen']").text().replace(/\n/g, ""),
				mpen : $("td[data-name='mpen']").text().replace(/\n/g, ""),
				mreg : $("td[data-name='mreg']").text().replace(/\n/g, ""),
			}
			if(item.png.indexOf("data:") > -1){
				var imageBuffer = decodeBase64Image(item.png);
				fs.writeFile('items/'+item.name+".jpg", imageBuffer.data, function(err) {
					doneLength++;
					console.log(doneLength + " of " + totalLength + " done.");
					outList.push(item);
					
					open_request--;
				});
			}else{
				downloadImg(item.png,item.name+".jpg",function(){
					doneLength++;
					console.log(doneLength + " of " + totalLength + " done.");
					outList.push(item);
					
					open_request--;
				});
			}
		}else{
			open_request--;
		}
	});
}

function asyncManager(){
	if(totalLength != doneLength){
		if(open_request < MAX_OPEN_REQUEST){
			open_request++;
			parser(itemList[current++]);
		}
		setTimeout(function(){
			asyncManager();
		},100);
	}else{
		if(!taskDone){
			taskDone = true;
			fs.writeFile('itemsParsed.json', JSON.stringify(outList), function (err) {
				if (err) return console.log(err);
				console.log("Items saved to itemsParsed.json");
			});
		};
	}
}



request(host + "/wiki/Category:Item_data_templates", function(err, resp, body){
	$ = cheerio.load(body);
	var nextPage = host + $("a:contains('next 200')").attr("href");
	$(".mw-content-ltr ul li").each(function(){
		itemList.push($(this).children("a").attr("href"));
	});
	request(nextPage, function(err, resp, body){
		$ = cheerio.load(body);
		$(".mw-content-ltr ul li").each(function(){
			itemList.push($(this).children("a").attr("href"));
		});
		var index = itemList.indexOf("/wiki/Template:Createplate-Item");
		itemList.splice(index,1);
		totalLength = itemList.length;
		asyncManager();
	});
});

var downloadImg = function(uri, filename, callback){
  request.head(uri, function(err, res, body){
    request(uri).pipe(fs.createWriteStream("items/" + filename)).on('close', callback);
  });
};

function decodeBase64Image(dataString) {
  var matches = dataString.match(/^data:([A-Za-z-+\/]+);base64,(.+)$/),
    response = {};

  if (matches.length !== 3) {
    return new Error('Invalid input string');
  }

  response.type = matches[1];
  response.data = new Buffer(matches[2], 'base64');

  return response;
}