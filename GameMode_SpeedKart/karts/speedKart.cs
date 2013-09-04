//we need the Jeep add-on for this, so force it to load
%error = ForceRequiredAddOn("Vehicle_Jeep");

if(%error == $Error::AddOn_Disabled)
{
   //A bit of a hack:
   //  we just forced the -- to load, but the user had it disabled
   //  so lets make it so they can't select it
   JeepVehicle.uiName = "";
}

if(%error == $Error::AddOn_NotFound)
{
   //we don't have the --, so we're screwed
   error("ERROR: Vehicle_SpeedKart - required add-on Vehicle_Jeep not found");
   return;
}



// SpeedKart_spring.cs

datablock WheeledVehicleSpring(SpeedKartSpring)
{
   // Wheel suspension properties
   length = 0.35;			 // Suspension travel
   force = 4000; //3000;		 // Spring force
   damping = 900; //600;		 // Spring damping
   antiSwayForce = 3; //3;		 // Lateral anti-sway force
};





// SpeedKart_tire.cs

datablock WheeledVehicleTire(SpeedKartTire)
{
   // Tires act as springs and generate lateral and longitudinal
   // forces to move the vehicle. These distortion/spring forces
   // are what convert wheel angular velocity into forces that
   // act on the rigid body.
   shapeFile = "./SpeedKarttire.dts";
   
	
	mass = 10;
   radius = 1;
   staticFriction = 5;
   kineticFriction = 5;
   restitution = 0.5;	

   // Spring that generates lateral tire forces
   lateralForce = 6000;
   lateralDamping = 500;
   lateralRelaxation = 0.3;

   // Spring that generates longitudinal tire forces
   longitudinalForce = 5000;
   longitudinalDamping = 2000;
   longitudinalRelaxation = 0.1;
};

datablock WheeledVehicleTire(SpeedkartBlockoTire : SpeedkartTire) //"SpeedkartWheelTire" does not exist anywhere in the script. You should have referenced "SpeedKartTire" ~Barna
{
        shapeFile = "./SpeedkartBlockowheel.dts";
};




