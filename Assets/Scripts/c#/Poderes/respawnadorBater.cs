using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class respawnadorBater : MonoBehaviour {
    public GameObject respawnado;
    public AudioClip esteAparece,acertando;
    private AudioSource esteAudio;
    public float dura = 5f;
    private float durou = 0f;
    private float dor;
    private List<GameObject> lista = new List<GameObject>();
    private Rigidbody2D esteCorpo;
    private Collider2D esteColl;
    private bool criti;
    private float dano,mult;
    private bool acertou = false;
    private Renderer reder;
    private List<Collider2D> listIgnorados = new List<Collider2D>();
    private float duraIg = 0f;
	// Use this for initialization
	void Awake () {
        esteAudio = GetComponent<AudioSource>();
        GameObject aux;
        for(int i = 0; i < 1; i++)
        {
            aux = Instantiate(respawnado);
            lista.Add(aux);
            aux.SetActive(false);
        }
        esteCorpo = GetComponent<Rigidbody2D>();
        esteColl = GetComponent<Collider2D>();
        reder = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!acertou)
        {
            AcertouInimigo();
            if (durou > dura)
            {
                gameObject.SetActive(false);
            }
            else
            {
                durou += Time.deltaTime;
            }

            if(duraIg > 0f)
            {
                duraIg -= Time.deltaTime;
            }else if(listIgnorados.Count > 0)
            {
                auxiliares.RemoverIgnoradosColiders(listIgnorados,esteColl);
            }
        }else
        {           
            if(!acertando || durou> acertando.length)
            {
                gameObject.SetActive(false);
            }else
            {
                durou += Time.deltaTime;
            }
        }
	}

    public void Respaw(Vector2 pos,Vector2 forca,float damage, float multiplicador, bool critico,float pain)
    {
        duraIg = 0f;
        listIgnorados.Clear();
        dor = pain;
        reder.enabled = true;
        acertou = false;
        esteCorpo.gravityScale = 1f;
        esteColl.enabled = true;
        transform.position = pos;
        gameObject.SetActive(true);
        esteCorpo.AddForce(forca*esteCorpo.mass, ForceMode2D.Impulse);
        criti = critico;
        mult = multiplicador;
        dano = damage;
        durou = 0f;
        esteAudio.PlayOneShot(esteAparece);
    }

    private void AcertouInimigo()
    {
        Vector2 a = esteColl.bounds.center;
        Vector2 b = a;
        a.x += esteColl.bounds.extents.x;
        a.y += esteColl.bounds.extents.y;
        b.x -= esteColl.bounds.extents.x;
        b.y -= esteColl.bounds.extents.y;
        Collider2D bateu = Physics2D.OverlapArea(a, b, fases.faseX.iniMask);

        if (bateu)
        {
            atributosInimigos atb = bateu.GetComponent<atributosInimigos>();
            bool esquivou = atb.Esquivou();
            atb.AplicarDano(0f, dor, esquivou, Vector2.zero, false);
            if (!esquivou)
            {
                GameObject objX = auxiliares.RetornaObj(lista);
                areaTempo area = objX.GetComponentInChildren<areaTempo>();
                area.Respaw(transform.position, dano, criti, mult);
                esteColl.enabled = false;
                esteCorpo.gravityScale = 0f;
                esteCorpo.velocity = Vector2.zero;
                acertou = true;
                esteAudio.clip = null;
                esteAudio.PlayOneShot(acertando);
                durou = 0f;
                reder.enabled = false;
            }
            else
            {
                Physics2D.IgnoreCollision(esteColl, bateu);
                listIgnorados.Add(bateu);
                duraIg = 1f;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject objX = auxiliares.RetornaObj(lista);
        areaTempo area = objX.GetComponentInChildren<areaTempo>();
        area.Respaw(collision.contacts[0].point, dano, criti, mult);
        esteColl.enabled = false;
        esteCorpo.gravityScale = 0f;
        esteCorpo.velocity = Vector2.zero;
        acertou = true;
        esteAudio.clip = null;
        esteAudio.PlayOneShot(acertando);
        durou = 0f;
        reder.enabled = false;
    }
}
