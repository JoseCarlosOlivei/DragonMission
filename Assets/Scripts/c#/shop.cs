using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class shop : MonoBehaviour {
    public AudioClip compraSom;
    private AudioSource somSaidaBotoes;
    public Text descricao, saida,escores,espaco,numeroContinues;
    public static shop shopNow;
    private string escoresComeco;
    private string semSelecionar;
    private string descricaoContinue,descricaoMochila;
    public Text precoContinue,precoMochila;
	// Use this for initialization
	void Awake () {
        shopNow = this;
        somSaidaBotoes = GetComponent<AudioSource>();
        if (idioma.idiomaEscolhido == "Portugueis")
        {
            saida.text = "Sair";
            escoresComeco = "Escores: ";
            semSelecionar = "Clique em um item para compra-lo.";
            descricaoContinue = "Volte do último cristal de retorno que você pegou.";
            descricaoMochila = "Possibilita carregar mais itens em seu inventário.";
        }
        else if (idioma.idiomaEscolhido == "Ingles")
        {
            saida.text = "Exit";
            escoresComeco = "Scores: ";
            semSelecionar = "Click an item to purchase it.";
            descricaoContinue = "Come back from the return crystal.";
            descricaoMochila = "Load more items into your inventory.";
        }
    }

    public string GetSemSelecionar()
    {
        return semSelecionar;
    }

    public void DescriContinue()
    {
        descricao.text = descricaoContinue;
    }

    public void DescriMochila()
    {
        descricao.text = descricaoMochila;
    }

    public void CompraSucedida(bool deu)
    {
        if (deu)
        {
            somSaidaBotoes.PlayOneShot(compraSom);
        }else
        {
            auxiliares.SomErro(somSaidaBotoes);
        }
        AtualizarInf();
    }

    private int ContinuePreco()
    {
        return 100 + 100 * dataSave.jogoAtual.NvHero();
    }

    public void SairDescri()
    {
        descricao.text = semSelecionar;
    }

    private int MochilaPreco()
    {
        return (int) (100f * Mathf.Pow(dataSave.jogoAtual.capacidadeMochila + 1,2)) ;
    }

    public void ComprarContinue()
    {
        if (ContinuePreco()<=dataSave.jogoAtual.escores && dataSave.jogoAtual.AddContinue(1))
        {
            dataSave.jogoAtual.AddEscoresGasto(ContinuePreco());
            dataSave.jogoAtual.escores -= ContinuePreco();
            CompraSucedida(true);
        }
        else
        {
            CompraSucedida(false);
        }
    }

    public void ComprarMochila()
    {
        if (MochilaPreco() <= dataSave.jogoAtual.escores && dataSave.jogoAtual.capacidadeMochila < 10f)
        {
            dataSave.jogoAtual.AddEscoresGasto(MochilaPreco());
            dataSave.jogoAtual.escores -= MochilaPreco();
            dataSave.jogoAtual.capacidadeMochila += 1f;
            CompraSucedida(true);
        }
        else
        {
            CompraSucedida(false);
        }
    }

    private void AtualizarInf()
    {
        espaco.text = ((int)(mochilaCalanguito.mochila.PesoTotal() * 50f)) + "/" + ((int)(dataSave.jogoAtual.capacidadeMochila * 50f));
        escores.text = escoresComeco + (upamento.ScoresTotal());
        numeroContinues.text = "x" + dataSave.jogoAtual.continues;
        if (dataSave.jogoAtual.continues >= 9)
        {
            precoContinue.text = "";
        }
        else
        {
            precoContinue.text = ContinuePreco() + "$";
        }
        if (dataSave.jogoAtual.capacidadeMochila >= 10f)
        {
            precoMochila.text = "";
        }
        else
        {
            precoMochila.text = MochilaPreco() + "$";
        }
    }

    public void AparecerShop()
    {
        fases.faseX.PausePlaySomFase(true);
        gameObject.SetActive(true);
        descricao.text = "Clique no item que deseja comprar.";
        AtualizarInf();
    }

    public void Sair()
    {
        somSaidaBotoes.PlayOneShot(auxiliares.completoClick);
        gameObject.SetActive(false);
        vendedor.quemVende.Despedindo();
        fases.faseX.PausePlaySomFase(false);
    }
}
