﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**This Item greatly boosts fire rate.*/
public class ItemRate : Item {

	public override void SetEffects(){
		effects_.Add (new Rateup(14));
		effects_.Add (new ShowText ("Repeater Upgrade"));
	}
}
