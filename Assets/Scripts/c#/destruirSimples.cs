using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destruirSimples : MonoBehaviour {
    private ParticleSystem particulas;
    public Sprite destruidoSprite;
    private SpriteRenderer redere;
    private AudioSource esteAudio;
    public AudioClip somDestruindo;
	// Use this for initialization
	void Awake () {
        particulas = GetComponent<ParticleSystem>();
        redere = GetComponent<SpriteRenderer>();
        esteAudio = GetComponent<AudioSource>();
	}
	
	public void Destruir()
    {
        redere.sprite = destruidoSprite;
        particulas.Play();
        esteAudio.PlayOneShot(somDestruindo);
    }
}
