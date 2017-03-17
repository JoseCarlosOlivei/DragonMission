using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class upamento : MonoBehaviour {
	private bool podeUpar = false;
	//public static int nvHp=0, nvEsquiva=0, nvDef=0, nvRege=0, nvRecu=0, nvAtaq= 0, nvVita = 0, nvCrit = 0 , nvAgili = 0,nvVigo;
	public Slider hpSld, esqSld, defSld, regSld, recSld, atqSld, vitSld, crtSld, agiSld,vgrSld;
	public Text hpTex, esqTex, defTex, regTex, recTex, atqTex, vitTex, crtTex, agiTex,vgrTex;
	public Text  esqTex1, defTex1, regTex1, recTex1, atqTex1, vitTex1, crtTex1, agiTex1,vgrTex1;
    public Text sair;
	public Text descricao;
	private string desHp,desEsq,desDef,desReg,desRec,desAtq,desVit,desCrt,desAgi,desVgr,desComeco;
	public Text uparText, AuxText;
	private string idioma;
    private int pontoPrecisa;
    private AudioSource saidaSom;
	// Use this for initialization
	void Awake () {
		//Debug.Log (nvX);
		VerificaUp ();
		AtulizarAtriubtos ();

		idioma = PlayerPrefs.GetString ("Idioma");
		if (idioma == "Portugueis") {
            sair.text = "Sair";
			esqTex1.text = "Esquiva"; 
			defTex1.text = "Defesa";
			regTex1.text = "Regeneração";
			recTex1.text = "Recuperação";
			atqTex1.text = "Dano";
			vitTex1.text = "Vitalidade";
			crtTex1.text = "Crítico";
			agiTex1.text = "Agilidade";
			vgrTex1.text = "Vigor";
			desComeco = "Clique no nome dos atributos para mais infromações sobre eles.";
			desHp = "HP: Quanto maior o HP mais dano você tera de receber para morrer.";
			desEsq = "Esquiva: É a chance que você tem de esquivar automaticamente de um golpe.";
			desDef = "Defesa: Reduz o dano recebido.";
			desReg = "Regeneração: Porcentagem de HP regecuperado por segundo.";
			desRec = "Recuperação: Valor de vigor recuperado por segundo fora de luta.";
			desAtq = "Dano: Quantidade de dano que você aplica nos inimigos com seu ataque base, também aumenta a força de seus poderes.";
			desVit = "Vitalidade: Ela multiplica-se ao dano e diminui dependedo de seu HP e vigor perdidos, quando você a aumenta ela se mantem mais proximo valores proximos de 100%.";
			desCrt = "Crítico: A chaces de dano crítico, dano crítico da 3 vezes mais dano.";
			desAgi = "Agilidade: Golpes dados em 1 segundo, quanto maios esse atributo menos vigor você gasta para bater e também aumenta a força de seus poderes.";
			desVgr = "Vigor: É utilizado para usar vários movimentos e golpes, como: atacar,poderes e pular.";
			descricao.text = desComeco;
		} else if (idioma == "Ingles") {
            sair.text = "Exit";
			esqTex1.text = "Dodge"; 
			defTex1.text = "Defense";
			regTex1.text = "Regeneration";
			recTex1.text = "Recovery";
			atqTex1.text = "Damage";
			vitTex1.text = "Vitality";
			crtTex1.text = "Critical";
			agiTex1.text = "Agility";
			vgrTex1.text = "Stamina";
			desComeco = "Press the name of the attributes for more informations about then.";
			desHp = "HP: How much bigger the HP more damage you need to receive to die.";
            desEsq = "Dodge: It's the chance you have to dodge automatically from a blow.";
            desDef = "Defense: Reduces the damage received.";
			desReg = "Regeneration: Percentagen of HP per second.";
			desRec = "Recovery: Stamina value recovered per second out of fight.";
			desAtq = "Damage: Amount of damage that you apply to enemies with your base attack, also increases the strength of your powers.";
			desVit = "Vitality: It multiplies itself to damage and decreases depending on your HP and stamina lost, when you increase it, it stays closer to 100% values.";
			desCrt = "Critical: The chances of critical damage, critical damage from 3 times more damage.";
			desAgi = "Agility: Dice hits in 1 second, how much lesser attribute this attribute you spend to hit and also increases the strength of your powers.";
			desVgr = "Stamina: It is used to use various moves and scams, such as: attacking, powers and jumping.";
			descricao.text = desComeco;
		}

        saidaSom = GetComponent<AudioSource>();
		VerificaUp ();
	}
	
    public void Sair()
    {
        SceneManager.LoadSceneAsync("mapaMunde");
    }

	public void DescHP(){
		descricao.text = desHp;
        auxiliares.BotaoSom(saidaSom);
	}

	public void DescEsq(){
		descricao.text = desEsq;
        auxiliares.BotaoSom(saidaSom);
    }

    public void DescDef(){
		descricao.text = desDef;
        auxiliares.BotaoSom(saidaSom);
    }

    public void DescRege(){
		descricao.text = desReg;
        auxiliares.BotaoSom(saidaSom);
    }

    public void DescRecu(){
		descricao.text  = desRec;
        auxiliares.BotaoSom(saidaSom);
    }

    public void DescVgr(){
		descricao.text = desVgr;
        auxiliares.BotaoSom(saidaSom);
    }

    public void DescDn(){
		descricao.text = desAtq;
        auxiliares.BotaoSom(saidaSom);
    }

    public void DescAgi(){
		descricao.text  = desAgi;
        auxiliares.BotaoSom(saidaSom);
    }

    public void DescVit(){
		descricao.text  = desVit;
        auxiliares.BotaoSom(saidaSom);
    }

    public void DescCrt(){
		descricao.text  = desCrt;
	}

	private void VerificaUp(){
        pontoPrecisa = (dataSave.jogoAtual.NvHero() + 1) * 100;
		if (dataSave.jogoAtual.escores >= pontoPrecisa) {
			podeUpar = true;
		} else {
			podeUpar = false;
		}
		if (idioma == "Portugueis") {
            if (dataSave.jogoAtual.NvHero() < 100)
            {
                uparText.text = "Você tem "+ dataSave.jogoAtual.escores +" escores ,você precisa de " + pontoPrecisa + " escores para aumentar seu poder.";
            }else
            {
                uparText.text = "Você está no nivel máximo.";
            }
        } else if (idioma == "Ingles") {
            if (dataSave.jogoAtual.NvHero() < 100)
            {
                uparText.text = "You have " + dataSave.jogoAtual.escores + ",you need " + pontoPrecisa + " scores to strengthen yourself.";
            }
            else
            {
                uparText.text = "You're at the highest level.";
            }
        }
	}

    public static int ScoresTotal()
    {
        return dataSave.jogoAtual.escores;
    }

    public static void AddScores(int add)
    {
        if (dataSave.jogoAtual.escores < 1000000)
        {
            dataSave.jogoAtual.escores += add;
        }
    }

    private void Upou()
    {
        dataSave.jogoAtual.escores -= pontoPrecisa;
        auxiliares.CompletoSom(saidaSom);
        VerificaUp();
    }

	public void HpUp(){
		if (podeUpar && dataSave.jogoAtual.hpNv<10) {
			dataSave.jogoAtual.hpNv += 1;
			hpTex.text = ""+(160 + 16 * dataSave.jogoAtual.hpNv);
			hpSld.value = dataSave.jogoAtual.hpNv;
            Upou();
        }
    }

	public void EsqUp(){
		if (podeUpar && dataSave.jogoAtual.esqNv<10) {
			dataSave.jogoAtual.esqNv += 1;
			esqTex.text = ""+(5 * dataSave.jogoAtual.esqNv)+"%";
			esqSld.value = dataSave.jogoAtual.esqNv;
            Upou();
		}
	}

	public void DefUp(){
		if (podeUpar && dataSave.jogoAtual.defNv<10) {
			dataSave.jogoAtual.defNv += 1;
			defTex.text = ""+(16 * dataSave.jogoAtual.defNv);
			defSld.value = dataSave.jogoAtual.defNv;
            Upou();
		}
	}

	public void RegeUp(){
		if (podeUpar && dataSave.jogoAtual.regeNv<10) {
			dataSave.jogoAtual.regeNv += 1;
			regTex.text = ""+(Mathf.Round(dataSave.jogoAtual.regeNv*100000f/320f)/1000f)+"%";
			regSld.value = dataSave.jogoAtual.regeNv;
            Upou();
		}
	}


	public void RecuUp(){
		if (podeUpar && dataSave.jogoAtual.recuNv<10) {
			dataSave.jogoAtual.recuNv+= 1;
			recTex.text = ""+(Mathf.Round(10000f/16f+dataSave.jogoAtual.recuNv*10000f/64f)/100f);
			recSld.value = dataSave.jogoAtual.recuNv;
            Upou();
		}
	}


	public void VitaUp(){
		if (podeUpar && dataSave.jogoAtual.vitaliNv<10) {
			dataSave.jogoAtual.vitaliNv += 1;
			vitTex.text = ""+(dataSave.jogoAtual.vitaliNv*10)+"%";
			vitSld.value = dataSave.jogoAtual.vitaliNv;
            Upou();
		}
	}


	public void DanoUp(){
		if (podeUpar && dataSave.jogoAtual.danoNv<10) {
			dataSave.jogoAtual.danoNv += 1;
			atqTex.text = ""+(160f+dataSave.jogoAtual.danoNv*16f);
			atqSld.value = dataSave.jogoAtual.danoNv;
            Upou();
		}
	}


	public void CriticoUp(){
		if (podeUpar && dataSave.jogoAtual.critNv<10) {
			dataSave.jogoAtual.critNv += 1;
			crtTex.text = ""+dataSave.jogoAtual.critNv*5+"%";
			crtSld.value = dataSave.jogoAtual.critNv;
            Upou();
		}
	}


	public void AgilidadeUp(){
		if (podeUpar && dataSave.jogoAtual.agilNv<10) {
			dataSave.jogoAtual.agilNv += 1;
			agiTex.text = ""+(2f+0.2f*dataSave.jogoAtual.agilNv);
			agiSld.value = dataSave.jogoAtual.agilNv;
            Upou();
		}
	}


	public void VigorUp(){
		if (podeUpar && dataSave.jogoAtual.vigorNv<10) {
			dataSave.jogoAtual.vigorNv += 1;
			vgrTex.text = ""+(int)(100f*(1f/3f+2f*dataSave.jogoAtual.vigorNv/30f));
			vgrSld.value = dataSave.jogoAtual.vigorNv;
            Upou();
		}
	}


	private void AtulizarAtriubtos(){
		hpTex.text = ""+(160 + 16 * dataSave.jogoAtual.hpNv);
		hpSld.value = dataSave.jogoAtual.hpNv;
		esqTex.text = ""+(5 * dataSave.jogoAtual.esqNv)+"%";
		esqSld.value = dataSave.jogoAtual.esqNv;
		defTex.text = ""+(16 * dataSave.jogoAtual.defNv);
		defSld.value = dataSave.jogoAtual.defNv;
		regTex.text = ""+(Mathf.Round(dataSave.jogoAtual.regeNv*100000f/320f)/1000f)+"%";
		regSld.value = dataSave.jogoAtual.regeNv;
		recTex.text = ""+(Mathf.Round(10000f/16f+dataSave.jogoAtual.recuNv*10000f/64f)/100f);
		recSld.value = dataSave.jogoAtual.recuNv;
		vitTex.text = ""+(dataSave.jogoAtual.vitaliNv*10)+"%";
		vitSld.value = dataSave.jogoAtual.vitaliNv;
		atqTex.text = ""+(160f+dataSave.jogoAtual.danoNv*16f);
		atqSld.value = dataSave.jogoAtual.danoNv;
		crtTex.text = ""+dataSave.jogoAtual.critNv*5+"%";
		crtSld.value = dataSave.jogoAtual.critNv;
		agiTex.text = ""+(2f+0.2f*dataSave.jogoAtual.agilNv);
		agiSld.value = dataSave.jogoAtual.agilNv;
		vgrTex.text = ""+(int)(100f*(1f/3f+2f*dataSave.jogoAtual.vigorNv/30f));
		vgrSld.value = dataSave.jogoAtual.vigorNv;
	}
}
