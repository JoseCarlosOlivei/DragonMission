using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class litper : MonoBehaviour {
    private atributosInimigos atributos;
    public AudioClip jogandoMartelo, carregando, tacadaNoChaoSom, erroMartelada;
    public Transform saida;
    private moveCPU movimentos;
    private float kAcoes = 0f;
    private float[] poderesC = new float[4];
    private float tempoX,tempoAcoes;
    private int acao = 0;
    public GameObject marteloArremesso, marteloQuica, eathWave;
    private Renderer rederizado;
    public AudioSource exitSom;
    private bool deixaSom = false;
    private List<GameObject> arremessos = new List<GameObject>(), quica = new List<GameObject>(),ult = new List<GameObject>();
    // Use this for initialization
    void Awake () {
        rederizado = GetComponent<Renderer>();
        atributos = GetComponent<atributosInimigos>();
        movimentos = GetComponent<moveCPU>();

        poderesC[0] = 20f;//Ataque Base 1
        poderesC[1] = 5f;//Arremesso Martelo 2
        poderesC[2] = 10f;//Jogar martelos que quicam 3
        poderesC[3] = 2.5f;//eath wave ult 4
        for(int i = 0; i < poderesC.Length; i++)
        {
            kAcoes += poderesC[i];
        }

        tempoX = atributos.TempoReacaoCalc();
        GameObject aux;
        for(int i = 0; i < 5; i++)
        {
            aux = Instantiate(marteloArremesso, Vector2.zero, Quaternion.identity) as GameObject;
            aux.SetActive(false);
            arremessos.Add(aux);
            aux = Instantiate(marteloQuica, Vector2.zero, Quaternion.identity) as GameObject;
            aux.SetActive(false);
            quica.Add(aux);
            aux = Instantiate(eathWave, Vector2.zero, Quaternion.identity) as GameObject;
            aux.SetActive(false);
            ult.Add(aux);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (fases.podeMover)
        {
            if (movimentos.viu)
            {
                if (atributos.estados == 0)
                {
                    // ações
                    if (rederizado.isVisible)
                    {
                        if (acao == 0)
                        {
                            movimentos.AndarAletoriamente();
                        }
                        else if (acao == 1)
                        {
                            // Atacar
                            if (0.25f >= movimentos.DistaciaX(transform.position.x, atributosHeroi.heroiX.transform.position.x) && movimentos.SemObstaculosEntrePlay())
                            {
                                if (!movimentos.MesmaAltura())
                                {
                                    movimentos.Pular();
                                }

                                if (movimentos.DistaciaX(transform.position.y, atributosHeroi.heroiX.transform.position.y) < 0.25f)
                                {
                                    Atacar();
                                    tempoAcoes = 0f;
                                }
                            }
                            else
                            {
                                movimentos.SeAproximar(0.25f, false);
                            }
                        }
                        else if (acao == 2)
                        {
                            // Aremesso Martelo
                            if (moveCPU.limiteTela.x >= movimentos.DistaciaX(transform.position.x, atributosHeroi.heroiX.transform.position.x) && movimentos.SemObstaculosEntrePlay())
                            {
                                if (movimentos.noChao)
                                {
                                    movimentos.Pular();
                                }
                                else
                                {
                                    ArremessoMartelo();
                                    tempoAcoes = 0f;
                                }
                            }else
                            {
                                movimentos.SeAproximar(moveCPU.limiteTela.x, false);
                            }
                        }else if(acao == 3)
                        {
                            //Martelo que quica
                            if (1.5f >= movimentos.DistaciaX(transform.position.x, atributosHeroi.heroiX.transform.position.x) && movimentos.DistaciaX(transform.position.y,atributosHeroi.heroiX.transform.position.y)< 0.5f && movimentos.SemObstaculosEntrePlay())
                            {
                                //Debug.Log (1);
                                MarteloQuica();
                                tempoAcoes = 0f;
                            }
                            else
                            {
                                if (movimentos.MesmaAltura())
                                {
                                    movimentos.SeAproximar(1.5f, false);
                                }
                                else
                                {
                                    movimentos.SeAproximar(movimentos.xColl, false);
                                }
                            }
                        }else if(acao == 4)
                        {
                            if (moveCPU.limiteTela.x >= movimentos.DistaciaX(transform.position.x, atributosHeroi.heroiX.transform.position.x) && movimentos.MesmaAltura() && movimentos.SemObstaculosEntrePlay())
                            {
                                Ult();
                                tempoAcoes = 0f;
                            }
                            else
                            {
                                if (movimentos.MesmaAltura())
                                {
                                    movimentos.SeAproximar(moveCPU.limiteTela.x, false);
                                }else
                                {
                                    movimentos.SeAproximar(movimentos.xColl, false);
                                }
                            }
                        }
                        // trocar ação
                        if (tempoX < tempoAcoes)
                        {
                            tempoX = atributos.TempoReacaoCalc();
                            tempoAcoes = 0f;
                            SuaVez();
                        }
                        else
                        {
                            tempoAcoes += Time.deltaTime;
                        }
                    }else
                    {
                        movimentos.Move(false, true,true);
                        movimentos.VirarParaPlayer();
                    }
                }
            }
        }

        if(atributos.estados==4 || atributos.estados == 5 || atributos.estados == 0)
        {
            PararSom();
        }
    }

    public void Atacar()
    {
        if (atributos.estados == 0)
        {
            movimentos.NaoDeslizar();
            movimentos.VirarParaPlayer();
            atributos.estados = 1;
            movimentos.anima.SetInteger("estados", 1);
            movimentos.anima.SetFloat("qualPoder", 0f);
            movimentos.anima.speed = atributos.agilidade;
            movimentos.Parar();
            SuaVez();
            exitSom.PlayOneShot(erroMartelada);
        }
    }

    public void ArremessoMartelo()
    {
        if (atributos.estados == 0)
        {
            movimentos.NaoDeslizar();
            movimentos.VirarParaPlayer();
            atributos.estados = 6;
            movimentos.anima.SetInteger("estados", 6);
            movimentos.anima.SetFloat("qualPoder", 1f);
            movimentos.anima.speed = 1f;
            movimentos.Parar();

            SuaVez();
        }
    }

    public void InvocarMaterloArremesso()
    {
        GameObject obj = ListaQuemDesativo(arremessos);
        obj.GetComponent<recocheteaProgetil>().Respaw(movimentos.viradoDireita, saida.position, atributos.HabilidaPoder(), 0.2f, atributos.FoiCritico(),0.5f);

        PararSom();
        if (exitSom.enabled)
        {
            exitSom.PlayOneShot(jogandoMartelo);
        }
    }

    public void MarteloQuica()
    {
        if (atributos.estados == 0 && movimentos.noChao)
        {
            movimentos.VirarParaPlayer();
            atributos.estados = 6;
            movimentos.anima.SetInteger("estados", 6);
            movimentos.anima.SetFloat("qualPoder", 2f);
            movimentos.anima.speed = 1f;
            movimentos.NaoDeslizar();

            SuaVez();
        }
    }

    public void InvocaMarteloQuica()
    {
        GameObject obj = ListaQuemDesativo(quica);
        obj.GetComponent<bounciProgt>().Respaw(atributos.HabilidaPoder(),1f/64f,0f,atributos.FoiCritico(),movimentos.viradoDireita,saida.position,1.35f);

        PararSom();
        if (exitSom.enabled)
        {
            exitSom.PlayOneShot(jogandoMartelo);
        }
    }

    public void Ult()
    {
        if (atributos.estados == 0 && movimentos.noChao)
        {
            movimentos.VirarParaPlayer();
            atributos.estados = 6;
            movimentos.anima.SetInteger("estados", 6);
            movimentos.anima.SetFloat("qualPoder", 3f);
            movimentos.anima.speed = 1f;
            movimentos.NaoDeslizar();

            exitSom.loop = true;
            exitSom.clip = carregando;
            if (exitSom.enabled)
            {
                exitSom.Play();
            }
            SuaVez();
        }
    }

    public void PararSom()
    {
        if (exitSom.isPlaying && !deixaSom)
        {
            exitSom.Stop();
            exitSom.loop = false;
            SuaVez();
        }
    }

    private void PararSoSom()
    {
        exitSom.Stop();
        exitSom.loop = false;
    }

    public void InvocaUlt()
    {
        GameObject obj = ListaQuemDesativo(ult);
        obj.GetComponent<noChaoPoder>().Respaw(saida.position,movimentos.viradoDireita,atributos.FoiCritico(),atributos.HabilidaPoder(),(0.4f+0.5f/64f),1f,transform.parent);
        BaterNoChaoSom(false);
    }

    public void BaterNoChaoSom()
    {
        PararSom();
        if (exitSom.enabled)
        {
            exitSom.PlayOneShot(tacadaNoChaoSom);
        }

        deixaSom = true;
    }

    public void BaterNoChaoSom(bool continuarSom)
    {
        PararSom();
        if (exitSom.enabled)
        {
            exitSom.PlayOneShot(tacadaNoChaoSom);
        }

        deixaSom = continuarSom;
    }


    public void ResetarEstado()
    {
        atributos.estados = 0;
        movimentos.anima.SetFloat("qualPoder", 0f);
        movimentos.anima.speed = 1f;
        movimentos.anima.SetInteger("estados", 0);
    }

    private void SuaVez()
    {
        if (deixaSom)
        {
            deixaSom = false;
        }
        if (atributos.ChancesDeReagir(movimentos.encurralado) > Random.value)
        {
            acao = Reacoes();
        }
        else
        {
            acao = 0;
        }
    }

    private int Reacoes()
    {
        int resp = 0;
        float x = Random.Range(0, kAcoes);
        float auxCalc = 0f;
       
        for(int i = 0; i < poderesC.Length; i++)
        {
            if (x < poderesC[i] + auxCalc)
            {
                resp = i+1;
                i = poderesC.Length;
            }else
            {
                auxCalc += poderesC[i];
            }
        }

        return resp;
    }

    private GameObject ListaQuemDesativo(List<GameObject> lista)
    {
        GameObject ret = null;
        for (int i = 0; i < lista.Count; i++)
        {
            if (!lista[i].activeInHierarchy)
            {
                ret = lista[i];
                i = lista.Count;
            }
        }

        if (ret == null)
        {
            ret = Instantiate(lista[0]);
            lista.Add(ret);
        }

        return ret;
    }
}
