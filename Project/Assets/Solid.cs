using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This class is used for walls and floors.
*/
public class Solid : MonoBehaviour {

	/**
	 * Called when a projectile strikes the Solid.
	 */
	public virtual void Explode(){} //Used by breakable children.

}
