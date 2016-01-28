/* Settings */

var host = "http://leagueoflegends.wikia.com";
var MAX_OPEN_REQUEST = 5;

/* Dont touch below */

var request = require('request');
var cheerio = require('cheerio');
var fs = require('fs');
var outList = [];


request("http://leagueoflegends.wikia.com/wiki/List_of_champions", function(err, resp, body){
	$ = cheerio.load(body);
	$(".stdt .character_icon").each(function(){
		var name = $(this).children("a").attr("title").replace(/\'/g,"").replace(/\s+/g,"").replace(/\./g,"")
		outList.push(name);
	});
	
	fs.writeFile("champions.json", JSON.stringify(outList), function(){
		console.log("Job Done");
	});
});