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
		%var = getField(%line, 0);
		%data = getField(%line, 1);
		%client.Microlite[%var] = %data;
	}
	echo("Loaded character.");
	messageClient(%client, '', "\c5Welcome back to " @ Microlite.worldName @ "," SPC %client.MLCharacterName);
}
