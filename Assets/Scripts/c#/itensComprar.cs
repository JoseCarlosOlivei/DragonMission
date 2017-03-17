using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class itensComprar : MonoBehaviour {
    public GameObject itemCompra;
    private itensUsaveis itemVenda;
    public Text preco, peso;
    public string descricaoPt,descricaoIn;
	// Use this for initialization
	void Awake () {
        itemVenda =  itemCompra.GetComponent<itensUsaveis>();
        preco.text = itemVenda.PrecoAtual()+"$";
        peso.text = ""+(int)(itemVenda.pesoUnidade*50f);
	}
	
    public void Comprar()
    {
        bool deu = false;
        if (itemVenda.PrecoAtual() <= upamento.ScoresTotal() && mochilaCalanguito.mochila.ColocarUmItem(itemVenda))
        {
            int gasto = itemVenda.PrecoAtual();
            upamento.AddScores(-gasto);
            dataSave.jogoAtual.AddEscoresGasto(gasto);
            deu = true;
        }

        shop.shopNow.CompraSucedida(deu);
    }

    public void Descricao()
    {
        if (idioma.idiomaEscolhido == "Portugueis")
        {
            shop.shopNow.descricao.text = descricaoPt;
        }else if(idioma.idiomaEscolhido == "Ingles")
        {
            shop.shopNow.descricao.text = descricaoIn;
        }
    }

    public void SairDescricao()
    {
        shop.shopNow.descricao.text = shop.shopNow.GetSemSelecionar();
    }
}
