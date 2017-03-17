using UnityEngine;
using System.Collections;

public class chegandoTeleporte : MonoBehaviour {
	private Animator anima;
	private float tempX = 0f;
	private bool vai = false;
    private AudioSource esteAudio;
    public AudioClip teleporteSom;
    private Transform filho;
    private Renderer redere;
	// Use this for initialization
	void Awake () {
		anima = GetComponent<Animator> ();
		anima.enabled = false;
        esteAudio = GetComponent<AudioSource>();
        filho = transform.GetChild(0);
        redere = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
		if (vai) {
			if (tempX == 0f) {
				anima.enabled = true;
                esteAudio.PlayOneShot(teleporteSom);
			}
            if (tempX < 1f)
            {
                tempX += Time.deltaTime;
               
                if(tempX > 35f / 60f)
                {
                    redere.enabled = false;
                }
            }
            else
            {
                filho.SetParent(null);
                gameObject.SetActive(false);
            }
        }
	}

	public void ChegandoComTeleporte(){
		vai = true;
	}
}