// Vehicle //
/////////////
datablock WheeledVehicleData(SpeedKartVehicle)
{
	category = "Vehicles";
	displayName = " ";
	shapeFile = "./SpeedKart.dts"; //"~/data/shapes/skivehicle.dts"; //
	emap = true;
	minMountDist = 3;
   
   numMountPoints = 1;
   mountThread[0] = "sit";


	maxDamage = 200.00;
	destroyedLevel = 200.00;
	energyPerDamagePoint = 160;
	speedDamageScale = 1.04;
	collDamageThresholdVel = 20.0;
	collDamageMultiplier   = 0.02;

	massCenter = "0 -0.3 0.3";
   //massBox = "2 5 1";

	maxSteeringAngle = 0.6;  // Maximum steering angle, should match animation
	integration = 4;           // Force integration time: TickSec/Rate
	tireEmitter = VehicleTireEmitter; // All the tires use the same dust emitter

	// 3rd person camera settings
	cameraRoll = false;         // Roll the camera with the vehicle
	cameraMaxDist = 6;         // Far distance from vehicle
	cameraOffset = 4;        // Vertical offset from camera mount point
	cameraLag = 0.0;           // Velocity lag of camera
	cameraDecay = 0.75;        // Decay per sec. rate of velocity lag
	cameraTilt = 0.3;
   collisionTol = 0.1;        // Collision distance tolerance
   contactTol = 0.1;

	useEyePoint = false;	

	defaultTire	= SpeedKartTire;
	defaultSpring	= SpeedKartSpring;
	//flatTire	= SpeedKartFlatTire;
	//flatSpring	= SpeedKartFlatSpring;

   numWheels = 4;

	// Rigid Body
	mass = 200;
	density = 5.0;
	drag = 4.5;
	bodyFriction = 0.6;
	bodyRestitution = 0.6;
	minImpactSpeed = 10;        // Impacts over this invoke the script callback
	softImpactSpeed = 10;       // Play SoftImpact Sound
	hardImpactSpeed = 15;      // Play HardImpact Sound
	groundImpactMinSpeed    = 10.0;

	// Engine
	engineTorque = 3200; //4000;       // Engine power
	engineBrake = 700;         // Braking when throttle is 0
	brakeTorque = 3200;        // When brakes are applied
	maxWheelSpeed = 45;        // Engine scale by current speed / max speed

	rollForce		= 900;
	yawForce		= 600;
	pitchForce		= 1000;
	rotationalDrag		= 0;

   // Advanced Steering
   steeringAutoReturn = true;
   steeringAutoReturnRate = 1;
   steeringAutoReturnMaxSpeed = 10;
   steeringUseStrafeSteering = true;
   steeringStrafeSteeringRate = 0.08;

	// Energy
	maxEnergy = 100;
	jetForce = 3000;
	minJetEnergy = 30;
	jetEnergyDrain = 2;

	splash = vehicleSplash;
	splashVelocity = 4.0;
	splashAngle = 67.0;
	splashFreqMod = 300.0;
	splashVelEpsilon = 0.60;
	bubbleEmitTime = 1.4;
	splashEmitter[0] = vehicleFoamDropletsEmitter;
	splashEmitter[1] = vehicleFoamEmitter;
	splashEmitter[2] = vehicleBubbleEmitter;
	mediumSplashSoundVelocity = 10.0;   
	hardSplashSoundVelocity = 20.0;   
	exitSplashSoundVelocity = 5.0;
		
	//mediumSplashSound = "";
	//hardSplashSound = "";
	//exitSplashSound = "";
	
	// Sounds
	//   jetSound = ScoutThrustSound;
	//engineSound = idleSound;
	//squealSound = skidSound;
	softImpactSound = slowImpactSound;
	hardImpactSound = fastImpactSound;
	//wheelImpactSound = slowImpactSound;

	//   explosion = VehicleExplosion;
	justcollided = 0;

   uiName = "SpeedKart";
	rideable = true;
		lookUpLimit = 0.65;
		lookDownLimit = 0.45;

	paintable = true;
   
   damageEmitter[0] = VehicleBurnEmitter;
	damageEmitterOffset[0] = "0.0 0.0 0.0 ";
	damageLevelTolerance[0] = 0.99;

   damageEmitter[1] = VehicleBurnEmitter;
	damageEmitterOffset[1] = "0.0 0.0 0.0 ";
	damageLevelTolerance[1] = 1.0;

   numDmgEmitterAreas = 1;

  

   burnTime = 4000;

   

   minRunOverSpeed    = 4;   //how fast you need to be going to run someone over (do damage)
   runOverDamageScale = 8;   //when you run over someone, speed * runoverdamagescale = damage amt
   runOverPushScale   = 1.2; //how hard a person you're running over gets pushed

   //protection for passengers
   protectPassengersBurn   = false;  //protect passengers from the burning effect of explosions?
   protectPassengersRadius = true;  //protect passengers from radius damage (explosions) ?
   protectPassengersDirect = false; //protect passengers from direct damage (bullets) ?
};


