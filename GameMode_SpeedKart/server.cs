if($pref::Server::SpeedKart::RoundLimit $= "")
{
   $pref::Server::SpeedKart::RoundLimit = 8;
}

$SK::Initialized = false;

function SK_BuildTrackList()
{
   //
   %pattern = "Add-Ons/SpeedKart_*/save.bls";
   
   $SK::numTracks = 0;
   
   %file = findFirstFile(%pattern);
   while(%file !$= "")
   {
      $SK::Track[$SK::numTracks] = %file;
      $SK::numTracks++;

      %file = findNextFile(%pattern);
   }
}

function SK_DumpTrackList()
{
   echo("");
   if($SK::numTracks == 1)
      echo("1 track");
   else
      echo($SK::numTracks @ " tracks");
   for(%i = 0; %i < $SK::numTracks; %i++)
   {
      %displayName = $SK::Track[%i];
      %displayName = strReplace(%displayName, "Add-Ons/SpeedKart_", "");
      %displayName = strReplace(%displayName, "/save.bls", "");
      %displayName = strReplace(%displayName, "_", " ");

      if(%i == $SK::CurrentTrack)
         echo(" >" @ %displayName);
      else
         echo("  " @ %displayName);
   }
   echo("");
}

function SK_NextTrack()
{
   $SK::CurrentTrack = mFloor($SK::CurrentTrack);
   $SK::CurrentTrack++;
   $SK::CurrentTrack = $SK::CurrentTrack % $SK::numTracks;

   $SK::ResetCount = 0;

   SK_LoadTrack_Phase1($SK::Track[$SK::CurrentTrack]);
}

function SK_LoadTrack_Phase1(%filename)
{
   //suspend minigame resets
   $SK::MapChange = 1;

   //put everyone in observer mode
   %mg = $DefaultMiniGame;
   if(!isObject(%mg))
   {
      error("ERROR: SK_LoadTrack( " @ %filename  @ " ) - default minigame does not exist");
      return;
   }
   for(%i = 0; %i < %mg.numMembers; %i++)
   {
      %client = %mg.member[%i];
      %player = %client.player;
      if(isObject(%player))
         %player.delete();

      %camera = %client.camera;
      %camera.setFlyMode();
      %camera.mode = "Observer";
      %client.setControlObject(%camera);
   }
   
   //clear all bricks 
   // note: this function is deferred, so we'll have to set a callback to be triggered when it's done
   BrickGroup_888888.chaindeletecallback = "SK_LoadTrack_Phase2(\"" @ %filename @ "\");";
	BrickGroup_888888.chaindeleteall();
}

function SK_LoadTrack_Phase2(%filename)
{
   echo("Loading speedkart track " @ %filename);

   %displayName = %filename;
   %displayName = strReplace(%displayName, "Add-Ons/SpeedKart_", "");
   %displayName = strReplace(%displayName, "/save.bls", "");
   %displayName = strReplace(%displayName, "_", " ");
   
   %loadMsg = "\c5Now loading \c6" @ %displayName;

   //read and display credits file, if it exists
   // limited to one line
   %creditsFilename = filePath(%fileName) @ "/credits.txt";
   if(isFile(%creditsFilename))
   {
      %file = new FileObject();
      %file.openforRead(%creditsFilename);

      %line = %file.readLine();
      %line = stripMLControlChars(%line);
      %loadMsg = %loadMsg @ "\c5, created by \c3" @ %line;

      %file.close();
      %file.delete();
   }

   messageAll('', %loadMsg);

   //load environment if it exists
   %envFile = filePath(%fileName) @ "/environment.txt"; 
   if(isFile(%envFile))
   {  
      //echo("parsing env file " @ %envFile);
      //usage: GameModeGuiServer::ParseGameModeFile(%filename, %append);
      //if %append == 0, all minigame variables will be cleared 
      %res = GameModeGuiServer::ParseGameModeFile(%envFile, 1);

      EnvGuiServer::getIdxFromFilenames();
      EnvGuiServer::SetSimpleMode();

      if(!$EnvGuiServer::SimpleMode)     
      {
         EnvGuiServer::fillAdvancedVarsFromSimple();
         EnvGuiServer::SetAdvancedMode();
      }
   }
   
   //load save file
   schedule(10, 0, serverDirectSaveFileLoad, %fileName, 3, "", 2, 1);
}


