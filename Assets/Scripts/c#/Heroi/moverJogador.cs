
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class moverJogador : MonoBehaviour {

	public Collider2D ultSaida;
	public Slider powerSlider;
	private bool ataque1 = true;
	public bool viradoDireita = true;
	public float speed = 2f;
	public float puloAltura = 1.0f;
	public int estados = 0;// 0 parado, 1 atacando,2 defesa, 3 quebra defesa, 4 stun, 5 morto, 6 soltando poder
	public Animator animacoes;
    private float moveX = 0f;
	// obstaculos
	public bool obstaculoEsq= false;
	public bool obstaculoDir= false;
	private float tempoMorte = 0f;
	public Rigidbody2D esteCorpo;
	// efetos sonoros
	public AudioSource esteAudio,daEspada;
	public AudioClip batendoEspada;
	private bool caiu = false;
	private float tempoAudioCaindo = 0f;
	public AudioClip umPasso,outroPasso;
	private bool trocarPasso = false;
	private float tempoPasso = 0f;
	public AudioClip caindoNoChao;
	public AudioClip defendendoAtaque;
	public AudioClip EspadadaAcerto;
	private atributosHeroi atrib ;
	public bool noChao = false;
	public bool sofreo = false;
	private bool exausto= false;
	public float dorPor = 0f;
	public float stun;
	public bool esquivou= false;
	public Sprite[] poderes = new Sprite[9];
	public Image poderNow;
	private float tempPoder=0f;
	private float terreAux=0f;
	public atributosInimigos[] inimigosAfetados;
    private bool axisTrocaPoder = false,axisTrocaItens = false;
    public Transform saida;
    public GameObject[] poderesPrefabs;
    private List<GameObject> listaPocoesAcido = new List<GameObject>();
    private float tSegurandoP = 0f;
    private bool paraMoveJogador = false;
    public GameObject exaustoG;
    private SpriteRenderer saidaRender;
    public AudioClip rebolarItemSom;
    public poeiraRespingos poeira, poeiraQueda;

	void Awake () {
		puloAltura = Mathf.Pow (puloAltura*Physics2D.gravity.magnitude*2.5f,0.5f);
		animacoes = gameObject.GetComponent<Animator> ();
		esteCorpo = this.gameObject.GetComponent<Rigidbody2D>();
		atrib = this.gameObject.GetComponent<atributosHeroi>();
		esteAudio = this.gameObject.GetComponent<AudioSource>();
		AtulizarImagemPoder ();
        GameObject aux;
        for(int i = 0; i < 5; i++)
        {
            for(int i2 = 0; i2 < poderesPrefabs.Length; i2++)
            {
                aux = Instantiate(poderesPrefabs[i2]);
                aux.SetActive(false);
                if (i2 == 0)
                {
                    listaPocoesAcido.Add(aux);
                }
            }
        }

        saidaRender = saida.GetComponent<SpriteRenderer>();

        poeira = Instantiate(poeira.gameObject).GetComponent<poeiraRespingos>();
        poeira.AddPai(transform, GetComponent<BoxCollider2D>());
        poeira.AtivarDesativar(false);

        poeiraQueda = Instantiate(poeiraQueda.gameObject).GetComponent<poeiraRespingos>();
        poeiraQueda.AddPai(transform, GetComponent<BoxCollider2D>());
        poeiraQueda.AtivarDesativar(false);
    }

    void Update () {
		//caiu
		if (esteCorpo.velocity.y != 0f) {
			if (noChao) {
				noChao = false;
			}
			if (transform.parent) {
				transform.parent = null;
			}
		} 
		// morto
		if (atrib.hpAtual <= 0f) {
			if (tempoMorte == 0f) {
				fases faseX = FindObjectOfType<fases> ();
                esteAudio.pitch = 1f;
                esteAudio.clip = faseX.morteSom;
				esteAudio.Play ();
			}
			tempoMorte += Time.deltaTime;
			if (tempoMorte > 1f) {
				animacoes.enabled = false;
				GetComponent<SpriteRenderer> ().sprite = null;
			}
		}
        //Exausto

        if (exausto)
        {
            if (!exaustoG.activeInHierarchy)
            {
                exaustoG.SetActive(true);
            }
            if (atrib.vigorAtual >= atrib.vigor || estados == 5)
            {
                exausto = false;
                animacoes.speed = 1f;
                esteAudio.pitch = 1f;
                exaustoG.SetActive(false);
            }
        }
        else if (atrib.vigorAtual <= 0f)
        {
            exausto = true;
            animacoes.speed = 0.5f;
            esteAudio.pitch = 0.5f;
        }
        
        //sofrendo
        if (dorPor > 0f && estados != 5) {
			estados = 4;
			animacoes.SetInteger ("estado", estados);
			if (!sofreo) {
				//Debug.Log (1);
				if (esquivou) {
					animacoes.SetFloat ("oQSofre", 2f);
				} else {
					animacoes.SetFloat ("oQSofre", 1f);
				}
				sofreo = true;
				animacoes.speed =  1f/dorPor;
			}
			if (Input.GetButtonDown ("Defesa") && atrib.vigorAtual >= 1f / 16f && stun <= 0f) {
				dorPor = 0f;
				atrib.vigorAtual -= 1f / 16f;
			}
			dorPor -= Time.deltaTime;
		} else if(estados != 5 && sofreo){
			//Debug.Log (2);
			sofreo = false;
			if (exausto && stun <= 0f) {
				animacoes.speed = 0.5f;
			} else {
				animacoes.speed = 1f;
			}
			dorPor = 0f;
			if (stun <= 0) {
				estados = 0;
            }
			animacoes.SetFloat ("oQSofre", 0f);
            if (esquivou) {
                atrib.missC.SetActive(false);
                esquivou = false;
            }
        }
		//stuneado
		if (stun > 0f && estados != 5) {
			stun -= Time.deltaTime;
			estados = 4;
			animacoes.SetInteger ("estado", estados);
		} else if (estados == 4 && dorPor <= 0f) {
			estados = 0;
			animacoes.SetInteger ("estado", estados);
			stun = 0;
		}
	
		if (moveX != 0 && noChao) {
			animacoes.SetFloat ("andando", 1f);
			if (estados == 0) {
				animacoes.speed = speed / atrib.speed0;
			}
		} else if(animacoes.GetFloat("andando") != 0f){
			if (estados == 0) {
				animacoes.speed = 1f;
			}
			animacoes.SetFloat ("andando", 0f);
		}

		//caindo e no ar
		if (noChao){
            if (!animacoes.GetBool("solo"))
            {
                animacoes.SetBool("solo", true);
                if (animacoes.GetInteger("estado") != 0 && animacoes.GetInteger("estado") != 4 && animacoes.GetInteger("estado") != 5)
                {
                    ResetarEstados();
                }
            }
		} else {
			animacoes.SetBool ("solo", false);
            poeira.AtivarDesativar(false);
			if (esteCorpo.velocity.y > 0) {
				//animacoes.SetBool ("caindo", false);
				animacoes.SetFloat ("subindo", 1f);
			} else {
				animacoes.SetFloat ("subindo", 0f);
				//animacoes.SetBool ("caindo", true);
			}
		}
		//tirar coisas
		if (powerSlider.gameObject.activeInHierarchy && estados != 6) {
			powerSlider.gameObject.SetActive (false);
		}
		// pode se mover
		if (fases.podeMover) {
            paraMoveJogador = true;
            moveX = 0f;
			//defesa
			if ((estados == 0 || estados == 2) && dorPor <= 0) {
				if (Input.GetButton ("Defesa") && noChao && atrib.vigorAtual > 0f && !exausto) {
					estados = 2;
                    NaoDeslizar();
				}
			} 

			if (Input.GetAxisRaw("TrocarPoder") != 0) {
                if (!axisTrocaPoder)
                {
                    axisTrocaPoder = true;
                    AlterarPoder();
                    //Debug.Log (poderId);
                }
            }else if (axisTrocaPoder)
            {
                axisTrocaPoder = false;
            }
            //troca itens
            if (Input.GetAxisRaw("TrocarItens") != 0)
            {
                if (!axisTrocaItens)
                {
                    axisTrocaItens = true;
                    mochilaCalanguito.mochila.AlterarItens((int)Input.GetAxisRaw("TrocarItens"));
                }
            }
            else if (axisTrocaItens)
            {
                axisTrocaItens = false;
            }

            if (estados == 0) {
				moveX = Input.GetAxisRaw ("Horizontal");

				if (!exausto) {
					// Atacando
					if (Input.GetButtonDown ("Ataque")) {
						Atacar ();
					}
					//pulando
					if (Input.GetButtonDown ("Jump") && noChao && atrib.vigorAtual >= 1f / 64f) {
						//float v = Mathf.Pow (puloAltura*Physics2D.gravity.magnitude*2f,0.5f);
						//Debug.Log (v);
						esteCorpo.velocity = new Vector2 (esteCorpo.velocity.x, puloAltura);
						atrib.vigorAtual -= 1f / 64f;
						//moverse = true;
					}
					//Caindo
					if (Input.GetAxisRaw ("Vertical") < 0f && noChao) {
						DescerPlata ();
					}
                    //Usa itens
                    if (Input.GetButtonDown("UsarItens") )
                    {
                        if (mochilaCalanguito.mochila.PodeUsarItem(saidaRender))
                        {
                            NaoDeslizar();
                            if (mochilaCalanguito.mochila.needRebolar)
                            {
                                estados = 6;
                                animacoes.SetFloat("poderes", 0f);
                            }else
                            {
                                mochilaCalanguito.mochila.UsarItem().Usar(Vector2.zero,true);
                            }
                        }                      
                    }
					//Poder
					if (dataSave.jogoAtual.poderId != 0) {
                        if (Input.GetButtonDown("Poder"))
                        {
                            tSegurandoP = 0f;
                            NaoDeslizar();
                            if (dataSave.jogoAtual.poderId == 1)
                            {
                                Terremoto();
                            }
                        }

                        if (Input.GetButtonUp("Poder"))
                        {
                            NaoDeslizar();
                            if (dataSave.jogoAtual.poderId == 2)
                            {
                                InicializarPocoeAcido();
                            }
                        }
                    }
				}
			}

            if (Input.GetButton("Poder"))
            {
                if (tSegurandoP < 1f)
                {
                    tSegurandoP += Time.deltaTime;
                }
                else if (tSegurandoP > 1f)
                {
                    tSegurandoP = 1f;
                }
            }

            if (estados == 1) {
				moveX = Input.GetAxis ("Horizontal");
				if (noChao || (viradoDireita && moveX< 0f) || (!viradoDireita && moveX > 0f)) {
                    moveX = 0f;
				} 
			}         

            //Lado que está virado
            if (moveX > 0) {
				viradoDireita = true;
			} else if (moveX < 0) {
				viradoDireita = false;
			} 
			
			if (exausto) {
				moveX = moveX / 2f;
			}
		}else if (paraMoveJogador)
        {
            moveX = 0f;
            paraMoveJogador = false;
        }

        //Poderes
        if (estados == 6 && animacoes.GetFloat("poderes")!=0)
        {
            if (dataSave.jogoAtual.poderId == 1)
            {
                AplicandoTerremto();
            }
        }

        if (saidaRender.sprite != null && estados!=6)
        {
            SaidaInvisivel();
        }

        if (estados == 2 && !sofreo) {
            // defesa
            if (animacoes.GetCurrentAnimatorStateInfo (0).IsName ("defendendo") && (Input.GetButtonUp("Defesa") || atrib.vigorAtual<=0f)) {
				estados = 0;
				animacoes.SetInteger ("estado", estados);
			}else{
				// para ele poder se virar em modo de defesa
				if(Input.GetAxis("Horizontal")>0){
					viradoDireita = true;
				}else if(Input.GetAxis("Horizontal")<0){
					viradoDireita = false;
				}
			}
		}
        //Espelhar
        Espelhar();
        //Mover
        if (esteCorpo.velocity.x*esteCorpo.velocity.x < speed*speed && !((moveX > 0 && obstaculoDir)||(moveX < 0 && obstaculoEsq)))
        {
            esteCorpo.AddForce(Vector2.right * moveX*speed/6f, ForceMode2D.Impulse);
        }

        if(moveX == 0f && estados == 0)
        {
            NaoDeslizar();
        }
		//andar som
		if (noChao && moveX != 0f) {
			if (!caiu) {
                poeira.AtivarDesativar(true);
				//Debug.Log (tempoPasso);
				if (tempoPasso == 0f) {
					if (trocarPasso) {
						esteAudio.clip = outroPasso;
					} else {
						esteAudio.clip = umPasso;
					}

					if (1f / (animacoes.speed*4f) < umPasso.length) {
						esteAudio.pitch = animacoes.speed*1.8f;
					}
					esteAudio.Play ();
					esteAudio.loop = false;
				}
				if (tempoPasso < 1f / (animacoes.speed*4f)) {
					tempoPasso += Time.deltaTime;
				} else {
					tempoPasso = 0f;
					trocarPasso = !trocarPasso;
				}
			} 
		} else {
			tempoPasso = 0f;
			trocarPasso = !trocarPasso;
            poeira.AtivarDesativar(false);
		}
		// audio caindo
		if (caiu) {
            //Debug.Log (tempoAudioCaindo);
			if (tempoAudioCaindo == 0f) {
                estados = 0;
                esteAudio.pitch = 1f;
				esteAudio.clip = caindoNoChao;
				esteAudio.Play ();
                poeiraQueda.AtivarDesativar(true);
			}
			if (tempoAudioCaindo > 0.2f) {
				caiu = false;
				//Debug.Log (tempoAudioCaindo);
				tempoAudioCaindo = 0f;
				//foiAudioQueda = true;
			} else {
				tempoAudioCaindo += Time.deltaTime;
			}
		}
        if(animacoes.GetInteger("estado") != estados)
        {
            animacoes.SetInteger("estado", estados);
        }
    }

    public void RebolarItem()
    {
        SaidaInvisivel();
        esteAudio.PlayOneShot(rebolarItemSom);
        mochilaCalanguito.mochila.UsarItem().Usar(saida.position, viradoDireita);
    }

    private void SaidaInvisivel()
    {
        saidaRender.sprite = null;
    }

    public void ResetarEstados()
    {
        estados = 0;
        animacoes.speed = 1f;
    }

    public void NaoDeslizar()
    {
        if (esteCorpo.velocity.x != 0f)
        {
            esteCorpo.AddForce(Vector2.left * esteCorpo.velocity.x, ForceMode2D.Impulse);
        }
    }

	public void Atacar(){
        NaoDeslizar();
		if( atrib.vigorAtual >= 1f / (atrib.agilidade * 16f)) {
			atrib.vitalidadeAtaq = atrib.vitalidadeAtual;
			animacoes.speed = atrib.agilidade;
			estados = 1;
			if (ataque1) {
				animacoes.SetFloat ("ataque1", 1f);
			} else {
				animacoes.SetFloat ("ataque1", 0f);
			}
			ataque1 = !ataque1;
			atrib.vigorAtual -= 1f / (atrib.agilidade*16f);
			daEspada.pitch = atrib.agilidade / 2f;
			daEspada.PlayOneShot (batendoEspada);
		}					
	}

	public void Terremoto(){
		if (estados == 0 && noChao && atrib.vigorAtual >= 0.25f) {
			powerSlider.maxValue = 15f;
			atrib.vigorAtual -= 0.25f;
			estados = 6;
            cameraSeguir.cam.Tremer(0.05f, 0.1f);
            animacoes.SetFloat("poderes", 1f);
		}
	}

	private void AplicandoTerremto(){
		if (tempPoder < 3f) {
            if (terreAux > 0f) {
				terreAux -= Time.deltaTime*2f;
			} else if (terreAux < 0f) {
				terreAux = 0f;
			}
			if (Input.GetButtonDown ("Poder") && terreAux < 17f) {
				terreAux += 1f;
			} 
			if (!StunearTodosNoChao (Time.deltaTime)) {
				tempPoder = 3f;
            }else if(!powerSlider.gameObject.activeInHierarchy)
            {
                powerSlider.gameObject.SetActive(true);
            }
            powerSlider.value = terreAux;
            cameraSeguir.cam.Tremer(0.05f+(powerSlider.value/80f), 0.1f);
            tempPoder += Time.deltaTime;
		} else {
			terreAux = 0f;
			tempPoder = 0f;
			float xStun = powerSlider.value / 16f;
			StunearTodosNoChao (xStun);
			estados = 0;
            cameraSeguir.cam.ParaTremer();
        }
    }

	private bool StunearTodosNoChao(float quanto){
		bool alguem = false;
		Vector2 diagonalRet0 = new Vector2 (transform.position.x - moveCPU.limiteTela.x, transform.position.y - moveCPU.limiteTela.y - 0.5f);
		Vector2 diagonalRetF = new Vector2 (transform.position.x + moveCPU.limiteTela.x, transform.position.y + moveCPU.limiteTela.y + 0.5f);
		//Debug.Log (diagonalRet0+" "+ diagonalRetF);
		Collider2D[] coliders = Physics2D.OverlapAreaAll (diagonalRet0, diagonalRetF,fases.faseX.iniMask);
		inimigosAfetados = new atributosInimigos[coliders.Length];

        for (int i=0; i < coliders.Length; i++) {
			inimigosAfetados [i] = coliders [i].GetComponent<atributosInimigos> ();
			if (inimigosAfetados [i].animacoes.GetBool ("solo")) {
				if (inimigosAfetados [i].esquiva < Random.value) {
					inimigosAfetados [i].AplicarStun (quanto);
					alguem = true;
				}
			}
		}

		return alguem;
	}
		
	void OnCollisionStay2D(Collision2D coll) {
		//Debug.Log (esteCorpo.velocity.y);
		if(coll.gameObject.layer == 8){
			//Debug.Log ("q");
			BoxCollider2D box = coll.gameObject.GetComponent<BoxCollider2D>();
			//Debug.Log ("SpeedY "+esteCorpo.velocity.y+" !noChao "+!noChao+" mas alto "+MaisAltoPlata(box));
			if (MaisAltoPlata(box) && !noChao && esteCorpo.velocity.y <= 0f) {
				noChao = true;
				caiu = true;
				if (ultSaida != box) {
					if (ultSaida) {
						Physics2D.IgnoreCollision (ultSaida, GetComponent<Collider2D> (), false);
					}
					ultSaida = box;
				}
			}
		}
	}
		
	private void DescerPlata(){
        if (ultSaida)
        {
            PlatformEffector2D ePlataforma = ultSaida.gameObject.GetComponent<PlatformEffector2D>();
            if (noChao && ePlataforma)
            {
                Physics2D.IgnoreCollision(ultSaida, GetComponent<Collider2D>());
            }
        }
	}

	public bool MaisAltoPlata(BoxCollider2D plata){
		bool retorno = false;
		float alt = plata.transform.position.y+plata.bounds.extents.y;
		alt -= 0.01f;
		if (transform.position.y > alt) {
			retorno = true;
		}
		//Debug.Log (plata.bounds.extents.y);
		return retorno;
	}

	public void Defendeu(float speedAudio){
		//Debug.Log (defendendoAtaque.length);
		if (1f / speedAudio < defendendoAtaque.length) {
			speedAudio = speedAudio / defendendoAtaque.length;
			daEspada.pitch = speedAudio;
		} else {
			daEspada.pitch = 1f;
		}
		daEspada.clip = defendendoAtaque;
		daEspada.Play();
	}

	private void AlterarPoder(){
		int x = (int)Input.GetAxisRaw ("TrocarPoder");
		int idX = dataSave.jogoAtual.poderId;
        bool naoHaPoder = true;

		for (int i = 0; i < dataSave.jogoAtual.poderesDestravados.Length; i++) {
			idX += i * x;
			if (idX > dataSave.jogoAtual.poderesDestravados.Length) {
				idX = 1;
			}else if(idX <= 0){
				idX = dataSave.jogoAtual.poderesDestravados.Length;
			}
			if (dataSave.jogoAtual.poderesDestravados [idX - 1]) {	
				dataSave.jogoAtual.poderId = idX;
                naoHaPoder = false;
				i = dataSave.jogoAtual.poderesDestravados.Length;
			}
		}
        if (naoHaPoder)
        {
            dataSave.jogoAtual.poderId = 0;
        }

		AtulizarImagemPoder ();
	}

	public void Movimento(bool moverse,bool direita){
        paraMoveJogador = false;
		if (moverse) {
            if (direita)
            {
                moveX = 1f;
            }else
            {
                moveX = -1f;
            }
		} else {
			moveX = 0f;
		}
        viradoDireita = direita;
        Espelhar();
	}

    private void InicializarPocoeAcido()
    {
        if(atrib.vigor>= 0.1f)
        {
            int nMax = (int)(atrib.vigorAtual / 0.1f);
            int n =(int) (tSegurandoP*4f + 1f);
            if (n > nMax)
            {
                n = nMax;
            }

            Vector2 forca = Vector2.right*2f;
            Vector3 angV = Vector3.forward;
            if (!viradoDireita)
            {
                forca.x *= -1;
                angV *= -1f;
            }
            float ang = 30f;
            for(int i = 0; i < n; i++)
            {
                ang += 5f;
                Vector2 fX = Quaternion.Euler(angV * ang) * forca*(1f+i/5f);
                auxiliares.RetornaObj(listaPocoesAcido).GetComponent<respawnadorBater>().Respaw(saida.position, fX, atrib.PoderHabilidade(), 0.1f, atrib.FoiCritico(), 0.5f);
            }
           
            atrib.vigorAtual -= 0.1f*n;
        }
    }

    private void Espelhar()
    {
        if ((viradoDireita && transform.lossyScale.x < 0f) || (!viradoDireita && transform.lossyScale.x > 0f))
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

	private void AtulizarImagemPoder(){
        if (dataSave.jogoAtual.poderId > 0)
        {
            if (!poderNow.enabled)
            {
                poderNow.enabled = true;
            }
            poderNow.sprite = poderes[dataSave.jogoAtual.poderId - 1];
        }else if(poderNow.enabled)
        {
            poderNow.enabled = false;
        }
	}
}
