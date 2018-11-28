using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This item doubles the player's range and increases their fire rate slightly.
public class ItemRange : Item {

	public override void SetEffects(){
		effects_.Add (new Rangeup (8));
		effects_.Add (new Rateup (7));
		effects_.Add (new Damageup (5));
		effects_.Add (new ShowText ("Mid-Range Blaster"));
	}
}
