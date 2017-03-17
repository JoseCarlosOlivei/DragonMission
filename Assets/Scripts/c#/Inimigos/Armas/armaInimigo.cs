using UnityEngine;
using System.Collections;

public class armaInimigo : MonoBehaviour {

	private atributosInimigos atributos;
	private moveCPU move;
	public AudioSource esteAudio;
	private Collider2D esteCollider;
	public AudioClip acertou;
    public Vector2 forca;
	public float somX = 1f;
	//public bool defendeuSim = false;
	// Use this for initialization
	void Start () {
		esteAudio = GetComponent<AudioSource> ();
        move = GetComponentInParent<moveCPU>();
		atributos = GetComponentInParent<atributosInimigos> ();
	}

	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.tag == "Player") {

            if (atributos.estados == 1)
            {
                atributosHeroi atribHeroi = coll.gameObject.GetComponent<atributosHeroi>();
                bool esq = false;
                float damages = atributos.DanoCalcu(atribHeroi.defesa, 1f / 16f);
                //Debug.Log (damages);
                if (atributos.estados == 1 && !atribHeroi.DefendeuSim(transform.position.x))
                {
                  
                    if (Random.value < atribHeroi.esquiva)
                    {
                       
                        esq = true;
                    }
                    else
                    {
                        esteAudio.pitch = atributos.agilidade * somX;
                        esteAudio.clip = acertou;
                        esteAudio.Play();
                    }
                    Vector2 f2 = forca;
                    if (!move.viradoDireita)
                    {
                        f2 *= -1f;
                    }
                    atribHeroi.AplicarDano(damages, 1f / atributos.agilidade, esq, f2,atributos.FoiCritico());
                }
                else if (atribHeroi.DefendeuSim(transform.position.x))
                {
                    atribHeroi.Denfender(damages, 1f / atributos.agilidade,atributos.FoiCritico());
                }
            }else
            {
                Vector2 f3 = Vector2.one * 2f;
                if (!move.viradoDireita)
                {
                    f3.x *= -1f;
                }
                coll.GetComponent<Rigidbody2D>().AddForce(f3, ForceMode2D.Impulse);
            }
		} 
	}
}
