using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mochilaCalanguito : MonoBehaviour {
    public GameObject[] itensUsa;
    private static GameObject[] itensAtuais;
    private itensUsaveis[] itens;
    public static mochilaCalanguito mochila;
    public bool needRebolar = false;
    public Image itensImage;
    public Text quantidadeText;
    private static int idItem;
    private float size;
    // Use this for initialization
    void Awake()
    {
        mochila = this;
        size = itensImage.rectTransform.sizeDelta.x;
        itensAtuais = new GameObject[itensUsa.Length];
        itens = new itensUsaveis[itensUsa.Length];
        for (int i = 0; i < itensAtuais.Length; i++)
        {
            itensAtuais[i] = Instantiate(itensUsa[i]);
            itensAtuais[i].SetActive(false);
            itens[i] = itensAtuais[i].GetComponent<itensUsaveis>();
        }
        CarregarItens();
        idItem = itens.Length;
        AlterarItens(1);
    }

    public itensUsaveis[] GetItensUsaveis()
    {
        return itens;
    }

    public void AtulizarQuantidade(int quantidade)
    {
        quantidadeText.text = "x" + quantidade;
    }

    public void AlterarItens(int dire)
    {
        int idX = idItem;

        bool aItem = false;
        for (int i = 0; i < itens.Length; i++)
        {
            idX +=dire;

            if (idX > itens.Length)
            {
                idX = 1;
            }
            else if (idX < 1)
            {
                idX = itens.Length;
            }
            if (itens[idX-1].Tem())
            {
                idItem = idX;
                i = itens.Length;
                aItem = true;
            }
        }

       if(!aItem && idItem > 0)
        {
            if (!itens[idItem - 1].Tem())
            {
                idItem = 0;
            }
        }
        AtualizarImagens();
    }


    public void PrecisaRebolar()
    {
        if (idItem > 0)
        {
            if (itens[idItem - 1].tipo == "Direto")
            {
                needRebolar = false;
            }
            else
            {
                needRebolar = true;
            }
        }
    }

    private void AtualizarImagens()
    {
        if(idItem > 0)
        {
            if (!itensImage.gameObject.activeInHierarchy)
            {
                itensImage.gameObject.SetActive(true);
                quantidadeText.gameObject.SetActive(true);
            }
            Sprite sp = itens[idItem-1].EsteSprite();
            itensImage.sprite = sp;
            float maior = sp.bounds.size.y;
            itensImage.SetNativeSize();
            if (sp.bounds.size.x > sp.bounds.size.y)
            {
                maior = sp.bounds.size.x;
            }
            float ajust = size/(100f*maior);
            itensImage.rectTransform.sizeDelta = itensImage.rectTransform.sizeDelta * ajust;
            quantidadeText.text = "x" + itens[idItem - 1].Quantidade();
        }
        else if(itensImage.gameObject.activeInHierarchy)
        {
            itensImage.gameObject.SetActive(false);
            quantidadeText.gameObject.SetActive(false);
        }

        PrecisaRebolar();
    }

    public bool UsarContinue()
    {
        bool ret = false;

        if (dataSave.jogoAtual.continues > 0)
        {
            dataSave.jogoAtual.continues -= 1;
            ret = true;
        }

        return ret;
    }

    public static int NumeroContinues()
    {
        return dataSave.jogoAtual.continues;
    }

    public bool AddContinue()
    {
        bool ret = false;
        if (dataSave.jogoAtual.continues <= 9)
        {
            ret = true;
            dataSave.jogoAtual.continues += 1;
        }

        return ret;
    }

    public bool AddCapacidade()
    {
        bool ret = false;

        if (dataSave.jogoAtual.capacidadeMochila < 12f)
        {
            ret = true;
            dataSave.jogoAtual.capacidadeMochila += 1f;
        }

        return ret;
    }

    public float PesoTotal()
    {
        float peso = 0;
        for(int i = 0; i < itens.Length; i++)
        {
            peso += itens[i].PesoTotal();
        }
        return peso;
    }

    public bool PodeUsarItem(SpriteRenderer render)
    {
        bool ret = false;
        if (idItem > 0)
        {
            ret = itens[idItem - 1].Tem();
            if (ret)
            {
                render.sprite = itens[idItem - 1].EsteSprite();
                render.enabled = true;
            }
        }
        return ret;
    }

    public itensUsaveis UsarItem()
    {
        return itens[idItem-1];
    }

    public bool ColocarUmItem(itensUsaveis item)
    {
        bool ret = true;
        if (PesoTotal() + item.pesoUnidade > dataSave.jogoAtual.capacidadeMochila+0.01f)
        {
            ret = false;
        }
        int iX = 0;
        if (ret)
        {
            for (int i = 0; i < itens.Length; i++)
            {
                if (itens[i].nome == item.nome)
                {
                    if (PesoTotal() == 0f)
                    {
                        itens[i].Add();
                        idItem = i + 1;
                        AtualizarImagens();
                    }else
                    {
                        itens[i].Add();
                    }
                    iX = i;
                    i = itens.Length;
                }
            }
        }

        if (idItem - 1 == iX)
        {
            AtulizarQuantidade(itens[iX].Quantidade());
        }

        return ret;
    }

    private void CarregarItens()
    {
        for(int i= 0; i < itens.Length; i++)
        {
            itens[i].SetQuantidade(dataSave.jogoAtual.itensQuantidade[i]);
        }
    }
}