//some horrible /commands to change tracks and such
function serverCmdTrackList(%client)
{
   for(%i = 0; %i < $SK::numTracks; %i++)
   {
      %displayName = $SK::Track[%i];
      %displayName = strReplace(%displayName, "Add-Ons/SpeedKart_", "");
      %displayName = strReplace(%displayName, "/save.bls", "");
      %displayName = strReplace(%displayName, "_", " ");

      if(%i == $SK::CurrentTrack)
         messageClient(%client, '', " >" @ %i @ ". \c6" @ %displayName);
      else
         messageClient(%client, '', "  " @ %i @ ". \c6" @ %displayName);
   }
}
function serverCmdSetTrack(%client, %i)
{
   if(!%client.isAdmin)
      return;

   if(mFloor(%i) !$= %i)
   {
      messageClient(%client, '', "Usage: /setTrack <number>");
      return;
   }

   if(%i < 0 || %i > $SK::numTracks)
   {
      messageClient(%client, '', "serverCmdSetTrack() - out of range index");
      return;
   }

   messageAll( 'MsgAdminForce', '\c3%1\c2 changed the track', %client.getPlayerName());
   
   $SK::CurrentTrack = %i - 1;
   SK_NextTrack();
}

function serverCmdNextTrack(%client, %i)
{
   if(!%client.isAdmin)
      return;

   messageAll( 'MsgAdminForce', '\c3%1\c2 changed the track', %client.getPlayerName());
   
   SK_NextTrack();
}

package GameModeSpeedKartPackage
{
   //this is called when save loading finishes 
   function GameModeInitialResetCheck()
   {
      Parent::GameModeInitialResetCheck();

      //if there is no track list, attempt to create it
      if($SK::numTracks == 0)
         SK_BuildTrackList();
      
      //if tracklist is still empty, there are no tracks
      if($SK::numTracks == 0)
      {
         messageAll('', "\c5No SpeedKart tracks available!");
         return;
      }

      if($SK::Initialized)
         return;

      $SK::Initialized = true;
      $SK::CurrentTrack = -1;
            
      SK_NextTrack();
   }

   //when we're done loading a new track, reset the minigame
   function ServerLoadSaveFile_End()
   {
      Parent::ServerLoadSaveFile_End();

      //new track has loaded, reset minigame
      if($DefaultMiniGame.numMembers > 0) //don't bother if no one is here (this also prevents starting at round 2 on server creation)
         $DefaultMiniGame.scheduleReset(); //don't do it instantly, to give people a little bit of time to ghost
   }
   
   //vehicles should explode in water
   function VehicleData::onEnterLiquid(%data, %obj, %coverage, %type)
   {
      Parent::onEnterLiquid(%data, %obj, %coverage, %type);

      %obj.damage(%obj, %obj.getPosition(), 10000, $DamageType::Lava);
      %obj.finalExplosion();
   }

   //players should die in water
   function Armor::onEnterLiquid(%data, %obj, %coverage, %type)
   {
      Parent::onEnterLiquid(%data, %obj, %coverage, %type);
      %obj.hasShotOnce = true;
      %obj.invulnerable = false;
      %obj.damage(%obj, %obj.getPosition(), 10000, $DamageType::Lava);
   }

   //when vehicle spawns, it cannot move (event must enable it)
   //this solves the driving through the garage problem
   function WheeledVehicleData::onAdd(%data,%obj)
   {
      Parent::onAdd(%data, %obj);

      for(%i = 0; %i < %data.numWheels; %i++)
         %obj.setWheelPowered(%i, %on);
   }

   //also you cannot click-push a vehicle while it is in non-moving mode
   function vehicle::OnActivate(%vehicle, %activatingObj, %activatingClient, %pos, %vec)
   {
      //just check a wheel
      if(!%vehicle.getWheelPowered(2))
         return;

      Parent::OnActivate(%vehicle, %activatingObj, %activatingClient, %pos, %vec);
   }


   //if you kill yourself in a vehicle, kill the vehicle
   function serverCmdSuicide(%client)
   {
      %player = %client.player;
      if(!isObject(%player))
         return;
      
      %vehicle = %player.getObjectMount();

      //kill the vehicle we're in
      if(isObject(%vehicle))
      {
         //if wheels are not powered, we're probably at the start of the race, so don't allow suicide in a vehicle
         %poweredTime = getSimTime() - %vehicle.poweredTime;
         %doBuzzer = false;
         if(%vehicle.getClassName() $= "AIPlayer")
            %doBuzzer = true;
         else if(!%vehicle.getWheelPowered(2) || %poweredTime < 8000)
            %doBuzzer = true;

         if(%doBuzzer)
         {
            //spam protect the buzzer enough to prevent serious problems
            if(getSimTime() - %player.lastSuicideBuzzerTime > 200)
            {
               %player.lastSuicideBuzzerTime = getSimTime();
               serverPlay3d("Beep_No_Sound", %vehicle.getPosition());
            }
            return;
         }

         //if vehicle is on fire, do final explosion
         //otherwise kill vehicle
         if(%vehicle.getDamagePercent() >= 1.0)
            %vehicle.finalExplosion();
         else
         {
            %vehicle.damage(%vehicle, %vehicle.getPosition(), 10000, $DamageType::Default);        
            %player.burnPlayer(5);
         }
      }
      else
      {         
         //no vehicle, normal suicide
         Parent::ServerCmdSuicide(%client);
         return;
      }

   }
      
   //resume death animation after corpse is booted out of burning vehicle
   function Armor::onUnmount(%data, %obj, %slot)
   {
      Parent::onUnmount(%data, %obj, %slot);

      if(%obj.getDamagePercent() >= 1)
      {
         %obj.playthread(3, "death1");
      }
   }
   
   //if smoeone gets back into a garage after the game starts, we don't want them to be able to press a button and respawn someone's cart from under them
   function fxDTSBrick::setVehicle(%obj, %data, %client)
   {
      if(isObject(%obj.vehicle))
      {
         //vehicle exists, if it is far from spawn, don't do this event
         %vec = vectorSub(%obj.vehicle.getPosition(), %obj.getPosition());
         %dist = vectorLen(%vec);
         if(%dist > 10)
         {
            return;
         }
      }

      Parent::setVehicle(%obj, %data, %client);
   }
   
   //total hack: when a vehicle is turned on record the start-of-race time
   // other options would be to make another event or add in a time offset 
   function fxDTSBrick::setVehiclePowered(%obj, %on, %client)
   {
      Parent::setVehiclePowered(%obj, %on, %client);

      if(%on)
      {
         if(isObject($DefaultMiniGame))
         {
            if($DefaultMiniGame.raceStartTime <= 0)
               $DefaultMiniGame.raceStartTime = getSimTime();
         }
      }
   }

   function MiniGameSO::Reset(%obj, %client)
   {
      //make sure this value is an number
      $pref::Server::SpeedKart::RoundLimit = mFloor($pref::Server::SpeedKart::RoundLimit);

      //handle our race time hack
      %obj.raceStartTime = 0;

      //count number of minigame resets, when we reach the limit, go to next track
      if(%obj.numMembers >= 0)
      {
         $SK::ResetCount++;
      }

      if($SK::ResetCount > $pref::Server::SpeedKart::RoundLimit)
      {
         $SK::ResetCount = 0;
         SK_NextTrack();
      }
      else
      {
         messageAll('', "\c5Beginning round " @ $SK::ResetCount @ " of " @ $pref::Server::SpeedKart::RoundLimit);
         Parent::Reset(%obj, %client);
      }
   }  
};
activatePackage(GameModeSpeedKartPackage);

