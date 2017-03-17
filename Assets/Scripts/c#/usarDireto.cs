using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class usarDireto : MonoBehaviour {
    public float cura;
    public AudioClip usandoSom;
    private AudioSource esteAudio;
    private float tempo = 0;
    private static Transform seguir;
	// Use this for initialization
	void Awake () {
        esteAudio = GetComponent<AudioSource>();
        if (!seguir && atributosHeroi.heroiX)
        {
            seguir = atributosHeroi.heroiX.transform;
        }
	}

    void LateUpdate()
    {
        if (tempo < 3f)
        {
            transform.position = seguir.position;
            tempo += Time.deltaTime;
        }else
        {
            gameObject.SetActive(false);
        }
    }

    public void Usar()
    {
        atributosHeroi.heroiX.GetComponent<atributosHeroi>().CurarPorcento(cura);
        gameObject.SetActive(true);
        esteAudio.PlayOneShot(usandoSom);
        tempo = 0f;
    }
}
