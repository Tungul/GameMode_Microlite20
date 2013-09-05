function Microlite::importCharacter(%this, %client)
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
	echo("Imported character.");
	messageClient(%client, '', "\c5Welcome back to " @ %this.worldName @ "," SPC %client.Microlite["name"]);
	%client.Microlite["hascharacter"] = true;
	MicroliteFO.close(); MicroliteFO.delete();
}

function Microlite::exportCharacter(%this, %client)
{
	echo("Exporting character for " @ %client.blid);
	%path = "microlite/" @ %client.blid @ ".txt");
	
	new fileObject("MicroliteFO");
	MicroliteFO.openForWrite(%path);
	for(%i = 0; %i < getWordCount(%this.dataList); %i++)
	{
		%var = getWord(%this.dataList, i);
		MicroliteFO.writeLine(%var @ "\t" @ %client.Microlite[%var]);
	}
	echo("Exported character.");
	MicroliteFO.close(); MicroliteFO.delete();
}