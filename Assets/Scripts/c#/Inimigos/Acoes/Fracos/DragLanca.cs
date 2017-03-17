using UnityEngine;
using System.Collections;

public class DragLanca : MonoBehaviour {

	private Animator anima;
	private moveCPU movimento;
	private atributosInimigos atributos;
	private float tempoAcao = 0f;
	private float auxAudioGolpes = 0f;
	public AudioClip errou;
	private AudioSource lancaAudio;
	//private armaDragLanca lanca;
	private int acao = 0;
	private float tempoAcoes = 0f;
	private float tempoX;
	private float[] acoesFre = new float[2];
	private float kAcoes = 0f;
	private Renderer renderizado;
	// Use this for initialization
	void Start () {
		anima = GetComponent<Animator> ();
		movimento = GetComponent<moveCPU> ();
		atributos = GetComponent<atributosInimigos> ();
		lancaAudio = GetComponentInChildren<armaDragLanca> ().gameObject.GetComponent<AudioSource>();
		renderizado = GetComponent<Renderer> ();
		SuaVez ();
		tempoX = atributos.TempoReacaoCalc ();
		//lanca = GetComponentInChildren<armaDragLanca> ();
		acoesFre [0] = 16f*1.6f;
		acoesFre [1] = 5f;
		for (int i = 0; i < acoesFre.Length; i++) {
			kAcoes += acoesFre [i];
		}
	}

	// Update is called once per frame
	void Update () {
		if (fases.podeMover) {
			if (movimento.viu) {
				if (atributos.estados == 0) {
					if (renderizado.isVisible) {
						if (acao == 0) {
							movimento.SeAfastar (1f, true);
						} else if (acao == 1) {
							//atacar normal
							if (movimento.MesmaAltura() && movimento.DistaciaX (transform.position.x, atributosHeroi.heroiX.transform.position.x) < 0.5f && movimento.SemObstaculosEntrePlay()) {
								Atacar ();
							} else {
                                if (movimento.MesmaAltura())
                                {
                                    movimento.SeAproximar(0.5f, false);
                                }else
                                {
                                    movimento.SeAproximar(movimento.xColl, false);
                                }
							}
						} else if (acao == 2) {
							if (movimento.MesmaAltura() && movimento.DistaciaX (transform.position.x, atributosHeroi.heroiX.transform.position.x) < 0.4f && movimento.SemObstaculosEntrePlay()) {
								PoderIniciar ();
							} else {
                                if (movimento.MesmaAltura())
                                {
                                    movimento.SeAproximar(0.4f, false);
                                }else
                                {
                                    movimento.SeAproximar(movimento.xColl, false);
                                }
							}
						}
						// trocar ação
						if (tempoX < tempoAcoes) {
							tempoX = atributos.TempoReacaoCalc ();
							tempoAcoes = 0f;
							SuaVez ();
						} else {
							tempoAcoes += Time.deltaTime;
						}
					} else {
						movimento.SeAproximar (1.5f, true);
					}
				} 
			}
		}
		// fazendo ações
		if (anima.GetInteger ("estados") == 1) {
			if (anima.GetFloat ("qualPoder") == 0f) {
				tempoAcao += Time.deltaTime;
				if (tempoAcao > 1f / atributos.agilidade) {
					anima.speed = 1f;
					atributos.estados = 0;
					anima.SetInteger ("estados", 0);
				}
			} else if(anima.GetFloat ("qualPoder") == 1f){
				// começo poder
				tempoAcao += Time.deltaTime;
				if (tempoAcao > 0.5f) {
					anima.SetFloat ("qualPoder", 2f);
					tempoAcoes = 0f;
				}
			}else if(anima.GetFloat ("qualPoder") == 2f){
				// fazendo poder
				tempoAcao += Time.deltaTime;
				if (auxAudioGolpes == 0f) {
					//Debug.Log (2);
					lancaAudio.pitch = 8f;
					lancaAudio.clip = errou;
					lancaAudio.Play ();
					auxAudioGolpes += Time.deltaTime;
				} else if (auxAudioGolpes >= 1f / 8f) {
					auxAudioGolpes = 0f;
				} else {
					auxAudioGolpes += Time.deltaTime;
				}
				if (tempoAcao > 2f) {
					lancaAudio.pitch = 1f;
					lancaAudio.clip = errou;
					lancaAudio.Play ();
					anima.SetFloat ("qualPoder", 3f);
					tempoAcoes = 0f;
					auxAudioGolpes = 0f;
				}
			}else if(anima.GetFloat ("qualPoder") == 3f){
				// final poder
				//Debug.Log(tempoAcoes);
				tempoAcoes += Time.deltaTime;
				 if (tempoAcoes > 0.5f) {
					anima.speed = 1f;
					atributos.estados = 0;
					//Debug.Log (1);
					anima.SetInteger ("estados", 0);
					anima.SetFloat ("qualPoder", 0f);
					tempoAcoes = 0f;
				}
				//Debug.Log (tempoAcoes);
			}
		} else if (anima.GetInteger ("estados") == 0) {
			anima.speed = 1f;
		}
	}

	public void Atacar(){
		if (movimento.noChao && anima.GetInteger ("estados") == 0) {
			anima.SetFloat ("qualPoder", 0f);
			movimento.VirarParaPlayer ();
			lancaAudio.pitch = 1f;
			lancaAudio.clip = errou;
			lancaAudio.Play ();
			tempoAcao = 0f;
			anima.speed = atributos.agilidade;
			atributos.estados = 1;
            movimento.NaoDeslizar();
			anima.SetInteger ("estados", 1);
			SuaVez ();
		} 
	}

	public void PoderIniciar(){
		if (movimento.noChao && anima.GetInteger ("estados") == 0) {
			anima.SetFloat ("qualPoder", 1f);
			movimento.VirarParaPlayer ();
			lancaAudio.pitch = 1f;
			lancaAudio.clip = errou;
			lancaAudio.Play ();
			tempoAcao = 0f;
			auxAudioGolpes = 0f;
			anima.speed = 1f;
            movimento.NaoDeslizar();
			atributos.estados = 1;
			anima.SetInteger ("estados", 1);
			SuaVez ();
		} 
	}

	private void SuaVez(){
		if (atributos.ChancesDeReagir(movimento.encurralado) > Random.value) {
			float sort = Random.Range (0f, kAcoes);
			float aux = 0f;
			//Debug.Log (sort);
			for (int i = 0; i < acoesFre.Length; i++) {
				//Debug.Log (aux);
				aux += acoesFre[i];
				if (aux >= sort) {
					acao = i + 1;
					//Debug.Log (acao);
					i = acoesFre.Length;
				}
			}
		} else {
			acao = 0;
		}
	}
}
