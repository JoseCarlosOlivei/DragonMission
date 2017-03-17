using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class faseCompleta : MonoBehaviour {
    public Text clearStage, bonusText,infText, continuar;
    private AudioSource saidaBotao;
    public AudioClip textAparicao;
    public GameObject load;

	// Use this for initialization
	void Awake () {
        saidaBotao = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private int BonusPorHp()
    {
        int porct = (int)(atributosHeroi.heroiX.GetComponent<atributosHeroi>().hpBarra.normalizedValue * 100f);

        upamento.AddScores(porct);

        return porct;
    }

    public void AtualizarInf()
    {
        saidaBotao.PlayOneShot(textAparicao);
        int pT = fases.faseX.PontuacaoTempo();
        int pHp = BonusPorHp();
        if (idioma.idiomaEscolhido == "Portugueis")
        {     
            clearStage.text = "Fase Completa";
            bonusText.text = "Pontuação Bônus";
            infText.text = "Bônus por Tempo: " + pT + "\n" + "Bônus por Hp Restante: " + pHp + "\n" + "Bônus Total: " + (pHp + pT);
            continuar.text = "Continuar";
        }else if(idioma.idiomaEscolhido == "Ingles")
        {
            clearStage.text = "Stage Clear";
            bonusText.text = "Bonus Scores";
            infText.text = "Time Bonus: " + pT + "\n" + "Bonus for remaining Hp: " + pHp + "\n" + "Total Bonus: " + (pHp + pT);
            continuar.text = "Continue";
        }
    }

    public void Continuar()
    {
        auxiliares.CompletoSom(saidaBotao);
        load.SetActive(true);
        SceneManager.LoadSceneAsync("mapaMunde");
    }
}
