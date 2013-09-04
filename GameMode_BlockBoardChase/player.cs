datablock PlayerData(PlayerBBC : PlayerStandardArmor)
{
	jumpforce=0;
	canjet = 0;
	uiname = "BBCPlayer";
};
function BBCPickProjectile::oncollision(%this,%obj,%col,%fade,%pos,%normal)
{
	if(%col.getclassname()$="fxDTSBrick" && %col.Datablock $="brick4xCubeData")
	{
		%bc=getClosestPaintColor(%obj.client.chestColor);
		%pp=%obj.client.player.getforwardvector();
		if(mABS(getword(%pp,0)) > mABS(getword(%pp,1)))
		{
			if(stricmp(getword(%pp,0),mABS(getword(%pp,0))) == 0)
				%col.BBCrelay(%bc,1);
			else
				%col.BBCrelay(%bc,2);
		}
		else
		{
			if(stricmp(getword(%pp,1),mABS(getword(%pp,1))) == 0)
				%col.BBCrelay(%bc,3);
			else
				%col.BBCrelay(%bc,4);
		}
	}
}