function Microlite::DMControl(%this, %client, %data) // pulled from chat
{
	%var = getWord(%line, 0);
	%data = restWords(%line); //getWords(%line, 1, getWordCount(%line)); // --Meshiest
	if(%this.dungeonMaster != %client.bl_id)
	{
		messageClient(%client, '', "\c6You are not the DM.");
		return;
	}
	switch$(%var) 
	{
		case "help": //Added command lists // --Meshiest
			messageClient(%client, '', "\c6Commands:");
			messageClient(%client, '', "\c5!DM worldname WORLD_NAME\c6 - Renames the world");
			messageClient(%client, '', "\c5!DM switchdm TARGET_BLID\c6 - Changes the Dungeon Master");
			messageClient(%client, '', "\c5!DM togglewebserver\c6 - Toggles the web server");
		case "worldname":
			%this.worldName = %data;
			messageAll('', "\c6The DM has updated the name of the world. Welcome to \c3" @ %this.worldName @ "\c6!");
			%this.save("microlite/config.cs");
			for(%i = 0; %i < clientGroup.getCount(); %i++)
			{
				%cl = clientGroup.getObject(%i);
				if(%cl.Microlite["race"] !$= "dwarf") return;
			}
			messageAll('', "\c4Strike the earth!");
			//break;
		case "switchdm":
			%target = getWord(%data, 0);
			if(!isInt(%target))
			{
				messageClient(%client, '', "\c6The proper usage of this command is \c5!DM switchdm TARGET_BLID");
				return;
			}
			%this.dungeonMaster = %target;
			messageClient(findClientByBL_ID(%this.dungeonMaster), '', "\c4You are now the Dungeon Master. Say !DM help for information on DM-specific commands.");
			%this.save("microlite/config.cs");
			//break;
		case "togglewebserver":
			messageClient(%client, '', "\c6Attempting to toggle webserver... please stand by...");
			%this.toggleWebserver();
			%this.save("microlite/config.cs");
			//break;
		default:
			messageClient(%client, '', "\c6I think I missed something - you didn't input a command. Here's the help function: !DM help");
			serverCmdMessageSent(%client, "!DM help");
			//break; // --Meshiest (You don't need these breaks, you can call return, though)
	}
}