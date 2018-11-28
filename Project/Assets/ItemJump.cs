using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This item grants double jumping.
public class ItemJump : Item {

	public override void SetEffects(){
		effects_.Add (new JumpChange (2));
		effects_.Add (new ShowText ("Double Jump"));
	}
}

