using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class fases : MonoBehaviour {
	public AudioClip musicaDaFase,morteSom,pegandoItem,encontrado,pegandoMoeda;
	public float konstTempo=1f;
	private float tempo =0f;
	public Text scoresTexto;
	public Text tempoText;
	public static bool podeMover = true;
	public  AudioSource esteAudio;
	//private Camera cam;
	public string idiomaEscolhido;
	//public GameObject player;
	public GameObject[] plataformas;
	public LayerMask playMask,iniMask,waterMask;
    public LayerMask chao;
    public static fases faseX;
    // Use this for initialization
    public Slider hpBoss;
    private Text nomeBoss;
    public Sprite bufDef, bufRege, bufSpeed, bufForca, bufAgil;
    public Sprite dBufDef, dBufRege, dBufSpeed, dBufForca, dBufAgil;
    public AudioClip musicaFalaBoss,musicaTensa;
    private bool bossMorto = false;
    public bool bossPodeMorrer = true;
    public GameObject fundoBranco;
    public GameObject caixaMensagens;
    public Text quemfala;
    public Text fala;
    private AudioClip clipAnterior;
    public atributosInimigos[] inimigos;
    public GameObject[] coletaveis;
    private int escoreGravado;
    public int inimigosMortos, tesouros, jaulasAbertas;
    public GameObject canvasPassarDeFase,canvasPrincipal;

	void Awake(){
        bossMorto = false;
		tempo = 0f;
		idiomaEscolhido = PlayerPrefs.GetString ("Idioma");
		AtulizarTempo ();
		AtulizarScores ();
        nomeBoss=hpBoss.GetComponentInChildren<Text>();
        esteAudio = GetComponent<AudioSource>();
        faseX = this;
        plataformas = GameObject.FindGameObjectsWithTag("Chao");
        hpBoss.gameObject.SetActive(false);
        inimigos = FindObjectsOfType<atributosInimigos>();
        coletaveis = GameObject.FindGameObjectsWithTag("Coletaveis");
        if (dataSave.jogoAtual.voltarCkeckPoint)
        {
            ResetarCheckPoint();
        }

        AtulizarScores();
    }

    // Update is called once per frame
    void Update () {
		if (podeMover) {
			//Debug.Log (scores);
			tempo+= Time.deltaTime;
			AtulizarTempo ();
		}

        if(hpBoss.value<=0f && !bossMorto)
        {
            hpBoss.gameObject.SetActive(false);
            bossMorto = true;
        }
        if (bossMorto && !fundoBranco.activeInHierarchy && bossPodeMorrer)
        {
            fundoBranco.SetActive(true);
            bossPodeMorrer = false;
            fases.podeMover = false;
        }

        if (escoreGravado != dataSave.jogoAtual.escores)
        {
            AtulizarScores();
        }
    }

    public AudioClip MusicaFase()
    {
        return esteAudio.clip;
    }

    public void ResetarCheckPoint()
    {
        atributosHeroi.heroiX.GetComponent<atributosHeroi>().hpAtual = dataSave.jogoAtual.hpHero;
        atributosHeroi.heroiX.transform.position = dataSave.jogoAtual.PosHero();

        tempo = dataSave.jogoAtual.tempoFase;

        for (int i = 0; i < dataSave.jogoAtual.inimigosInformacoes.Length; i++)
        {       
            if (dataSave.jogoAtual.inimigosInformacoes[i].morto)
            {
                inimigos[i].gameObject.SetActive(false);
            }else
            {
                inimigos[i].transform.position = dataSave.jogoAtual.inimigosInformacoes[i].PosGravada();
            }
        }

        for (int i = 0; i < dataSave.jogoAtual.coisasNaoPegas.Length; i++)
        {
            if (!dataSave.jogoAtual.coisasNaoPegas[i])
            {
                coletaveis[i].gameObject.SetActive(false);
            }
        }
    }

    public float GetTempo()
    {
        return tempo;
    }

    public Slider HpBossUtilizar(string nome)
    {
        nomeBoss.text = nome;
        bossMorto = false;
        return hpBoss;
    }

    public void TrocarMusica(AudioClip clip)
    {
        if (esteAudio.clip)
        {
            clipAnterior = esteAudio.clip;
        }
        esteAudio.clip = null;
        esteAudio.Stop();
        esteAudio.clip = clip;
        esteAudio.Play();
    }

    public void VoltarMusicaAnterior()
    {
        esteAudio.clip = null;
        esteAudio.Stop();
        esteAudio.clip = clipAnterior;
        esteAudio.Play();
    }

    public void PausePlaySomFase(bool pausa)
    {
        if (pausa)
        {
            esteAudio.Pause();
        }else
        {
            esteAudio.Play();
        }
    }

    public void LutaBossComeco()
    {
        hpBoss.gameObject.SetActive(true);
    }

	private void AtulizarScores(){
        escoreGravado = dataSave.jogoAtual.escores;
		if (scoresTexto) {
			if (idiomaEscolhido == "Portugueis") {
                scoresTexto.text = "Pontuação: " + dataSave.jogoAtual.escores;
			} else if (idiomaEscolhido == "Ingles") {
				scoresTexto.text = "Scores: " + dataSave.jogoAtual.escores;
			}
		}
	}


	private void AtulizarTempo(){
		if (tempoText) {
			if (idiomaEscolhido == "Portugueis") {
				tempoText.text = "Tempo: " + (int)tempo;
			} else if (idiomaEscolhido == "Ingles") {
				//Debug.Log (scoresTexto + " " + scores);
				tempoText.text = "Time: " + (int)tempo;
			}
		}
	}

	public int PontuacaoTempo(){
		int pts = (int)(konstTempo*200f / (konstTempo + tempo));
        upamento.AddScores(pts);
		return pts;
	}

	public void ZerarTempo(){
		tempo = 0f;
	}

    public void PassarDeFase()
    {
        canvasPrincipal.SetActive(false);
        canvasPassarDeFase.SetActive(true);
    }
}
