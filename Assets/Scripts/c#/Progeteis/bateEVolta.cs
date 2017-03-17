using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bateEVolta : MonoBehaviour {

    public float speed = 2f;
    public Vector2 forca;
    public int durabilidade = 5;
    private int duraX = 0;
    public float duracao = 5f;
    private float durou = 0f;
    private Rigidbody2D esteCorpo;
    private Collider2D esteColl;
    private bool critico = false;
    private float dor, mult,tempoX=0,dano;
    private Renderer reder;
    private AudioSource esteAudio;
    public AudioClip acertando;
    private List<Collider2D> ignorados = new List<Collider2D>();
    private int sentidoSpeed = 1;
	// Use this for initialization
	void Awake () {
        esteCorpo = GetComponent<Rigidbody2D>();
        esteColl = GetComponent<Collider2D>();
        esteAudio = GetComponent<AudioSource>();
        reder = GetComponent<Renderer>();
	}

    public void Respaw(bool direita, Vector2 pos0, float damage, float multiplo, bool critical, float pain)
    {
        esteColl.enabled = true;
        reder.enabled = true;
        tempoX = 0f;
        durou = 0f;
        duraX = durabilidade;

        if (direita)
        {
            transform.localEulerAngles = new Vector3(0f, 0f, 0f);
            sentidoSpeed = 1;
        }
        else
        {
            transform.localEulerAngles = new Vector3(0f, 180f, 0f);
            sentidoSpeed = -1;
        }
        auxiliares.RemoverIgnoradosColiders(ignorados, esteColl);
        critico = critical;
        dano = damage;
        mult = multiplo / (auxiliares.VantagemSpeedAreaT(speed, duracao, esteColl.bounds.size,1f)*(0.5f+0.5f*durabilidade));
        transform.position = pos0;
        dor = pain;
        gameObject.SetActive(true);

    }

    // Update is called once per frame
    void Update () {
        if (durou < duracao)
        {
            durou += Time.deltaTime;
        }
        
        if(durou >= duracao || duraX <=0)
        {
            durou = 0f;
            gameObject.SetActive(false);
        }

        if (esteCorpo.velocity.x*sentidoSpeed < speed)
        {
            Vector2 v = (speed*sentidoSpeed-esteCorpo.velocity.x)*Vector2.right;

            esteCorpo.AddForce(v, ForceMode2D.Impulse);
        }

        if (tempoX > 0f)
        {
            tempoX -= Time.deltaTime;
        }
        else if (Physics2D.GetIgnoreCollision(esteColl, atributosHeroi.heroiX.GetComponent<Collider2D>()))
        {
            Physics2D.IgnoreCollision(atributosHeroi.heroiX.GetComponent<Collider2D>(), esteColl, false);
        }
    }

    private void Acertou()
    {
        if (transform.eulerAngles.y == 0f)
        {
            transform.rotation = Quaternion.Euler(Vector3.up * 180f);
        }
        else
        {
            transform.rotation = Quaternion.Euler(Vector3.zero);
        }

        sentidoSpeed *= -1;
        esteAudio.PlayOneShot(acertando);
        duraX -= 1;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Inimigo")
        {
            atributosInimigos atrb = coll.gameObject.GetComponent<atributosInimigos>();
            float damages = 0f;
            if (dano > 0f && mult > 0f)
            {
                damages = dano / (atrb.defesa + dano);
                damages *= dano * mult;
            }
           
            if (atrb.DefendeuSim(damages,dor,transform.position.x,critico))
            {
                Acertou();
            }
            else if (atrb.esquiva > Random.value)
            {
                atrb.AplicarDano(0f, dor, true, forca, critico);
                Physics2D.IgnoreCollision(coll.collider, esteColl, true);
                ignorados.Add(coll.collider);
                tempoX = 1f;
            }
            else
            {
                atrb.AplicarDano(damages, dor, false, forca, critico);
                Acertou();
            }
        }
        else if ((coll.gameObject.tag == "Chao" || coll.gameObject.tag == "Parede") && coll.contacts[0].normal.x!=0f)
        {
            Acertou();
        }
    }
}
