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

function Microlite::importCharacter(%client)
{
	echo("Importing character for " @ %client.blid);
	%path = "microlite/" @ %client.blid @ ".txt");
	if(!isFile(%path))
	{
		echo("No character found.");
		messageClient(%client, '', "\c6I couldn't find a character for you, you can make one with !newCharacter");
		return;
	}
	new fileObject("MicroliteFO");
	MicroliteFO.openForRead(%path);
	while(!MicroliteFO.isEOF())
	{
		%line = MicroliteFO.readLine();
		Microlite::parseCharacterLine(%client, %line);
	}
	echo("Loaded character.");
	messageClient(%client, '', "\c5Welcome back to " @ Microlite.worldName @ "," SPC %client.MLCharacterName);
}