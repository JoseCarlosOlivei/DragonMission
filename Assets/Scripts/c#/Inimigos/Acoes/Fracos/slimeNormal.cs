using UnityEngine;
using System.Collections;

public class slimeNormal : MonoBehaviour {

	private Animator anima;
	private moveCPU movimento;
	private atributosInimigos atributos;
	public AudioClip errou;
	private AudioSource armaAudio;
	private int acao = 0;
	private float tempoAcoes = 0f;
	private float tempoX;
	private Renderer renderizado;
	// Use this for initialization
	void Start () {
		anima = GetComponent<Animator> ();
		movimento = GetComponent<moveCPU> ();
		atributos = GetComponent<atributosInimigos> ();
		armaAudio = gameObject.GetComponentInChildren<armaInimigo> ().GetComponent <AudioSource>();
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
							movimento.SeAfastar (1f, true);
						} else {
							//float distY = movimento.player.transform.position.y - transform.position.y;
							if (movimento.MesmaAltura() && movimento.DistaciaX (transform.position.x, atributosHeroi.heroiX.transform.position.x) < 0.7f && movimento.SemObstaculosEntrePlay()) {
								Atacar ();
							} else {
                                if (movimento.MesmaAltura())
                                {
                                    movimento.SeAproximar(0.7f, false);
                                }else
                                {
                                    movimento.SeAproximar(0f, false);
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
					movimento.SeAproximar (1f, true);
				}
			}
		}

		// fazendo ações
         if (anima.GetInteger ("estados") == 0 && anima.speed != 1f) {
			anima.speed = 1f;
		}
	}

    public void AcabarAcao()
    {
        anima.speed = 1f;
        atributos.estados = 0;
        anima.SetInteger("estados", 0);
    }

    public void VaiAtaque()
    {
        Vector2 f = Vector2.one;
        f.x *= transform.lossyScale.x;
        movimento.esteCorpo.AddForce(f, ForceMode2D.Impulse);
    }

	public void Atacar(){
		if (movimento.noChao && atributos.estados == 0) {
			movimento.VirarParaPlayer ();
			armaAudio.pitch = atributos.agilidade;
			armaAudio.clip = errou;
			armaAudio.Play ();
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
