function Microlite::createCharacter(%this, %client, %data) {// semi-recursive function for "step by step" character creation
	%override = (%data $= "override");

	if(%client.Microlite["hasChar"] && !%override)
	{
		messageClient(%client, '', "\c0---Warning---");
		messageClient(%client, '', "\c5You already have a character, if you want to make a new one run \c3!newchar override");
		return;
	}

	%client.Microlite["charphase"] = 0;

	switch(%client.Microlite["charphase"])
	{
		case 0: // intro
			%client.Microlite["charphase"] = 1;
			%client.Microlite["hasChar"] = false;
			messageClient(%client, '', "\c6Welcome to the character creator, you're in phase 1.");
			messageClient(%client, '', "\c6You need to define your name, race, and class.");
			messageClient(%client, '', "\c6Say !newChar \c3nameHere raceHere classHere");
			messageClient(%client, '', "\c6Names can be either one or two words long.");
			messageClient(%client, '', "\c6Available choices for race and class can be viewed with \c3!viewChoices\c6.");
		case 1: // info and name, race, class
			%count = getWordCount(%data);
			if(%count == 3 || %count == 4) {
				%client.Microlite["charphase"] = 2;
				switch(%count) {
					case 3:
						%client.Microlite["name"] = getWord(%data, 0);
						%client.Microlite["race"] = getWord(%data, 1);
						%client.Microlite["class"] = getWord(%data, 2);
					case 4:
						%client.Microlite["name"] = getWords(%data, 0, 1);
						%client.Microlite["race"] = getWord(%data, 2);
						%client.Microlite["class"] = getWord(%data, 3);
				}
				messageClient(%client, '', "\c6Alright, now we're going to figure out your stats.");
				messageClient(%client, '', "\c6To do so, I'll roll 4d6 and drop the lowest. You need to tell me where you want that number applied: STR, DEX, or MIND.");
				messageClient(%client, '', "\c6Say \c3!newChar statHere");
				messageClient(%client, '', "\c6Where statHere is either STR, DEX, or MIND. Caps not neccesary.");
				%client.Microlite["tempphase"] = 0;
				%client.Microlite["level"] = 1;
				serverCmdMessageSent(%client, "!newChar next");
			}
			else {
				messageClient(%client, '', "\c6You did something wrong. Did you have the right number of arguments? Try again: ");
				messageClient(%client, '', "\c6Say !newChar \c3nameHere raceHere classHere");
				messageClient(%client, '', "\c6Names can be either one or two words long.");
				messageClient(%client, '', "\c6Available choices for this phase can be viewed with \c3!viewChoices\c6.");
			}
		case 2: // str, dex, mind rolls - "choose an order from highest to lowest for these three stats"
			%client.Microlite["temp4d6"] = Microlite.charGen4d6Drop();
			%client.Microlite["tempphase"] = "str dex mind";
			messageClient(%client, '', "\c6Where do you want the following number assigned?\c3" SPC (hasItemOnList(%client.Microlite["tempphase"], "str") ? "STR" : "") SPC (hasItemOnList(%client.Microlite["tempphase"], "dex") ? "DEX" : "") SPC (hasItemOnList(%client.Microlite["tempphase"], "mind") ? "MIND" : ""));
			switch$(firstWord(%data)) {
				case "STR":
					if(%client.Microlite["str"] $= "") {
						%client.Microlite["str"] = %client.Microlite["tempd4d6"];
					}
					else {
						messageClient(%client, '', "\c6Sorry, that stat already has a number in it. Put it in a different one.");
						return;
					}
				case "DEX":
					if(%client.Microlite["dex"] $= "") {
						%client.Microlite["dex"] = %client.Microlite["tempd4d6"];
					}
					else {
						messageClient(%client, '', "\c6Sorry, that stat already has a number in it. Put it in a different one.");
						return;
					}
				case "MIND":
					if(%client.Microlite["mind"] $= "") {
						%client.Microlite["mind"] = %client.Microlite["tempd4d6"];
					}
					else {
						messageClient(%client, '', "\c6Sorry, that stat already has a number in it. Put it in a different one.");
						return;
					}
			}
			if(%client.Microlite["str"] && %client.Microlite["dex"] && %client.Microlite["mind"]) {
				%client.Microlite["charphase"] = 3;

				serverCmdMessageSent("!newChar");
			}
		case 3: // hp
			%client.Microlite["charphase"] = 4;
			messageClient(%client, '', "\c6Your HP is " @ (%client.Microlite["hp"] = %client.Microlite["str"] + getRandom(1,6)));
		case 4: // armor - list of purchaseable armor and benefit it provides, pick one 
			messageClient(%client, '', "\c6Next up is weaponry. I'll show you a list of the weapons available to you.");
			messageClient(%client, '', "\c6The data is as follows: Weapon name, Cost, Damage, and Range.");
			CenterprintTextScroller.beginPrint(%client, )

			%client.Microlite["charphase"] = 5;
		case 5: // fastpack
			messageClient(%client, '', "\c6You can pick from one of 3 fast packs. They all cost 45 GP, but they're an easy bundle."); //Creative fucking license, not RAW, deal with it.
			%client.Microlite["charphase"] = 6;
		case 6: // weapons
			%client.Microlite["charphase"] = 7;
		case 7: // misc. inventory
			%client.Microlite["charphase"] = 8;
		case 8: // fin
			%client.Microlite["hasChar"] = true;
		
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