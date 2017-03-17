using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class recocheteaProgetil : MonoBehaviour {
	public float speed = 2f;
	public float duracao = 5f;
	private float durou = 0f;
    private Rigidbody2D esteCorpo;
	public float dano = 20f;
	private float dor = 0f;
	//public bool direita;
	public float forca = 0f;
	public bool critico = false;
	public float mult = 1f/16f;
    private Collider2D esteColl;
    private float tempoX = 0f,tAcertou;
    public float angulo0;
    private Vector2 velocida;
    private Renderer rederizador;
    private bool acertou = false;
    private AudioSource esteAudio;
    public AudioClip somAcertou,somRecocheteo;
    private ParticleSystem particulas;
    private float dire;
    public bool doAliado;
    private List<Collider2D> ignorados = new List<Collider2D>();

	// Use this for initialization
	void Awake () {
        esteColl = GetComponent<Collider2D>();
        esteCorpo = GetComponent<Rigidbody2D>();
        rederizador = GetComponent<Renderer>();
        esteAudio = GetComponent<AudioSource>();
        particulas = GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		if (durou < duracao) {
			durou += Time.deltaTime;
		} else {
			durou = 0f;
			gameObject.SetActive(false);
		}

        if(esteCorpo.velocity.magnitude < speed || dire*esteCorpo.velocity.x<0f || velocida.y*esteCorpo.velocity.y<0f)
        {
            Vector2 v =speed*velocida;
            if (v.x < esteCorpo.velocity.x *dire)
            {
                v.x = v.x-esteCorpo.velocity.x*dire;
            }
            if ((v.y < esteCorpo.velocity.y && v.y >=0f)||(v.y > esteCorpo.velocity.y && v.y <= 0f))
            {
                v.y -= esteCorpo.velocity.y;
            }
            v.x *= dire;
            esteCorpo.AddForce(v, ForceMode2D.Impulse);
        }

        if (tempoX > 0f)
        {
            tempoX -= Time.deltaTime;
        }
        else if (ignorados.Count > 0)
        {
            auxiliares.RemoverIgnoradosColiders(ignorados,esteColl);
        }

        if (acertou)
        {
            if (tAcertou == 0f)
            {
                particulas.Play();
                esteAudio.PlayOneShot(somAcertou);
                rederizador.enabled = false;
                esteColl.enabled = false;
            }

            if (tAcertou < 3f)
            {
                tAcertou += Time.deltaTime;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void Respaw(bool direita,Vector2 pos0,float damage, float multiplo, bool critical,float pain){
        esteColl.enabled = true;
        particulas.Stop();
        rederizador.enabled = true;
        tempoX = 0f;
        durou = 0f;
        tAcertou = 0f;
        acertou = false;
        velocida = Quaternion.Euler(0f, 0f, angulo0) * Vector2.right;
        if (direita)
        {
            transform.localEulerAngles = new Vector3(0f, 0f, angulo0);
            dire = 1f;
        }else
        {
            transform.localEulerAngles = new Vector3(0f, 180f, angulo0);
            dire = -1f;
        }

        auxiliares.RemoverIgnoradosColiders(ignorados, esteColl);
        critico = critical;
        dano = damage;
        mult = multiplo/auxiliares.VantagemSpeedAreaT(speed,duracao,esteColl.bounds.size,1.5f);
        transform.position = pos0;
        dor = pain;
        gameObject.SetActive(true);
        
    }

	void OnCollisionEnter2D (Collision2D coll){
        if (coll.gameObject.tag == "Player")
        {
            atributosHeroi atriHeroi = coll.gameObject.GetComponent<atributosHeroi>();
            float damages = 0f;
            if (dano > 0f && mult > 0f)
            {
                damages = dano / (atriHeroi.defesa + dano);
                damages *= dano * mult;
            }
            if (critico)
            {
                damages *= 3f;
            }
            float angulo = transform.localEulerAngles.z;
            Vector2 vX = Quaternion.Euler(0f, 0f, angulo) * Vector2.right * forca;
            vX.x *= dire;
            if (atriHeroi.DefendeuSim(transform.position.x))
            {
                atriHeroi.Denfender(damages, dor, critico);
                acertou = true;
            }
            else if (atriHeroi.esquiva > Random.value)
            {
                atriHeroi.AplicarDano(0f, dor, true, vX, critico);
                Physics2D.IgnoreCollision(coll.collider, esteColl, true);
                ignorados.Add(coll.collider);
                tempoX = 1f;
            }
            else
            {
                atriHeroi.AplicarDano(damages, dor, false, vX, critico);
                angulo *= Mathf.PI / 180f;
                //Debug.Log (vX);
                acertou = true;
            }
        }
        else if (coll.gameObject.tag == "Inimigo") {
            atributosInimigos atrb = coll.gameObject.GetComponent<atributosInimigos>();
            float damages = 0f;
            if (dano > 0f && mult > 0f)
            {
                damages = dano / (atrb.defesa + dano);
                damages *= dano * mult;
            }

            if (atrb.DefendeuSim(damages, dor, transform.position.x, critico))
            {
                acertou = true;
            }
            else if (atrb.esquiva > Random.value)
            {
                atrb.AplicarDano(0f, dor, true, Vector2.zero, critico);
                Physics2D.IgnoreCollision(coll.collider, esteColl, true);
                ignorados.Add(coll.collider);
                tempoX = 1f;
            }
            else
            {
                float angulo = transform.localEulerAngles.z;
                Vector2 vX = Quaternion.Euler(0f, 0f, angulo) * Vector2.right * forca;
                vX.x *= dire;
                atrb.AplicarDano(damages, dor, false,vX, critico);
                acertou = true;
            }
        }
        else if (coll.gameObject.tag == "Chao" || coll.gameObject.tag == "Parede")
        {

            Vector2 v = CentroColiderLocal(atributosHeroi.heroiX);
            Vector2 dist = v - new Vector2(transform.position.x, transform.position.y);
            Vector2 direcao = dist / dist.magnitude;
            float ang = transform.eulerAngles.y;
            Vector2 aux = Vector2.right;
            float angZ = 0f;
            bool podeIr = false;
            if (doAliado)
            {
                Vector2 pontoB, pontoA = transform.position;
                pontoB = pontoA;
                pontoA -= moveCPU.limiteTela;
                pontoB += moveCPU.limiteTela;
                Collider2D[] colls = Physics2D.OverlapAreaAll(pontoA, pontoB);

                for (int i = 0; i < colls.Length; i++)
                {
                    v = colls[i].bounds.center;
                    dist = v - new Vector2(transform.position.x, transform.position.y);
                    direcao = dist / dist.magnitude;

                    atributosInimigos atb = colls[i].GetComponent<atributosInimigos>();
                    if (atb.estados != 5 && ConsegueIr(direcao, dist.magnitude))
                    {
                        podeIr = true;
                        i = colls.Length;
                    }
                }
            }
            else
            {
                moverJogador moveJ = atributosHeroi.heroiX.GetComponent<moverJogador>();

                podeIr = ConsegueIr(direcao, dist.magnitude);
                if (moveJ.estados == 5)
                {
                    podeIr = false;
                }
            }

            if (podeIr)
            {
                dire = 1f;
                if (ang != 180f && transform.position.x > v.x)
                {
                    ang = 180f;
                }
                else if (transform.position.x <= v.x && ang != 0f)
                {
                    ang = 0f;
                }
                if (ang == 180f)
                {
                    dire = -1f;
                }
                angZ = Angulo2d(transform.position, v);
                transform.eulerAngles = new Vector3(0f, ang, angZ * dire);

                velocida = Quaternion.Euler(0f, 0f, angZ * dire) * aux;
                esteAudio.PlayOneShot(somRecocheteo);
            }
            else if ((coll.contacts[0].normal.x > 0f && dire < 0f) || (coll.contacts[0].normal.x < 0f && dire > 0f) || (coll.contacts[0].normal.y > 0f && velocida.y < 0f) || (coll.contacts[0].normal.y < 0f && velocida.y > 0f))
            {
                if (dire == 1f)
                {
                    ang = 180f;
                    dire = -1f;
                }
                else
                {
                    ang = 0f;
                    dire = 1f;
                }

                transform.rotation = transform.rotation * Quaternion.Euler(0, 180f, 0f);
                angZ = transform.eulerAngles.z;
                velocida = Quaternion.Euler(0f, 0f, angZ) * aux;
                esteAudio.PlayOneShot(somRecocheteo);
            }
        }
	}

	private Vector2 CentroColiderLocal(GameObject obj){
		
		BoxCollider2D box = obj.GetComponent<BoxCollider2D> ();

        return box.bounds.center;
	}

    private bool ConsegueIr(Vector2 dire,float distancia)
    {
        bool ret = true;
        distancia += 0.005f;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dire, distancia, fases.faseX.chao.value);

        if (hit)
        {
            ret = false;
        }

        return ret;
    }

	private float Angulo2d(Vector2 pos0,Vector2 pos1){
		float retorno = (pos1.y-pos0.y)/(pos1.x-pos0.x);
		retorno = Mathf.Atan (retorno);
		retorno *= 180f / Mathf.PI;
		return retorno;
	}
}
