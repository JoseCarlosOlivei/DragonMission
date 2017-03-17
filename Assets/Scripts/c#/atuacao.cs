using UnityEngine;
using System.Collections;

public class atuacao : MonoBehaviour {
	private float tempoFala = 0f;
	private Animator anima;
	public AudioClip falaSom;
	private AudioSource esteAudio;
    public float speedFala = 1f;
	// Use this for initialization
	void Awake () {
		anima = GetComponent<Animator> ();
		esteAudio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if (tempoFala > 0f) {
			tempoFala -= Time.deltaTime;
		} else if(anima.GetFloat("falando") != 0f){
			esteAudio.loop = false;
			anima.SetFloat ("falando", 0f);
		}

        if(anima.GetFloat("falando") == 0f && esteAudio.clip == falaSom)
        {
            esteAudio.clip = null;
        }
	}

	public void Atuando(int exprecao,int nChatateres){
       float fala = 0;
       if(nChatateres>0){
			fala = 1f;
			tempoFala = nChatateres/(40f*speedFala);
			esteAudio.loop = true;
			esteAudio.pitch = speedFala;
			esteAudio.clip = falaSom;
			esteAudio.Play ();
        }
		anima.SetFloat ("atuacao",exprecao);
		anima.SetFloat ("falando",fala);
	}
}
