// This add-on assumes you've read the rules of Microlite20, see the rules.zip file, or the following links:
// http://microlite20.net/ // official site, bitch to navigate
// http://1d4chan.org/wiki/Microlite20 // core rules, not 100% up to date
// http://arthur.jfmi.net/m20/ // 'pocket' sized rulebooks, easiest to find the rules, includes main rules as well

new SimObject("Microlite")
{
	datalList = "name level str dex mind hp ac class race physical subterfuge knowledge communication strmod dexmod mindmod attackmeleemod attackmagicmod attackmisslemod inventory";
	worldName = "Placeholder"; // todo: put this somewhere export("Microlite*", "microlite/config.cs"););
};

exec("./dice.cs");
exec("./package.cs");
exec("./fileio.cs");
exec("./lib_lists.cs");
exec("./dm.cs");
exec("./player.cs");
