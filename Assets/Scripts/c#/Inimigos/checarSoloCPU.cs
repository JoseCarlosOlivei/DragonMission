using UnityEngine;
using System.Collections;

public class checarSoloCPU : MonoBehaviour {
	private moveCPU move;
	private bool aindaEsta = false;
	// Use this for initialization
	void Start () {
		move = GetComponentInParent<moveCPU> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (aindaEsta) {
			aindaEsta = false;
		}
	}

	void OnTriggerStay2D(Collider2D coll) {
		if (coll.tag == "Chao") {
			move.noChao = true;
			aindaEsta = true;
		} else {
			aindaEsta = false;
		}
	}
	
	void OnTriggerExit2D(Collider2D coll) {
		if (!aindaEsta) {
			move.noChao = false;
		}
		aindaEsta = false;
	}
}
