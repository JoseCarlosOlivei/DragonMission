using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itensUsaveis : MonoBehaviour {
    public float pesoUnidade;
    private int quantidade = 0;
    public float precoK;
    public string nome;
    public string tipo;
    public GameObject itemBase;
    private List<GameObject> lista = new List<GameObject>();
    public bool daDano = true;
	// Use this for initialization
	void Awake () {
        GameObject objX;
        for(int i = 0; i < 5; i++)
        {
            objX = Instantiate(itemBase);
            objX.SetActive(false);
            lista.Add(objX);
        }
	}

    public bool Tem()
    {
        bool ret = false;
        if (quantidade > 0)
        {
            ret = true;
        }

        return ret;
    }

    public void Usar(Vector2 pos, bool dirt)
    {
        atributosHeroi atb = atributosHeroi.heroiX.GetComponent<atributosHeroi>();
        if (tipo == "Progetil")
        {
            progeteis p = auxiliares.RetornaObj(lista).GetComponent<progeteis>();
            if (daDano)
            {
                p.Respaw(pos, dirt, atb.PoderHabilidade(), 0.25f, pesoUnidade, atb.FoiCritico());
            }
            else
            {
                p.Respaw(pos, dirt, 0f, 0f, pesoUnidade, atb.FoiCritico());
            }
        }else if(tipo == "Direto")
        {
            usarDireto u = auxiliares.RetornaObj(lista).GetComponent<usarDireto>();
            u.Usar();
        }else if (tipo == "Bomerang")
        {
            vaiEVolta p = auxiliares.RetornaObj(lista).GetComponent<vaiEVolta>();

            p.Respaw(dirt, pos, atb.PoderHabilidade(), pesoUnidade, atb.FoiCritico(), 0.25f, atb.GetComponent<Collider2D>(), 0,this);
        }else if(tipo == "Bate e Rebate")
        {
            bateEVolta b = auxiliares.RetornaObj(lista).GetComponent<bateEVolta>();

            b.Respaw(dirt, pos, atb.PoderHabilidade(), pesoUnidade, atb.FoiCritico(), 0.5f);
        }else if(tipo == "Recochetea")
        {
            recocheteaProgetil r = auxiliares.RetornaObj(lista).GetComponent<recocheteaProgetil>();

            r.Respaw(dirt, pos, atb.PoderHabilidade(), pesoUnidade, atb.FoiCritico(), 0.5f);
        }

        quantidade -= 1;

        if(quantidade  == 0)
        {
            mochilaCalanguito.mochila.AlterarItens(1);
        }
        else
        {
            mochilaCalanguito.mochila.AtulizarQuantidade(quantidade);
        }
    }

    public void Add()
    {
        quantidade += 1;
    }

    public void SetQuantidade(int quantia)
    {
        quantidade = quantia;
    }

    public float PesoTotal()
    {
        return pesoUnidade * quantidade;
    }
  
    public Sprite EsteSprite()
    {
        return itemBase.GetComponent<SpriteRenderer>().sprite;
    }

    public int PrecoAtual()
    {
        return (int)(precoK +  precoK* dataSave.jogoAtual.NvHero());
    }

    public int Quantidade()
    {
        return quantidade;
    }
}