package HugPackage
{
   function armor::onMount(%this,%obj,%col,%slot)
   {
      Parent::onMount(%this,%obj,%col,%slot);
      if(%col.dataBlock $= SpeedKartVehicle)
         %obj.playThread(2, armReadyBoth);
      Parent::onMount(%this,%obj,%col,%slot);
      if(%col.dataBlock $= SpeedKartmuscleVehicle)
         %obj.playThread(2, armReadyBoth);
      Parent::onMount(%this,%obj,%col,%slot);
      if(%col.dataBlock $= SpeedKartformulaVehicle)
         %obj.playThread(2, armReadyBoth);
      Parent::onMount(%this,%obj,%col,%slot);
      if(%col.dataBlock $= SpeedKarthotrodVehicle)
         %obj.playThread(2, armReadyBoth);
      Parent::onMount(%this,%obj,%col,%slot);
      if(%col.dataBlock $= SpeedKartvintageVehicle)
         %obj.playThread(2, armReadyBoth);
      Parent::onMount(%this,%obj,%col,%slot);
      if(%col.dataBlock $= SpeedKartbuggyVehicle)
         %obj.playThread(2, armReadyBoth);
      Parent::onMount(%this,%obj,%col,%slot);
      if(%col.dataBlock $= SpeedKartclassicVehicle)
         %obj.playThread(2, armReadyBoth);
      Parent::onMount(%this,%obj,%col,%slot);
      if(%col.dataBlock $= SpeedKartclassicgtVehicle)
         %obj.playThread(2, armReadyBoth);
      Parent::onMount(%this,%obj,%col,%slot);
      if(%col.dataBlock $= SpeedKartjeepVehicle)
         %obj.playThread(2, armReadyBoth);
      Parent::onMount(%this,%obj,%col,%slot);
      if(%col.dataBlock $= SpeedKarthyperionVehicle)
         %obj.playThread(2, armReadyBoth);
      Parent::onMount(%this,%obj,%col,%slot);
      if(%col.dataBlock $= SpeedKart64Vehicle)
         %obj.playThread(2, armReadyBoth);
      Parent::onMount(%this,%obj,%col,%slot);
      if(%col.dataBlock $= SpeedKart7Vehicle)
         %obj.playThread(2, armReadyBoth);
      Parent::onMount(%this,%obj,%col,%slot);
      if(%col.dataBlock $= SpeedKartBlockoVehicle)
         %obj.playThread(2, armReadyBoth);
      Parent::onMount(%this,%obj,%col,%slot);
      if(%col.dataBlock $= SpeedKartLeMansVehicle)
         %obj.playThread(2, armReadyBoth);
   }

   function armor::onUnMount(%this,%obj,%col,%slot)
   {
      Parent::onUnMount(%this,%obj,%col,%slot);
      if(%col.dataBlock $= SpeedKartVehicle)
         %obj.playThread(2, root);
         %obj.playThread(0, root);
      Parent::onUnMount(%this,%obj,%col,%slot);
      if(%col.dataBlock $= SpeedKartmuscleVehicle)
         %obj.playThread(2, root);
         %obj.playThread(0, root);
      Parent::onUnMount(%this,%obj,%col,%slot);
      if(%col.dataBlock $= SpeedKartformulaVehicle)
         %obj.playThread(2, root);
         %obj.playThread(0, root);
      Parent::onUnMount(%this,%obj,%col,%slot);
      if(%col.dataBlock $= SpeedKarthotrodVehicle)
         %obj.playThread(2, root);
         %obj.playThread(0, root);
      Parent::onUnMount(%this,%obj,%col,%slot);
      if(%col.dataBlock $= SpeedKartvintageVehicle)
         %obj.playThread(2, root);
         %obj.playThread(0, root);
      Parent::onUnMount(%this,%obj,%col,%slot);
      if(%col.dataBlock $= SpeedKartbuggyVehicle)
         %obj.playThread(2, root);
         %obj.playThread(0, root);
      Parent::onUnMount(%this,%obj,%col,%slot);
      if(%col.dataBlock $= SpeedKartclassicVehicle)
         %obj.playThread(2, root);
         %obj.playThread(0, root);
      Parent::onUnMount(%this,%obj,%col,%slot);
      if(%col.dataBlock $= SpeedKartclassicgtVehicle)
         %obj.playThread(2, root);
         %obj.playThread(0, root);
      Parent::onUnMount(%this,%obj,%col,%slot);
      if(%col.dataBlock $= SpeedKartjeepVehicle)
         %obj.playThread(2, root);
         %obj.playThread(0, root);
      Parent::onUnMount(%this,%obj,%col,%slot);
      if(%col.dataBlock $= SpeedKarthyperionVehicle)
         %obj.playThread(2, root);
         %obj.playThread(0, root);
      Parent::onUnMount(%this,%obj,%col,%slot);
      if(%col.dataBlock $= SpeedKart64Vehicle)
         %obj.playThread(2, root);
         %obj.playThread(0, root);
      Parent::onUnMount(%this,%obj,%col,%slot);
      if(%col.dataBlock $= SpeedKart7Vehicle)
         %obj.playThread(2, root);
         %obj.playThread(0, root);
      Parent::onUnMount(%this,%obj,%col,%slot);
      if(%col.dataBlock $= SpeedKartBlockoVehicle)
         %obj.playThread(2, root);
         %obj.playThread(0, root);
Parent::onUnMount(%this,%obj,%col,%slot);
      if(%col.dataBlock $= SpeedKartLeMansVehicle)
         %obj.playThread(2, root);
         %obj.playThread(0, root);
   }
};
activatepackage(HugPackage);


function SpeedKartvehicle::onadd(%this,%obj)
{ 
	parent::onadd(%this,%obj);
	   %obj.playthread(0,"propslow");
	
}

function SpeedKartmusclevehicle::onadd(%this,%obj)
{ 
	parent::onadd(%this,%obj);
	   %obj.playthread(0,"propslow");
	
}

datablock WheeledVehicleData(SpeedKartmuscleVehicle : SpeedKartVehicle)
{
	category = "Vehicles";
	//- Render -
	shapeFile = "./SpeedKartmuscle.dts";
	emap = true;
	//- Vehicle Data -
	uiName = "SpeedKart Muscle";
	finalExplosionProjectile = SpeedKartFinalExplosionProjectile;
};


