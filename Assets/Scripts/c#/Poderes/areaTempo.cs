using UnityEngine;
using System.Collections;

public class areaTempo : MonoBehaviour
{
    public float tempoDeVida = 0f;
    private float durou = 0f;
    public float dano;
    private bool foiCritico;
    private float multiplo;
    private Collider2D esteColl;
    //public AudioClip som;
    // Use this for initialization
    void Awake()
    {
        esteColl = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (durou >= tempoDeVida)
        {
            gameObject.SetActive(false);
            durou = 0f;
        }
        else
        {
            durou += Time.deltaTime;
        }
        EstaNaArea();
    }

    public void Respaw(Vector2 pos, float damage, bool critico, float mult)
    {
        transform.position = pos;
        dano = damage;
        multiplo = mult / tempoDeVida;
        foiCritico = critico;
        gameObject.SetActive(true);
    }

    private void EstaNaArea()
    {
        Collider2D[] colls = Physics2D.OverlapBoxAll(esteColl.bounds.center, esteColl.bounds.extents,0f);
         for(int i = 0; i < colls.Length; i++)
        {
            if (dano > 0f)
            {
                if (colls[i].tag == "Inimigo")
                {
                    atributosInimigos atb = colls[i].GetComponent<atributosInimigos>();
                    float danoX = dano / (atb.defesa + dano);
                    danoX *= dano * multiplo * Time.deltaTime;
                    danoX /= atb.PoderEsquiva();
                    atb.AplicarDano(danoX, 0, false, Vector2.zero, foiCritico);
                }
            }
        }
    }
}
