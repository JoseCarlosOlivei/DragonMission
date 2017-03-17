using UnityEngine;
using System.Collections;

public class areaBuff : MonoBehaviour {
	public GameObject buff;
	private buffs buffX;
	// Use this for initialization
	void Awake () {
		buffX = buff.GetComponent<buffs> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay2D(Collider2D coll) {
		if (coll.tag == "Inimigo") {
			atributosInimigos atb = coll.GetComponent<atributosInimigos> ();
			atb.Buffa (buffX);
		}
	}
}
