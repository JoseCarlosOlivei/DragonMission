using UnityEngine;
using System.Collections;

public class mataSeEncosta : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D coll) {
		//Debug.Log (esteCorpo.velocity.y);
		if (coll.gameObject.tag == "Player") {
			//Debug.Log ("Saiu");
			atributosHeroi atrb = coll.gameObject.GetComponent<atributosHeroi> ();
			atrb.hpAtual = 0f;
		} else if (coll.gameObject.tag == "Inimigo") {
			atributosInimigos atbr = coll.gameObject.GetComponent<atributosInimigos> ();
			atbr.hpAtual = 0f;
		}
	}
}
