datablock ProjectileData(BBCPickProjectile)
{
   directDamage        = 0;
   radiusDamage        = 0;
   damageRadius        = 0;
   explosion           = hammerExplosion;
   muzzleVelocity      = 50;
   velInheritFactor    = 1;
   armingDelay         = 0;
   lifetime            = 150;
   fadeDelay           = 70;
   bounceElasticity    = 0;
   bounceFriction      = 0;
   isBallistic         = false;
   gravityMod = 0.0;
   hasLight    = false;
   lightRadius = 3.0;
   lightColor  = "0 0 0.5";
};

datablock ItemData(BBCPickItem)
{
	category = "Weapon";
	className = "Weapon";
	shapeFile = "base/data/shapes/hammer.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;
	uiName = "BBCPick";
	iconName = "base/client/ui/ItemIcons/hammer";
	doColorShift = true;
	colorShiftColor = "0.5 0.5 0.5 1";
	image = BBCPickImage;
};
datablock ShapeBaseImageData(BBCPickImage)
{
   shapeFile = "base/data/shapes/hammer.dts";
   emap = true;
   mountPoint = 0;
   offset = "0 0 0"; 
   correctMuzzleVector = false;
   eyeOffset = "0.7 1.2 -0.25";
   className = "WeaponImage";
   item = BBCPickItem;
   ammo = " ";
   projectile = BBCPickProjectile;
   projectileType = Projectile;
   melee = true;
   doRetraction = false;
   armReady = true;
   doColorShift = true;
   colorShiftColor = "0.5 0.5 0.5 1";
	stateName[0]                     = "Activate";
	stateTimeoutValue[0]             = 1;
	stateTransitionOnTimeout[0]       = "Ready";

	stateName[1]                     = "Ready";
	stateTransitionOnTriggerDown[1]  = "Fire";
	stateAllowImageChange[1]         = true;

	stateName[3]                    = "Fire";
	stateTransitionOnTimeout[3]     = "StopFire";
	stateTimeoutValue[3]            = 0.2;
	stateFire[3]                    = true;
	stateAllowImageChange[3]        = false;
	stateSequence[3]                = "Fire";
	stateScript[3]                  = "onFire";
	stateWaitForTimeout[3]		= true;

	stateName[5]                    = "StopFire";
	stateTransitionOnTimeout[5]     = "Ready";
	stateTimeoutValue[5]            = 3;
	stateAllowImageChange[5]        = false;
	stateWaitForTimeout[5]		= true;
	stateSequence[5]                = "StopFire";
	stateScript[5]                 = "onStopFire";
};
function BBCPickImage::onFire(%this, %obj, %slot)
{
	parent::onFire(%this,%obj,%slot);
	%obj.playthread(2, armattack);
}
function BBCPickImage::onStopFire(%this, %obj, %slot)
{	
	%obj.playthread(2, root);
}