using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosoes : MonoBehaviour {
    private ParticleSystem particulas;
    private AudioSource esteAudio;
    public AudioClip explosoesSom, finalExplosao;
    private Collider2D esteColl;
    public bool explodir;
    public float duracaoExplosao = 2f;
    private float tempo = 0f;

	void Awake () {
        esteAudio = GetComponentInChildren<AudioSource>();
        particulas = GetComponentInChildren<ParticleSystem>();
        esteColl = GetComponent<Collider2D>();
	}
	
	// Update is called once per frame
	void Update () {
        if (explodir)
        {
            if (tempo == 0f)
            {
                particulas.Play();
                esteAudio.loop = true;
                esteAudio.clip = explosoesSom;
                esteAudio.Play();
            }
            if (tempo<duracaoExplosao)
            {
                tempo += Time.deltaTime;
            }else if(tempo<duracaoExplosao+1f)
            {
                if (esteAudio.loop)
                {
                    esteAudio.clip = null;
                    particulas.Stop();
                    auxiliares.DeixarTudoInvisivel(gameObject);
                    esteAudio.loop = false;
                    esteAudio.PlayOneShot(finalExplosao);
                    if (esteColl)
                    {
                        esteColl.enabled = false;
                    }
                }
                tempo += Time.deltaTime;
            }else
            {
                gameObject.SetActive(false);
            }
        }
	}
}
