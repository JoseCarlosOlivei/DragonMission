using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class respira : MonoBehaviour {
    public float folego = 16f;
    public GameObject objOxigenio;
    private Slider sldOxigenio;
    private bool abaixoAgua;
    public bool respiraAbaixoDaqua;
    private float oxigenio;
    private Collider2D esteColl;
    private Vector2 ajust0,sizeBox;
    private atributosHeroi atbHero;
    private atributosInimigos atbIni;
	// Use this for initialization
	void Awake () {
        if (objOxigenio)
        {
            sldOxigenio = objOxigenio.GetComponentInChildren<Slider>();
            sldOxigenio.maxValue = folego;
            sldOxigenio.value = folego;
        }
        oxigenio = folego;
        esteColl = GetComponent<Collider2D>();
        ajust0 = esteColl.bounds.size;
        ajust0.y *= 0.4f;
        sizeBox = esteColl.bounds.size;
        sizeBox.y *= 0.2f;

        if(gameObject.tag == "Player")
        {
            atbHero = GetComponent<atributosHeroi>();
        }else if(gameObject.tag == "Inimigo")
        {
            atbIni = GetComponent<atributosInimigos>();
        }
	}
	
	// Update is called once per frame
	void Update () {
        AbaixoAgua();
        if (abaixoAgua)
        {
            if (respiraAbaixoDaqua)
            {
                RecuperaAr();
            }else
            {
                SemAr();
            }
        }else
        {
            if (!respiraAbaixoDaqua)
            {
                RecuperaAr();
            }
            else
            {
                SemAr();
            }
        }

        if(sldOxigenio)
        {
            if (sldOxigenio.value != oxigenio)
            {
                if (!objOxigenio.activeInHierarchy)
                {
                    objOxigenio.SetActive(true);
                }
                sldOxigenio.value = oxigenio;
            }else if(objOxigenio.activeInHierarchy)
            {
                objOxigenio.SetActive(false);
            }
        }
	}

    private void RecuperaAr()
    {
        if(oxigenio != folego)
        {
            if (oxigenio < folego)
            {
                oxigenio += folego * Time.deltaTime / 4f;
            }
            else if (oxigenio > folego)
            {
                oxigenio = folego;
            }
        }
    }

    private void AbaixoAgua()
    {
        Vector2 posI = esteColl.bounds.center;
        posI += ajust0;
        abaixoAgua = Physics2D.OverlapBox(posI, sizeBox, 0f, fases.faseX.waterMask);
    }

    private void SemAr()
    {
        if (oxigenio > 0f)
        {
            oxigenio -= Time.deltaTime;
        }
        else if (oxigenio < 0f)
        {
            oxigenio = 0f;
            if (atbHero)
            {
                atbHero.hpAtual = 0f;
            }else if (atbIni)
            {
                atbIni.hpAtual = 0f;
            }
        }
    }
}