function SpeedKartvehicle::onadd(%this,%obj)
{ 
	parent::onadd(%this,%obj);
	   %obj.playthread(0,"propslow");
	
}

function SpeedKartformulavehicle::onadd(%this,%obj)
{ 
	parent::onadd(%this,%obj);
	   %obj.playthread(0,"propslow");
	
}

datablock WheeledVehicleData(SpeedKartformulaVehicle : SpeedKartVehicle)
{
	category = "Vehicles";
	//- Render -
	shapeFile = "./SpeedKartformula.dts";
	emap = true;
	//- Vehicle Data -
	uiName = "SpeedKart Formula";
	finalExplosionProjectile = SpeedKartFinalExplosionProjectile;
};



function SpeedKartvehicle::onadd(%this,%obj)
{ 
	parent::onadd(%this,%obj);
	   %obj.playthread(0,"propslow");
	
}

function SpeedKarthotrodvehicle::onadd(%this,%obj)
{ 
	parent::onadd(%this,%obj);
	   %obj.playthread(0,"propslow");
	
}

datablock WheeledVehicleData(SpeedKarthotrodVehicle : SpeedKartVehicle)
{
	category = "Vehicles";
	//- Render -
	shapeFile = "./SpeedKarthotrod.dts";
	emap = true;
	//- Vehicle Data -
	uiName = "SpeedKart Hotrod";
	finalExplosionProjectile = SpeedKartFinalExplosionProjectile;
};


function SpeedKartvehicle::onadd(%this,%obj)
{ 
	parent::onadd(%this,%obj);
	   %obj.playthread(0,"propslow");
	
}

function SpeedKartVintagevehicle::onadd(%this,%obj)
{ 
	parent::onadd(%this,%obj);
	   %obj.playthread(0,"propslow");
	
}

datablock WheeledVehicleData(SpeedKartVintageVehicle : SpeedKartVehicle)
{
	category = "Vehicles";
	//- Render -
	shapeFile = "./SpeedKartVintage.dts";
	emap = true;
	//- Vehicle Data -
	uiName = "SpeedKart Vintage";
	finalExplosionProjectile = SpeedKartFinalExplosionProjectile;
};

function SpeedKartvehicle::onadd(%this,%obj)
{ 
	parent::onadd(%this,%obj);
	   %obj.playthread(0,"propslow");
	
}

function SpeedKartBuggyvehicle::onadd(%this,%obj)
{ 
	parent::onadd(%this,%obj);
	   %obj.playthread(0,"propslow");
	
}

datablock WheeledVehicleData(SpeedKartBuggyVehicle : SpeedKartVehicle)
{
	category = "Vehicles";
	//- Render -
	shapeFile = "./SpeedKartBuggy.dts";
	emap = true;
	//- Vehicle Data -
	uiName = "SpeedKart Buggy";
	finalExplosionProjectile = SpeedKartFinalExplosionProjectile;
};

function SpeedKartvehicle::onadd(%this,%obj)
{ 
	parent::onadd(%this,%obj);
	   %obj.playthread(0,"propslow");
	
}

function SpeedKartClassicvehicle::onadd(%this,%obj)
{ 
	parent::onadd(%this,%obj);
	   %obj.playthread(0,"propslow");
	
}

datablock WheeledVehicleData(SpeedKartClassicVehicle : SpeedKartVehicle)
{
	category = "Vehicles";
	//- Render -
	shapeFile = "./SpeedKartClassic.dts";
	emap = true;
	//- Vehicle Data -
	uiName = "SpeedKart Classic";
	finalExplosionProjectile = SpeedKartFinalExplosionProjectile;
};


function SpeedKartvehicle::onadd(%this,%obj)
{ 
	parent::onadd(%this,%obj);
	   %obj.playthread(0,"propslow");
	
}

function SpeedKartClassicGTvehicle::onadd(%this,%obj)
{ 
	parent::onadd(%this,%obj);
	   %obj.playthread(0,"propslow");
	
}

datablock WheeledVehicleData(SpeedKartClassicGTVehicle : SpeedKartVehicle)
{
	category = "Vehicles";
	//- Render -
	shapeFile = "./SpeedKartClassicGT.dts";
	emap = true;
	//- Vehicle Data -
	uiName = "SpeedKart Classic GT";
	finalExplosionProjectile = SpeedKartFinalExplosionProjectile;
};


