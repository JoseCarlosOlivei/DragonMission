using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class prologo : MonoBehaviour {
	//private fases faseX;
	private GameObject caixaMensagem;
	public GameObject sparkAtor,calanguitoAtor,litpterAtor;
    public GameObject colunaDirt, colunaEsq;
    public unknow desconhecido;
	private moverJogador playMove;
	private float tempoCena =0f;
	private Text fala,quem;
	private float tempoPressionado = 0f;
	private bool podePassar = false;
	public Transform focoLuta,pontoUltimaCena;
    public destruirSimples janelaDoRei;
    private float tempoAux = 0f;
    public AudioClip bossMusica;
	// Use this for initialization
	void Awake () {
		fala = GameObject.FindGameObjectWithTag ("Fala").GetComponent<Text>();
		quem = GameObject.FindGameObjectWithTag ("QuemFala").GetComponent<Text>();
		caixaMensagem = fala.gameObject.transform.parent.gameObject;
		caixaMensagem.SetActive (false);
		playMove = calanguitoAtor.GetComponent<moverJogador> ();
        litpterAtor.GetComponent<atributosInimigos>().eBoss = true;
        litpterAtor.GetComponent<atuacao>().Atuando(1, 0);
	}

    void Start()
    {
        if (dataSave.jogoAtual.cena > 0)
        {
            fases.faseX.TrocarMusica(fases.faseX.musicaDaFase);
        }
    }

    // Update is called once per frame
    void Update () {
		if (dataSave.jogoAtual.cena == 0) {
			Cena0 ();
		} else if (dataSave.jogoAtual.cena == 1) {
			atuacao atuaSpark = sparkAtor.GetComponent<atuacao> ();
			atuaSpark.Atuando (0, 0);
			atuacao atuaCalaguinto = calanguitoAtor.GetComponent<atuacao> ();
			atuaCalaguinto.Atuando (0, 0);
			spark sparkSaida = sparkAtor.GetComponent<spark> ();
			sparkSaida.IrEmbora ();
			dataSave.jogoAtual.cena += 1;
            fases.faseX.TrocarMusica(fases.faseX.musicaDaFase);
        }
        else if (dataSave.jogoAtual.cena == 2) {
			Cena1 ();
		}else if (dataSave.jogoAtual.cena == 3 && atributosHeroi.heroiX.transform.position.x >= focoLuta.position.x - 1f) {
			Cena2 ();
		}else if(dataSave.jogoAtual.cena == 4)
        {
            Cena3();
        }else if(dataSave.jogoAtual.cena == 5 && !litpterAtor.activeInHierarchy)
        {
            fases.faseX.fundoBranco.SetActive(false);
            Cena4();
        }

		if (podePassar) {
			if (Input.GetButton ("Passar")) {
				tempoPressionado += Time.deltaTime;
			}
			if (Input.GetButtonUp ("Passar")) {
				tempoPressionado = 0f;
			}
		}
		if (tempoPressionado > 1f) {
			tempoPressionado = 0f;
			AcabarCena ();
		}
	}

	private void Cena0(){
		chegandoTeleporte chegadaSpark = sparkAtor.GetComponentInParent<chegandoTeleporte> ();
		atuacao atuaSpark = sparkAtor.GetComponent<atuacao> ();
		atuacao atuaCalaguinto = calanguitoAtor.GetComponent<atuacao> ();
        if (dataSave.jogoAtual.subCena == 0) {
            fases.podeMover = false;
            if (tempoCena < 1f) {
                tempoCena += Time.deltaTime;
            } else {
                tempoCena = 0f;
                dataSave.jogoAtual.subCena += 1;
            }
        } else if (dataSave.jogoAtual.subCena == 1) {
            if (calanguitoAtor.transform.position.x <= 2f) {
                playMove.Movimento(true, true);
            } else if (tempoCena < 1f) {
                playMove.Movimento(false, true);
                tempoCena += Time.deltaTime;
            } else {
                tempoCena = 0f;
                dataSave.jogoAtual.subCena += 1;
            }
        } else if (dataSave.jogoAtual.subCena == 2) {
            if (calanguitoAtor.transform.position.x >= 1f) {
                playMove.Movimento(true, false);
            } else if (tempoCena < 1f) {
                playMove.Movimento(false, false);
                tempoCena += Time.deltaTime;
            } else {
                tempoCena = 0f;
                dataSave.jogoAtual.subCena += 1;
            }
        } else if (dataSave.jogoAtual.subCena == 3) {
            if (calanguitoAtor.transform.position.x <= 1.5f) {
                playMove.Movimento(true, true);
            } else {
                playMove.Movimento(false, true);
                atuaSpark.Atuando(2, 0);
                chegadaSpark.ChegandoComTeleporte();
                quem.text = "Calanguito";
                fala.text = "Spark !!!";
                atuaCalaguinto.Atuando(2, fala.text.Length);
                fases.faseX.TrocarMusica(fases.faseX.musicaTensa);
                ComecarCena();
            }
        } else if (dataSave.jogoAtual.subCena == 4) {
            PassarSubCena();
        } else if (dataSave.jogoAtual.subCena == 5) {
            quem.text = "Spark";
            fala.text = "O castelo foi invadido !!!";
            atuaSpark.Atuando(2, fala.text.Length);
            dataSave.jogoAtual.subCena += 1;
        } else if (dataSave.jogoAtual.subCena == 6) {
            PassarSubCena();
        } else if (dataSave.jogoAtual.subCena == 7) {
            quem.text = "Calanguito";
            fala.text = "Vamos proteger nossa magestade !!!";
            atuaCalaguinto.Atuando(1, fala.text.Length);
            dataSave.jogoAtual.subCena += 1;
        } else if (dataSave.jogoAtual.subCena == 8) {
            PassarSubCena();
        } else if (dataSave.jogoAtual.subCena == 9) {
            quem.text = "Spark";
            fala.text = "Não, ele deve está bem, vamos fugir logo.";
            atuaSpark.Atuando(2, fala.text.Length);
            dataSave.jogoAtual.subCena += 1;
        } else if (dataSave.jogoAtual.subCena == 10) {
            PassarSubCena();
        } else if (dataSave.jogoAtual.subCena == 11) {
            quem.text = "Calanguito";
            fala.text = "Fui criado aqui Spark, eles não me abandonaram quando mais precisei eu não vou abandonar eles agora que precisam de mim.";
            atuaCalaguinto.Atuando(1, fala.text.Length);
            dataSave.jogoAtual.subCena += 1;
        } else if (dataSave.jogoAtual.subCena == 12) {
            PassarSubCena();
        } else if (dataSave.jogoAtual.subCena == 13) {
            quem.text = "Spark";
            fala.text = "Esquece isso, vamos fugir enquanto há tempo.";
            atuaSpark.Atuando(2, fala.text.Length);
            dataSave.jogoAtual.subCena += 1;
        } else if (dataSave.jogoAtual.subCena == 14) {
            PassarSubCena();
        } else if (dataSave.jogoAtual.subCena == 15) {
            quem.text = "Calanguito";
            fala.text = "...";
            atuaCalaguinto.Atuando(1, 0);
            atuaSpark.Atuando(3, 0);
            dataSave.jogoAtual.subCena += 1;
        } else if (dataSave.jogoAtual.subCena == 16) {
            PassarSubCena();
        } else if (dataSave.jogoAtual.subCena == 17) {
            quem.text = "Calanguito";
            fala.text = "Pode fugir eu ficarei.";
            atuaCalaguinto.Atuando(1, 0);
            atuaSpark.Atuando(3, 0);
            dataSave.jogoAtual.subCena += 1;
        } else if (dataSave.jogoAtual.subCena == 18) {
            quem.text = "Spark";
            fala.text = "Não sei porque ainda tento.";
            atuaSpark.Atuando(3, fala.text.Length);
            dataSave.jogoAtual.subCena += 1;
        } else if (dataSave.jogoAtual.subCena == 19) {
            PassarSubCena();
        } else if (dataSave.jogoAtual.subCena == 20) {
            quem.text = "Calanguito";
            fala.text = "Esperava mais de você Spark...";
            atuaCalaguinto.Atuando(1, fala.text.Length);
            dataSave.jogoAtual.subCena += 1;
        } else if (dataSave.jogoAtual.subCena == 21) {
            PassarSubCena();
        } else if (dataSave.jogoAtual.subCena == 22) {
            atuaCalaguinto.Atuando(1, 0);
            quem.text = "Spark";
            fala.text = "...";
            atuaSpark.Atuando(1, 0);
            dataSave.jogoAtual.subCena += 1;
        } else if (dataSave.jogoAtual.subCena == 23) {
            PassarSubCena();
        } else if (dataSave.jogoAtual.subCena == 24) {
            quem.text = "Spark";
            fala.text = "Eu prometo te ajudar em outra hora...tchau.";
            atuaSpark.Atuando(1, fala.text.Length);
            dataSave.jogoAtual.subCena += 1;
        } else if (dataSave.jogoAtual.subCena == 25) {
            PassarSubCena();
        } else if (dataSave.jogoAtual.subCena == 26) {
            AcabarCena();
        }
	}

	private void Cena1(){
		if (dataSave.jogoAtual.subCena == 0) {
			quem.text = "Sistema";
			fala.text = "j ataca, l defende, w pula, s desce de certas plataformas.";
			ComecarCena ();
		}else if(dataSave.jogoAtual.subCena == 1){
            PassarSubCena();
		}else if(dataSave.jogoAtual.subCena == 2)
        {
            AcabarCena();
        }	
	}

	private void Cena2(){
        atuacao hero = calanguitoAtor.GetComponent<atuacao>();
        moverJogador movCala = calanguitoAtor.GetComponent<moverJogador>();
        atuacao litper = litpterAtor.GetComponent<atuacao>();
        if (dataSave.jogoAtual.subCena == 0)
        {
            litper.GetComponent<moveCPU>().Move(false, false);
            movCala.Movimento(false, true);
        }
        if (movCala.noChao)
        {
            if (dataSave.jogoAtual.subCena == 0)
            {
                quem.text = "Calanguito";
                fala.text = "Litper!!! Você está bem ?";
                hero.Atuando(2, fala.text.Length);
                ComecarCena();
                fases.faseX.TrocarMusica(fases.faseX.musicaFalaBoss);
            }
            else if (dataSave.jogoAtual.subCena == 1)
            {
                PassarSubCena();
            }
            else if (dataSave.jogoAtual.subCena == 2)
            {
                hero.Atuando(2, 0);
                quem.text = "Litper";
                fala.text = "Estou bem.";
                litper.Atuando(1, fala.text.Length);
                dataSave.jogoAtual.subCena += 1;
            }
            else if (dataSave.jogoAtual.subCena == 3)
            {
                PassarSubCena();
            }
            else if (dataSave.jogoAtual.subCena == 4)
            {
                quem.text = "Calanguito";
                fala.text = "Rápido temos que salvar nosso rei !!!";
                litper.Atuando(1, 0);
                hero.Atuando(1, fala.text.Length);
                dataSave.jogoAtual.subCena += 1;
            }
            else if (dataSave.jogoAtual.subCena == 5)
            {
                PassarSubCena();
            }
            else if (dataSave.jogoAtual.subCena == 6)
            {
                quem.text = "Litper";
                fala.text = "Pra que ???";
                litper.Atuando(1, fala.text.Length);
                hero.Atuando(1, 0);
                dataSave.jogoAtual.subCena += 1;
            }
            else if (dataSave.jogoAtual.subCena == 7)
            {
                PassarSubCena();
            }
            else if (dataSave.jogoAtual.subCena == 8)
            {
                quem.text = "Calanguito";
                fala.text = "Como assim!!! O nosso rei está é perigo!!!";
                litper.Atuando(1, 0);
                hero.Atuando(2, fala.text.Length);
                dataSave.jogoAtual.subCena += 1;
            }
            else if (dataSave.jogoAtual.subCena == 9)
            {
                PassarSubCena();
            }
            else if (dataSave.jogoAtual.subCena == 10)
            {
                quem.text = "Litper";
                fala.text = "Olha aqui, esse trabalho não dá muito, esses caras que invadiram me ofereceram muito mais que ganho aqui.";
                litper.Atuando(1, fala.text.Length);
                hero.Atuando(2, 0);
                dataSave.jogoAtual.subCena += 1;
            }
            else if (dataSave.jogoAtual.subCena == 11)
            {
                PassarSubCena();
            }
            else if (dataSave.jogoAtual.subCena == 12)
            {
                quem.text = "Calanguito";
                fala.text = "Você nós traiu ???";
                litper.Atuando(1, 0);
                hero.Atuando(2, fala.text.Length);
                dataSave.jogoAtual.subCena += 1;
            }
            else if (dataSave.jogoAtual.subCena == 13)
            {
                PassarSubCena();
            }
            else if (dataSave.jogoAtual.subCena == 14)
            {
                quem.text = "Litper";
                fala.text = "Sim, e você ainda pode se unir a nós.";
                litper.Atuando(1, fala.text.Length);
                hero.Atuando(2, 0);
                dataSave.jogoAtual.subCena += 1;
            }
            else if (dataSave.jogoAtual.subCena == 15)
            {
                PassarSubCena();
            }
            else if (dataSave.jogoAtual.subCena == 16)
            {
                quem.text = "Calanguito";
                fala.text = "Saia da frente e lhe deixo viver.";
                litper.Atuando(1, 0);
                hero.Atuando(1, fala.text.Length);
                dataSave.jogoAtual.subCena += 1;
            }
            else if (dataSave.jogoAtual.subCena == 17)
            {
                PassarSubCena();
            }
            else if (dataSave.jogoAtual.subCena == 18)
            {
                hero.Atuando(1, 0);
                dataSave.jogoAtual.subCena += 1;
            }
            else if (dataSave.jogoAtual.subCena == 19)
            {
                AcabarCena();
            }
        }
	}

    private void Cena3()
    {
        atuacao litp = litpterAtor.GetComponent<atuacao>();
        atuacao hero = calanguitoAtor.GetComponent<atuacao>();
        if (dataSave.jogoAtual.subCena == 0)
        {
            fases.faseX.TrocarMusica(bossMusica);
            quem.text = "Litper";
            fala.text = "Então não poderei deixar você passar.";
            litp.Atuando(1, fala.text.Length);
            ComecarCena();
            podePassar = false;
        }else if(dataSave.jogoAtual.subCena == 1)
        {
            if (tempoAux < 2f)
            {
                if(tempoAux == 0f)
                {
                    cameraSeguir.cam.Focar(focoLuta, false, 2f);
                }
                tempoAux += Time.deltaTime;
            }
            else
            {
                dataSave.jogoAtual.subCena += 1;
                litp.Atuando(0, 0);
                caixaMensagem.SetActive(false);
                tempoAux = 0f;
            }
        }
        else if (dataSave.jogoAtual.subCena == 2)
        {
            if(tempoAux < 1.5f)
            {
                tempoAux += Time.deltaTime;
            }
            else if(tempoAux < 2.5f)
            {
                tempoAux += Time.deltaTime;
                colunaDirt.SetActive(true);
                colunaEsq.SetActive(true);
            }else
            {
                dataSave.jogoAtual.subCena += 1;
            }         
        }else if(dataSave.jogoAtual.subCena == 3)
        {
            fases.faseX.LutaBossComeco();
            litp.GetComponent<moveCPU>().viu = true;
            litp.GetComponent<atributosInimigos>().invul = false;
            hero.Atuando(0, 0);
            AcabarCena();
        }
    }

    private void Cena4()
    {
        moverJogador moveCala =calanguitoAtor.GetComponent<moverJogador>();
        atuacao atuaCalango = calanguitoAtor.GetComponent<atuacao>();
        if (dataSave.jogoAtual.subCena == 0)
        {
            explosoes exD = colunaDirt.GetComponent<explosoes>();
            exD.explodir = true;
            colunaEsq.GetComponent<explosoes>().explodir = true;
            ComecarCena();
            podePassar = false;
            caixaMensagem.SetActive(false);
            cameraSeguir.cam.Tremer(0.2f, exD.duracaoExplosao);
        }else if(dataSave.jogoAtual.subCena == 1)
        {
            if (!colunaDirt.activeInHierarchy && !colunaEsq.activeInHierarchy)
            {
                dataSave.jogoAtual.subCena += 1;
            }      
        }
        else if(dataSave.jogoAtual.subCena == 2)
        {
            if (tempoCena == 0)
            {
                cameraSeguir.cam.Focar(calanguitoAtor.transform, false, 2f);
            }
            else if (tempoCena > 2f)
            {
                dataSave.jogoAtual.subCena += 1;
            }
            tempoCena += Time.deltaTime;        
        }else if(dataSave.jogoAtual.subCena == 3)
        {
            if (calanguitoAtor.transform.position.x < pontoUltimaCena.position.x)
            {
                moveCala.Movimento(true, true);
            }
            else
            {
                moveCala.Movimento(false, true);
                dataSave.jogoAtual.subCena += 1;
                tempoCena = 0f;
            }          
        }else if(dataSave.jogoAtual.subCena == 4)
        {
            if (tempoCena < 1f)
            {
                if (tempoCena == 0f)
                {
                    cameraSeguir.cam.Focar(desconhecido.transform, false, 1f);
                }
                tempoCena += Time.deltaTime;
            }
            else
            {
                janelaDoRei.Destruir();
                desconhecido.gameObject.SetActive(true);
                dataSave.jogoAtual.subCena += 1;
                tempoCena = 0f;
            }         
        }else if (dataSave.jogoAtual.subCena == 5)
        {
            if (tempoCena < 2f)
            {
                if (tempoCena == 0f)
                {
                    atuaCalango.Atuando(2, 0);
                    cameraSeguir.cam.Focar(atuaCalango.transform, false, 1f);
                }
                tempoCena += Time.deltaTime;
            }
            else
            {
                if (desconhecido.solo)
                {
                    caixaMensagem.SetActive(true);
                    quem.text = "Desconhecido";
                    fala.text = "...";
                    dataSave.jogoAtual.subCena += 1;
                    tempoCena = 0f;
                }
            }
        }else if(dataSave.jogoAtual.subCena == 6)
        {
            PassarSubCena();         
        }else if(dataSave.jogoAtual.subCena == 7)
        {
            auxiliares.Falar(fala, quem, 2, calanguitoAtor, "Esse é o rei ???", "Calanguito", caixaMensagem);
            dataSave.jogoAtual.subCena += 1;
        }else if (dataSave.jogoAtual.subCena == 8)
        {
            PassarSubCena();         
        }else if(dataSave.jogoAtual.subCena == 9)
        {
            caixaMensagem.SetActive(false);
            desconhecido.Pular();
            dataSave.jogoAtual.subCena += 1;
        }else if(dataSave.jogoAtual.subCena == 10)
        {
            if (tempoAux < 2f)
            {
                tempoAux += Time.deltaTime;
            }else
            {
                dataSave.jogoAtual.subCena += 1;
            }
        }else if(dataSave.jogoAtual.subCena == 11)
        {
            dataSave.jogoAtual.AddFaseNova("Distrito da Terra");
            fases.faseX.PassarDeFase();
        }
    }


    private void ComecarCena(){
		caixaMensagem.SetActive (true);
		fases.podeMover = false;
        calanguitoAtor.GetComponent<moverJogador>().Movimento(false, true);
		podePassar = true;
		dataSave.jogoAtual.subCena += 1;
        tempoCena = 0f;
	}

	private void PassarSubCena(){
		if( Input.GetButtonDown("Passar")) {
			dataSave.jogoAtual.subCena += 1;
		} 
	}

	private void AcabarCena(){
		caixaMensagem.SetActive (false);
		fases.podeMover = true;
        playMove.Movimento(false, true);
		dataSave.jogoAtual.cena += 1;
		dataSave.jogoAtual.subCena = 0;
		podePassar = false;
		tempoPressionado = 0f;
        tempoAux = 0f;
	}
}
