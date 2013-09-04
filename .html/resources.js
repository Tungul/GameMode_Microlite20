var level, str, dex, mind, hp, ac,
	physical, subterfuge, knowledge, communication,
	strmod, dexmod, mindmod,
	numdice, dicesize;

var dr = new DiceRoller();

function gebi(input) {
	return document.getElementById(input + "div");
}

function calc() {
	level = parseInt(gebi('level').value);
	str = parseInt(gebi('str').value);
	dex = parseInt(gebi('dex').value);
	mind = parseInt(gebi('mind').value);
	hp = parseInt(gebi('hp').value);
	ac = parseInt(gebi('ac').value);

	physical = level;
	subterfuge = level;
	knowledge = level;
	communication = level;

	// console.log("STR:" + str + ";DEX:" + dex + ";MIND:" + mind);

	var pclass = "";
	switch (gebi('pclass').value) {
		case "0":
			pclass = "fighter";
			physical += 3;
			break;
		case "1":
			pclass = "rogue";
			subterfuge += 3;
			break;
		case "2":
			pclass = "magi";
			knowledge += 3;
			break;
		case "3":
			pclass = "cleric";
			communication += 3;
			break;
	}

	// console.log(pclass);

	var race = "";
	switch (gebi('race').value) {
		case "0":
			race = "human";
			// skill bonus
			break;
		case "1":
			race = "elf";
			mind += 2;
			break;
		case "2":
			race = "dwarf";
			str += 2;
			break;
		case "3":
			race = "halfling";
			dex += 2;
			break;
	}

	strmod = Math.floor((str - 10) / 2);
	dexmod = Math.floor((dex - 10) / 2);
	mindmod = Math.floor((mind - 10) / 2);

	gebi('strmod').innerHTML = strmod;
	gebi('dexmod').innerHTML = dexmod;
	gebi('mindmod').innerHTML = mindmod;
	gebi('hpmod').innerHTML = (str + hp);
	gebi('acmod').innerHTML = (10 + dexmod + ac);
	gebi('physical').innerHTML = physical;
	gebi('subterfuge').innerHTML = subterfuge;
	gebi('knowledge').innerHTML = knowledge;
	gebi('communication').innerHTML = communication;
	gebi('attackmelee').innerHTML = (strmod + level);
	gebi('attackmissle').innerHTML = (dexmod + level);
	gebi('attackmagic').innerHTML = (mindmod + level);
	numdice = gebi("numdice").value.toString();
	dicesize = gebi("dicesize").value.toString();
	twohand = gebi("twohand").checked;
	twolight = gebi("twolight").checked;
}

function attack(modifier, target, ismagic) {
	var result = dr.roll("1d20+" + modifier);
	var damageroll = dr.roll(numdice + "d" + dicesize);
	var damage = damageroll.total + strmod + (twohand ? strmod : 0);
	var maxdamage = (numdice * dicesize) + strmod + (twohand ? strmod : 0);
	var out = "";

	if(ismagic) {
		out += "1d20 = <b>" + result.total + "</b>";
		if(result.rolls[0] == 20) {
			out += " <tt><font color='green'>[CRITICAL]</font></tt>";
		}
		else if(result.rolls[0] >= 10) {
			out += " <tt><font color='green'>[SUCCESS]</font></tt>";
		}
		else {
			out += " <tt><font color='red'>[FAILURE]</font></tt>";
		}
	}
	else {
		out += "1d20 + " + modifier + " = <b>" + result.total + "</b>";
		if(result.rolls[0] == 20) {
			out += " <tt><font color='green'>[CRITICAL] (" + maxdamage + ")</font></tt>";
		}
		else if(result.total >= target) {
			out += " <tt><font color='green'>[SUCCESS] (" + damage + ")</font></tt>";
		}
		else {
			out += " <tt><font color='red'>[FAILURE]</font></tt>";
		}
	}
	out += "<br>";

	return out;
}

function doRoll(mode, type) {
	calc();
	if(mode == "attack") {
		var modifier = 0;
		var target = parseInt(gebi("target").value);
		var ismagic = false;
	
		switch(type) {
			case "meleeAttack":
				modifier = parseInt(gebi('attackmelee').innerText);
				break;
			case "missleAttack":
				modifier = parseInt(gebi('attackmissle').innerText);
				break;
			case "magicAttack":
				modifier = parseInt(gebi('attackmagic').innerText);
				ismagic = true;
				break;
		}

		var output = "";

		if(twolight) {
			modifier -= 2;
			output += attack(modifier, target, ismagic);
		}

		output += attack(modifier, target, ismagic);

		while(parseInt(modifier) >= 6) {
			modifier -= 5;

			output += attack(modifier, target, ismagic);
		}

		gebi('attackRoll').innerHTML = output;
	}

	else if(mode == "check") {
		var modifier = "";
		var target = parseInt(gebi("target").value);
	
		switch(type) {
			case "phystr":
				modifier = physical + strmod;
				break;
			case "phydex":
				modifier = physical + dexmod;
				break;
			case "subdex":
				modifier = subterfuge + dexmod;
				break;
			case "submind":
				modifier = subterfuge + mindmod;
				break;
			case "knowmind":
				modifier = knowledge + mindmod;
				break;
			case "commind":
				modifier = communication + mindmod;
				break;
		}
	
		var result = dr.roll("1d20+" + modifier);
	
		var output = "1d20 + " + modifier + " = <b>" + result.total + "</b>";
		
		if(result.rolls[0] == 20) {
			output += " <tt><font color='green'>[CRITICAL]</font></tt>";
		}
		else if(result.total >= target) {
			output += " <tt><font color='green'>[SUCCESS]</font></tt>";
		}
		else {
			output += " <tt><font color='red'>[FAILURE]</font></tt>";
		}
	
		gebi('checkRoll').innerHTML = output;
	}
}