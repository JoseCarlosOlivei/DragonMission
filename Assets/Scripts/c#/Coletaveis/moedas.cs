using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moedas : MonoBehaviour {
    private Collider2D esteColl;
    private ParticleSystem particulas;
    public int valor = 5;
    private bool pego = false;
    private float tPegado = 0f;
    private Renderer reder;
    private AudioSource esteAudio;
	// Use this for initialization
	void Awake () {
        esteColl = GetComponent<Collider2D>();
        particulas = GetComponentInChildren<ParticleSystem>();
        reder = GetComponent<Renderer>();
        esteAudio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        if (pego)
        {
            if (tPegado > 3)
            {
                gameObject.SetActive(false);
            }else
            {
                tPegado += Time.deltaTime;
            }
        }else
        {
            Pegado();
        }
	}

    private void Pegado()
    {
        Vector2 p0 = esteColl.bounds.center;
        Vector2 p1 = p0;
        p0.x -= esteColl.bounds.extents.x;
        p0.y -= esteColl.bounds.extents.y;
        p1.x += esteColl.bounds.extents.x;
        p1.y += esteColl.bounds.extents.y;

        Collider2D coll = Physics2D.OverlapArea(p0,p1,fases.faseX.playMask);

        if (coll)
        {
            pego = true;
            upamento.AddScores(valor);
            reder.enabled = false;
            particulas.Stop();
            esteAudio.PlayOneShot(fases.faseX.pegandoMoeda);
        }
    }
}
