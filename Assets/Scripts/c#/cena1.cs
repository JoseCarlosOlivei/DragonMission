using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class cena1 : MonoBehaviour {
    private bool escolherIdioma = true;
	// Use this for initialization
	void Awake () {
		if (PlayerPrefs.GetString("Idioma") == null || PlayerPrefs.GetString("Idioma") == "")
        {
            escolherIdioma = false;
        }else
        {
            idioma.idiomaEscolhido = PlayerPrefs.GetString("Idioma");
        }
    }
    

    public void CarregarCena()
    {
        if (escolherIdioma)
        {
            SceneManager.LoadSceneAsync("salveLoad");
        }else
        {
            auxiliares.idCena = SceneManager.GetSceneByName("salveLoad").buildIndex;
            SceneManager.LoadSceneAsync("opcoes");
        }
    }
}
