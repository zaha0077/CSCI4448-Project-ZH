using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity {

	private int ticks = 16;
	private bool up = true;

	public override void Move(){
		Vector3 temp = this.transform.position;
		if (up) {
			temp.y += getSpeed ();
		} else {
			temp.y -= getSpeed ();
		}
		this.transform.position = temp;
	}

	// Use this for initialization
	void Start () {
		setSpeed (0.01f);
	}
	
	// Update is called once per frame
	void Update () {
		Move ();
		if (ticks == 0) {
			ticks = 32;
			up = !up;
		}
		ticks--;
	}
}
