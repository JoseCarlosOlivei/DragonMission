using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class atributosInimigos : MonoBehaviour {
    //Atributos Pricipais
	public float hp;
    public bool morreDeUma;
	public float esquiva;
	public float regeneracao;
	public float defesa;
	public float ataque;
	public float critico;
	public float agilidade;
	public float vigorDefesa = 0f;
	private float ataque0,defesa0,agilidade0, resistencia = 1f;
	public float tempoReacao = 3f;
	public float chanceDReagir;
	public float scoresDefesa = 0f;
	public float scoresPoder = 0f;
	public int max= 4;
	private int scoresValor;
	public AudioClip defendeu;
    public SpriteRenderer bForca, bRegenera, bDefesa, bSpeed, bAgil, bImortal;
	//public float tempoAcoes;
	// Auxiliares atributos
	public float hpAtual;
	private float tempoMorte = 0f;
	//Barra de vida
	private Slider hpBarra;
	//0 fora de luta, 1 atacando, 2 defesa ,3 quebradefesa,4 apanahdo, 5 morto,6 soltando poder
	public int estados =0;
	public Animator animacoes;
	private float dorPor=0f;
	public bool pararDor= false;
	private bool esquivou = false;
	private moveCPU move;
	private float[] buffPoder = new float [3], debuffPoder = new float[3];//0 dano/defesa, 1 resistencia,2 agilidade
    private List<buffs> meusBuff = new List<buffs>();
	private List<buffsSpeed> meusSpeedBuffs = new List<buffsSpeed>();
	private float speedX=1f;
	private List<buffsRecupera> buffsRegen = new List<buffsRecupera> ();
	private float stun = 0f,dorAcumulo = 0f;
	public bool leve = false;
	public float superArmadura = 0f;
	private bool sentiu = false;
	private float tempoDefesa = 0f;
    public bool eBoss;
    public bool invul = false;
    private float xLevouCritico = 0f;
    private SpriteRenderer esteRede;
    public GameObject missMensagem;
    public bool repetirAcao;
    public int acaoARepetir;

    // Use this for initialization
    void Awake()
    {
        hpAtual = hp;
        animacoes = this.gameObject.GetComponent<Animator>();
        move = GetComponent<moveCPU>();
        move.speed0 = move.speed;
        agilidade0 = agilidade;
        if (dataSave.jogoAtual.dificuldade >= 8f)
        {
            float ataqX = ataque;
            ataque = 320f * 16f / max;
            ataque = (ataque + Mathf.Pow(Mathf.Pow(ataque, 2f) + 640f * ataque, 0.5f)) / 2f;
            //Debug.Log ((ataque/(ataque+160f))*ataque/16f);
            ataqX = ataque / ataqX;
            dataSave.jogoAtual.dificuldade = 8f;
            scoresValor = (int)(scoresPoder * dataSave.jogoAtual.dificuldade * ataqX + scoresDefesa * dataSave.jogoAtual.dificuldade);
        }
        else
        {
            ataque *= dataSave.jogoAtual.dificuldade;
            scoresValor = (int)(scoresPoder * dataSave.jogoAtual.dificuldade * dataSave.jogoAtual.dificuldade + scoresDefesa * dataSave.jogoAtual.dificuldade);
        }
        ataque0 = ataque;
        defesa0 = defesa;
        for (int i = 0; i < buffPoder.Length; i++)
        {
            buffPoder[i] = 1f;
            debuffPoder[i] = 1f;
        }

        hpBarra = GetComponentInChildren<Slider>();
        hpBarra.maxValue = hp;
        hpBarra.value = hp;
        esteRede = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        if (eBoss)
        {
            hpBarra.gameObject.SetActive(false);
            hpBarra = fases.faseX.HpBossUtilizar(gameObject.name);
            hpBarra.maxValue = hp;
            hpBarra.value = hp;
            invul = true;
        }
    }

    // Update is called once per frame
    void Update () {
        //Ficar vermelho quando levar critico
        if (xLevouCritico > 0f)
        {
            if (xLevouCritico > 0.5f)
            {
                esteRede.color = Color.Lerp(Color.red, Color.white, xLevouCritico - xLevouCritico* 2f-1f);
            }
            else
            {
                esteRede.color = Color.Lerp(Color.white, Color.red, xLevouCritico * 2f );
            }

            xLevouCritico -= Time.deltaTime;
        }
        else if (esteRede.color != Color.white)
        {
            esteRede.color = Color.white;
        }
        if (estados != 5)
        {
            // Stun
            if (stun > 0f)
            {
                move.esteAudio.clip = null;
                if (animacoes.GetInteger("estados") != 4)
                {
                    estados = 4;
                    animacoes.SetInteger("estados", estados);
                }
                if (dorPor <= 0)
                {
                    animacoes.SetFloat("oQueSofre", 0f);
                }
                stun -= Time.deltaTime;
            }
            else if (stun <= 0f && estados == 4 && animacoes.GetFloat("oQueSofre") != 1f && animacoes.GetFloat("oQueSofre") != 2f)
            {
                stun = 0f;
                sentiu = false;
                estados = 0;
                animacoes.SetInteger("estados", estados);
            }
            //dor e esquiva
            if (dorPor > 0f)
            {
                if (pararDor)
                {
                    move.esteAudio.clip = null;
                    pararDor = false;
                    if (superArmadura <= 0f && animacoes.GetFloat("oQueSofre") != 1f && animacoes.GetFloat("oQueSofre") != 2f)
                    {
                        estados = 4;
                        animacoes.SetInteger("estados", estados);
                        animacoes.speed = 1f / dorPor;
                        if (esquivou)
                        {
                            animacoes.SetFloat("oQueSofre", 2f);
                        }
                        else
                        {
                            animacoes.SetFloat("oQueSofre", 1f);
                        }
                    }
                    else if (superArmadura > 0f)
                    {
                        dorAcumulo += dorPor;
                    }
                }

                if (estados == 2)
                {
                    if (tempoDefesa < 0f)
                    {
                        estados = 0;
                        animacoes.SetInteger("estados", estados);
                    }
                    else
                    {
                        tempoDefesa -= Time.deltaTime;
                    }
                }
                if (superArmadura <= 0)
                {
                    dorPor -= Time.deltaTime;
                }
                else if (dorAcumulo >= superArmadura)
                {
                    sentiu = true;
                    stun += superArmadura;
                    dorAcumulo -= superArmadura;
                }
            }
            else if ((animacoes.GetFloat("oQueSofre") == 1f || animacoes.GetFloat("oQueSofre") == 2f))
            {
                //Debug.Log (dorPor);
                if (stun == 0f)
                {
                    estados = 0;
                    animacoes.SetInteger("estados", estados);
                    animacoes.SetFloat("oQueSofre", 0f);
                    animacoes.speed = 1f;
                }
            }
        }
		//Debug.Log (animacoes.speed);
		if (hpAtual < 0) {
			hpAtual = 0;
		}
		if (hpAtual == 0f) {
			if (tempoMorte == 0f) {
                fases.faseX.inimigosMortos += 1;
				AudioSource esteAudio = GetComponent<AudioSource> ();
				animacoes.speed = 1f;
				estados = 5;
				animacoes.SetInteger ("estados", estados);
				GetComponent<Rigidbody2D> ().gravityScale = 0f;
				GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
				GetComponent<Collider2D> ().enabled = false;
				esteAudio.enabled = true;
				esteAudio.clip = fases.faseX.morteSom;
				esteAudio.pitch = 1f;
				esteAudio.Play ();
			}
			tempoMorte += Time.deltaTime;
			//Debug.Log (estados);
		}
		if (tempoMorte >= 3f) {
			upamento.AddScores(scoresValor);
            dataSave.jogoAtual.AddInimigosMortos(1);
			gameObject.SetActive (false);
		}
        float regeneB = 0f;
        if (fases.podeMover)
        {
            regeneB = BuffsCura() / resistencia;
            if (hpAtual < hp && estados != 5f)
            {
                hpAtual += regeneracao * hp * Time.deltaTime;
            }

            if (regeneB != 0f)
            {
                hpAtual += regeneB * Time.deltaTime;
            }
            // aplicando buffs
            VerificarBuffs();
        }
		if(hpAtual>hp){
			hpAtual = hp;
		}

		if (hpBarra.value != hpAtual) {
			hpBarra.value = hpAtual;
		}
		
        if (speedX != 1f)
        {
            move.speed = move.speed0 * speedX;
            if (speedX > 1f)
            {
                bSpeed.sprite = fases.faseX.bufSpeed;
            }
            else
            {
                bSpeed.sprite = fases.faseX.dBufSpeed;
            }
            bSpeed.gameObject.SetActive(true);
        }else if(move.speed != move.speed0)
        {
            move.speed = move.speed0;
            bSpeed.gameObject.SetActive(false);
        }

        if(regeneB != 0f)
        {
            if(regeneB > 0f)
            {
                bRegenera.sprite = fases.faseX.bufRege;
            }else
            {
                bRegenera.sprite = fases.faseX.dBufRege;
            }
            if (!bRegenera.gameObject.activeInHierarchy)
            {
                bRegenera.gameObject.SetActive(true);
            }
        }
        else if (bRegenera.gameObject.activeInHierarchy)
        {
            bRegenera.gameObject.SetActive(false);
        }

        if(buffPoder[0] / debuffPoder[0] != 1f)
        {
            ataque = ataque0 * buffPoder[0] / debuffPoder[0];
            defesa = defesa0 * buffPoder[0] / debuffPoder[0];
            if (buffPoder[0] / debuffPoder[0] > 1f)
            {
                bForca.sprite = fases.faseX.bufForca;
            }else
            {
                bForca.sprite = fases.faseX.dBufForca;
            }
            bForca.gameObject.SetActive(true);
        }else if(ataque != ataque0 || defesa != defesa0)
        {
            ataque = ataque0;
            defesa = defesa0;
            bForca.gameObject.SetActive(false);
        }

        if (buffPoder[1] / debuffPoder[1] != 1f)
        {
            resistencia = buffPoder[1] * dataSave.jogoAtual.dificuldade / debuffPoder[1];
            if (buffPoder[1] / debuffPoder[1] > 1f)
            {
                bDefesa.sprite = fases.faseX.bufDef;
            }
            else
            {
                bDefesa.sprite = fases.faseX.dBufDef;
            }
            bDefesa.gameObject.SetActive(true);
        }
        else if (resistencia != dataSave.jogoAtual.dificuldade)
        {
            resistencia = dataSave.jogoAtual.dificuldade;
            bDefesa.gameObject.SetActive(false);
        }

        if (buffPoder[2] / debuffPoder[2] != 1f)
        {
            agilidade = agilidade0 * buffPoder[2] / debuffPoder[2];
            if (buffPoder[2] / debuffPoder[2] > 1f)
            {
                bAgil.sprite = fases.faseX.bufAgil;
            }
            else
            {
                bAgil.sprite = fases.faseX.dBufAgil;
            }
            bAgil.gameObject.SetActive(true);
        }
        else if (agilidade != agilidade0)
        {
            agilidade = agilidade0;
            bAgil.gameObject.SetActive(false);
        }
       
	}

	public bool PlayerLadoDoAtaque(Transform ataque){
		float posRela = transform.position.x - ataque.position.x;
		if (posRela <= 0 && transform.localScale.x == 1f) {
			return true;
		} else if (posRela > 0 && transform.localScale.x == -1f) {
			return true;
		} else {
			return false;
		}
	}

	//Curas por tempo
	public void AddRecupera(buffsRecupera buff){
        if (!invul)
        {
            buffsRegen.Add(buff);
        }
	}

    private void AddBuffs(buffs b)
    {
        if (!invul)
        {
            GameObject b2 = Instantiate(b.gameObject);
            meusBuff.Add(b2.GetComponent<buffs>());
            b2.SetActive(false);
        }
    }

	private float BuffsCura(){
		float cura = 0f;
		for (int i = 0; i < buffsRegen.Count; i++) {
            if (!buffsRegen[i].verificado)
            {
                if (buffsRegen[i].durou <= buffsRegen[i].duracao)
                {
                    buffsRegen[i].durou += Time.deltaTime;
                    cura += buffsRegen[i].taxaRecuperacao;
                }
                else
                {
                    buffsRegen[i].verificado = true;
                }
            }
		}
		return cura;
	}

    public void AddBuffCura(buffsRecupera b)
    {
        if (!invul)
        {
            bool achou = false;
            for (int i = 0; i < buffsRegen.Count; i++)
            {
                if (buffsRegen[i].verificado && buffsRegen[i].nome == b.nome)
                {
                    buffsRegen[i].verificado = false;
                    buffsRegen[i].durou = 0f;
                    buffsRegen[i].taxaRecuperacao = b.taxaRecuperacao;
                    achou = true;
                }
            }
            if (!achou)
            {
                GameObject aux = Instantiate(b.gameObject);
                aux.SetActive(false);
                buffsRegen.Add(aux.GetComponent<buffsRecupera>());
            }
        }
    }

	public float DanoCalcu(float def,float mult){
		float dano = ataque/(def+ataque);
		dano *= ataque * mult;
		//Debug.Log (dano);
		if (FoiCritico()) {
			dano *= 3f;
		}

		return dano;
	}

	public float HabDanoCalcu(float def, float mult){
		float dano = ataque/(def+ataque);
		dano *= ataque * mult*agilidade;		

		return dano;
	}

    public float HabilidaPoder()
    {
        return ataque * agilidade;
    }

    //buffs
    public void Buffa(buffs buff)
    {
        PodeBuffa(buff);
        int onde = OndeBuffa(buff);
        if (meusBuff[onde])
        {
            if (meusBuff[onde].nome == buff.nome)
            {
                meusBuff[onde].durou = 0f;
            }
            else
            {
                GameObject aux = Instantiate(buff.gameObject) as GameObject;
                buff = aux.GetComponent<buffs>();
                buff.durou = 0f;
                buff.verificado = false;
                Destroy(meusBuff[onde]);
                meusBuff[onde] = buff;
            }
        }
        else
        {
            GameObject aux = Instantiate(buff.gameObject) as GameObject;
            buff = aux.GetComponent<buffs>();
            buff.durou = 0f;
            buff.verificado = false;
            meusBuff[onde] = buff;
        }
    }

	public void PodeBuffa(buffs qual){
		bool pode = false;
		for (int i = 0; i < meusBuff.Count; i++) {
			if (!meusBuff [i] || meusBuff[i].nome == qual.nome || meusBuff[i].durou>= meusBuff[i].duracao) {
				pode = true;
				i = meusBuff.Count;
			}
		}
        if (!pode)
        {
            AddBuffs(qual);
            pode = true;
        }
	}
	private int OndeBuffa(buffs qual){
		int onde = -1;
		for (int i = 0; i < meusBuff.Count; i++) {
			if (!meusBuff [i] || meusBuff[i].durou>= meusBuff[i].duracao) {
				onde = i;
			}
			if (meusBuff[i] && meusBuff [i].nome == qual.nome) {
				onde = i;
				i = meusBuff.Count;
			}
		}
		return onde;
	}
	public float TempoRest(buffs qual){
		float tempoRest = 0f;
		for (int i = 0; i < meusBuff.Count; i++) {
			if (meusBuff[i].nome == qual.nome) {
				tempoRest = meusBuff [i].durou;
				i = meusBuff.Count;
			}
		}
		return tempoRest;
	}
	private void VerificarBuffs(){
		for (int i = 0; i < meusBuff.Count; i++) {
			if (meusBuff [i]) {
				if (meusBuff [i].durou == 0f && !meusBuff[i].verificado) {
					buffPoder = meusBuff [i].SomaBuff (buffPoder);
					meusBuff [i].durou += Time.deltaTime;
					meusBuff [i].verificado = true;
				} else if (meusBuff [i].durou < meusBuff [i].duracao) {
					meusBuff [i].durou += Time.deltaTime;
				} else if (meusBuff [i].verificado) {
					buffPoder = meusBuff [i].SubtraBuff (buffPoder);
					meusBuff [i].verificado = false;
				}
			}
		}

		for (int i = 0; i < meusSpeedBuffs.Count; i++) {
			if (meusSpeedBuffs [i]) {
				if (meusSpeedBuffs [i].durou == 0f && !meusSpeedBuffs[i].verificado) {
					speedX *= meusSpeedBuffs [i].multSpeed;
					meusSpeedBuffs [i].durou += Time.deltaTime;
					meusSpeedBuffs [i].verificado = true;
				} else if (meusSpeedBuffs [i].durou < meusSpeedBuffs [i].duracao) {
					meusSpeedBuffs [i].durou += Time.deltaTime;
				} else if (meusSpeedBuffs [i].verificado) {
					speedX /= meusSpeedBuffs[i].multSpeed;
					meusSpeedBuffs[i].verificado = false;
				}
			}
		}
	}

    public void PodeBuffaSpeed(buffsSpeed qual){
        if (!invul)
        {
            bool pode = false;
            for (int i = 0; i < meusSpeedBuffs.Count; i++)
            {
                if (!meusSpeedBuffs[i] || meusSpeedBuffs[i].nome == qual.nome || meusSpeedBuffs[i].durou >= meusSpeedBuffs[i].duracao)
                {
                    pode = true;
                    i = meusBuff.Count;
                }
            }

            if (!pode)
            {
                GameObject b2 = Instantiate(qual.gameObject);
                meusSpeedBuffs.Add(b2.GetComponent<buffsSpeed>());
            }
        }
	}

	private int OndeBuffaSpeed(buffsSpeed qual){
		int onde = -1;
		for (int i = 0; i < meusSpeedBuffs.Count; i++) {
			if (!meusSpeedBuffs [i] || meusSpeedBuffs[i].durou>= meusSpeedBuffs[i].duracao) {
				onde = i;
			}
			if (meusSpeedBuffs[i] && meusSpeedBuffs [i].nome == qual.nome) {
				onde = i;
                i = meusBuff.Count;
			}
		}
		return onde;
	}


    public void SpeedBuff(buffsSpeed buff)
    {
        PodeBuffaSpeed(buff);
        int onde = OndeBuffaSpeed(buff);
        if (meusSpeedBuffs[onde])
        {
            if (meusSpeedBuffs[onde].nome == buff.nome)
            {
                meusSpeedBuffs[onde].durou = 0f;
            }
            else
            {
                GameObject aux = Instantiate(buff.gameObject) as GameObject;
                buff = aux.GetComponent<buffsSpeed>();
                buff.durou = 0f;
                buff.verificado = false;
                Destroy(meusSpeedBuffs[onde]);
                meusSpeedBuffs[onde] = buff;
            }
        }
        else
        {
            GameObject aux = Instantiate(buff.gameObject) as GameObject;
            buff = aux.GetComponent<buffsSpeed>();
            buff.durou = 0f;
            buff.verificado = false;
            meusSpeedBuffs[onde] = buff;
        }
    }

	public void AplicarDano(float dano,float dor,bool esquivou1,Vector2 force,bool critc){
        if (!invul)
        {
            move.viu = true;
            dano /= resistencia;
            esquivou = esquivou1;

            if (esquivou)
            {
                dano = 0f;
                if (missMensagem.transform.lossyScale.x < 0f)
                {
                    Vector3 scalX = missMensagem.transform.localScale;
                    scalX.x *= -1f;
                    missMensagem.transform.localScale = scalX;
                }
                missMensagem.SetActive(true);

                if (superArmadura > 0f)
                {
                    dorAcumulo -= dor;
                }
            }
            else
            {
                if (critc)
                {
                    dano *= 3f;
                    xLevouCritico = 1f;
                    cameraSeguir.cam.Tremer(0.05f, 0.5f);
                }
                if (dano < 0f)
                {
                    dano = 0f;
                    dor = 0f;
                }

                if (dor > 0f)
                {
                    if (!sentiu)
                    {
                        //Debug.Log (123);
                        dorPor = dor;
                        pararDor = true;
                    }

                    if (superArmadura <= 0f)
                    {
                        estados = 4;
                        move.NaoDeslizar();
                    }
                }

                move.esteCorpo.AddForce(force, ForceMode2D.Impulse);
                missMensagem.SetActive(false);             
            }

            hpAtual -= dano;
        }
	}

	public bool DefendeuSim(float danoX,float dorX,float posX,bool critc){
		bool retorno = false;
        if (critc)
        {
            danoX *= 3f;
            xLevouCritico = 1f;
        }

        if (FoiCritico())
        {
            danoX /= 3f;
        }
		if (vigorDefesa > 0 && (estados == 0 || (estados == 4 && stun == 0f)) && move.viu) {
			float chc = danoX / HabDanoCalcu (0f,1f);
			if (estados == 4 && stun <= 0f) {
				chc += 1f / 16f;
			}
            chc /= PoderEsquiva();
			chc /= vigorDefesa;
			chc = 1f - chc;
			if (chc > Random.value) {
				retorno = true;
				estados = 2;
				animacoes.SetInteger ("estados", estados);
				move.esteAudio.clip = defendeu;
				if (1f / dorX < defendeu.length) {
					move.esteAudio.pitch = defendeu.length / dorX;
				} else {
					move.esteAudio.pitch = 1f;
				}

				move.esteAudio.Play ();
				if (posX > transform.position.x) {
					move.viradoDireita = false;
				} else {
					move.viradoDireita = true;
				}
				tempoDefesa = dorX;
			}
		}

        if (retorno)
        {
            move.NaoDeslizar();
        }

		return retorno;
	}

	public void AplicarStun(float quanto){
        if (!invul)
        {
            move.NaoDeslizar();
            move.viu = true;
            stun += quanto;
        }
	}

    public float PoderEsquiva()
    {
        return 1f / (1f - esquiva);
    }

    public bool Esquivou()
    {
        bool ret = false;

        if (esquiva > Random.value)
        {
            ret = true;
        }

        return ret;
    }

	public float TempoReacaoCalc(){
		float tempo = Random.Range (0.5f, 1.5f)*tempoReacao;
		return tempo;
	}

	public float ChancesDeReagir(bool encurralado){
		float retorno = chanceDReagir;
		if (encurralado) {
			retorno = 1f - chanceDReagir;
			retorno /= 2f;
			retorno = 1f - retorno;
		}
       
		return retorno;
	}

	public bool FoiCritico(){
		bool ret = false;
		if (critico > Random.value) {
			ret = true;
		}
		return ret;
	}
}
