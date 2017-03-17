using UnityEngine;
using System.Collections;

public class DragEspada : MonoBehaviour {
	private Animator anima;
	private moveCPU movimento;
	private atributosInimigos atributos;
	private float tempoAcao = 0f;
	public AudioClip errou;
	private AudioSource espadaAudio;
	private int acao = 0;
	private float tempoAcoes = 0f;
	private float tempoX;
	private Renderer renderizado;
	// Use this for initialization
	void Start () {
		anima = GetComponent<Animator> ();
		movimento = GetComponent<moveCPU> ();
		atributos = GetComponent<atributosInimigos> ();
		espadaAudio = GetComponentInChildren<AudioSource> ();
		renderizado = GetComponent<Renderer> ();
		SuaVez ();
		tempoX = atributos.TempoReacaoCalc ();
	}
	
	// Update is called once per frame
	void Update () {
		if (fases.podeMover) {
			if (movimento.viu) {
				if (renderizado.isVisible) {
					if (atributos.estados == 0) {
						if (acao == 0) {
							movimento.SeAfastar (1.25f, true);
						} else {
							//float distY = movimento.player.transform.position.y - transform.position.y;
							if (movimento.DistaciaYPega (0.34f,atributosHeroi.heroiX.transform.position.y) && movimento.DistaciaX (transform.position.x, atributosHeroi.heroiX.transform.position.x) < 0.34f && movimento.SemObstaculosEntrePlay()) {
								Atacar ();
							} else {
                                if (movimento.MesmaAltura())
                                {
                                    movimento.SeAproximar(0.34f, false);
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
					}
				} else {
					movimento.SeAproximar (1.5f, true);
				}
			}
		}
		// fazendo ações
		if (anima.GetInteger ("estados") == 1) {
			tempoAcao += Time.deltaTime;
			if (tempoAcao > 1f / atributos.agilidade) {
				anima.speed = 1f;
				atributos.estados = 0;
				anima.SetInteger ("estados", 0);
			}
		} else if (anima.GetInteger ("estados") == 0) {
			anima.speed = 1f;
		}
	}

	public void Atacar(){
		if (movimento.noChao && anima.GetInteger ("estados") == 0) {
			movimento.VirarParaPlayer ();
			espadaAudio.pitch = atributos.agilidade;
			espadaAudio.clip = errou;
			espadaAudio.Play ();
			tempoAcao = 0f;
			anima.speed = atributos.agilidade;
			atributos.estados = 1;
			anima.SetInteger ("estados", 1);
			SuaVez ();
            movimento.NaoDeslizar();
		}
	}

	private void SuaVez(){
		if (atributos.ChancesDeReagir(movimento.encurralado) > Random.value) {
			acao = 1;
		} else {
			acao = 0;
		}
	}
}
