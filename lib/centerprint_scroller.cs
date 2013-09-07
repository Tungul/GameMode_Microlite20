new SimObject("CenterprintTextScroller");


package CenterprintTextScroller {
	function serverCmdShiftBrick(%client, %x, %y, %z) {
		if(%client.inScrollableListMode) {
			switch(%z) {
				case 3:
					%client.scroller.pageUp();
				case 1:
					%client.scroller.lineUp();
				case -1:
					%client.scroller.lineDown();
				case -3:
					%client.scroller.pageDown();
				default:
					messageClient(%client, '', "\c5You're supposed to use the up and down brick shift keys.");
			}

		}
		else
			return serverCmdShiftBrick(%client, %x, %y, %z);
	}
	function serverCmdPlantBrick(%client) {
		if(%client.inScrollableListMode) {
			%client.scroller.printLoop(0);
		}
		else
			return serverCmdPlantBrick(%client);
	}
	function serverCmdClearGhostBrick(%client) {
		if(%client.inScrollableListMode) {
			messageClient(%client, '', "\c5To scroll by line, use your shift brick up/down 1/3 keys.");
			messageClient(%client, '', "\c5To scroll by page, use your normal shift brick up/down keys.");
			messageClient(%client, '', "\c5To exit, use your plant brick key.");
			
		}
	}
};
activatePackage("CenterprintTextScroller");

function CenterprintTextScroller::beginPrint(%this, %client, %data, %linesShown) {
	%client.scroller = new scriptObject()
	{
		class = "ScrollerObject";
		headLine = 0;
		linesShown = %linesShown;
	};

	if(%data != strReplace(%data, "\t", "")) // can't handle \t within text yet, sorry. currently using a cheapass method.
		return 0;

	%data = strReplace(strReplace(%data, "\n", "\t"), "\r", "");
	
	%client.scroller.lineCount = getFieldCount();

	for(%i = 0; %i < getFieldCount(%data); %i++) {
		%client.scroller.lines[i] = getField(%data, i) @ "<br>";
	}

	%client.scroller.printLoop(1);
}

function ScrollerObject::printLoop(%this, %on) {
	if(isObject(%this.printLoopSchedule))
		cancel(%this.printLoopSchedule);

	if(!%on) {
		%this.client.inScrollableListMode = 0;
		return;
	}
	%this.client.inScrollableListMode = 1;

	%this.printLoopSchedule = %this.schedule(250, printLoop, 1);

	%data = "Push your clear ghost brick key for help.";
	for(%i = %this.headLine; %i < %this.linesShown; %i++) {
		%data = %data @ %this.lines[i];
	}
	%this.client.centerPrint(%data);
}

function ScrollerObject::pageUp(%this) {
	%this.headLine -= %this.lineShown;
	if(%this.headLine < 0) {
		%this.headLine = 0;
		return;
	}
}
function ScrollerObject::lineUp(%this) {
	%this.headLine -= 1;
	if(%this.headLine < 0) {
		%this.headLine = 0;
		return;
	}
}
function ScrollerObject::lineDown(%this) {
	%this.headLine += 1;
	if(%this.headLine + %this.lineShown > %this.lineCount) {
		%this.headLine = %this.headLine - %this.lineShown;
		return;
	}
}
function ScrollerObject::pageDown(%this) {
	%this.headLine += %this.lineShown;
	if(%this.headLine + %this.lineShown > %this.lineCount) {
		%this.headLine = %this.headLine - %this.lineShown;
		return;
	}
}

function serverCmdTestCenterprintScroller(%client) {
	CenterprintTextScroller.beginPrint(%client, "test1\ntest2\ntest3\ntest4\ntest5\ntest6\ntest7\ntest8\nend", 4);
}