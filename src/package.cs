package Microlite20 {
	function serverCmdMessageSent(%client, %msg)	{
		if(getSubStr(%msg, 0, 1) $= "!") {// check for special crap, otherwise don't parse anything
			%cmd = getSubStr(getWord(%msg, 0), 1, strLen(%cmd));
			%parse = restWords(%msg); // all words except the first
			switch$(%cmd) {
				case "help":
					messageClient(%client, '', "\c3GameMode_Microlite20 help");
					messageClient(%client, '', "\c3!roll [formula] - ");
				case "roll": // normal random/spontaneous roll
					Microlite.rollDice(%client, %parse);
					return;
				case "attack": // attack rolls
					Microlite.attackRoll(%parse);
					return;
				case "check": // make a str+dex check to dodge the falling rock!
					Microlite.rollCheck(%client, %parse);
					return;
				case "inventory":
					Microlite.inventory(%client, %parse); // inventory actions
					return;
				case "dm":
					Microlite.dmcontrol(%client, %parse);
				case "newchar":
					Microlite.createCharacter(%client, %parse);
					// todo more stuff
			}
		}
		// don't forget in character chat!
		return parent::serverCmdMessageSent(%client, %msg);
	}
};
activatePackage("Microltie20");