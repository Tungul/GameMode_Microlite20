// Script_Dice from M, BL_ID 4332, used and modified without permission.
// http://forum.blockland.us/index.php?topic=189605.0

$Dice::MaxCount = 30;
$Dice::MinSides = 3;
$Dice::MaxSides = 20; // You'll never go above a 20 in Microlite.
$Dice::MaxShownCount = 7;
$Dice::HardErrors = 1;

function parseDiceFormat(%str)
{
	deleteVariables("$DiceTemp::*");
	%pos = stripos(%str,"d");
	if(%pos $= -1)
	{
		return 0 TAB "\c6You must enter input in the format \c2count\c6d\c2sides\c7[\xB1offset] [comparison value]";
	}
	%count = trim(getsubstr(%str,0,%pos));
	%str = trim(getsubstr(%str,%pos+1,strlen(%str)));
	%pos = striposAny(%str,"<= >= != == = < >");
	if(%pos $= "-1")
	{
		%sb = %str;
	} else {
		%f = getField(%pos,0);
		%comp = getWord(%f,1);
		%pos = getWord(%f,0);
		%sb = trim(getsubstr(%str,0,%pos));
		%compVal = trim(getsubstr(%str,%pos + strlen(%comp),strlen(%str)));
	}
	%pos = striposAny(%sb,"- +");
	if(%pos $= "-1")
	{
		%sides = %sb;
	} else {
		%f = getField(%pos,0);
		%offsetType = getWord(%pos,1);
		%pos = getWord(%pos,0);
		%sides = trim(getsubstr(%sb,0,%pos));
		%offset = trim(getsubstr(%sb,%pos+1,strlen(%sb)));
	}
	if(!isInt(%count) || !isInt(%sides) || %count <= 0 || %sides <= 0 || (%offset !$= "" && !isInt(%offset)) || (%compVal !$= "" && !isInt(%compVal)))
	{
		return 0 TAB "\c6You must enter input in the format \c2count\c6d\c2sides\c7[\xB1offset] [comparison value]";
	} else {
		if($Dice::HardErrors)
		{
			if(%count < 1 || %count > $Dice::MaxCount)
			{
				return 0 TAB "\c6You can only roll between \c21\c6 and \c2" @ $Dice::MaxCount SPC "\c6dice.";
			}
			if(%sides < $Dice::MinSides || %sides > $Dice::MaxSides)
			{
				return 0 TAB "\c6You can only roll dice with between \c2" @ $Dice::MinSides SPC "\c6and\c2" SPC $Dice::MaxSides SPC "\c6sides.";
			}
		} else {
			%count = mClampF(%count,1,$Dice::MaxCount);
			%sides = mClampF(%sides,$Dice::MinSides,$Dice::MaxSides);
		}
		%total = 0;
		if(%count > 1)
		{
			for(%i=0;%i<%count;%i++)
			{
				%r = getRandom(1,%sides);
				%total += %r;
				if(%comp $= "")
				{
					%rolls = %rolls @ (%i < %count - 1 ? (%i != 0 ? ", " : "") : " and ") @ %r;
				}
			}
		} else {
			%total = getRandom(1,%sides);
			%rolls = "\c3" @ %total @ "\c6";
		}
		%str = "rolled" SPC %count @ "d" @ %sides;
		$DiceTemp::Count = %count;
		$DiceTemp::Sides = %sides;
		$DiceTemp::RawTotal = %total;
		if(%offsetType !$= "")
		{
			%str = %str @ %offsetType @ %offset;
			if(%offsetType $= "-")
			{
				%total -= %offset;
			} else {
				%total += %offset;
			}
		}
		$DiceTemp::Total = %total;
		if((%comp $= "" || (%count == 1 && %offsetType $= "")) && %count <= $Dice::MaxShownCount)
		{
			%str = %str @ ", rolling" SPC %rolls;
		}
		if((%offsetType !$= "" && %count == 1) || %count != 1)
		{
			%str = %str SPC "for a total of\c3" SPC %total @ "\c6";
		}
		if(%comp !$= "")
		{
			switch$(%comp)
			{
				case ">":
					%str = %str @ ", target over" SPC %compVal @ ":";
					if(%total > %compVal)
					{
						$DiceTemp::Result = 1;
						%str = %str SPC "\c2Success!";
					} else {
						$DiceTemp::Result = 0;
						%str = %str SPC "\c0Failure...";
					}
				case "<":
					%str = %str @ ", target under" SPC %compVal @ ":";
					if(%total < %compVal)
					{
						$DiceTemp::Result = 1;
						%str = %str SPC "\c2Success!";
					} else {
						$DiceTemp::Result = 0;
						%str = %str SPC "\c0Failure...";
					}
				case ">=":
					%str = %str @ ", target over or equal to" SPC %compVal @ ":";
					if(%total >= %compVal)
					{
						$DiceTemp::Result = 1;
						%str = %str SPC "\c2Success!";
					} else {
						$DiceTemp::Result = 0;
						%str = %str SPC "\c0Failure...";
					}
				case "<=":
					%str = %str @ ", target under or equal to" SPC %compVal @ ":";
					if(%total <= %compVal)
					{
						$DiceTemp::Result = 1;
						%str = %str SPC "\c2Success!";
					} else {
						$DiceTemp::Result = 0;
						%str = %str SPC "\c0Failure...";
					}
				case "!=":
					%str = %str @ ", target not equal to" SPC %compVal @ ":";
					if(%total != %compVal)
					{
						$DiceTemp::Result = 1;
						%str = %str SPC "\c2Success!";
					} else {
						$DiceTemp::Result = 0;
						%str = %str SPC "\c0Failure...";
					}
				default:
					%str = %str @ ", target equal to" SPC %compVal @ ":";
					if(%total == %compVal)
					{
						$DiceTemp::Result = 1;
						%str = %str SPC "\c2Success!";
					} else {
						$DiceTemp::Result = 0;
						%str = %str SPC "\c0Failure...";
					}
			}
		} else {
			$DiceTemp::Result = -1;
			%str = %str @ ".";
		}
		return 1 TAB "\c6" @ %str;
	}
}
function isInt(%i)
{
	return mFloor(%i) $= %i;
}
function striposAny(%str,%possibles,%offset)
{
	%possibles = strReplace(strReplace(strReplace(%possibles,"\t"," "),"\n"," "),"\r","");
	for(%i=0;%i<getWordCount(%possibles);%i++)
	{
		%x = getWord(%possibles,%i);
		if((%j = stripos(%str,%x,%offset)) != -1)
		{
			%result = %result TAB %j SPC %x;
		}
	}
	if(%result !$= "")
	{
		return trim(%result);
	} else {
		return -1;
	}
}


