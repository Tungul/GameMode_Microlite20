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
					Microlite.rollDice(%client, %parse);
					break; // not sure this is neccesary, but it is in javascript...
				case "!attack": // attack rolls
					Microlite.attackRoll(%parse);
					break;
				case "!stealth": // allows for stealth checks and hidden maneuvers.
					Microlite.stealthCheck(%client, %parse);
					return;
				case "!dm":
					Microlite.dmcontrol(%client, %parse);
					break;
					// todo more stuff
			}
		}
		return parent::serverCmdMessageSent(%client, %msg);
	}
}
