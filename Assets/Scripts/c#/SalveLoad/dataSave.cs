using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class dataSave {

    public static dataSave jogoAtual;
    public int danoNv, defNv, agilNv, vigorNv, vitaliNv, hpNv, esqNv, regeNv, recuNv, critNv;
    public int escores,continues;
    public float capacidadeMochila;
    public bool[] poderesDestravados = new bool[8];
    private List<string> mapasDestrava;
    private List<string> mapasPrincipaisPassados;
    private List<string> mapasExtrasPassados;
    public float dificuldade;
    public bool finalizou;
    public int[] itensQuantidade = new int[23];
    public int poderId;
    public int cena, subCena;
    public bool voltarCkeckPoint;
    public infInimigos[] inimigosInformacoes;
    private string cenaCarregar;
    public float posHeroX, posHeroY, hpHero;
    public bool[] coisasNaoPegas;
    public float tempoFase;
    private int gastoNaLoja, inimigosDerrotados, bausDoTesouroPego, jaulasAbertas;
    private static float volumeMaster = PlayerPrefs.GetFloat("somVolume");
    public bool vendorPrimeiro, vendorDica;
    private dataSave anteriorSave;

    public dataSave()
    {
        danoNv = 0;
        defNv = 0;
        agilNv = 0;
        vigorNv = 0;
        vitaliNv = 0;
        hpNv = 0;
        esqNv = 0;
        regeNv = 0;
        recuNv = 0;
        critNv = 0;
        escores = 0;
        poderId = 0;

        voltarCkeckPoint = false;
        finalizou = false;
        dificuldade = 1f;
        continues = 3;
        capacidadeMochila = 1f;

        cena = 0;
        subCena = 0;

        for(int i = 0; i < itensQuantidade.Length; i++)
        {
            itensQuantidade[i] = 0;
        }
        for(int i = 0; i < poderesDestravados.Length; i++)
        {
            poderesDestravados[i] = false;
        }

        mapasDestrava = new List<string>();
        mapasPrincipaisPassados = new List<string>();
        mapasExtrasPassados = new List<string>();
        gastoNaLoja = 0;
        jaulasAbertas = 0;
        inimigosDerrotados = 0;
        bausDoTesouroPego = 0;

        vendorDica = false;
        vendorPrimeiro = true;

        jogoAtual = this;
    }

    public dataSave(dataSave copia)
    {
        continues = copia.continues;
        escores = copia.escores;

        capacidadeMochila = copia.capacidadeMochila;

        for (int i = 0; i < itensQuantidade.Length; i++)
        {
            itensQuantidade[i] = copia.itensQuantidade[i];
        }

        gastoNaLoja = copia.gastoNaLoja;
        jaulasAbertas = copia.jaulasAbertas;
        inimigosDerrotados = copia.inimigosDerrotados;
        bausDoTesouroPego = copia.bausDoTesouroPego;

        vendorDica = copia.vendorDica;
        vendorPrimeiro = copia.vendorPrimeiro;

        mapasDestrava = copia.mapasDestrava;
        mapasPrincipaisPassados = copia.mapasPrincipaisPassados;
        mapasExtrasPassados = copia.mapasExtrasPassados;
        poderesDestravados = copia.poderesDestravados;

        cena = 0;
        subCena = 0;

        cenaCarregar = copia.cenaCarregar;
    }

    public void FasePrincipalPassada(string faseFinalizada)
    {
        if (!mapasPrincipaisPassados.Exists(x => x == faseFinalizada))
        {
            mapasPrincipaisPassados.Add(faseFinalizada);
        }
    }

    public bool TemEssaFase(string nomeFase)
    {
        return mapasDestrava.Exists(x => x == nomeFase);
    }

    public void AddFaseNova(string fase)
    {
        if(!mapasDestrava.Exists(x=>x==fase))
        {
            mapasDestrava.Add(fase);
        }
    }

    public bool AddContinue(int quantia)
    {
        bool ret = false;
        if (continues+quantia <= 9)
        {
            continues += quantia;
            ret = true;
        }

        return ret;
    }

    public void AddFasesExtrasFinalizadas(string passada)
    {
        if (!mapasExtrasPassados.Exists(x => x == passada))
        {
            mapasExtrasPassados.Add(passada);
        }
    }

    public static void SetVolume(float valor)
    {
        volumeMaster = valor;
        AudioListener.volume = valor;
        PlayerPrefs.SetFloat("somVolume", valor);
    }

    public static float GetVolume()
    {
        return volumeMaster;
    }

    public void ResetarGravacao()
    {
        mapasDestrava.Clear();
        mapasPrincipaisPassados.Clear();
        mapasExtrasPassados.Clear();
        gastoNaLoja = 0;
        jaulasAbertas = 0;
        inimigosDerrotados = 0;
        bausDoTesouroPego = 0;
    }

    public void AddInimigosMortos(int quantia)
    {
        if (inimigosDerrotados < 300)
        {
            if (inimigosDerrotados + quantia > 300)
            {
                inimigosDerrotados = 300;
            }else
            {
                inimigosDerrotados += quantia;
            }
        }
    }

    public void AddJaulasAbertas(int quantia)
    {
        if (jaulasAbertas < 16)
        {
            if (jaulasAbertas + quantia > 16)
            {
                jaulasAbertas = 16;
            }
            else
            {
                jaulasAbertas += quantia;
            }
        }
    }

    public void AddEscoresGasto(int quantia)
    {
        if (gastoNaLoja < 5000)
        {
            if (gastoNaLoja + quantia > 5000)
            {
                gastoNaLoja = 5000;
            }
            else
            {
                gastoNaLoja += quantia;
            }
        }
    }

    public void AddTesourosPegos(int quantia)
    {
        if (bausDoTesouroPego < 8)
        {
            if (bausDoTesouroPego + quantia > 8)
            {
                bausDoTesouroPego = 8;
            }
            else
            {
                bausDoTesouroPego += quantia;
            }
        }
    }

    public string Informacoes()
    {
        string ret = "";

        if(idioma.idiomaEscolhido == "Portugueis")
        {
            ret = "Nv :" + NvHero() + "\n" + "Fases Desbloqueadas: " + NumeroDeFases() + "\n" + "Poderes: " + NumeroDePoderes() + "\n" + "Dificuldade: ";
            if (dificuldade == 1f / 4f)
            {
                ret += "Muito fácil";
            }else if(dificuldade == 1f / 2f)
            {
                ret += "Fácil";
            }else if(dificuldade == 1f)
            {
                ret += "Normal";
            }else if (dificuldade == 2f)
            {
                ret += "Difícil";
            }else if(dificuldade == 4f)
            {
                ret += "Muito difícil";
            }
            else
            {
                ret += "Insano";
            }
        }

        return ret;
    }


    private int NumeroDePoderes()
    {
        int n = 0;

        for (int i = 0; i < poderesDestravados.Length; i++)
        {
            if (poderesDestravados[i])
            {
                n += 1;
            }
        }

        return n;
    }

    private int NumeroDeFases()
    {
        return mapasDestrava.Count;
    }

    private void VoltarPontoX(dataSave dataX,bool resetarContinues)
    {
        if (voltarCkeckPoint)
        {
            continues = 3;
            voltarCkeckPoint = false;
        }
        escores = dataX.escores;
        capacidadeMochila = dataX.capacidadeMochila;

        for (int i = 0; i < itensQuantidade.Length; i++)
        {
            itensQuantidade[i] = dataX.itensQuantidade[i];
        }

        gastoNaLoja = dataX.gastoNaLoja;
        jaulasAbertas = dataX.jaulasAbertas;
        inimigosDerrotados = dataX.inimigosDerrotados;
        bausDoTesouroPego = dataX.bausDoTesouroPego;

        vendorDica = dataX.vendorDica;
        vendorPrimeiro = dataX.vendorPrimeiro;

        poderId = dataX.poderId;
        cena = 0;
        subCena = 0;

        cenaCarregar = dataX.cenaCarregar;
    }

    public void Carregar()
    {
        jogoAtual = this;
        if (!voltarCkeckPoint || continues < 0)
        {
            if (continues >= 0)
            {
                VoltarPontoX(anteriorSave,false);
            }else
            {
                VoltarPontoX(anteriorSave, true);
            }
            saveLoad.Save();
        }

        SceneManager.LoadSceneAsync(cenaCarregar);     
    }

    public void IrMapaMunde()
    {
        cena = 0;
        subCena = 0;
        escores = anteriorSave.escores;
        capacidadeMochila = anteriorSave.capacidadeMochila;

        for (int i = 0; i < itensQuantidade.Length; i++)
        {
            itensQuantidade[i] = anteriorSave.itensQuantidade[i];
        }

        gastoNaLoja = anteriorSave.gastoNaLoja;
        jaulasAbertas = anteriorSave.jaulasAbertas;
        inimigosDerrotados = anteriorSave.inimigosDerrotados;
        bausDoTesouroPego = anteriorSave.bausDoTesouroPego;

        vendorDica = anteriorSave.vendorDica;
        vendorPrimeiro = anteriorSave.vendorPrimeiro;

        voltarCkeckPoint = false;
        SceneManager.LoadSceneAsync("mapaMunde");
    }

    public void Salvar(string cenaX)
    {
        cenaCarregar = cenaX;
        cena = 0;
        subCena = 0;
        anteriorSave = new dataSave(this);

        saveLoad.Save();
    }

    public void Salvar(itensUsaveis[] itens,atributosInimigos[] inimigos,string nomeCenaAtual,GameObject hero,GameObject[] coletaveis,float tempo)
    {
        for (int i = 0; i < itens.Length; i++)
        {
            itensQuantidade[i] = itens[i].Quantidade();
        }

        cenaCarregar = nomeCenaAtual;
        voltarCkeckPoint = true;
        inimigosInformacoes = new infInimigos[inimigos.Length];
        for(int i = 0; i < inimigos.Length; i++)
        {
            inimigosInformacoes[i] = new infInimigos(inimigos[i].transform.position, inimigos[i].hpAtual <= 0);
        }

        coisasNaoPegas = new bool[coletaveis.Length];
        for(int i = 0; i < coletaveis.Length; i++)
        {
            coisasNaoPegas[i] = coletaveis[i].activeInHierarchy;
        }

        atributosHeroi atb = hero.GetComponent<atributosHeroi>();
        hpHero = atb.hpAtual;
        posHeroX = hero.transform.position.x;
        posHeroY = hero.transform.position.y;

        tempoFase = tempo;

        saveLoad.Save();
    }

    public int NvHero()
    {
        return agilNv + critNv + hpNv + esqNv + defNv + danoNv + recuNv + regeNv + vigorNv + vitaliNv;
    }

    public Vector2 PosHero()
    {
        return new Vector2(posHeroX, posHeroY);
    }
}
