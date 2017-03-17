using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pause : MonoBehaviour {
    private AudioSource esteAudio;
    public GameObject pausaObj,loand;
    public static pause pausaX;
	// Use this for initialization
	void Awake () {
        pausaX = this;
        esteAudio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Pausa"))
        {
            if(Time.timeScale > 0f && fases.podeMover)
            {
                Time.timeScale = 0f;
                pausaObj.SetActive(true);
                fases.faseX.PausePlaySomFase(true);
                fases.podeMover = false;
                auxiliares.CompletoSom(esteAudio);
            }else if(Time.timeScale == 0f){
                Despausar();
            }
        }
	}

    public void Despausar()
    {
        Time.timeScale = 1f;
        pausaObj.SetActive(false);
        fases.faseX.PausePlaySomFase(false);
        fases.podeMover = true;
        auxiliares.CompletoSom(esteAudio);
    }

    public void VoltarChkPoint()
    {
        Time.timeScale = 1f;
        loand.SetActive(true);
        dataSave.jogoAtual.Carregar();
    }

    public void IrMapaMunde()
    {
        Time.timeScale = 1f;
        loand.SetActive(true);
        dataSave.jogoAtual.IrMapaMunde();
    }

    public void EscolherDificuldade()
    {
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync("dificuldade");
        loand.SetActive(true);
    }
}
