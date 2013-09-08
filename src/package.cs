package Microlite20 {
	function serverCmdMessageSent(%client, %msg)	{
		if(getSubStr(%msg, 0, 1) $= "!") {// check for special crap, otherwise don't parse anything
			%cmd = getSubStr(firstWord(%msg), 1, strLen(%msg));
			%parse = restWords(%msg); // all words except the first
			switch$(%cmd) {
				case "help":
					messageClient(%client, '', "\c3GameMode_Microlite20 help");
					messageClient(%client, '', "\c3!roll [formula] \c6- eg: 1d20+5");
					messageClient(%client, '', "\c3!check [kind] \c6- Valid options are: phystr, phydex, subdex, submind, commind, knowmind");
					messageClient(%client, '', "\c3!attack [type] \c6- eg: melee, missle, magic");
					messageClient(%client, '', "\c3!stats [BL_ID] \c6- BL_ID is of the target, leave it blank to show your own.");
					return;

				case "roll": // normal random/spontaneous roll
					Microlite.rollDice(%client, %parse);

				case "attack": // attack rolls
					Microlite.rollAttack(%parse);

				case "check": // make a str+dex check to dodge the falling rock!
					Microlite.rollCheck(%client, %parse);

				// case "inventory":
				// 	Microlite.inventory(%client, %parse); // inventory actions
				// 	return;

				case "stats":
					Microlite.showStats(%client, %data);

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
activatePackage("Microlite20");