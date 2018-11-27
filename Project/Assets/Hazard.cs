using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : Enemy {

	//Built-in Unity functions from the MonoBehavior class.
	// Initialization
	void Start () {
		hp_ = 9999;
		setInvincible (true);
	}

	// Behavior to be executed every frame.
	void Update () {

	}
}
