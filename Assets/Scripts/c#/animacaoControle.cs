using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animacaoControle : MonoBehaviour {
    private Animator anima;
	// Use this for initialization
	void Awake () {
        anima = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Parar()
    {
        anima.enabled = false;
    }

    public void Continuar()
    {
        anima.enabled = true;
    }
}
