using UnityEngine;
using System.Collections;

public class buffsRecupera : MonoBehaviour {
	public float duracao = 0f;
	public float durou = 0f;
	public float taxaRecuperacao = 0f;
	public bool verificado = false;
    public float vPonts;
    public string nome;

    public void RecuperaValor(bool critico)
    {
        taxaRecuperacao = taxaRecuperacao / duracao;
        if (critico)
        {
            taxaRecuperacao *= 3f;
        }
    }
}
