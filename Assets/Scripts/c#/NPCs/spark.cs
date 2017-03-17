using UnityEngine;
using System.Collections;

public class spark : MonoBehaviour {
	private Animator anima;
	private bool ir = false,foi = false;
    private Rigidbody2D esteCorpo;
    private float forcaPulo;
    private float tempoSumir = 0f;
    private AudioSource esteAudio;
    public AudioClip indo;
	// Use this for initialization
	void Awake () {
		anima = GetComponent<Animator> ();
        gameObject.SetActive(false);
        esteCorpo = GetComponent<Rigidbody2D>();
        forcaPulo = Mathf.Pow(Physics2D.gravity.magnitude * 2.5f, 0.5f);
        esteAudio = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
		if (ir) {
			if (esteCorpo.velocity.y <= 0f) {
                if (!anima.GetBool("teleporte"))
                {
                    anima.SetBool("teleporte", true);
                    esteCorpo.gravityScale = 0f;
                    esteCorpo.velocity = Vector2.zero;
                    esteAudio.clip = null;
                    esteAudio.loop = false;
                    esteAudio.PlayOneShot(indo);
                }
                else if(foi)
                {
                    if (tempoSumir > 0f)
                    {
                        tempoSumir -= Time.deltaTime;
                    }else
                    {
                        gameObject.SetActive(false);
                    }
                }
            }
		}
	}

	public void IrEmbora(){
		ir = true;
		anima.SetBool ("pular",true);
        esteCorpo.AddForce(forcaPulo*Vector2.up, ForceMode2D.Impulse);
	}

    public void Desaparecer()
    {
        tempoSumir = 1f;
        auxiliares.DeixarInvisivel(gameObject);
        foi = true;
    }
}
