using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class atributosHeroi : MonoBehaviour {
	public float hp;
	public float danoBase;
	public float esquiva;
	public float vitalidade;
	public float defesa;
	public float agilidade;
	public float critico;
	public float regeneracao;
	public float vigor;
	public float recuperacao;
	public float hpAtual;
	public float vitalidadeAtual;
	public float vitalidadeAtaq=1f;
	public float vigorAtual;
	public Slider hpBarra;
	public Slider vigorBarra;
	public Text hpValor;
	public Text vigorValor;
	public Text vitalidadeValor;
	private moverJogador jogador;
	private float[] buffPoder = new float [3], debuffPoder = new float[3];//0 dano/defesa, 1 resistencia,2 agilidade
	private buffs[] meusBuff = new buffs[10];
	private buffsSpeed[] meusSpeedBuffs = new buffsSpeed[5];
	private float speedX=1f;
	private List<buffsRecupera> buffsRegen = new List<buffsRecupera> ();
	private float dano0,defesa0,agilidade0, resistencia =1f;
	public float speed0;
    public static GameObject heroiX;
    public GameObject missC;
    public Image bDef, bForca, bSpeed, bAgil, bRege, bImortal;
    private float xLevouCritico = 0f;
    private SpriteRenderer esteRede;
    public Image[] continues;
    public GameObject gameOverTela;
    private float tMorte = 0f;
    // Use this for initialization
    void Awake()
    {
        heroiX = gameObject;
        esteRede = GetComponent<SpriteRenderer>();
        AtualizarContinues();

        hp = 160f + 16f * dataSave.jogoAtual.hpNv;
        danoBase = 160f + 16f * dataSave.jogoAtual.danoNv;
        esquiva = 0.05f * dataSave.jogoAtual.esqNv;
        vitalidade = 0.1f * dataSave.jogoAtual.vitaliNv;
        defesa = 16f * dataSave.jogoAtual.defNv;
        agilidade = 2f + 0.2f * dataSave.jogoAtual.agilNv;
        critico = 0.05f * dataSave.jogoAtual.critNv;
        regeneracao = dataSave.jogoAtual.regeNv / 320f;
        vigor = 1f / 3f + 2f * dataSave.jogoAtual.vigorNv / 30f;
        recuperacao = 1f / 16f + dataSave.jogoAtual.recuNv / 64f;
        hpAtual = hp;
        vigorAtual = vigor;
        vitalidadeAtual = 1f;

        hpBarra.maxValue = hp;
        hpBarra.value = hpAtual;
        hpValor.text = "" + (int)hp;

        vigorBarra.maxValue = vigor;
        vigorBarra.value = vigorAtual;
        vigorValor.text = "" + (int)(vigorAtual * 100f);

        jogador = this.gameObject.GetComponent<moverJogador>();

        dano0 = danoBase;
        agilidade0 = agilidade;
        defesa0 = defesa;
        speed0 = jogador.speed;

        for (int i = 0; i < buffPoder.Length; i++)
        {
            buffPoder[i] = 1f;
            debuffPoder[i] = 1f;
        }
    }
    
    public void AtualizarContinues()
    {
        for(int i = 0; i < continues.Length; i++)
        {
            if (i >= dataSave.jogoAtual.continues)
            {
                continues[i].gameObject.SetActive(false);
            }else
            {
                continues[i].gameObject.SetActive(true);
            }
        }
    }

	// Update is called once per frame
	void Update () {
        //Ficar vermelho quando levar critico
        if (xLevouCritico > 0f)
        {
            if (xLevouCritico > 0.5f)
            {
                esteRede.color = Color.Lerp(Color.red, Color.white, xLevouCritico - xLevouCritico * 2f - 1f);
            }
            else
            {
                esteRede.color = Color.Lerp(Color.white, Color.red, xLevouCritico * 2f);
            }

            xLevouCritico -= Time.deltaTime;
        }
        else if (esteRede.color != Color.white)
        {
            esteRede.color = Color.white;
        }

        if (vigorAtual > vigor) {
			vigorAtual = vigor;
		}
		if (vigorAtual < vigor && jogador.estados == 0) {
			vigorAtual +=recuperacao*Time.deltaTime;
		}
		vigorBarra.value = vigorAtual;
		vigorValor.text =""+ (int)(vigorAtual * 100f);
		if(hpAtual>hp){
			hpAtual = hp;
		}

        float regeB = 0;
        if (fases.podeMover)
        {
            regeB = BuffsCura() / resistencia;
            if (hpAtual < hp && jogador.estados != 5)
            {
                hpAtual += regeneracao * hp * Time.deltaTime;
            }
            if (regeB != 0f)
            {
                hpAtual += regeB * Time.deltaTime;
            }
            // aplicando buffs
            VerificarBuffs();
        }

        if (hpAtual == 0 && jogador.estados != 5) {
			jogador.animacoes.speed = 1f;
			jogador.estados = 5;
			jogador.animacoes.SetInteger ("estado", 5);
			GetComponent<Rigidbody2D> ().gravityScale = 0f;
			GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
			GetComponent<Collider2D> ().enabled = false;
		}

        if(jogador.estados == 5)
        {
            if (tMorte < 2f)
            {
                tMorte += Time.deltaTime;
            }else if (!gameOverTela.activeInHierarchy)
            {
                gameOverTela.SetActive(true);
            }
        }

		hpBarra.value = hpAtual;
		hpValor.text =""+ (int)hpAtual;

		vitalidadeAtual = ((hpAtual / hp) + (vigorAtual / vigor)) * 0.5f*(1f-vitalidade)+vitalidade;
		if (idioma.idiomaEscolhido == "Portugueis") {
			vitalidadeValor.text = "Vitalidade: " + (int)(vitalidadeAtual * 100f) + "%";
		}else if (idioma.idiomaEscolhido == "Ingles"){
			vitalidadeValor.text = "Vitality: " + (int)(vitalidadeAtual * 100f) + "%";
		}	

        if (regeB!= 0f)
        {
            if (regeB > 0f)
            {
               bRege.sprite = fases.faseX.bufRege;
            }
            else
            {
                bRege.sprite = fases.faseX.dBufRege;
            }
            if (!bRege.gameObject.activeInHierarchy)
            {
                bRege.gameObject.SetActive(true);
            }

        }
        else if (bRege.gameObject.activeInHierarchy)
        {
            bRege.gameObject.SetActive(false);
        }

        if (speedX != 1f)
        {
            jogador.speed = speed0 * speedX;
            if (speedX > 1f)
            {
                bSpeed.sprite = fases.faseX.bufSpeed;
            }
            else
            {
                bSpeed.sprite = fases.faseX.dBufSpeed;
            }
            bSpeed.gameObject.SetActive(true);
        }
        else if(jogador.speed != speed0)
        {
            jogador.speed = speed0;
            bSpeed.gameObject.SetActive(false);
        }

        if (buffPoder[0] / debuffPoder[0] != 1f)
        {
            danoBase = dano0 * buffPoder[0] / debuffPoder[0];
            defesa = defesa0 * buffPoder[0] / debuffPoder[0];
            if (buffPoder[0] / debuffPoder[0] > 1f)
            {
                bForca.sprite = fases.faseX.bufForca;
            }
            else
            {
                bForca.sprite = fases.faseX.dBufForca;
            }
            bForca.gameObject.SetActive(true);
        }
        else if(dano0 != danoBase || defesa != defesa0)
        {
            danoBase = dano0;
            defesa = defesa0;
            bForca.gameObject.SetActive(false);
        }
        if(buffPoder[1] / debuffPoder[1] != 1f)
        {
            resistencia = buffPoder[1] / debuffPoder[1];
            if (buffPoder[1] / debuffPoder[1] > 1f)
            {
                bDef.sprite = fases.faseX.bufDef;
            }
            else
            {
                bDef.sprite = fases.faseX.dBufDef;
            }
            bDef.gameObject.SetActive(true);
        }
        else if(resistencia != 1f)
        {
            resistencia = 1f;
            bDef.gameObject.SetActive(false);
        }
		
        if(buffPoder[2] / debuffPoder[2] != 1f)
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
        }else if(agilidade != agilidade0)
        {
            agilidade = agilidade0;
            bAgil.gameObject.SetActive(false);
        }
    }

	//Curas por tempo
	public void AddRecupera(buffsRecupera buff){
		buffsRegen.Add (buff);
	}

	private float BuffsCura(){
		float cura = 0f;
		for (int i = 0; i < buffsRegen.Count; i++) {
            if (!buffsRegen[i].verificado)
            {
                if (buffsRegen[i].durou <= buffsRegen[i].duracao)
                {
                    buffsRegen[i].durou += Time.deltaTime;
                    if (buffsRegen[i].taxaRecuperacao > 0)
                    {
                        cura += buffsRegen[i].taxaRecuperacao;
                    }
                    else
                    {
                        cura += buffsRegen[i].taxaRecuperacao;
                    }
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
        bool achou = false;
        for (int i = 0; i < buffsRegen.Count; i++)
        {
            if (buffsRegen[i].verificado && buffsRegen[i].nome == b.nome)
            {
                buffsRegen[i].verificado = false;
                buffsRegen[i].taxaRecuperacao = b.taxaRecuperacao;
                achou = true;
            }
        }
        if (!achou)
        {
            GameObject aux = Instantiate(b.gameObject);
            buffsRegen.Add(aux.GetComponent<buffsRecupera>());
        }
    }

    //Buffs
    private void VerificarBuffs(){
		for (int i = 0; i < meusBuff.Length; i++) {
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
		for (int i = 0; i < meusSpeedBuffs.Length; i++) {
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

	public bool PodeBuffaSpeed(buffsSpeed qual){
		bool pode = false;
		for (int i = 0; i < meusSpeedBuffs.Length; i++) {
			if (!meusSpeedBuffs [i] || meusSpeedBuffs[i].nome == qual.nome || meusSpeedBuffs[i].durou>= meusSpeedBuffs[i].duracao) {
				pode = true;
				i = meusBuff.Length;
			}
		}

		return pode;
	}

	private int OndeBuffaSpeed(buffsSpeed qual){
		int onde = -1;
		for (int i = 0; i < meusSpeedBuffs.Length; i++) {
			if (!meusSpeedBuffs [i] || meusSpeedBuffs[i].durou>= meusSpeedBuffs[i].duracao) {
				onde = i;
			}
			if (meusSpeedBuffs[i] && meusSpeedBuffs [i].nome == qual.nome) {
				onde = i;
				i = meusBuff.Length;
			}
		}
		return onde;
	}

	public void SpeedBuff(buffsSpeed buff){
		if (PodeBuffaSpeed (buff)) {
			int onde = OndeBuffaSpeed (buff);
			if (meusSpeedBuffs [onde]) {
				if (meusSpeedBuffs [onde].nome == buff.nome) {
					meusSpeedBuffs [onde].durou = 0f;
				} else {
					GameObject aux = Instantiate (buff.gameObject) as GameObject;
					buff = aux.GetComponent<buffsSpeed>();
					buff.durou = 0f;
					buff.verificado = false;
					Destroy (meusSpeedBuffs [onde]);
					meusSpeedBuffs[onde] = buff;
				}
			} else {
				GameObject aux = Instantiate (buff.gameObject) as GameObject;
				buff = aux.GetComponent<buffsSpeed> ();
				buff.durou = 0f;
				buff.verificado = false;
				meusSpeedBuffs [onde] = buff;
			}
		}
	}

	//buffs
	public void Buffa(buffs buff){
		if (PodeBuffa (buff)) {
			int onde = OndeBuffa (buff);
			if (meusBuff [onde]) {
				if (meusBuff [onde].nome == buff.nome) {
					meusBuff [onde].durou = 0f;
				} else {
					GameObject aux = Instantiate (buff.gameObject) as GameObject;
					buff = aux.GetComponent<buffs> ();
					buff.durou = 0f;
					buff.verificado = false;
					Destroy (meusBuff [onde]);
					meusBuff [onde] = buff;
				}
			} else {
				GameObject aux = Instantiate (buff.gameObject) as GameObject;
				buff = aux.GetComponent<buffs> ();
				buff.durou = 0f;
				buff.verificado = false;
				meusBuff [onde] = buff;
			}
		}
	}

	private int OndeBuffa(buffs qual){
		int onde = -1;
		for (int i = 0; i < meusBuff.Length; i++) {
			if (!meusBuff [i] || meusBuff[i].durou>= meusBuff[i].duracao) {
				onde = i;
			}
			if (meusBuff[i] && meusBuff [i].nome == qual.nome) {
				onde = i;
				i = meusBuff.Length;
			}
		}
		return onde;
	}

	public bool PodeBuffa(buffs qual){
		bool pode = false;
		for (int i = 0; i < meusBuff.Length; i++) {
			if (!meusBuff [i] || meusBuff[i].nome == qual.nome || meusBuff[i].durou>= meusBuff[i].duracao) {
				pode = true;
				i = meusBuff.Length;
			}
		}

		return pode;
	}

	public bool PlayerLadoDoAtaque(Transform ataque){
		float posRela = transform.position.x - ataque.position.x;
		if (posRela <= 0 && jogador.viradoDireita) {
			return true;
		} else if (posRela > 0 && !jogador.viradoDireita) {
			return true;
		} else {
			return false;
		}
	}

	public float DanoCalcu(float def,float mult){
		float dano = danoBase/(def+danoBase);
		dano *= danoBase * mult;
        dano *= vitalidadeAtaq;
		return dano;
	}

	public void AplicarDano(float dano,float dor,bool esquivou,Vector2 force,bool critc){
		//Debug.Log (1);
		jogador.esquivou = esquivou;

        if (esquivou)
        {
            dano = 0f;
            if (missC.transform.lossyScale.x < 0f)
            {
                Vector3 scalX = missC.transform.localScale;
                scalX.x *= -1f;
                missC.transform.localScale = scalX;
            }
            missC.SetActive(true);
        } else {
            if (critc)
            {
                dano *= 3f;
                xLevouCritico = 1f;
                cameraSeguir.cam.Tremer(0.05f, 0.5f);
            }
            missC.SetActive(false);
            if (dor > 0f)
            {
                jogador.NaoDeslizar();
                jogador.estados = 4;
            }
            jogador.esteCorpo.AddForce(force, ForceMode2D.Impulse);
        }

		if (hpAtual > dano) {
			hpAtual -= dano/resistencia;
			jogador.dorPor = dor;
			//jogador.ani
		} else {
			hpAtual = 0f;
		}
	}

    public float PoderEsquiva()
    {
        return 1f / (1f - esquiva);
    }

	public bool DefendeuSim(float posX){
		bool denfendeu = false;
		posX -= transform.position.x;
		if (jogador.estados == 2) {
			//float minhaScala = transform.localScale.x;
			if (posX >= 0f && jogador.viradoDireita) {
				denfendeu = true;
			} else if (posX <= 0f && !jogador.viradoDireita) {
				denfendeu = true;
			}
		}
	
		return denfendeu;
	}

    public float PoderHabilidade()
    {
        return danoBase * agilidade * vitalidadeAtual;
    }

	public void Denfender(float danoX,float dorX,bool critc){
        if (critc)
        {
            danoX *= 3f;
            xLevouCritico = 1f;
        }
		float vigorPerda = danoX / PoderHabilidade();
        vigorPerda /= PoderEsquiva();
        if (FoiCritico())
        {
            vigorPerda /= 3f;
        }

		if (vigorAtual > vigorPerda) {
			vigorAtual -= vigorPerda;
		} else {
			vigorAtual = 0f;
			vigorPerda = vigorAtual / vigorPerda;
			AplicarDano (danoX, 0f, false,Vector2.zero,false);
			Stunear (dorX);
		}
		jogador.Defendeu (0.5f/dorX);
	}

	public void Stunear(float quanto){
        jogador.NaoDeslizar();
		jogador.stun += quanto;
	}

    public void CurarPorcento(float curaPorcento)
    {
        hpAtual += hp * curaPorcento;

        if (hpAtual > hp)
        {
            hpAtual = hp;
        }
    }

	public bool FoiCritico(){
		bool ret = false;
		if (critico > Random.value) {
			ret = true;
        }
        return ret;
	}
}

