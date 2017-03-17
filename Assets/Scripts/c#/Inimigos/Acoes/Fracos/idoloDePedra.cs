using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class idoloDePedra : MonoBehaviour {
	private atributosInimigos atributos;
	private moveCPU movimentos;
	private Animator anima;
	private float tempoAcoes = 0f;
	public Transform saida;
	public GameObject tiro;
	public GameObject magiaAura;
	public List<GameObject> tiros = new List<GameObject>(), auras = new List<GameObject>();
	private int acao = 0;
	private float[] acoesFre = new float[2];
	private float kAcoes = 0f;
	private Renderer rederizado;
	public AudioClip carregandoPoder,tiroSolto,fasendoAura;
	private AudioSource exitSom;
	private float tempoX;
   
	// Use this for initialization
	void Awake () {
		GameObject aux;
		for (int i = 0; i < 5; i++) {
			aux =Instantiate (tiro);
			aux.SetActive (false);
			tiros.Add (aux);
			aux = Instantiate (magiaAura);
			aux.SetActive (false);
			auras.Add (aux);
		}
		atributos = GetComponent<atributosInimigos> ();
		movimentos = GetComponent<moveCPU> ();
		anima = GetComponent<Animator> ();
		acoesFre [0] = 16f;
		acoesFre [1] =  5f;
		rederizado = GetComponent<Renderer> ();
		exitSom = saida.GetComponent<AudioSource> ();
		for (int i = 0; i < acoesFre.Length; i++) {
			kAcoes += acoesFre [i];
		}
		tempoX = atributos.TempoReacaoCalc ();
		SuaVez ();
	}
	
	// Update is called once per frame
	void Update () {
        if(atributos.estados == 4 || atributos.estados == 5 || atributos.estados == 0)
        {
            exitSom.Stop();
        }
		if (fases.podeMover) {
			if (movimentos.viu) {
				if (atributos.estados == 0) {					
					// ações
					if (rederizado.isVisible) {
						if (acao == 0) {
							movimentos.SeAfastar (moveCPU.limiteTela.x, true);
						} else if (acao == 1) {
							// Atirar
							//Debug.Log (movimentos.DistaciaX (transform.position.x, movimentos.player.transform.position.x));
							if (moveCPU.limiteTela.x >= movimentos.DistaciaX (transform.position.x,atributosHeroi.heroiX.transform.position.x) && movimentos.MesmaAltura () && movimentos.noChao && movimentos.SemObstaculosEntrePlay()) {
								//Debug.Log (1);
								Atirar ();
								tempoAcoes = 0f;
							} else {
                                if (movimentos.MesmaAltura())
                                {
                                    movimentos.SeAproximar(moveCPU.limiteTela.x, false);
                                }else
                                {
                                    movimentos.SeAproximar(movimentos.xColl, false);
                                }
							}
						} else if (acao == 2) {
							// Buff pedra
							movimentos.SeAfastar (1f, false);
							if (1f <= movimentos.DistaciaX (transform.position.x, atributosHeroi.heroiX.transform.position.x) && movimentos.noChao) {
								AuraDefensiva ();
								tempoAcoes = 0f;
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
						movimentos.SeAproximar (moveCPU.limiteTela.x, true);
					}
				}
			}
		}		
	}

    public void ZerarEstado()
    {
        atributos.estados = 0;
        anima.SetFloat("qualPoder", 0f);
        anima.speed = 1f;
        anima.SetInteger("estados", 0);
    }

	public void Atirar(){
        if (atributos.estados == 0 && movimentos.noChao) {
            movimentos.VirarParaPlayer();
            atributos.estados = 6;
			anima.SetInteger ("estados", 6);
			anima.SetFloat ("qualPoder", 1f);
			exitSom.clip = carregandoPoder;
			exitSom.pitch = 1f;
			exitSom.Play ();
			anima.speed = atributos.agilidade;
            movimentos.NaoDeslizar();
            SuaVez();
		}
	}

    public void VaiTiro()
    {
        exitSom.clip = tiroSolto;
        exitSom.pitch = 1f;
        exitSom.Play();
        GameObject bala = ListaQuemDesativo(tiros);
        progeteis prt = bala.GetComponent<progeteis>();
        prt.Respaw(saida.position, movimentos.viradoDireita, atributos.ataque, 1f / atributos.agilidade, 1f / 16f,atributos.FoiCritico());
    }

	public void AuraDefensiva(){
		if (atributos.estados == 0 && movimentos.noChao) {
			atributos.estados = 6;
			exitSom.pitch = 1f;
            exitSom.loop = false;
            exitSom.PlayOneShot(fasendoAura);
			anima.SetInteger ("estados", 6);
			anima.SetFloat ("qualPoder", 2f);
			anima.speed = 1f;
            movimentos.NaoDeslizar();
			SuaVez ();
		}
	}

    public void VaiAura()
    {
        GameObject aura = ListaQuemDesativo(auras);
        aura.transform.position = transform.position;
        aura.transform.localScale = transform.lossyScale;
        aura.transform.parent = transform.parent;
        aura.SetActive(true);
    }

	private void SuaVez(){
		if (atributos.ChancesDeReagir(movimentos.encurralado)> Random.value) {
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

	private GameObject ListaQuemDesativo(List<GameObject> lista){
		GameObject ret = null;

		for (int i = 0; i < lista.Count; i++) {
			if (!lista [i].activeInHierarchy) {
				ret = lista [i];
				i = lista.Count;
			}
		}
        if (!ret)
        {
            ret = Instantiate(lista[0]);
        }

		return ret;
	}
}
