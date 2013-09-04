//+===================+
//| Block Board Chase |
//|      By: ZSNO     |
//|    and Saxophone  |
//|  Idea from Kirby  |
//|  64, Multiplayer  |
//+===================+

$gamemode::ringFall1 = 30;
$gamemode::ringFall2 = 45;
$gamemode::ringFall3 = 55;
$BBC::Normal=getClosestPaintColor("0.5 0.5 0.5 1");
$BBC::Dark=getClosestPaintColor("0.25 0.25 0.25 1");

exec( "./weapon.cs" );
exec( "./player.cs" );

if($BBC::Normal == $BBC::Dark)
{
	$BBC::Dark = getclosestpaintcolor("0 0 0 1");
}
function getClosestPaintColor(%rgba)
{
	%prevdist = 100000;
	%colorMatch = 0;
	for(%i = 0;%i < 64;%i++)
	{
		%color = getColorIDTable(%i);
		if(vectorDist(%rgba,getWords(%color,0,2)) < %prevdist && getWord(%rgba,3) - getWord(%color,3) < 0.3 && getWord(%rgba,3) - getWord(%color,3) > -0.3)
		{
			%prevdist = vectorDist(%rgba,%color);
			%colormatch = %i;
		}
	}
	return %colormatch;
}
function fxDTSBrick::BBCrelay(%this,%bc,%num)
{
	%this.setcolor(%bc);
	%this.schedule(500,fakeKillbrick,"0 0 0",3);
	%this.schedule(2000,setcolor,$BBC::Normal);
	%pos = %this.getPosition();
	if(%num == 1 || %num == 2)
	{
		if(%num == 1)
			%position = getword(%pos,0) + 2 SPC getwords(%pos,1,2);
		else
			%position = getword(%pos,0) - 2 SPC getwords(%pos,1,2);
	}
	else
	{
		if(%num == 3)
			%position = getword(%pos,0) SPC getword(%pos,1) + 2 SPC getword(%pos,2);
		else
			%position = getword(%pos,0) SPC getword(%pos,1) - 2 SPC getword(%pos,2);
	}
	%boxSize = "0.25 0.25 0.25";
	%mask = ($TypeMasks::FxBrickObjectType);
	initContainerBoxSearch(%position,%boxSize,%mask);
	while(%brick = containerSearchNext())
	{
		if(%brick == %this)
			return;
		else
			%brick.BBCrelay(%bc,%num);
	}
}
package BBC
{
	function MinigameSO::reset(%this,%a)
	{
		%g = ClientGroup.getCount();
		for(%q=0;%q<%g;%q++)
		{
			%client = ClientGroup.getObject(%q);
			%client.isDead = 0;
		}
		%this.aliveCount = %this.numMembers;
		cancel($BBCRing1);
		cancel($BBCRing2);
		cancel($BBCRing3);
		%brick=BrickGroup_888888;
		for(%d=1;%d<4;%d++)
		{
			if(%brick.NTObjectCount_BBC_Ring[%d])
			{
				for(%i=0;%i<%brick.NTObjectCount_BBC_Ring[%d];%i++)
				{
					%p = %d @ "_" @ %i;
					%br = %brick.NTObject_BBC_Ring[%p];
					%br.respawn();
					%br.setcolor($BBC::Normal);
					%br.setraycasting(1);
				}
			}
		}
		$BBCRing1 = schedule($gamemode::ringFall1 * 1000,0,BBCringfalloff,1);
		$BBCRing2 = schedule($gamemode::ringFall2 * 1000,0,BBCringfalloff,2);
		$BBCRing3 = schedule($gamemode::ringFall3 * 1000,0,BBCringfalloff,3);
		parent::reset(%this,%a);
	}
	function gameConnection::spawnPlayer(%client)
	{
		if(%client.isDead)
		{
			%r = parent::spawnPlayer(%client);
			messageClient(%client,'',"\c6 You're dead, please wait until the round ends.");
			%client.player.delete();
			%client.setControlObject(%client.camera);
			return %r;
		}
		else
		{
			return parent::spawnPlayer(%client);
		}
	}
	function GameConnection::onDeath(%this, %obj, %sourceObject, %sourceClient, %damageType, %damLoc)
	{
		%r = parent::onDeath(%this, %obj, %sourceObject, %sourceClient, %damageType, %damLoc);
		%this.isDead = 1;
		%this.minigame.aliveCount -= 1;
		if(%this.minigame.aliveCount <= 1)
		{
			%g = ClientGroup.getCount();
			for(%i=0;%i<%g;%i++)
			{
				%client = ClientGroup.getObject(%i);
				if(!%client.isDead)
					%sc = %client;
			}
			if(!%sc)
			{
				for(%i=0;%i<%g;%i++)
					%sc = ClientGroup.getObject(%i);
			}
			announce("\c3" SPC %sc.name @ " \c6has won the round! Resetting...");
			$defaultMinigame.schedule(3000,reset,0);
		}
		return %r;
	}
	function MinigameSO::addMember(%this,%c)
	{
		parent::addMember(%this,%c);
		%c.isDead = 1;
		if(%this.numMembers == 1)
			%this.reset(0);
	}
	function MinigameSO::removeMember(%this,%c)
	{
		if(isObject(%c.player))
		{
			%c.delete();
		}
		parent::removeMember(%this,%c);
		if(!%c.isDead)
			%this.aliveCount--;
		if(%this.aliveCount < 2 && %this.numMembers > 1)
		{
			announce("\c6The other person left, restarting round...");
			$defaultMinigame.schedule(3000,reset,0);
		}
	}
};
activatePackage(BBC);
function BBCringfalloff(%num)
{
	%brick=BrickGroup_888888;
	if(%brick.NTObjectCount_BBC_Ring[%num])
	{
		for(%i=0;%i<%brick.NTObjectCount_BBC_Ring[%num];%i++)
		{
			%p = %num @ "_" @ %i;
			%br = %brick.NTObject_BBC_Ring[%p];
			%br.setcolor($BBC::Dark);
			%br.setraycasting(0);
			%br.schedule(1000,fakekillbrick,"0 0 0",-1);
		}
	}
}