function Microlite::rollDice(%this, %client, %str)
{
	%result = parseDiceFormat(%str);
	%msg = getField(%result,1);
	%result = getField(%result,0);
	if(%result)
	{
		messageAll('',"\c3" @ %client.getPlayerName() SPC %msg);
		// messageClient(%client,'',"\c6You" SPC %msg);
	} else {
		messageClient(%client,'',%msg);
	}
}
function Microlite::DMRoll(%this, %client, %str) //proll was a good name - privateroll - but only the DM is supposed to use it.
{
	%result = parseDiceFormat(%str);
	%msg = getField(%result,1);
	%result = getField(%result,0);
	if(%result)
	{
		messageClient(%client,'',"\c6You" SPC %msg);
	} else {
		messageClient(%client,'',%msg);
	}
}

// if(isFile("Add-Ons/Event_Variables.zip") && $AddOn__Event_Variables == 1) // unneccesary, but VCE might (read: probably won't) be implemented at a later date
// {
// 	forceRequiredAddOn("Event_Variables");
// 	registerOutputEvent("fxDTSbrick","rollDice","string 64 120\tstring 8 30",1);
// 	registerSpecialVar("fxDTSbrick","dtotal","%this.lastRollTotal");
// 	registerSpecialVar("fxDTSbrick","drawtotal","%this.lastRollRawTotal");
// 	registerSpecialVar("fxDTSbrick","dcount","%this.lastRollCount");
// 	registerSpecialVar("fxDTSbrick","dsides","%this.lastRollSides");
// }
// function fxDTSbrick::rollDice(%this,%str,%subdata,%client)
// {
// 	%str = filterVariableString(%str,%this,%client,%client.player);
// 	%result = parseDiceFormat(%str);
// 	%msg = getField(%result,1);
// 	%result = getField(%result,0);
// 	if(%result)
// 	{
// 		%this.lastRollTotal = $DiceTemp::Total;
// 		%this.lastRollRawTotal = $DiceTemp::RawTotal;
// 		%this.lastRollCount = $DiceTemp::Count;
// 		%this.lastRollSides = $DiceTemp::Sides;
// 		if($DiceTemp::Result != -1)
// 		{
// 			if(getWordCount(%subdata) != 2)
// 			{
// 				if($DiceTemp::Result == 1)
// 					%this.onVariableTrue(%client);
// 				else
// 					%this.onVariableFalse(%client);
// 				return;
// 			}
// 			%substart = getWord(%subdata,0);
// 			%subend = getWord(%subdata,1);
// 			if(!isInt(%substart) && isInt(%subend))
// 			{
// 				switch$(%substart)
// 				{
// 					case "<":
// 						%substart = 0;
// 						%subend = %subend - 1;
// 					case "<=":
// 						%substart = 0;
// 					case ">":
// 						%substart = %subend + 1;
// 						%subend = %this.numEvents;
// 					case ">=":
// 						%substart = %subend;
// 						%subend = %this.numEvents;
// 					case "==":
// 						%substart = %subend;
// 					case "~=":
// 						%sublike = %subend;
// 					case "!=":
// 						%subignore = %subend;
// 					default:
// 						%substart = 0;
// 						%subend = %this.numEvents;
// 				}
// 			}
// 			%substart = %substart < -1 ? 0 : %substart;
// 			%subend = %subend > %this.numEvents ? %this.numEvents : %subend;
// 			for(%i=0;%i<%this.numEvents;%i++)
// 			{
// 				if(((%i < %substart || %i > %subend) && %sublike $= "" && %subignore $= "") || (strPos(%i,%sublike) == -1 && %sublike !$= "") || (%subignore == %i && %subignore !$= ""))
// 				{
// 					if(%this.eventInput[%i] $= "onVariableTrue" || %this.eventInput[%i] $= "onVariableFalse")
// 					{
// 						%oldEnabled[%i] = %this.eventEnabled[%i];
// 						%this.eventEnabled[%i] = 0;
// 					}
// 				}
// 			}
// 			if($DiceTemp::Result == 1)
// 				%this.onVariableTrue(%client);
// 			else
// 				%this.onVariableFalse(%client);
// 			for(%i=0;%i<%this.numEvents;%i++)
// 			{
// 				if(%oldEnabled[%i] !$= "")
// 					%this.eventEnabled[%i] = %oldEnabled[%i];
// 			}
// 		}
// 	} else {
// 		%client.centerPrint(%msg,4);
// 	}
// }