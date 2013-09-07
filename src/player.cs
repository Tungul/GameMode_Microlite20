function Microlite::createCharacter(%this, %client, %data) // semi-recursive function for "step by step" character creation
{
	
//<<<<<<< HEAD
	%cmd = getWord(%data, 0);
	%parse = getWords(%data, 1, getWordCount(%data));

	if(%client.Microlite["hasChar"] && !%override)
	{
		messageClient(%client, '', "\c0---Warning---");
		messageClient(%client, '', "\c3You already have a character, if you want to make a new one run !newchar");
	}
	switch(%phase)
	{
		case 0: // info and name, race, class
		case 1: // str, dex, mind rolls - "choose an order from highest to lowest for these three stats"
		case 2: // hp
		case 3: // armor - list of purchaseable armor and benefit it provides, pick one 
		case 4: // fastpack
		case 5: // weapons
		case 6: // misc. inventory
		
	}
}

function Microlite::rollCheck(%this, %client, %data) {
	%cmd = firstWord(%data);
	%parse = restWords(%data);
	
	switch$(%cmd) {
		case "phystr":
			Microlite.rollDice(%client, "1d20+" @ (%client.microlite["physical"] + %client.microlite["strmod"]));
		case "phydex":
			Microlite.rollDice(%client, "1d20+" @ (%client.microlite["physical"] + %client.microlite["dexmod"]));
		case "subdex":
			Microlite.rollDice(%client, "1d20+" @ (%client.microlite["subterfuge"] + %client.microlite["dexmod"]));
		case "submind":
			Microlite.rollDice(%client, "1d20+" @ (%client.microlite["subterfuge"] + %client.microlite["mindmod"]));
		case "commind":
			Microlite.rollDice(%client, "1d20+" @ (%client.microlite["communication"] + %client.microlite["mindmod"]));
		case "knowmind":
			Microlite.rollDice(%client, "1d20+" @ (%client.microlite["knowledge"] + %client.microlite["mindmod"]));
	}
//=======
	if(%client.Microlite["hasChar"] && !%override) // --Meshiest (Added Bracket)
	{
		messageClient(%client, '', "\c0---Warning---");
		messageClient(%client, '', "\c3You already have a character, if you want to make a new one run !");
      switch(%phase)
      {
         case 0: // info and name, race, class
         case 1: // str, dex, mind rolls - "choose an order from highest to lowest for these three stats"
         case 2: // hp
         case 3: // armor - list of purchaseable armor and benefit it provides, pick one 
         case 4: // fastpack
         case 5: // weapons
         case 6: // misc. inventory
         
      }
	} // --Meshiest (Added missing brace) 
//>>>>>>> Fixed syntax errors
}