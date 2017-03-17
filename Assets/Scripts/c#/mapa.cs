using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class mapa : MonoBehaviour {
    public GameObject[] locais;
    public GameObject aviso,confirma;
    public static bool podeEscolher = true;
    private string mapaIr;
    public Text confirmaT, voltar, infMapa,confimaTextMapa;
    private AudioSource esteAudio;
	// Use this for initialization
	void Awake () {
        MapasAMostra();
        esteAudio = GetComponent<AudioSource>();
	}

    private void MapasAMostra()
    {
        for(int i = 0; i < locais.Length; i++)
        {
            if (dataSave.jogoAtual.TemEssaFase(locais[i].name))
            {
                locais[i].SetActive(true);
            }else
            {
                locais[i].SetActive(false);
            }
        }
    }


    public void MapaEscolhio(string nome)
    {
        if (podeEscolher)
        {
            mapaIr = nome;
            podeEscolher = false;
            if (idioma.idiomaEscolhido == "Portugueis")
            {
                confimaTextMapa.text = "Você deseja ir para este local ?";
                if (nome == "prologo")
                {
                    infMapa.text = "Prólogo";
                }else if(nome == "Distrito da Terra")
                {
                    infMapa.text = "Distrito da Terra";
                }
            }
        }
    }

    public void ConfirmaEscolha()
    {
        auxiliares.CompletoSom(esteAudio);
        SceneManager.LoadSceneAsync(mapaIr);
    }
}
