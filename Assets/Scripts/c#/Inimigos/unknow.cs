using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unknow : MonoBehaviour {
    private Rigidbody2D esteCorpo;
    private Animator anima;
    public bool solo = false;
    private Collider2D esteColl;
    private AudioSource esteAudio;
    public AudioClip queda;
	// Use this for initialization
	void Awake () {
        esteCorpo = GetComponent<Rigidbody2D>();
        anima = GetComponent<Animator>();
        esteColl = GetComponent<Collider2D>();
        esteAudio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if(esteCorpo.velocity.y == 0f && !anima.GetBool("solo"))
        {
            esteAudio.PlayOneShot(queda);
            anima.SetBool("solo", true);
            solo = true;
        }else if(esteCorpo.velocity.y > 0f && anima.GetFloat("speedY")<=0f)
        {
            solo = false;
            anima.SetBool("solo", solo);
            anima.SetFloat("speedY", 1f);
        }
        else if (esteCorpo.velocity.y < 0f && anima.GetFloat("speedY") >= 0f)
        {
            anima.SetFloat("speedY", -1f);
            solo = false;
            anima.SetBool("solo", solo);
        }
    }

    public void Pular()
    {
        if (solo)
        {
            esteCorpo.AddForce(Vector2.up * 2f, ForceMode2D.Impulse);
            esteColl.isTrigger = true;
        }
    }

    void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }
}