// more time to fly out of a burning car
$CorpseTimeoutValue = 7000;

//special event to explode vehicles that are left in the garage
registerOutputEvent("fxDTSBrick", "explodeNearVehicle", "");
function fxDTSBrick::explodeNearVehicle(%obj)
{
   %vehicle = %obj.vehicle;
   if(!isObject(%vehicle))
      return;

   %delta = vectorSub(%vehicle.getPosition(), %obj.getPosition());
   //echo("len = " @ vectorLen(%delta));
   if(vectorLen(%delta) < 5) //7.5)
      %vehicle.finalExplosion(); //damage(%vehicle, %vehicle.getPosition(), 10000, $DamageType::Default);
}


//special event to win the race, displays race time
registerOutputEvent("GameConnection", "winRace", "");
function GameConnection::winRace(%client)
{
   %mg = %client.miniGame;

   if(!isObject(%mg))
      %mg = $DefaultMiniGame;

   if(!isObject(%mg))
      return;

   //if race start time is not available, use time since last reset
   %startTime = %mg.raceStartTime;
   if(%startTime <= 0)
      %startTime = %mg.lastResetTime;

   %elapsedTime = getSimTime() - %startTime;
   %elapsedTime = mFloor(%elapsedTime / 1000);
   
   %mg.chatMessageAll(0,  "\c3" @ %client.getPlayerName() @ " \c5WON THE RACE IN \c6" @ getTimeString(%elapsedTime) @ "\c3!");
   %mg.scheduleReset(7000);
}  


//load the actual karts
// these are a feature locked version of the karts from a while ago
// so we only allow them to be used in the actual game mode
// if you want to use the speedkarts 
if($GameModeArg $= "Add-Ons/GameMode_SpeedKart/gamemode.txt")
{
   exec("./karts/speedKart.cs");
}