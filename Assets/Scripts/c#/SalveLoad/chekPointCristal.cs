using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class chekPointCristal : MonoBehaviour {
    public AudioClip somSalve;
    private AudioSource esteAudio;
    public AudioSource somSaida;
	// Use this for initialization
	void Awake () {
        esteAudio = GetComponent<AudioSource>();
	}
	
	private void SalvarNestePonto()
    {
        dataSave.jogoAtual.Salvar(mochilaCalanguito.mochila.GetItensUsaveis(), fases.faseX.inimigos, SceneManager.GetActiveScene().name,atributosHeroi.heroiX,fases.faseX.coletaveis,fases.faseX.GetTempo());
        somSaida.PlayOneShot(somSalve);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        esteAudio.Play();
    }

    void OnTriggerStay2D(Collider2D collision)
    {      
        if (Input.GetButtonDown("Passar"))
        {
            SalvarNestePonto();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        esteAudio.Pause();
    }
}
