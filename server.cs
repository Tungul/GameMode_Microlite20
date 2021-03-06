// This add-on assumes you've read the rules of Microlite20, see the rules.zip file, or the following links:
// http://microlite20.net/ // official site, bitch to navigate
// http://1d4chan.org/wiki/Microlite20 // core rules, not 100% up to date
// http://arthur.jfmi.net/m20/ // 'pocket' sized rulebooks, easiest to find the rules, includes main rules as well

if(!isObject(Microlite)) {
	new ScriptObject("Microlite") {
		datalList = "name level str dex mind hp ac class race physical subterfuge knowledge communication strmod dexmod mindmod attackmeleemod attackmagicmod attackmisslemod inventory";
		worldName = "Blockland";
		dungeonMaster = getNumKeyId();
	};
}

function Microlite::loadLibraries() {
	echo("Loading Microlite libraries.");
	exec("./lib/dice.cs");
	exec("./lib/lists.cs");
	exec("./lib/centerprint_scroller.cs");
}

function Microlite::loadSource() {
	echo("Loading Microlite source.");
	exec("./src/package.cs");
	exec("./src/fileio.cs");
	exec("./src/dm.cs");
	exec("./src/player.cs");
	exec("./src/inventory.cs");
}

if(isFile("config/microlite.cs")) {
	exec("config/microlite.cs");
}

Microlite.loadLibraries();
Microlite.loadSource();