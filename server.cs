// This add-on assumes you've read the rules of Microlite20, see the rules.zip file, or the following links:
// http://microlite20.net/ // official site, bitch to navigate
// http://1d4chan.org/wiki/Microlite20 // core rules, not 100% up to date
// http://arthur.jfmi.net/m20/ // 'pocket' sized rulebooks, easiest to find the rules, includes main rules as well

exec("./dice.cs");

new SimObject("Microlite20") {};

package Microlite20
{ // I've been javascript coding, the brackets next to the parenthesies habit will die hard
	function serverCmdMessageSent(%client, %msg)
	{
		if(getSubStr(%msg, 0, 1) $= "!") // check for special crap, otherwise don't parse anything
		{
			%parse = getWords(%msg, 1, getWordCount(%msg)); // all words except the first
			switch$(getWord(%msg, 0))
			{
				case "!roll": // normal random/spontaneous roll
					rollDice(%client, %parse);
					break; // not sure this is neccesary, but it is in javascript...
				case "!attack": // attack rolls
					Microlite20.attackRoll(%parse);
					break;
				case "!stealth": // allows for stealth checks and hidden maneuvers.
					Microlite20.stealthCheck(%client, %parse);
					return;
					// todo more stuff
			}
		}
		return parent::serverCmdMessageSent(%client, %msg);
	}
}