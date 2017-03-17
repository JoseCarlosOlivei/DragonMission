using UnityEngine;
using System.Collections;

public class bounciProgt : MonoBehaviour {
	public float duracao = 0f;
	private float duraX = 0f;
	public float speedGiro = 2f;
	public float dano = 10f;
	public float stun = 0f;
	public float dor = 0f;
	public float mult = 1f/16f;
    public Vector2 forca = Vector2.one;
    private bool critico = false,acertou = false;
	private Rigidbody2D esteCorpo;
	public bool girar = false;
    private Collider2D esteColl;
    private float tempoX,dire,tAcertado;
    public float giro0;
    public bool quebraDefesa;
    private ParticleSystem particulas;
    private AudioSource esteAudio;
    public AudioClip acertouSom,quicando;
    private Renderer rederizador;
	// Use this for initialization
	void Awake () {
        esteAudio = GetComponent<AudioSource>();
        particulas = GetComponent<ParticleSystem>();
		esteCorpo = GetComponent<Rigidbody2D> ();
        esteColl = GetComponent<Collider2D>();
        rederizador = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
		if (duraX < duracao) {
		    duraX += Time.deltaTime;
		}else{
			gameObject.SetActive (false);
		}

        if (girar && !acertou)
        {
            if (esteCorpo.velocity.x >= 0f && dire < 0f)
            {
                dire = 1f;
            }else if(esteCorpo.velocity.x < 0f && dire >= 0f)
            {
                dire = -1f;
            }
            transform.Rotate(0f, 0f, Time.deltaTime * -speedGiro*dire);
        }

        if (acertou)
        {
            if (tAcertado == 0f)
            {
                rederizador.enabled = false;
                esteAudio.PlayOneShot(acertouSom);
                esteCorpo.gravityScale = 0f;
                esteColl.enabled = false;
                particulas.Play();
            }

            if (tAcertado < 3f)
            {
                tAcertado += Time.deltaTime;
            }else
            {
                gameObject.SetActive(false);
            }
        }

        if (tempoX > 0f)
        {
            tempoX -= Time.deltaTime;
        }else if (Physics2D.GetIgnoreCollision(esteColl, atributosHeroi.heroiX.GetComponent<Collider2D>()))
        {
            Physics2D.IgnoreCollision(atributosHeroi.heroiX.GetComponent<Collider2D>(), esteColl, false);
        }
	}
		
	public void Respaw(float damage,float multi,float pain, bool criti,bool direita, Vector2 pos,float stunear){
        tempoX = 0f;
        acertou = false;
        rederizador.enabled = true;
        tAcertado = 0f;
		duraX = 0f;
        dano = damage;

        Vector2 speedAux = Vector2.one * 1.25f;

        float xFator = auxiliares.VantagemSpeedAreaT(speedAux.magnitude, duracao, esteColl.bounds.size, 1f);
        if (quebraDefesa)
        {
            xFator *= 2f;
        }
        mult = multi / xFator;
        dor = pain;
        critico = criti;
        stun = stunear/xFator;
		gameObject.SetActive (true);
        transform.position = pos;
        particulas.Stop();
        esteColl.enabled = true;
        esteCorpo.gravityScale = 1f;
        if (!direita)
        {
            speedAux.x *= -1f;
            dire = -1f;
            transform.eulerAngles = new Vector3(0f, 180f, giro0);
        }else
        {
            dire = 1f;
            transform.eulerAngles = new Vector3(0f, 0f, giro0);
        }
      
        esteCorpo.AddForce(speedAux, ForceMode2D.Impulse);
	}

	void OnCollisionEnter2D (Collision2D coll)
	{
		if (coll.gameObject.tag == "Player") {
			atributosHeroi atriHeroi = coll.gameObject.GetComponent<atributosHeroi> ();
			float damages = dano / (atriHeroi.defesa + dano);
			damages *= dano * mult;          
			if (atriHeroi.DefendeuSim (transform.position.x) && !quebraDefesa) {
                float t = 1f;
                if (dor > 0f)
                {
                    t = dor;
                }
                acertou = true;
                atriHeroi.Denfender(damages, t,critico);
            } else if (atriHeroi.esquiva > Random.value) {
				atriHeroi.AplicarDano (0f, dor, true,Vector2.zero,critico);
                Physics2D.IgnoreCollision(coll.collider, esteColl, true);
                tempoX = 1f;
			} else {
				atriHeroi.AplicarDano (damages, dor, false,forca,critico);
				atriHeroi.Stunear (stun);
                acertou = true;
            }
		}else
        {
            esteAudio.PlayOneShot(quicando);
        } 
	}
}
