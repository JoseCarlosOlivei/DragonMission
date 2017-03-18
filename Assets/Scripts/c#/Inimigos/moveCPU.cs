using UnityEngine;
using System.Collections;
//using System.Collections.Generic;

public class moveCPU : MonoBehaviour {
	public BoxCollider2D ultSaida;
    public static Vector2 limiteTela = new Vector2(2f, 1f);
    public GameObject exclamacao;
	public Rigidbody2D esteCorpo;
    private atributosInimigos atributos;
	public Animator anima;
	public AudioClip umPasso,outroPasso,caindo;
	public float pesoSom = 1f,demoraPassos = 1f;
	private float tempoAudioCaindo = 0f;
	public bool noChao;
	private float visaoY;
	//public float alcance=0f;
	public float puloAltura;
	//Vector2 posEste;
	//private GameObject[] players;
	//public GameObject player;
	public float speed = 2f;
	public bool viradoDireita = true;
	public float moveX = 0f;
	public bool viu= false;
	private float tempoCaminhar = 0f;
	public bool tolerado = false;
	public bool obstaculoEsq = false,nIrEsq= false;
	public bool obstaculoDir = false,nIrDirt= false;
	public bool pontaEsq = false,pontaDrt = false;
	//private float difeYTolerada;
	//private GameObject[] plataformas;
	private int idPlata,idPlata2;
	private float vPulo;
	public BoxCollider2D collid;
	//public static fases faseX;
	private bool caiu = false;
	public AudioSource esteAudio;
    private AudioSource exclamaAudio;
	//public LayerMask layPlayer;
	private bool trocarPasso = false;
	private float tempoPasso = 0f;
	public bool encurralado= false;
	public float speed0;
    public float posXPraIr = 0f, posXPraIr2=0f;
    private float tempoExclama = 0f;
    public bool podeCair = false;
    public float xColl;

    private foraDeSolo foraSolo;
    public bool voaTodaHora;
    public bool nada;
    private float sobraDistY = 0.02f;

