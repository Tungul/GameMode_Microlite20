function Microlite::createCharacter(%this, %client, %data) {// semi-recursive function for "step by step" character creation
	%override = (%data $= "override");

	if(%client.Microlite["hasChar"] && !%override)
	{
		messageClient(%client, '', "\c0---Warning---");
		messageClient(%client, '', "\c5You already have a character, if you want to make a new one run \c3!newchar override");
		return;
	}

	%client.Microlite["charphase"] = "info";
	%client.Microlite["hasChar"] = false;
	%client.Microlite["inCharGen"] = true;
	Microlite.characterCreatorParser(%client);
}

function Microlite::characterCreatorParser(%this, %client, %data) {
	switch$(%client.Microlite["charphase"])
	{
		case "info": // intro
			messageClient(%client, '', "\c6Welcome to the character creator, you're in the name phase.");
			messageClient(%client, '', "\c6You need to define your name, race, and class.");
			messageClient(%client, '', "\c6Say \c3nameHere");
			messageClient(%client, '', "\c6For example, just say in team chat: \c3Edward \"Dick\" Cullen");
			messageClient(%client, '', "\c6Names can be up to 30 characters long. Abuse of this will obviously warrant a ban.");
			%client.Microlite["charphase"] = "name";
		case "name":
			if(strLen(%data) > 30) {
				messageClient(%client, '', "\c6Your name is \c3" @ (strLen(%data) - 30) @ "\c6 characters too long.");
				return;
			}
			else {
				%client.Microlite["name"] = %data;
				%client.Microlite["charphase"] = "stats";
				messageClient(%client, '', "\c6Next we'll be defining your stats.");
				messageClient(%client, '', "\c6I'll roll 4d6 and drop the lowest and then show you the number.");
				messageClient(%client, '', "\c6You tell me (via teamchat) which stat you want it to apply to: STR, DEX, or MIND");
				%client.Microlite["todoStats"] = "str dex mind";
			}
		case "stats": //str, dex, mind
			if(%client.Microlite["temp4d6"] $= "") {
				%client.Microlite["temp4d6"] = Microlite.charGen4d6Drop();
			}
			messageClient(%client, '', "\c6Your current number is \c3" @ %client.Microlite["temp4d6"] @ "\c6.");
			if(getWordCount(%client.Microlite["todoStats"]) > 1) {
				switch$(%data) {
					case "str":
						if(%client.Microlite["str"] !$= "") {
							messageClient(%client, '', "\c6You already chose a roll to go in that skill. Pick another one.");
						}
						else
							%client.Microlite["str"] = %client.Microlite["temp4d6"];
							%client.Microlite["strmod"] = ((%client.Microlite["str"] - 10) / 2);
							%client.Microlite["temp4d6"] = "";
					case "dex":
						if(%client.Microlite["dex"] !$= "") {
							messageClient(%client, '', "\c6You already chose a roll to go in that skill. Pick another one.");
						}
						else
							%client.Microlite["dex"] = %client.Microlite["temp4d6"];
							%client.Microlite["dexmod"] = ((%client.Microlite["dex"] - 10) / 2);
							%client.Microlite["temp4d6"] = "";
					case "mind":
						if(%client.Microlite["mind"] !$= "") {
							messageClient(%client, '', "\c6You already chose a roll to go in that skill. Pick another one.");
						}
						else
							%client.Microlite["mind"] = %client.Microlite["temp4d6"];
							%client.Microlite["mindmod"] = ((%client.Microlite["mind"] - 10) / 2);
							%client.Microlite["temp4d6"] = "";
				}
			}
			else {
				%stat = trim(%client.Microlite["todoStats"]);
				%client.Microlite[%stat] = Microlite.charGen4d6Drop();
				%client.Microlite[%stat @ "mod"] = ((%client.Microlite[%stat] - 10) / 2);
			}

	}

}

function Microlite::showStats(%this, %client, %blid) {
	%target = findClientByBL_ID(%blid);
	if(!isObject(%target)) {
		messageClient(%client, '', "\c6I can't find the target:\c5" SPC %blid)
		return;
	}

	%same = (%target.bl_id == %client.bl_id);

	messageClient(%client, '', (%same ? "You are" : %target.getPlayerName() SPC "is") SPC "\c6playing as\c3" SPC %target.Microlite["name"] @ "\c6.");
	messageClient(%client, '', "\c6" @ (%same ? "You" : "They") SPC "are a level\c3" SPC %target.Microlite["level"] SPC %target.Microlite["class"] @ "\c6.");
	messageClient(%client, '', "\c6" @ (%same ? "You" : "They") SPC "have\c3" SPC %target.Microlite["hp"] @ "HP\c6, and a AC of\c3" SPC %target.Microlite["ac"] @ "\c6.");
	messageClient(%client, '', "\c6RAW STR/DEX/MIND: \c3" @ %target.Microlite["str"] @ "\c6/\c3" @ %target.Microlite["dex"] @ "\c6/\c3" @ %target.Microlite["mind"]);
	messageClient(%client, '', "\c6MOD STR/DEX/MIND: \c3" @ %target.Microlite["strmod"] @ "\c6/\c3" @ %target.Microlite["dexmod"] @ "\c6/\c3" @ %target.Microlite["mindmod"]);
	messageClient(%client, '', "\c6Physical/Subterfuge/Knowledge/Communication:" @ %target.Microlite["physical"] @ "\c6/\c3" @ %target.Microlite["subterfuge"] @ "\c6/\c3" @ %target.Microlite["knowledge"] @ "\c6/\c3" @ %target.Microlite["communication"]);

}

function Microlite::rollCheck(%this, %client, %data) {
	%cmd = firstWord(%data);
	%parse = restWords(%data);
	
	switch$(%cmd) {
		case "phystr":
			Microlite.rollDice(%client, "1d20+" @ (%client.microlite["physical"] + %client.microlite["strmod"]) @ ">" @ Microlite.CurrentDC);
		case "phydex":
			Microlite.rollDice(%client, "1d20+" @ (%client.microlite["physical"] + %client.microlite["dexmod"]) @ ">" @ Microlite.CurrentDC);
		case "subdex":
			Microlite.rollDice(%client, "1d20+" @ (%client.microlite["subterfuge"] + %client.microlite["dexmod"]) @ ">" @ Microlite.CurrentDC);
		case "submind":
			Microlite.rollDice(%client, "1d20+" @ (%client.microlite["subterfuge"] + %client.microlite["mindmod"]) @ ">" @ Microlite.CurrentDC);
		case "commind":
			Microlite.rollDice(%client, "1d20+" @ (%client.microlite["communication"] + %client.microlite["mindmod"]) @ ">" @ Microlite.CurrentDC);
		case "knowmind":
			Microlite.rollDice(%client, "1d20+" @ (%client.microlite["knowledge"] + %client.microlite["mindmod"]) @ ">" @ Microlite.CurrentDC);
	}
}

function Microlite::rollAttack(%this, %client, %data) {
	%cmd = firstWord(%data);
	%parse = restWords(%data);
	// %two-handed = 
	
	switch$(%cmd) {
		case "melee":
			Microlite.rollDice(%client, "1d20+" @ (%client.microlite["attackmeleemod"]) @ ">" @ Microlite.CurrentDC);
		case "missle":
			Microlite.rollDice(%client, "1d20+" @ (%client.microlite["attackmisslemod"]) @ ">" @ Microlite.CurrentDC);
		case "magic":
			Microlite.rollDice(%client, "1d20>10"); // for some goddamned reason this is all magicians ever do
	}

}