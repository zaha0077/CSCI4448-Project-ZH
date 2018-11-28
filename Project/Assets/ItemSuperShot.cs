using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This item grants a poweful long range shot.
public class ItemSuperShot : Item {
	
	public override void SetEffects(){
		effects_.Add (new Rangeup (32));
		effects_.Add (new Damageup (60));
		effects_.Add (new ShowText ("Super Blaster"));
	}
}
