using UnityEngine;
using System.Collections;

public class armaDragLanca : MonoBehaviour {
	 
	private atributosInimigos atributos;
	private moveCPU move;
	private Animator anima;
	private AudioSource esteAudio;
	private Collider2D esteCollider;
	public AudioClip acertou, defendeu;
	private float somX = 1f;
	//public float tPoder = 0f;
	//public int aux =0;
	//public bool defendeuSim = false;
	// Use this for initialization
	void Start () {
		esteAudio = GetComponent<AudioSource> ();
		//esteCollider = GetComponent<Collider2D> ();
		atributos = GetComponentInParent<atributosInimigos> ();
		anima = atributos.gameObject.GetComponent<Animator> ();
	}

	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.tag == "Player" ) {
			atributosHeroi atribHeroi = coll.gameObject.GetComponent<atributosHeroi> ();
			bool esq = false;
			float damages = 0f;
			//float multX = 1f;
			if (atributos.estados == 1) {
				float dor = 0f;
				if (anima.GetFloat ("qualPoder") == 0f ) {
					damages = atributos.DanoCalcu (atribHeroi.defesa,1f/16f);
					dor = 1f / atributos.agilidade;
					somX = atributos.agilidade;
				}else if(anima.GetFloat ("qualPoder") == 1f || anima.GetFloat ("qualPoder") == 3f){
					damages = atributos.HabDanoCalcu (atribHeroi.defesa,1f/16f);
					dor = 0.5f;
					somX = 1f;
				}else if (anima.GetFloat ("qualPoder") == 2f) {
					//aux += 1;
					damages = atributos.HabDanoCalcu (atributos.defesa,0.075f/16f);
					dor = 1f/8f;
					somX =  8f;
				}
					
				float mult = 1f;
				//Debug.Log (damages);
				if (atribHeroi.DefendeuSim (transform.position.x)) {
					atribHeroi.Denfender (damages, dor,atributos.FoiCritico());
				} else {
					if (Random.value < atribHeroi.esquiva) {
						mult = 0f;
						esq = true;
					} else {
						esteAudio.pitch = somX;
						esteAudio.clip = acertou;
						esteAudio.Play ();
					}
					atribHeroi.AplicarDano (damages * mult, dor, esq,Vector2.zero,atributos.FoiCritico());
				}
				//Debug.Log (damages * mult);
			}
		} 
	}
}