    public poeiraRespingos poeira, poeiraQueda;
    // Use this for initialization
    void Awake () {
        atributos = GetComponent<atributosInimigos>();
		vPulo = Mathf.Pow (puloAltura*Physics2D.gravity.magnitude*2.5f,0.5f);
		esteCorpo = GetComponent<Rigidbody2D> ();
		collid = GetComponent<BoxCollider2D> ();
		visaoY = collid.bounds.size.y* 2f;
        exclamaAudio = exclamacao.GetComponent<AudioSource>();
		if (visaoY < 0.4f) {
			visaoY = 0.4f;
		}

		anima = GetComponent<Animator> ();
		esteAudio = GetComponent<AudioSource> ();
        xColl = GetComponent<Collider2D>().bounds.extents.x;
        if(voaTodaHora || nada)
        {
            foraSolo = GetComponent<foraDeSolo>();
            if (voaTodaHora)
            {
                visaoY = 2f;
                foraSolo.voar = true;
            }
        }
        if (poeira)
        {
            poeira = Instantiate(poeira.gameObject).GetComponent<poeiraRespingos>();
            poeira.AddPai(transform, GetComponent<BoxCollider2D>());
            poeira.AtivarDesativar(false);
        }

        if (poeiraQueda)
        {
            poeiraQueda = Instantiate(poeiraQueda.gameObject).GetComponent<poeiraRespingos>();
            poeiraQueda.AddPai(transform, GetComponent<BoxCollider2D>());
            poeiraQueda.AtivarDesativar(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // No chao ?
        if (esteCorpo.velocity.y != 0f)
        {
            if (noChao)
            {
                noChao = false;
            }
            if (transform.parent)
            {
                transform.parent = null;
            }
        }

        if (anima.GetBool("solo") != noChao)
        {
            anima.SetBool("solo", noChao);
        }

        if (esteCorpo.velocity.y > 0f && !noChao)
        {
            anima.SetFloat("speedY", 1f);
        }
        else if (!noChao)
        {
            anima.SetFloat("speedY", 0f);
        }
        if (fases.podeMover)
        {
            if (!viu)
            {// Visao inimigo
                if (!atributos.eBoss)
                {
                    Ver();
                    Caminhar();
                }
            }
            else
            {
                if (atributosHeroi.heroiX.GetComponent<moverJogador>().estados == 5)
                {
                    viu = false;
                }
                if (tempoExclama > 0f)
                {
                    tempoExclama -= Time.deltaTime;
                }
                else if (exclamacao.activeInHierarchy)
                {
                    exclamacao.SetActive(false);
                }
            }
        }

        //Espelhar
        if ((viradoDireita && transform.lossyScale.x < 0f) || (!viradoDireita && transform.lossyScale.x > 0f))
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }

        //Se estiver nas pontas
        if (((pontaDrt && !podeCair) && moveX > 0f) || ((pontaEsq && !podeCair) && moveX < 0f))
        {
            moveX = 0f;
            Parar();
        }

        if ((obstaculoDir && moveX > 0f) || (obstaculoEsq && moveX < 0f))
        {
            moveX = 0f;
            NaoDeslizar();
        }

        if (anima.GetInteger("estados") == 4 || anima.GetInteger("estados") == 5)
        {
            moveX = 0f;
        }
        //animação caminhar ou parado
        if (moveX != 0)
        {
            anima.SetFloat("speedX", 1f);
            if (anima.GetInteger("estados") == 0)
            {
                anima.speed = speed / speed0;
            }
        }
        else
        {
            if (anima.GetInteger("estados") == 0)
            {
                anima.speed = 1f;
            }
            anima.SetFloat("speedX", 0f);
        }

        //Som se movimentando
        if (esteAudio.enabled)
        {
            if (noChao && moveX != 0f)
            {
                if (!caiu)
                {
                    poeira.AtivarDesativar(true);
                    //Debug.Log (tempoPasso);
                    if (tempoPasso == 0f)
                    {
                        if (trocarPasso && outroPasso)
                        {
                            esteAudio.clip = outroPasso;
                        }
                        else
                        {
                            esteAudio.clip = umPasso;
                        }

                        if (pesoSom / (anima.speed * 4f) < umPasso.length)
                        {
                            esteAudio.pitch = (anima.speed * 1.8f) / pesoSom;
                        }
                        else
                        {
                            esteAudio.pitch = 1f / pesoSom;
                        }
                        esteAudio.Play();
                        esteAudio.loop = false;
                    }
                    if (tempoPasso < demoraPassos / (anima.speed * 4f))
                    {
                        tempoPasso += Time.deltaTime;
                    }
                    else
                    {
                        tempoPasso = 0f;
                        trocarPasso = !trocarPasso;
                    }
                }
            }
            else
            {
                poeira.AtivarDesativar(false);
                tempoPasso = 0f;
                trocarPasso = !trocarPasso;
                if (esteAudio.clip == umPasso || (esteAudio.clip && esteAudio.clip == umPasso))
                {
                    esteAudio.Pause();
                }
            }
            // audio caindo
            if (caiu && esteAudio.enabled)
            {
                if (tempoAudioCaindo == 0f)
                {
                    poeiraQueda.AtivarDesativar(true);
                    if (anima.GetInteger("estados") != 0 && anima.GetInteger("estados") != 4 && anima.GetInteger("estados") != 5)
                    {
                        atributos.estados = 0;
                        anima.SetInteger("estados", 0);
                    }
                    esteAudio.loop = false;
                    esteAudio.pitch = pesoSom;
                    esteAudio.clip = null;
                    esteAudio.PlayOneShot(caindo);
                }
                if (tempoAudioCaindo > 0.2f)
                {
                    caiu = false;
                    tempoAudioCaindo = 0f;
                }
                else
                {
                    tempoAudioCaindo += Time.deltaTime;
                }
            }
        }       

        if (moveX != 0f)
            {
                if (esteCorpo.velocity.x * esteCorpo.velocity.x < speed * speed)
                {
                    esteCorpo.AddRelativeForce(Vector2.right * moveX * speed / 6f, ForceMode2D.Impulse);
                }
            }
            else if (anima.GetInteger("estados") == 0)
            {
                NaoDeslizar();
            }
    }

    public bool ForaChao()
    {
        bool ret = false;

        if (foraSolo)
        {
            ret = foraSolo.voar;
        }

        return ret;
    }

    public bool DistaciaYPega(float alcanceY,float y)
    {
        bool ret = false;
        float x = y - transform.position.y;
        if(x>=-0.005f && x <= alcanceY)
        {
            ret = true;
        }

        return ret;
    }
   

    public void NaoDeslizar()
    {
        moveX = 0f;
        if (esteCorpo.velocity.x != 0f)
        {
            esteCorpo.AddForce(Vector2.left * esteCorpo.velocity.x, ForceMode2D.Impulse);
        }
    }

	//Alcanca sem pulo
	public bool AlcacancaSemPulo(BoxCollider2D boxX){
		float yMax = boxX.bounds.extents.y + boxX.bounds.center.y;
		bool pode = false;     

        if (yMax < transform.position.y-sobraDistY) {
			yMax = DistaciaX (yMax, transform.position.y);
			float tempoQueda = Mathf.Pow (2f * yMax / Physics2D.gravity.magnitude, 0.5f);
			float distPulo = tempoQueda * speed;
            float distMin = transform.position.x - collid.bounds.extents.x - distPulo;
            float distMax = transform.position.x + collid.bounds.extents.x + distPulo;

            float pontD = paredes.PontaX(boxX.gameObject, true);
            float pontE = paredes.PontaX(boxX.gameObject, false);
            bool c1, c2, c3, c4;
            c1 = pontD > distMin;
            c2 = pontD > distMax;
            c3 = pontE > distMin;
            c4 = pontE > distMax;

			if ((c1 ^ c2) || (c3 ^ c4) || (c1 ^ c3) || (c1 ^ c4) || (c2 ^ c3) || (c2 ^ c4)) {
				pode = true;
			}
		}

		return pode;
	}

    //Som apenas quando esta na tela
    void OnBecameVisible(){
		if (!esteAudio.enabled) {
			esteAudio.enabled = true;
		}
	}
	void OnBecameInvisible(){
		if (esteAudio.enabled) {
			esteAudio.enabled = false;
		}
	}
	//Alcança com o pulo ?
	public bool AlcacancaPulo(GameObject objetoX){
		float yMax;
		bool podePular = false;
        Bounds b = objetoX.GetComponent<Collider2D>().bounds;
	    yMax = b.center.y+b.extents.y-transform.position.y;

        if (yMax < puloAltura)
        {
            float tempoDePulo = Mathf.Pow(2f * puloAltura / Physics2D.gravity.magnitude, 0.5f);
            float tempoQueda = Mathf.Pow(2f * (puloAltura-yMax) / Physics2D.gravity.magnitude, 0.5f);

            float distMin = 0f, distMax = 0f;
            //tempoQueda -= Mathf.Pow (2f * (puloAltura - yMax) / Physics2D.gravity.magnitude, 0.5f);
            if (yMax < 0f)
            {
                distMax = (tempoDePulo + tempoQueda) * speed;
                distMin = -distMax;              
            }else
            {
                float tempoChegada = Mathf.Pow(yMax * 2f / Physics2D.gravity.magnitude, 0.5f);

                distMax = (tempoDePulo + tempoChegada) * speed;
                distMin = -distMax;
            }

            distMax += transform.position.x + collid.bounds.extents.x;
            distMin += transform.position.x - collid.bounds.extents.x;

            float pontD = paredes.PontaX(objetoX, true);
            float pontE = paredes.PontaX(objetoX, false);
            bool c1, c2, c3, c4;
            c1 = pontD > distMin;
            c2 = pontD > distMax;
            c3 = pontE > distMin;
            c4 = pontE > distMax;

            if ((c1 ^ c2) || (c3 ^ c4) || (c1 ^ c3) || (c1 ^ c4) || (c2 ^ c3) || (c2 ^ c4))
            {
                if (objetoX.tag == "Chao")
                {
                    if (!EstaNoMeio(objetoX))
                    {
                        podePular = true;
                    }
                    else
                    {
                        PlatformEffector2D plataAux = objetoX.GetComponent<PlatformEffector2D>();
                        if (plataAux)
                        {
                            podePular = true;
                        }
                    }
                }else
                {
                    podePular = true;
                }              
            }
        }
		return podePular;
	}
	// No meio de objetos como plataformas
	private bool EstaNoMeio(GameObject obj){
		float aux = obj.GetComponent<BoxCollider2D> ().bounds.extents.x;
		bool esta = false;
		float minhaPosX = transform.position.x;
		float posObjX = obj.transform.position.x;
		if(minhaPosX >= (posObjX-aux) && minhaPosX <= (posObjX+aux)){
			esta = true;
			//Debug.Log ("Entrou");
		}
		return esta;
	}

    private void AproximarAcima()
    {
        Transform hero = atributosHeroi.heroiX.transform;
        bool semPulo = AlcacancaSemPulo(atributosHeroi.heroiX.GetComponent<BoxCollider2D>());
        idPlata = -1;
        if (SemObstaculosEntrePlay() && ((AlcancaHeroiEmPulo() && noChao) || semPulo))
        {
            podeCair = true;
            if (!semPulo && noChao)
            {
                Pular();
            }
            if(hero.position.x < transform.position.x)
            {
                viradoDireita = false;
            }else
            {
                viradoDireita = true;
            }

            if (DistaciaX(transform.position.x, atributosHeroi.heroiX.transform.position.x) < collid.bounds.extents.x)
            {
                Move(false, viradoDireita,true);
            }
            else
            {
                Move(true, viradoDireita,true);
            }
        }
        else
        {
            float distX1 = transform.position.x - hero.position.x;
            float distX2 = 0f;
            float distX2Longe = 0f;
            float distXX = 0f;

            for (int i = 0; i < fases.faseX.plataformas.Length; i++)
            {
                Bounds bod = fases.faseX.plataformas[i].GetComponent<BoxCollider2D>().bounds;
                float altMax = bod.center.y + bod.extents.y;

                if (distX1 >= 0f)
                {
                    distX2 = transform.position.x - paredes.PontaX(fases.faseX.plataformas[i], true);
                    distX2Longe = transform.position.x - paredes.PontaX(fases.faseX.plataformas[i], false);
                }
                else
                {
                    distX2 = transform.position.x - paredes.PontaX(fases.faseX.plataformas[i], false);
                    distX2Longe = transform.position.x - paredes.PontaX(fases.faseX.plataformas[i], true);
                }

                if (altMax > (transform.position.y + sobraDistY) && (altMax - transform.position.y) < puloAltura && distX2Longe * distX1 > 0f && (ultSaida == fases.faseX.plataformas[i] || (ultSaida && SemObstaculosEntre(ultSaida.gameObject, fases.faseX.plataformas[i]) || SemObstaculosEntre(gameObject, fases.faseX.plataformas[i]))))
                {
                    float distXX2 = DistaciaX(hero.position.y, altMax);

                    if (idPlata == -1)
                    {
                        idPlata = i;
                        distXX = distXX2;
                    }
                    else
                    {
                        if (distXX > distXX2)
                        {
                            idPlata = i;
                            distXX = distXX2;
                        }
                    }
                }
            }           

            if (idPlata < 0f)
            {
                for (int i = 0; i < fases.faseX.plataformas.Length; i++)
                {
                    if (!ultSaida || (ultSaida && fases.faseX.plataformas[i] != ultSaida.gameObject))
                    {
                        Bounds bod = fases.faseX.plataformas[i].GetComponent<BoxCollider2D>().bounds;
                        float altMax = bod.center.y + bod.extents.y;

                        if (altMax > (transform.position.y + sobraDistY) && (altMax - transform.position.y) < puloAltura && (SemObstaculosEntre(gameObject, fases.faseX.plataformas[i]) || (ultSaida && SemObstaculosEntre(ultSaida.gameObject, fases.faseX.plataformas[i]))))
                        {
                            if (distX1 >= 0f)
                            {
                                distX2 = transform.position.x - paredes.PontaX(fases.faseX.plataformas[i], true);
                            }
                            else
                            {
                                distX2 = transform.position.x - paredes.PontaX(fases.faseX.plataformas[i], false);
                            }
                            float distXX2 = 0f;
                            if (distX2 >= 0f)
                            {
                                distXX2 = DistaciaX(paredes.PontaX(fases.faseX.plataformas[i], true), transform.position.x);
                            }
                            else
                            {
                                distXX2 = DistaciaX(paredes.PontaX(fases.faseX.plataformas[i], false), transform.position.x);
                            }
                            if (idPlata == -1)
                            {
                                idPlata = i;
                                distXX = distX2;
                            }
                            else
                            {
                                if (distXX > distX2)
                                {
                                    idPlata = i;
                                    distXX = distXX2;
                                }
                            }
                        }
                    }
                }
            }

            if (idPlata >= 0)
            {
                if (distX1 <= 0f)
                {
                    posXPraIr = paredes.PontaX(fases.faseX.plataformas[idPlata], true);
                }
                else
                {
                    posXPraIr = paredes.PontaX(fases.faseX.plataformas[idPlata], false);
                }
                if (transform.position.x > posXPraIr)
                {
                    viradoDireita = false;
                }
                else
                {
                    viradoDireita = true;
                }

                if (AlcacancaPulo(fases.faseX.plataformas[idPlata]))
                {
                    Pular();
                }

                Move(true, viradoDireita,true);
            }
            else
            {
                if (transform.position.x > atributosHeroi.heroiX.transform.position.x)
                {
                    viradoDireita = false;
                }
                else
                {
                    viradoDireita = true;
                }
                if (DistaciaX(transform.position.x, atributosHeroi.heroiX.transform.position.x) < collid.bounds.extents.x)
                {
                    Move(false, viradoDireita,true);
                }
                else
                {
                    Move(true, viradoDireita,true);
                }
            }
        }
    } 

    public bool SemObstaculosEntrePlay()
    {
        bool ret = true;

        Vector2 pontaC = collid.bounds.center;
       
        Vector2 cObj = atributosHeroi.heroiX.GetComponent<Collider2D>().bounds.center;
        Vector2 distX = cObj - pontaC;
        distX = distX / distX.magnitude;
        RaycastHit2D[] hitados = Physics2D.RaycastAll(pontaC, distX, limiteTela.x);
        for (int i = 0; i < hitados.Length; i++)
        {
            if (hitados[i].collider.gameObject == atributosHeroi.heroiX)
            {
                i = hitados.Length;
            }
            else if (hitados[i].collider.tag == "Chao" || hitados[i].collider.tag == "Parede")
            {
                i = hitados.Length;
                ret = false;
            }

        }

        return ret;
    }

    public bool SemObstaculosEntre(GameObject obj0,GameObject objF)
    {
        Collider2D c0 = obj0.GetComponent<Collider2D>();
        Collider2D cF = objF.GetComponent<Collider2D>();

        Vector2 cent0 = c0.bounds.center;
        if (obj0.tag == "Chao")
        {
            cent0.y += c0.bounds.extents.y;
        }else
        {
            cent0.y -= c0.bounds.extents.y;
        }
        Vector2 cent02 = cent0;
        cent0.x += c0.bounds.extents.x;
        cent02.x -= c0.bounds.extents.x;

        Vector2 centF = cF.bounds.center;
        if(objF.tag == "Chao")
        {
            centF.y += cF.bounds.extents.y;
        }else
        {
            centF.y -= cF.bounds.extents.y;
        }
        centF.x += cF.bounds.extents.x;
        Vector2 centF2 = centF;
        centF2.x -= cF.bounds.size.x;

        float dist;
        Vector2 direX;

        RaycastHit2D hit1;

        if (centF.y-sobraDistY < cent0.y)
        {
            float anguloZ = -90f;

            RaycastHit2D hit2;
            Vector2 dire;

            dist = Vector2.Distance(centF, cent02);        

            if (dist < Vector2.Distance(centF2, cent0))
            {
                dist = Vector2.Distance(centF2, cent0);
            }

            dist += sobraDistY * 2f;
            cent0 += Vector2.one * sobraDistY;
            cent02.y += sobraDistY;
            cent02.x -= sobraDistY;

            for (int i = 0; i < 10; i++)
            {
                if (i > 0)
                {
                    anguloZ += 9f;
                }
                direX = Vector2.right;
                if (cent0.x < centF2.x)
                {
                    direX.x *= -1f;
                }
                dire = Quaternion.Euler(Vector3.forward * anguloZ) * direX;
                hit1 = Physics2D.Raycast(cent0, dire, dist, fases.faseX.chao);

                if (cent02.x > centF.x)
                {
                    direX = Vector2.left;
                }
                else
                {
                    direX = Vector2.right;
                }
                dire = Quaternion.Euler(Vector3.forward * anguloZ) * direX;
                hit2 = Physics2D.Raycast(cent02, dire, dist, fases.faseX.chao);
              
                if ((hit1 && hit1.collider.gameObject == objF && hit1.point.y + sobraDistY > centF2.y) || (hit2 && hit2.collider.gameObject == objF && hit2.point.y + sobraDistY > centF.y))
                {
                    return true;
                }
            }
        }else
        {
            dist = Vector2.Distance(centF, cent02);
            Vector2 centFX = centF, cent0X = cent02;
            cent0X.x -= sobraDistY;

            if (dist > Vector2.Distance(centF2, cent0))
            {
                dist = Vector2.Distance(centF2, cent0);
                centFX = centF2;
                cent0X = cent0;
                cent0X.x += sobraDistY;
            }

            if (dist > Vector2.Distance(centF, cent0))
            {
                dist = Vector2.Distance(centF, cent0);
                centFX = centF;
                cent0X = cent0;
                cent0X.x += sobraDistY;
            }

            if (dist > Vector2.Distance(centF2, cent02))
            {
                dist = Vector2.Distance(centF2, cent02);
                centFX = centF2;
                cent0X = cent02;
                cent0X.x -= sobraDistY;
            }

            direX = centFX - cent0X + sobraDistY * Vector2.right;
            direX = direX / dist;

            dist += sobraDistY*2f;
            cent0X.y += sobraDistY;

            hit1 = Physics2D.Raycast(cent0X, direX, dist, fases.faseX.chao);

            if (hit1 && hit1.collider.gameObject == objF)
            {
                //Debug.Log("Foi");
                return true;
            }
        }
        return false;
    }

    private void AproximarAbaixo()
    {
        idPlata = -1;
        Transform hero = atributosHeroi.heroiX.transform;
        bool semPulo = AlcacancaSemPulo(atributosHeroi.heroiX.GetComponent<BoxCollider2D>());
        if (SemObstaculosEntrePlay() && ((AlcancaHeroiEmPulo() && noChao) || semPulo))
        {
            podeCair = true;
            if (!semPulo && noChao)
            {
                Pular();
            }
            if (hero.position.x < transform.position.x)
            {
                viradoDireita = false;
            }
            else
            {
                viradoDireita = true;
            }

            if (DistaciaX(transform.position.x, atributosHeroi.heroiX.transform.position.x) < collid.bounds.extents.x)
            {
                Move(false, viradoDireita,true);
            }
            else
            {
                Move(true, viradoDireita,true);
            }
        }
        else
        {
            float distX2 = 0f;

            float posX1 = 0f;
            float posX2 = 0f;
            float altP = 0f;
            float distX1 = transform.position.x - hero.position.x;

            for (int i = 0; i < fases.faseX.plataformas.Length; i++)
            {
                if (distX1 > 0f)
                {
                    distX2 = transform.position.x - paredes.PontaX(fases.faseX.plataformas[i], false);
                    posX2 = paredes.PontaX(fases.faseX.plataformas[i], false);
                }
                else
                {
                    distX2 = transform.position.x - paredes.PontaX(fases.faseX.plataformas[i], true);
                    posX2 = paredes.PontaX(fases.faseX.plataformas[i], true);
                }
                Bounds bod = fases.faseX.plataformas[i].GetComponent<BoxCollider2D>().bounds;
                float altMax = bod.center.y + bod.extents.y;

                if (altMax < puloAltura + transform.position.y && distX2 * distX1 >= 0f && hero.position.y + sobraDistY >= altMax && (ultSaida == fases.faseX.plataformas[i] || SemObstaculosEntre(gameObject, fases.faseX.plataformas[i]) || (ultSaida && SemObstaculosEntre(ultSaida.gameObject, fases.faseX.plataformas[i]))))
                {
                    if (idPlata == -1)
                    {
                        idPlata = i;
                        altP = altMax;
                        posX1 = posX2;
                    }
                    else
                    {
                        float xy = DistaciaX(altP, hero.position.y);
                        float yy = DistaciaX(altMax, hero.position.y);
                        if (xy > yy)
                        {
                            idPlata = i;
                            posX1 = posX2;
                            altP = altMax;
                        }
                    }
                }
            }

            if (idPlata < 0)
            {
                for (int i = 0; i < fases.faseX.plataformas.Length; i++)
                {
                    Bounds bod = fases.faseX.plataformas[i].GetComponent<BoxCollider2D>().bounds;
                    float altMax = bod.center.y + bod.extents.y;

                    if (distX1 <= 0f)
                    {
                        distX2 = transform.position.x - paredes.PontaX(fases.faseX.plataformas[i], true);
                        posX2 = paredes.PontaX(fases.faseX.plataformas[i], true);
                    }
                    else
                    {
                        distX2 = transform.position.x - paredes.PontaX(fases.faseX.plataformas[i], false);
                        posX2 = paredes.PontaX(fases.faseX.plataformas[i], false);
                    }
                    if ((altMax < transform.position.y + puloAltura && ((ultSaida == fases.faseX.plataformas[i])|| SemObstaculosEntre(gameObject, fases.faseX.plataformas[i])) || (ultSaida && SemObstaculosEntre(ultSaida.gameObject, fases.faseX.plataformas[i]))))
                    {
                        if (idPlata == -1)
                        {
                            idPlata = i;
                            altP = altMax;
                            posX1 = posX2;
                        }
                        else
                        {
                            float xy = DistaciaX(altP, hero.position.y);
                            float yy = DistaciaX(altMax, hero.position.y);
                            if (xy > yy)
                            {
                                idPlata = i;
                                posX1 = posX2;
                                altP = altMax;
                            }
                        }
                    }
                }
            }

            if (idPlata >= 0)
            {
                if (transform.position.x <= posX1)
                {
                    viradoDireita = true;
                }
                else
                {
                    viradoDireita = false;
                }
                posXPraIr = posX1;
                BoxCollider2D box = fases.faseX.plataformas[idPlata].GetComponent<BoxCollider2D>();
                if (AlcacancaSemPulo(box))
                {
                    podeCair = true;
                    DescerPlataforma();
                }
                else if (AlcacancaPulo(fases.faseX.plataformas[idPlata]))
                {
                    Pular();
                }

                if(DistaciaX(transform.position.x,posXPraIr) < collid.bounds.extents.x/2f)
                {
                    Move(false, viradoDireita,true);
                }else
                {
                    Move(true, viradoDireita,true);
                }
            }
            else
            {
                if (transform.position.x > atributosHeroi.heroiX.transform.position.x)
                {
                    viradoDireita = false;
                }
                else
                {
                    viradoDireita = true;
                }
                if (DistaciaX(transform.position.x, atributosHeroi.heroiX.transform.position.x) < collid.bounds.extents.x)
                {
                    Move(false, viradoDireita,true);
                }
                else
                {
                    Move(true, viradoDireita,true);
                }
            }
        }
    }

    private void HeroiAproximaTolera(float quantoX, bool tolerancia)
    {
        float distX = DistaciaX(transform.position.x, atributosHeroi.heroiX.transform.position.x);
        if (!tolerado)
        {
            if (distX < quantoX && ultSaida && (paredes.PontaX(ultSaida.gameObject, true) < transform.position.x || paredes.PontaX(ultSaida.gameObject, false) < transform.position.x))
            {
                Move(false, EleEstaDireita(),true);
            }
            else
            {
                Move(true, EleEstaDireita(),true);
            }
        }
        else
        {
            Move(false, EleEstaDireita(),true);
            if (tolerancia)
            {           
                if (distX > quantoX + 0.25f)
                {
                    tolerado = false;
                }
            }
            else
            {
                tolerado = false;
            }
        }
    }
       
	public void SeAproximar(float quantoX,bool tolerencia){
		nIrEsq = false;
		nIrDirt = false;
        podeCair = false;

        if ((AlcancaHeroiEmPulo() && SemObstaculosEntrePlay()) || noChao)
        {
            float distY2 = transform.position.y - atributosHeroi.heroiX.transform.position.y;
            idPlata = -1;
            if (!MesmaAltura())
            {
                if (distY2 > 0)
                {
                    AproximarAbaixo();
                }
                else
                {
                    AproximarAcima();
                }
            }
            else
            {
                HeroiAproximaTolera(quantoX, tolerencia);
            }

            //Se estiver na ponta
            idPlata2 = -1;
            if (!podeCair && ((pontaDrt && viradoDireita) || (pontaEsq && !viradoDireita)))
            {
                SeAproximarCuidado();
            }
        }else if (idPlata >= 0 && idPlata2 < 0)
        {
            if (DistaciaX(transform.position.x, posXPraIr) >= collid.bounds.extents.x)
            {
                if (transform.position.x > posXPraIr)
                {
                    viradoDireita = false;
                }
                else
                {
                    viradoDireita = true;
                }
                Move(true, viradoDireita,true);
            }
            else
            {
                Move(false, viradoDireita,true);
            }
        }
        else if (idPlata2 >= 0)
        {
            if (DistaciaX(transform.position.x, posXPraIr2) >= collid.bounds.extents.x)
            {
                if (transform.position.x > posXPraIr2)
                {
                    viradoDireita = false;
                }
                else
                {
                    viradoDireita = true;
                }
                Move(true, viradoDireita,true);
            }
            else
            {
                Move(false, viradoDireita,true);
            }
        }else
        {
            HeroiAproximaTolera(quantoX, tolerencia);
        }
	}

    private bool AlcancaHeroiEmPulo()
    {
        bool ret = false;
        float tempoQueda = 0f;
        if (!noChao)
        {
            if (transform.position.y > atributosHeroi.heroiX.transform.position.y)
            {
                if (esteCorpo.velocity.y * Physics2D.gravity.y > 0f)
                {
                    tempoQueda = esteCorpo.velocity.y / Physics2D.gravity.y;
                }else
                {
                    tempoQueda = -esteCorpo.velocity.y / Physics2D.gravity.y + Mathf.Pow((transform.position.y - atributosHeroi.heroiX.transform.position.y) * -2f / Physics2D.gravity.y, 0.5f);
                }
            }
        }else
        {
            tempoQueda = Mathf.Pow((puloAltura+transform.position.y-atributosHeroi.heroiX.transform.position.y) * -2f / Physics2D.gravity.y, 0.5f)*2f;
        }

        if (DistaciaX(transform.position.x, atributosHeroi.heroiX.transform.position.x) <= tempoQueda * speed)
        {
            ret = true;
        }
        return ret;
    }

	//com cuidado para não cair
	private void SeAproximarCuidado(){
		//float altMax2 = 0f;
		idPlata2 = -1;
        if (AlcancaHeroiEmPulo() || (idPlata >= 0 && AlcacancaPulo(fases.faseX.plataformas[idPlata])))
        {
            Vector2 pos0 = Vector2.zero;
            if (idPlata >= 0)
            {
                pos0.x = paredes.PontaInversa(fases.faseX.plataformas[idPlata], posXPraIr);
                Bounds b = fases.faseX.plataformas[idPlata].GetComponent<BoxCollider2D>().bounds;
                pos0.y += b.center.y + b.extents.y;
            }
            else
            {
                pos0 = atributosHeroi.heroiX.transform.position;
            }
            Vector2 posF = Vector2.zero;
            Vector2 posF2 = Vector2.zero;
            float d1 = 0f, d0 = Vector2.Distance(transform.position, pos0);
            //Debug.Log ("id2 "+idPlata2);
            for (int i = 0; i < fases.faseX.plataformas.Length; i++)
            {
                Bounds bod = fases.faseX.plataformas[i].GetComponent<BoxCollider2D>().bounds;
                posF.y = bod.center.y + bod.extents.y;

                if (pos0.x > transform.position.x)
                {
                    posF.x = paredes.PontaX(fases.faseX.plataformas[i], false);
                }
                else
                {
                    posF.x = paredes.PontaX(fases.faseX.plataformas[i], true);
                }
                d1 = Vector2.Distance(pos0, posF);

                if ((posF.y - transform.position.y) < puloAltura && d1 < d0 && (SemObstaculosEntre(gameObject, fases.faseX.plataformas[i]) || (ultSaida && SemObstaculosEntre(ultSaida.gameObject, fases.faseX.plataformas[i]))))
                {
                    if (idPlata2 < 0)
                    {
                        idPlata2 = i;
                        posF2 = posF;
                    }
                    else
                    {
                        float dist1 = Vector2.Distance(posF2, transform.position);
                        float dist2 = Vector2.Distance(posF, transform.position);
                        if (dist1 > dist2)
                        {
                            //Debug.Log ("Entrou " + " d0 " + d0 + " d1 " + d1 + " plata " + faseX.plataformas [idPlata2].name + " d0 " + Vector2.Distance (transform.position, posF2) + " d1 " + Vector2.Distance (transform.position, posF));
                            idPlata2 = i;
                            posF2 = posF;
                        }
                    }
                }
            }
        }

		if (idPlata2 >= 0 && ultSaida && ultSaida.gameObject == fases.faseX.plataformas[idPlata2]) {
			idPlata2 = -1;
		}

		if (idPlata2 >= 0) {
            if (atributosHeroi.heroiX.transform.position.x < transform.position.x)
            {
                posXPraIr2 = paredes.PontaX(fases.faseX.plataformas[idPlata2], false);
            }else
            {
                posXPraIr2 = paredes.PontaX(fases.faseX.plataformas[idPlata2], true);
            }
			if (transform.position.x > posXPraIr2) {
				viradoDireita = false;
			} else {
				viradoDireita = true;
			}

			if (AlcacancaSemPulo (fases.faseX.plataformas [idPlata2].GetComponent<BoxCollider2D> ())) {
                podeCair = true;
                //Debug.Log(1);
				DescerPlataforma ();
			} else if (AlcacancaPulo (fases.faseX.plataformas [idPlata2])) {
				Pular ();
			}

            if (DistaciaX(transform.position.x, posXPraIr2) < collid.bounds.extents.x)
            {
                Move(false, viradoDireita,true);
            }
            else
            {
                Move(true, viradoDireita,true);
            }
        }
        else if(idPlata>=0)
        {
            if (transform.position.x > posXPraIr)
            {
                viradoDireita = false;
            }
            else
            {
                viradoDireita = true;
            }

            if (AlcacancaSemPulo(fases.faseX.plataformas[idPlata].GetComponent<BoxCollider2D>()))
            {
                podeCair = true;
                DescerPlataforma();
            }
            else if (AlcacancaPulo(fases.faseX.plataformas[idPlata]))
            {
                Pular();
            }

            if (DistaciaX(transform.position.x, posXPraIr) < collid.bounds.extents.x)
            {
                Move(false, viradoDireita,true);
            }
            else
            {
                Move(true, viradoDireita,true);
            }
        }else
        {
            Move(false, viradoDireita,true);
        }
	}

	private bool EleEstaDireita(){
		if (transform.position.x <= atributosHeroi.heroiX.transform.position.x) {
			return true;
		} else {
			return false;
		}
	}

	public void SeAfastar(float quantoX, bool tolera){
		encurralado = false;
        bool ir = false;
        if (noChao)
        {
            //Debug.Log ("Afastando");
            if (obstaculoDir || pontaDrt)
            {
                nIrDirt = true;
            }
            if (obstaculoEsq || pontaEsq)
            {
                nIrEsq = true;
            }
            float distX = DistaciaX(transform.position.x, atributosHeroi.heroiX.transform.position.x);
            if (!tolerado)
            {
                if (distX < quantoX)
                {
                    ir = true;
                    VirarParaPlayer();
                    viradoDireita = !EleEstaDireita();
                }
            }
            else
            {
                nIrEsq = false;
                nIrDirt = false;
                ir = false;
                if (tolera)
                {
                    quantoX = quantoX + 0.25f;
                    if (distX < quantoX)
                    {
                        tolerado = false;
                    }
                }
                else if (tolerado)
                {
                    tolerado = false;
                }
            }
            if (ir)
            {
                viradoDireita = !EleEstaDireita();
                if (viradoDireita)
                {
                    if (pontaDrt)
                    {
                        Move(false, EleEstaDireita(),false);
                        encurralado = true;
                    }
                }
                else
                {
                    if (pontaEsq)
                    {
                        Move(false, EleEstaDireita(),false);
                        encurralado = true;
                    }
                }
            }else
            {
                VirarParaPlayer();
            }

            if (!encurralado)
            {
                Move(ir, viradoDireita, true);
            }
        }else
        {
            Move(false, EleEstaDireita(),false);
        }
	}

	public bool MesmaAltura(){
		bool retorno = false;
		float distY = DistaciaX (transform.position.y, atributosHeroi.heroiX.transform.position.y);
		if (distY <= sobraDistY) {
			retorno = true;
		}

		return retorno;
	}

	public void Caminhar(){
        if (!atributos.eBoss)
        {
            if (obstaculoDir || pontaDrt)
            {
                nIrDirt = true;
            }
            if (obstaculoEsq || pontaEsq)
            {
                nIrEsq = true;
            }
            if (tempoCaminhar >= 3f)
            {
                int qualMove = Random.Range(-1, 2);// 0 parado, 1 direita, -1 esquerda
                nIrEsq = false;
                nIrDirt = false;
                //Debug.Log(qualMove);
                if (qualMove == 0)
                {
                    Move(false, viradoDireita,false);
                }
                else
                {
                    if (qualMove == -1)
                    {
                        viradoDireita = false;
                    }
                    else
                    {
                        viradoDireita = true;
                    }
                    Move(true, viradoDireita,false);
                }

                tempoCaminhar = 0f;
            }

            if (viradoDireita)
            {
                if (nIrDirt)
                {
                    Move(false, !viradoDireita,false);
                }
            }
            else
            {
                if (nIrEsq)
                {
                    Move(false, viradoDireita,false);
                }
            }

            tempoCaminhar += Time.deltaTime;
        }
	}

	private void Ver(){
        Vector2 pontaC,pontaI = transform.position;
        pontaC.x = transform.position.x;
        pontaC.y = transform.position.y;
        if (ForaChao())
        {
            pontaC.y += collid.bounds.size.y;
            pontaI.y -= visaoY;
        }
        else
        {
            pontaC.y += visaoY;
        }

		if (viradoDireita) {
			pontaC.x += limiteTela.x;
		} else {
			pontaC.x -= limiteTela.x;
            //Debug.Log(pontaC.x);
		}

        Collider2D collplay = Physics2D.OverlapArea(pontaI, pontaC, fases.faseX.playMask.value);
		if (collplay) {
            Vector2 olhosPos = transform.position;
            olhosPos.y += collid.bounds.size.y;
            Vector2 distX = new Vector2(collplay.transform.position.x,collplay.bounds.center.y) - olhosPos;
            distX = distX / distX.magnitude;
            RaycastHit2D[] hitados = Physics2D.RaycastAll (olhosPos,distX,limiteTela.x);
			for (int i = 0; i < hitados.Length; i++) {
				if(hitados[i].collider.tag == "Chao" || hitados[i].collider.tag == "Parede")
                {
                    i = hitados.Length;
                }
                else if (hitados[i].collider.tag == "Player")
                {
                    viu = true;                  
                    i = hitados.Length;
                    exclamacao.SetActive(true);
                    exclamaAudio.PlayOneShot(fases.faseX.encontrado);
                    tempoExclama = 1f;
                }
            }
		}
	}

	public void VirarParaPlayer(){
		if (EleEstaDireita ()) {
			viradoDireita = true;
		} else {
			viradoDireita = false;
		}
	}

	public float DistaciaX(float a, float b){
		float c = a - b;
		if (c < 0) {
			c = -c;
		}
		return c;
	}
		
	public void Pular(){
		if (noChao && anima.GetInteger("estados") == 0) {
			//Debug.Log ("Pulou");
			esteCorpo.velocity = new Vector2 (esteCorpo.velocity.x, vPulo);
		}
	}

	public void AndarAletoriamente(){
        bool mX = false;

        if(moveX != 0f)
        {
            mX = true;
        }
        if ((tempoCaminhar >= 2f && mX) || (tempoCaminhar >= 1f && !mX))
        {
            tempoCaminhar = 0f;
            if (Random.value >= 0.8f)
            {
                mX = true;
            }
            if (mX)
            {
                bool dir = false;
                if (Random.value >= 0.5f)
                {
                    dir = true;
                }
                if (dir && !obstaculoDir && !pontaDrt)
                {
                    viradoDireita = true;
                }else
                {
                    viradoDireita = false;
                }

                if (!dir && !obstaculoEsq && !pontaEsq)
                {
                    viradoDireita = false;
                }
                else
                {
                    viradoDireita = true;
                }
            }else
            {
                VirarParaPlayer();
            }
        }else
        {
            tempoCaminhar += Time.deltaTime;
        }
        if (noChao && ultSaida && DistaciaX(transform.position.x, atributosHeroi.heroiX.transform.position.x) <= DistasciaDoPulo() / 2f)
        {
            mX = true;
            VirarParaPlayer();
            Pular();
        }
        Move(mX,viradoDireita,false);
	}

	private float DistasciaDoPulo(){
		float tempoDePulo = Mathf.Pow(2f * puloAltura / Physics2D.gravity.magnitude,0.5f)*2f;
		return tempoDePulo * speed;
	}

	private void DescerPlataforma(){
		if (esteCorpo.velocity.y == 0f) {
            if (ultSaida)
            {
                PlatformEffector2D ePlataforma = ultSaida.gameObject.GetComponent<PlatformEffector2D>();
                if (noChao && ePlataforma)
                {
                    Physics2D.IgnoreCollision(ultSaida, GetComponent<Collider2D>());
                }
            }
		}
	}

	
	private bool MesmaAlturaBox(BoxCollider2D plata){
		bool retorno = false;
		float alt = plata.transform.position.y + plata.bounds.extents.y;
		// Margem de erro
		alt -= sobraDistY;
		//Debug.Log (alt);
		//Debug.Log (transform.position.y);
		if (alt <= transform.position.y) {
			retorno = true;
		}

		return retorno;
	}

	public bool MaisAltoPlata(BoxCollider2D plata){
		bool retorno = false;
		float alt = plata.transform.position.y+plata.bounds.extents.y;
		alt -= sobraDistY;
		if (transform.position.y > alt) {
			retorno = true;
		}
		//Debug.Log (retorno);
		return retorno;
	}

    private bool PodePularObstaculo(bool direita)
    {
        if (noChao)
        {
            Vector2 centBox = collid.bounds.center;
            RaycastHit2D hitX;
            if (direita)
            {
                centBox.x -= sobraDistY;
                hitX = Physics2D.BoxCast(centBox, collid.bounds.size, 0f, Vector2.right, sobraDistY * 2f, fases.faseX.chao);

                if (hitX)
                {
                    float altY = hitX.collider.bounds.center.y + hitX.collider.bounds.extents.y;
                    if (altY < puloAltura + transform.position.y)
                    {
                        return true;
                    }
                }
            }
            else
            {
                centBox.x += sobraDistY;
                hitX = Physics2D.BoxCast(centBox, collid.bounds.size, 0f, Vector2.left, sobraDistY * 2f, fases.faseX.chao);

                if (hitX)
                {
                    float altY = hitX.collider.bounds.center.y + hitX.collider.bounds.extents.y;
                    if (altY < puloAltura + transform.position.y)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public void Move(bool moverse, bool direita, bool pularObstaculos)
    {
        if (moverse)
        {
            moveX = 1f;
        }
        else
        {
            moveX = 0f;
            Parar();
        }

        if (direita)
        {
            viradoDireita = true;
            if(obstaculoDir && PodePularObstaculo(direita) && moveX > 0f && pularObstaculos)
            {
                Pular();
            }         
        }
        else
        {
            viradoDireita = false;
            if (obstaculoDir && PodePularObstaculo(direita) && moveX > 0f && pularObstaculos)
            {
                Pular();
            }
            else
            {
                moveX *= -1f;
            }
        }
    }

    public void Parar()
    {
        if (atributos.estados == 0)
        {
            NaoDeslizar();
        }
    }

	void OnCollisionStay2D(Collision2D coll) {
		BoxCollider2D box = coll.gameObject.GetComponent<BoxCollider2D>();
		//Debug.Log (coll.contacts [0].point.y);
		if ((MaisAltoPlata (box) && !noChao && esteCorpo.velocity.y <= 0f) || !ultSaida) {
			noChao = true;
            //tempoAudioCaindo = 0f;
            if (esteAudio.enabled)
            {
                caiu = true;
            }
			//Debug.Log (tempoAudioCaindo);
			if (ultSaida != box) {
				if (ultSaida) {
					Physics2D.IgnoreCollision (ultSaida, GetComponent<Collider2D> (), false);
				}
				ultSaida = box;
			}
		}
	}
}
