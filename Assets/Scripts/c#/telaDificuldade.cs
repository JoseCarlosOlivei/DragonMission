using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class telaDificuldade : MonoBehaviour {
	private string idioma;
	public Text txDidiculdade, txDescricao, txMuitoFacil, txFacil, txNormal, txDificil, txMuitoDificil, txInsano;
	public Color escolhidoCor;
	private string desMuitoFacil, desFacil, desNormal, desDificil, desMuitoDificil, desInsano;
    private AudioSource esteAudio;
    public GameObject load;

	// Use this for initialization
	void Awake () {
		idioma = PlayerPrefs.GetString ("Idioma");
		IniciarTela ();
        esteAudio = GetComponent<AudioSource>();
	}

    public void SomEscolha()
    {
        esteAudio.PlayOneShot(auxiliares.botao);
    }

	public void MuitoFacil(){
		Decelecioar ();
		txMuitoFacil.color = escolhidoCor;
		dataSave.jogoAtual.dificuldade = 0.25f;
		txDescricao.text = desMuitoFacil;
        SomEscolha();
	}

	public void Facil(){
		Decelecioar ();
		txFacil.color = escolhidoCor;
		dataSave.jogoAtual.dificuldade = 0.5f;
		txDescricao.text = desFacil;
        SomEscolha();
    }

    public void Normal(){
		Decelecioar ();
		txNormal.color = escolhidoCor;
		dataSave.jogoAtual.dificuldade = 1f;
		txDescricao.text = desNormal;
        SomEscolha();
    }

    public void Dificil(){
		Decelecioar ();
		txDificil.color = escolhidoCor;
		dataSave.jogoAtual.dificuldade = 2f;
		txDescricao.text = desDificil;
        SomEscolha();
    }

    public void MuitoDificil(){
		Decelecioar ();
		txMuitoDificil.color = escolhidoCor;
		dataSave.jogoAtual.dificuldade = 4f;
		txDescricao.text = desMuitoDificil;
        SomEscolha();
    }

    public void Insano(){
		Decelecioar ();
		txInsano.color = escolhidoCor;
		dataSave.jogoAtual.dificuldade = 8f;
		txDescricao.text = desInsano;
        SomEscolha();
    }

    private void Decelecioar(){
		if (dataSave.jogoAtual.dificuldade == 0.25f) {
			txMuitoFacil.color = Color.white;
		}else if(dataSave.jogoAtual.dificuldade == 0.5f){
			txFacil.color = Color.white;
		}else if(dataSave.jogoAtual.dificuldade == 1f){
			txNormal.color = Color.white;
		}else if(dataSave.jogoAtual.dificuldade == 2f){
			txDificil.color = Color.white;
		}else if(dataSave.jogoAtual.dificuldade == 4f){
			txMuitoDificil.color = Color.white;
		}else if(dataSave.jogoAtual.dificuldade == 8f){
			txInsano.color = Color.white;
		}
	}

    public void Sair()
    {
        load.SetActive(true);
        esteAudio.PlayOneShot(auxiliares.completoClick);
        dataSave.jogoAtual.ResetarGravacao();
        dataSave.jogoAtual.AddFaseNova("prologo");
        dataSave.jogoAtual.Salvar("prologo");
        SceneManager.LoadSceneAsync("prologo");
    }

	private void DescricoesInstancia(){
		if (idioma == "Portugueis") {
			desMuitoFacil = "Inimigos dão um quarto de dano e recebem quatro vezes mais dano, você recebe menos escores nesta dificuldade";
			desFacil = "Inimigos dão metade de dano e recebem duas vezes mais dano, você recebe menos escores nesta dificuldade";
			desNormal = "Inimigos dão e recebem dano normal, você recebe a quantia normal de escores nesta dificuldade";
			desDificil = "Inimigos dão dobro de dano e recebem metade de dano, você recebe mais escores nesta dificuldade";
			desMuitoDificil = "Inimigos dão quatro vezes mais dano  e recebem um quarto de dano, você recebe mais escores nesta dificuldade";
			desInsano = "Inimigos dão um dano insano e recebem um oitavo de dano, você recebe uma quantia insana de escores nesta dificuldade";
		} else if (idioma == "Ingles") {
			desMuitoFacil = "1/4";
			desFacil = "1/2";
			desNormal = "1";
			desDificil = "2";
			desMuitoDificil = "4";
			desInsano = ":O";
		}
	}

	private void IniciarTela(){
		DescricoesInstancia ();
		if (!dataSave.jogoAtual.finalizou && txInsano.gameObject.activeInHierarchy) {
			txInsano.GetComponentInParent<Button> ().gameObject.SetActive (false);
		}
		if (dataSave.jogoAtual.dificuldade == 0.25f) {
			txMuitoFacil.color = escolhidoCor;
			txDescricao.text = desMuitoFacil;
		}else if(dataSave.jogoAtual.dificuldade == 0.5f){
			txFacil.color = escolhidoCor;
			txDescricao.text = desFacil;
		}else if(dataSave.jogoAtual.dificuldade == 1f){
			txNormal.color = escolhidoCor;
			txDescricao.text = desNormal;
		}else if(dataSave.jogoAtual.dificuldade == 2f){
			txDescricao.text = desDificil;
		}else if(dataSave.jogoAtual.dificuldade == 4f){
			txDescricao.text = desMuitoDificil;
		}else if(dataSave.jogoAtual.dificuldade == 8f){
			txInsano.text = desInsano;
		}
		if (idioma == "Portugueis") {
			txMuitoFacil.text = "Muito fácil";
			txFacil.text = "Fácil";
			txNormal.text = "Normal";
			txDificil.text = "Difícil";
			txMuitoDificil.text = "Muito difícil";
			txDidiculdade.text = "Dificuldade";
			txInsano.text = "Insano";
		} else if (idioma == "Ingles") {
			txMuitoFacil.text = "Very easy";
			txFacil.text = "Easy";
			txNormal.text = "Normal";
			txDificil.text = "Hard";
			txMuitoDificil.text = "Very Hard";
			txInsano.text = "Insane";
			txDidiculdade.text = "Difficulty";
		}
	}
		
}
