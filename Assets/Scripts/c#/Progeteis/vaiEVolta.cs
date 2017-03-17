using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vaiEVolta : MonoBehaviour
{
    public AudioClip acertando;
    private itensUsaveis itemX;
    private float dano = 0f;
    private float dor = 0f;
    private float mult;
    //private atributosInimigos atrib;
    private int durabilidade = 3;
    public float distMax = 2f;
    public float tempoLife = 4f;
    private float tempo = 0f, tempoSom = 0f, tempoIgnorados;
    public float speed = 2.5f;
    public Vector2 forca;
    private bool critico = false;
    private Rigidbody2D rbd2d;
    private List<Collider2D> ignorados = new List<Collider2D>();
    private Collider2D esteColl;
    private Collider2D dono;
    private Vector2 posOrigin, posFinal;
    private Renderer rederer;
    private AudioSource esteAudio;
    private int dire = 1;
    private bool aliado;
    private bool podePegarDeVolta;
    private int acao;
    private bool indo;
    // Use this for initialization
    void Awake()
    {
        rbd2d = GetComponent<Rigidbody2D>();
        esteColl = GetComponent<Collider2D>();
        rederer = GetComponent<Renderer>();
        esteAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dire < 0 && transform.rotation.eulerAngles.y != 180f)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }

        if(dire>=0 && transform.rotation.eulerAngles.y != 0f)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        if (durabilidade>0)
        {
            if (tempoLife > tempo)
            {
                tempo += Time.deltaTime;

                if (indo)
                {
                    if (dire > 0)
                    {
                        if (posFinal.x < transform.position.x)
                        {
                            InverteDirecao();
                        }
                    }else
                    {
                        if (posFinal.x > transform.position.x)
                        {
                            InverteDirecao();
                        }
                    }
                }
                else
                {
                    if (dire > 0)
                    {
                        if (posOrigin.x < transform.position.x)
                        {
                            InverteDirecao();
                        }
                    }
                    else
                    {
                        if (posOrigin.x > transform.position.x)
                        {
                            InverteDirecao();
                        }
                    }
                }

                if (rbd2d.velocity.x * dire < speed)
                {
                    float speedX = speed*dire -rbd2d.velocity.x;
                    rbd2d.AddForce(Vector2.right * speedX, ForceMode2D.Impulse);
                }

                if (ignorados.Count > 0)
                {
                    if (tempoIgnorados <= 0f)
                    {
                        auxiliares.RemoverIgnoradosColiders(ignorados, esteColl);
                    }
                    else
                    {
                        tempoIgnorados -= Time.deltaTime;
                    }
                }
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            if (tempoSom < acertando.length)
            {
                if (tempoSom == 0f)
                {
                    rederer.enabled = false;
                    esteColl.enabled = false;
                }
                tempoSom += Time.deltaTime;
            }else
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void Respaw(bool direita, Vector2 pos0, float damage, float multiplo, bool critical, float pain, Collider2D pai,int numeroAcao,itensUsaveis itemUsado)
    {
        tempo = 0f;
        esteColl.enabled = true;
        rederer.enabled = true;
        durabilidade = 3;
        if (direita)
        {
            posOrigin = pos0;
            posFinal = pos0 + Vector2.right * distMax;
            dire = 1;
        }
        else
        {
            posOrigin = pos0;
            posFinal = pos0 + Vector2.left * distMax;
            dire = -1;
        }
        auxiliares.RemoverIgnoradosColiders(ignorados, esteColl);
        critico = critical;
        tempoSom = 0f;
        dano = damage;
        mult = multiplo / (4f * auxiliares.VantagemSpeedAreaT(speed, tempoLife, esteColl.bounds.size,1f));
        transform.position = pos0;
        dor = pain;
        tempoIgnorados = 0f;
        aliado = pai.CompareTag("Player");
        podePegarDeVolta = false;
        acao = numeroAcao;
        gameObject.SetActive(true);
        dono = pai;
        indo = true;
        itemX = itemUsado;
    }

    private void InverteDirecao()
    {
        indo = !indo;
        dire *= -1;
        if (!podePegarDeVolta)
        {
            podePegarDeVolta = true;
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            if (!aliado)
            {
                atributosHeroi atriHeroi = coll.gameObject.GetComponent<atributosHeroi>();
                float damages = 0f;
                if (dano > 0f && mult > 0f)
                {
                    damages = dano / (atriHeroi.defesa + dano);
                    damages *= dano * mult;
                }
                if (atriHeroi.DefendeuSim(transform.position.x))
                {
                    esteAudio.PlayOneShot(acertando);
                    durabilidade -= 1;
                    float t = 1f;
                    if (dor > 0f)
                    {
                        t = dor;
                    }
                    atriHeroi.Denfender(damages, t, critico);
                }
                else if (atriHeroi.esquiva > Random.value)
                {
                    atriHeroi.AplicarDano(0f, dor, true, Vector2.zero, critico);
                    Physics2D.IgnoreCollision(coll, esteColl, true);
                    tempoIgnorados = 1f;
                }
                else
                {
                    esteAudio.PlayOneShot(acertando);
                    durabilidade -= 1;
                    atriHeroi.AplicarDano(damages, dor, false, forca, critico);
                }
            }else if (podePegarDeVolta && mochilaCalanguito.mochila.ColocarUmItem(itemX))
            {
                gameObject.SetActive(false);
            }
        }else if (coll.CompareTag("Inimigo"))
        {
            atributosInimigos atr = coll.gameObject.GetComponent<atributosInimigos>();
            if (aliado)
            {
                float damages = 0f;
                if (dano > 0f && mult>0f)
                {
                    damages = dano / (atr.defesa + dano);
                    damages *= dano * mult;
                }

                if (!atr.DefendeuSim(damages, dor, transform.position.x, critico))
                {
                    durabilidade -= 1;
                    esteAudio.PlayOneShot(acertando);
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
                        esteAudio.PlayOneShot(acertando);
                    }
                }
            }else if(coll == dono && podePegarDeVolta)
            {
                atr.acaoARepetir = acao;
                atr.repetirAcao = true;
                gameObject.SetActive(false);
            }
        }
        else if (!coll.isTrigger)
        {
            durabilidade -= 1;
            esteAudio.PlayOneShot(acertando);
            InverteDirecao();
        }
    }
}
