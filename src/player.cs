function Microlite::createCharacter(%this, %client, %data) {// semi-recursive function for "step by step" character creation
	%cmd = firstWord(%data);
	%parse = restWords(%data);

	if(%client.Microlite["hasChar"] && !%override)
	{
		messageClient(%client, '', "\c0---Warning---");
		messageClient(%client, '', "\c5You already have a character, if you want to make a new one run \c3!newchar override");
		return;
	}

	Microlite.createCharacterBackend(%client);
}

function Microlite::createCharacterBackend(%this, %client, %data) {
	switch(%phase)
	{
		case 0: // info and name, race, class
			%client.Microlite["hasChar"] = false;
		case 1: // str, dex, mind rolls - "choose an order from highest to lowest for these three stats"
		case 2: // hp
		case 3: // armor - list of purchaseable armor and benefit it provides, pick one 
		case 4: // fastpack
		case 5: // weapons
		case 6: // misc. inventory
		
	}
}

function Microlite::showStats(%this, %client, %blid) {
	%target = findClientByBL_ID(%blid);
	if(!isObject(%target)) {
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