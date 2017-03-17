using UnityEngine;
using System.Collections;

public class progeteis : MonoBehaviour
{
    public AudioClip acertando;
    public float dano = 0f;
    public float stun;
    private float dor = 0f;
    private float mult;
    //private atributosInimigos atrib;
    public float vida = 1.25f;
    private float tempo = 0f, tempoSom = 0f;
    public float speed = 0f;
    public Vector2 forca, forcaAqui;
    private bool critico = false;
    private bool acertou = false;
    private ParticleSystem parti;
    private Rigidbody2D rbd2d;
    private float gravScala;
    private Collider2D esteColl;
    private buffsRecupera recup;
    private Renderer reder;
    // Use this for initialization
    void Awake()
    {
        rbd2d = GetComponent<Rigidbody2D>();
        parti = GetComponent<ParticleSystem>();
        gravScala = rbd2d.gravityScale;
        esteColl = GetComponent<Collider2D>();
        recup = GetComponent<buffsRecupera>();
        reder = GetComponent<Renderer>();

    }

    // Update is called once per frame
    void Update()
    {
        if (tempo < vida)
        {
            tempo += Time.deltaTime;
        }
        else if (!acertou)
        {
            gameObject.SetActive(false);
        }

        if (acertou)
        {
            if (tempoSom == 0f)
            {
                reder.enabled = false;
                AudioSource som = GetComponent<AudioSource>();
                som.PlayOneShot(acertando);
                esteColl.enabled = false;
                rbd2d.gravityScale = 0f;
                rbd2d.velocity = Vector2.zero;
                if (parti)
                {
                    parti.Play();
                }
            }
            tempoSom += Time.deltaTime;
            if (tempoSom > 1f)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void Respaw(Vector2 ond, bool direita, float damage, float pain, float multDano, bool criti)
    {
        acertou = false;
        tempoSom = 0f;
        tempo = 0f;
        rbd2d.gravityScale = gravScala;
        transform.position = ond;
        esteColl.enabled = true;
        reder.enabled = true;
        float ladoT = 1f;
        Vector2 fX = forcaAqui;

        if (direita)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, transform.rotation.eulerAngles.z);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, 180f, transform.rotation.eulerAngles.z);
            ladoT = -1f;
            fX.x *= -1f;
        }
        dano = damage;
        dor = pain;
        mult = multDano / auxiliares.VantagemSpeedAreaT(speed, vida, esteColl.bounds.size,1f);
        if (recup)
        {
            recup.taxaRecuperacao *= mult * recup.vPonts;
            mult *= 1f - recup.vPonts;
        }
        critico = criti;
        gameObject.SetActive(true);
        rbd2d.AddForce(Vector2.right * speed * ladoT, ForceMode2D.Impulse);
        rbd2d.AddForce(fX, ForceMode2D.Impulse);
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (!acertou)
        {
            if (coll.gameObject.tag == "Player")
            {
                // dano heroi
                atributosHeroi atriHeroi = coll.gameObject.GetComponent<atributosHeroi>();
                float damages = 0f;
                if (dano > 0f && mult > 0f)
                {
                    damages = dano / (atriHeroi.defesa + dano);
                    if (recup)
                    {
                        recup.taxaRecuperacao *= damages*dano;
                    }
                    damages *= dano * mult;
                }
                float localForca = coll.gameObject.transform.position.x - transform.position.x;
                Vector2 forcaX = forca;
                if (localForca < 0)
                {
                    forcaX.x *= -1f;
                }

                //float posRela = 
                if (atriHeroi.DefendeuSim(transform.position.x))
                {
                    atriHeroi.Denfender(damages, dor, critico);
                    acertou = true;
                }
                else if (atriHeroi.esquiva > Random.value)
                {
                    atriHeroi.AplicarDano(0f, dor, true, forcaX, critico);
                }
                else
                {
                    atriHeroi.AplicarDano(damages, dor, false, forcaX, critico);
                    atriHeroi.Stunear(stun);
                    if (recup)
                    {
                        recup.RecuperaValor(critico);
                        atriHeroi.AddBuffCura(recup);
                    }
                    acertou = true;
                }
            }
            else if (coll.gameObject.tag == "Inimigo")
            {
                // dano Inimigo
                atributosInimigos atr = coll.gameObject.GetComponent<atributosInimigos>();
                float damages = 0f;

                if (dano > 0f && mult > 0f)
                {
                    damages = dano / (atr.defesa + dano);
                    if (recup)
                    {
                        recup.taxaRecuperacao *= dano*damages;
                    }
                    damages *= dano * mult;
                }

                if (!atr.DefendeuSim(damages, dor, transform.position.x, critico))
                {
                    Vector2 fX = forca;
                    float localForca = coll.gameObject.transform.position.x - transform.position.x;
                    if (localForca < 0)
                    {
                        fX.x *= -1f;
                    }

                    if (atr.esquiva > Random.value)
                    {
                        atr.AplicarDano(0f, dor, true, fX, critico);
                    }
                    else
                    {
                        atr.AplicarDano(damages, dor, false, fX, critico);
                        if (recup)
                        {
                            recup.RecuperaValor(critico);
                            atr.AddBuffCura(recup);
                        }
                        atr.AplicarStun(stun);
                        acertou = true;
                    }

                }
            }
            else if (!coll.isTrigger)
            {
                acertou = true;
            }
        }
    }
}

