using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class noChaoPoder : MonoBehaviour {
    public float speed = 2f, duracao = 2.5f;
    public Vector2 forca;
    private float dano, dor, mult;
    private bool critico,acertado;
    private float tempoX, tempoSumir;
    private Rigidbody2D esteCorpo;
    public AudioClip acertando,movendo;
    private ParticleSystem particula;
    private AudioSource esteAudio;
    private float lado = 1f;
    private Collider2D esteColl,heroColl;
    private SpriteRenderer reder;
   
    // Use this for initialization
    void Awake () {
        esteCorpo = GetComponent<Rigidbody2D>();
        particula = GetComponent<ParticleSystem>();
        esteAudio = GetComponent<AudioSource>();
        esteColl = GetComponent<Collider2D>();
        reder = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        if (esteCorpo.velocity.y >= 0.001f || esteCorpo.velocity.y <= -0.001f)
        {
           NoAr();
        }

        if (tempoX < duracao)
        {
            tempoX += Time.deltaTime;
        }else
        {
            acertado = true;
        }

        if (acertado)
        {
            if (tempoSumir < 2f)
            {
               if(tempoSumir == 0f)
                {
                    esteAudio.loop = false;
                    esteCorpo.gravityScale = 0f;
                    esteColl.enabled = false;
                    esteCorpo.velocity = Vector2.zero;
                    reder.enabled = false;
                    esteAudio.clip = null;
                    esteAudio.PlayOneShot(acertando);
                    particula.Play();
                }
                tempoSumir += Time.deltaTime;
            }else
            {
                NoAr();
            }
        }else if (esteCorpo.velocity.x*lado < speed)
        {
            esteCorpo.AddForce(Vector2.right * (speed - esteCorpo.velocity.x*lado)*lado, ForceMode2D.Impulse);
        }

    }

    public void Respaw(Vector2 pos, bool direita,bool crit, float damage,float multiplicador,float pain,Transform pai)
    {
        transform.SetParent(pai);
        esteAudio.enabled = true;
        esteAudio.loop = true;
        esteAudio.clip = movendo;
        gameObject.SetActive(true);
        esteAudio.Play();
        esteCorpo.gravityScale = 1f;
        esteColl.enabled = true;
        reder.enabled = true;
        tempoSumir = 0f;
        tempoX = 0f;
        particula.Stop();
        acertado = false;
        Physics2D.IgnoreCollision(heroColl, esteColl, false);
        if (direita)
        {
            transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            lado = 1f;
        }else
        {
            transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
            lado = -1f;
        }

        if (tempoX > 0f)
        {
            tempoX -= Time.deltaTime;
        }
        else if (Physics2D.GetIgnoreCollision(esteColl, heroColl))
        {
            Physics2D.IgnoreCollision(heroColl, esteColl, false);
        }

        critico = crit;
        dano = damage;
        mult = multiplicador;
        dor = pain;

        transform.position = pos;
    }

    private void NoAr()
    {
        gameObject.SetActive(false);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            atributosHeroi atriHeroi = coll.gameObject.GetComponent<atributosHeroi>();
            float damages = dano / (atriHeroi.defesa + dano);
            damages *= dano * mult;          
            Vector2 fX = forca;
            fX.x *= lado;


            if (atriHeroi.DefendeuSim(transform.position.x))
            {
                atriHeroi.Denfender(damages, dor,critico);
                acertado = true;
            }
            else if (atriHeroi.esquiva > Random.value)
            {
                atriHeroi.AplicarDano(0f, dor, true, fX,critico);
                tempoX = 1f;
                Physics2D.IgnoreCollision(coll.collider, esteColl, true);
                heroColl = coll.collider;
            }
            else
            {
                atriHeroi.AplicarDano(damages, dor, false, fX,critico);
                acertado = true;
            }
        }
        else if (coll.contacts[0].normal.x != 0f)
        {
                acertado = true;
        }
    }
}
