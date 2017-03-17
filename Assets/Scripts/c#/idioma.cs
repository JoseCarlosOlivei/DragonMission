using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class idioma : MonoBehaviour {
	public Text mesagemEscolhido,escolhaText;
	public Text confirmaText;
	public static string idiomaEscolhido;
    public Scrollbar volumeSlide;
    public GameObject load;

	//PlayerPrefs.Set
	// Use this for initialization
	void Awake () {
		if (PlayerPrefs.GetString ("Idioma") == null || PlayerPrefs.GetString ("Idioma") == "") {
			PlayerPrefs.SetString ("Idioma","Ingles");
		} 
		idiomaEscolhido = PlayerPrefs.GetString ("Idioma");
		IniciarTextos ();
        volumeSlide.value = dataSave.GetVolume();
	}

	public void IdiomaBr(){
		PlayerPrefs.SetString ("Idioma", "Portugueis");
		mesagemEscolhido.text = "Escolheste o português como idioma.";
        escolhaText.text = "Escolha um idioma :";
		confirmaText.text = "Confirma";
		idiomaEscolhido = "Portugueis";
	}
	public void IdiomaUSA(){
		PlayerPrefs.SetString ("Idioma", "Ingles");
		mesagemEscolhido.text = "Chosen english as the language.";
        escolhaText.text = "Choose a language :";
		confirmaText.text = "Confirms";
		idiomaEscolhido = "Ingles";
	}

	private void IniciarTextos(){
		if (idiomaEscolhido == "Portugueis") {
			mesagemEscolhido.text = "Escolheste o português como idioma.";
            escolhaText.text = "Escolha um idioma :";
            confirmaText.text = "Confirma";
		} else if(idiomaEscolhido == "Ingles"){
			mesagemEscolhido.text = "Chosen english as the language.";
            escolhaText.text = "Choose a language :";
            confirmaText.text = "Confirms";
		}
	}

    public void AlterarVolume(float valor)
    {
        dataSave.SetVolume(valor);
    }

	public void Confirma(){
        SceneManager.LoadSceneAsync(auxiliares.idCena);
        load.SetActive(true);
	}
}
