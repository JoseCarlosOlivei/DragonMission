using UnityEngine;
using System.Collections;

public class armaHeroi : MonoBehaviour {
	private atributosHeroi atributos;
	private moverJogador jogador;
	private AudioSource esteAudio;
	//public Collider2D esteCollider;
	// Use this for initialization
	void Start () {
		esteAudio = GetComponent<AudioSource> ();
		jogador = GetComponentInParent<moverJogador> ();
		atributos = GetComponentInParent<atributosHeroi> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.tag == "Inimigo") {
			atributosInimigos atribIni = coll.gameObject.GetComponent<atributosInimigos> ();
			bool esq = false;
			if (jogador.estados == 1) {
				float damages = atributos.DanoCalcu(atribIni.defesa,1f/16f);
				float mult = 1f;
                Vector2 fX = Vector2.zero;
                if (Random.value < atribIni.esquiva) {
					mult = 0f;
					esq = true;
				} else {
					esteAudio.pitch = atributos.agilidade;
					esteAudio.clip = jogador.EspadadaAcerto;
					esteAudio.Play ();
				}
				if (!atribIni.DefendeuSim (damages,1f/atributos.agilidade,atributos.transform.position.x,atributos.FoiCritico())) {
                    if (atribIni.leve)
                    {
                        if (transform.position.x > coll.transform.position.x)
                        {
                            fX = Vector2.left;
                        }else
                        {
                            fX = Vector2.right;
                        }
                    }
					atribIni.AplicarDano (damages * mult, 1f / atributos.agilidade, esq,fX,atributos.FoiCritico());
                }
			}
		} 
	}
}
