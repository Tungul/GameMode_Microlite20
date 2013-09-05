new SimObject("CenterprintTextScroller");

package CenterprintTextScroller {
	function serverCmdShiftBrick(%client, %x, %y, %z) {
		if(%client.inScrollableListMode) {
			switch(%z) {
				case 3:
					%client.ScrollerObject.pageUp();
				case 1:
					%client.ScrollerObject.lineUp();
				case -1:
					%client.ScrollerObject.lineDown();
				case -3:
					%client.ScrollerObject.pageDown();
				default:
					messageClient("\c5You're supposed to use the up and down brick shift keys.")
			}

		}
		else
			return serverCmdShiftBrick(%client, %x, %y, %z);
	}
}

function ScrollerObject::pageUp(%this) {
	%client = %this.client;
	%head = %this.headLine;
	%shown = %this.linesShown;
	%head - %shown;
	if(%head - %shown < 0)
	{
		%head = 0;
		return;
	}
}
function ScrollerObject::lineUp(%this) {
	%client = %this.client;
}
function ScrollerObject::lineDown(%this) {
	%client = %this.client;
}
function ScrollerObject::pageDown(%this) {
	%client = %this.client;
}