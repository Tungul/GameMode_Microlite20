function Microlite::createCharacter(%this, %client, %data) // semi-recursive function for "step by step" character creation
{
	
	if(%client.Microlite["hasChar" && !%override)
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
}