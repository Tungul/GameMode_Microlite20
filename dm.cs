function Microlite.DMControl(%client, %data) // pulled from chat
{
	%var = getWord(%line, 0);
	%data = getWords(%line, 1, getWordCount(%line));
	if(Microlite.dungeonMaster != %client.bl_id)
	{
		messageClient(%client, '', "\c6You are not the DM.");
		return;
	}
	switch$(%var) 
	{
		case "help":
			messageClient(%client, '', "help goes here loel")
		case "worldname":
			Microlite.worldName = %data;
			messageAll('', "\c6The DM has updated the name of the world. Welcome to \c3" @ Microlite.worldName @ "\c6!");
			Microlite.save("microlite/config.cs");
			for(%i = 0; %i < clientGroup.getCount(); %i++)
			{
				%cl = clientGroup.getObject(%i);
				if(%cl.Microlite["race"] !$= "dwarf") return;
			}
			messageAll('', "\c4Strike the earth!");
			break;
		case "switchdm":
			%target = getWord(%data, 0);
			if(!isInt(%target))
			{
				messageClient(%client, '', "\c6The proper usage of this command is \c5!DM switchdm TARGET_BLID");
				return;
			}
			Microlite.dungeonMaster = %target;
			messageClient(findClientByBL_ID(Microlite.dungeonMaster), '', "\c4You are now the Dungeon Master. Say !DM help for information on DM-specific commands.");
			Microlite.save("microlite/config.cs");
			break;
		case "togglewebserver":
			messageClient(%client, '', "\c6Attempting to toggle webserver... please stand by...");
			Microlite.toggleWebserver();
			Microlite.save("microlite/config.cs");
			break;
		default:
			messageClient(%client, '', "\c6I think I missed something - you didn't input a command. Here's the help function: !DM help");
			serverCmdMessageSent(%client, "!DM help");
			break;
	}
}