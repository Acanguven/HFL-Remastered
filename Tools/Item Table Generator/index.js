var request = require('request');
var js2lua = require('js2lua');

request("https://ddragon.leagueoflegends.com/api/versions.json", function(err, resp, body){
	var versionList = JSON.parse(body);
	request("http://ddragon.leagueoflegends.com/cdn/"+versionList[0]+"/data/en_US/item.json", function(err,resp,body){
		var itemTable = JSON.parse(body).data;
		console.log(js2lua.convert(itemTable))
	});
});