function SpeedKartvehicle::onadd(%this,%obj)
{ 
	parent::onadd(%this,%obj);
	   %obj.playthread(0,"propslow");
	
}

function SpeedKartJeepvehicle::onadd(%this,%obj)
{ 
	parent::onadd(%this,%obj);
	   %obj.playthread(0,"propslow");
	
}

datablock WheeledVehicleData(SpeedKartJeepVehicle : SpeedKartVehicle)
{
	category = "Vehicles";
	//- Render -
	shapeFile = "./SpeedKartJeep.dts";
	emap = true;
	//- Vehicle Data -
	uiName = "SpeedKart Jeep";
	finalExplosionProjectile = SpeedKartFinalExplosionProjectile;
};


function SpeedKartvehicle::onadd(%this,%obj)
{ 
	parent::onadd(%this,%obj);
	   %obj.playthread(0,"propslow");
	
}

function SpeedKartHyperionvehicle::onadd(%this,%obj)
{ 
	parent::onadd(%this,%obj);
	   %obj.playthread(0,"propslow");
	
}

datablock WheeledVehicleData(SpeedKartHyperionVehicle : SpeedKartVehicle)
{
	category = "Vehicles";
	//- Render -
	shapeFile = "./SpeedKartHyperion.dts";
	emap = true;
	//- Vehicle Data -
	uiName = "SpeedKart Hyperion";
	finalExplosionProjectile = SpeedKartFinalExplosionProjectile;
};


function SpeedKartvehicle::onadd(%this,%obj)
{ 
	parent::onadd(%this,%obj);
	   %obj.playthread(0,"propslow");
	
}

function SpeedKart64vehicle::onadd(%this,%obj)
{ 
	parent::onadd(%this,%obj);
	   %obj.playthread(0,"propslow");
	
}

datablock WheeledVehicleData(SpeedKart64Vehicle : SpeedKartVehicle)
{
	category = "Vehicles";
	//- Render -
	shapeFile = "./SpeedKart64.dts";
	emap = true;
	//- Vehicle Data -
	uiName = "SpeedKart 64";
	finalExplosionProjectile = SpeedKartFinalExplosionProjectile;
};



function SpeedKartvehicle::onadd(%this,%obj)
{ 
	parent::onadd(%this,%obj);
	   %obj.playthread(0,"propslow");
	
}

function SpeedKartBlockovehicle::onadd(%this,%obj)
{ 
	parent::onadd(%this,%obj);
	   %obj.playthread(0,"propslow");
	
}

datablock WheeledVehicleData(SpeedKartBlockoVehicle : SpeedKartVehicle)
{
	category = "Vehicles";
	//- Render -
	shapeFile = "./SpeedKartBlocko.dts";
	emap = true;
	//- Vehicle Data -
	uiName = "SpeedKart Blocko";
	finalExplosionProjectile = SpeedKartFinalExplosionProjectile;
	defaultTire	= SpeedKartBlockoTire; //You need to specify the wheel datablock here if it is any different from the regular speedkart's ~Barna
}; //a semi colon was missing here ~Barna


function SpeedKartvehicle::onadd(%this,%obj)
{ 
	parent::onadd(%this,%obj);
	   %obj.playthread(0,"propslow");
	
}

function SpeedKart7vehicle::onadd(%this,%obj)
{ 
	parent::onadd(%this,%obj);
	   %obj.playthread(0,"propslow");
	
}

datablock WheeledVehicleData(SpeedKart7Vehicle : SpeedKartVehicle)
{
	category = "Vehicles";
	//- Render -
	shapeFile = "./SpeedKart7.dts";
	emap = true;
	//- Vehicle Data -
	uiName = "SpeedKart 7";
	finalExplosionProjectile = SpeedKartFinalExplosionProjectile;
};


function SpeedKartvehicle::onadd(%this,%obj)
{ 
	parent::onadd(%this,%obj);
	   %obj.playthread(0,"propslow");
	
}

function SpeedKartLeMansvehicle::onadd(%this,%obj)
{ 
	parent::onadd(%this,%obj);
	   %obj.playthread(0,"propslow");
	
}

datablock WheeledVehicleData(SpeedKartLeMansVehicle : SpeedKartVehicle)
{
	category = "Vehicles";
	//- Render -
	shapeFile = "./SpeedKartLeMans.dts";
	emap = true;
	//- Vehicle Data -
	uiName = "SpeedKart LeMans";
	finalExplosionProjectile = SpeedKartFinalExplosionProjectile;
};

