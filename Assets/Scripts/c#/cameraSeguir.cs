using UnityEngine;
using System.Collections;

public class cameraSeguir : MonoBehaviour {
	private Transform foco,pai;
	private float speedFoco = 0.1f;
	private bool focado = false;
	private float tempoFoco = 0f;
    private Vector3 posi0,posAjust;
	private float quantoTer = 0f, forcaTer=0f;
    public static cameraSeguir cam;
	//Camera minhaCam;

	// Use this for initialization
	void Awake () {
		//minhaCam = GetComponent<Camera> ();
		foco = GameObject.FindGameObjectWithTag("Player").transform;
		Focar (foco,true,1f);
        cam = this;
        pai = transform.parent;
        posAjust = transform.localPosition;
	}

	// Update is called once per frame
	void LateUpdate () {
		if (foco) {
			if (focado) {
				pai.position = foco.position;
			} else {
				if (tempoFoco < 1f) {
					if (tempoFoco == 0f) {
						posi0 = pai.position;
					}
					tempoFoco += Time.deltaTime * speedFoco;
				}else if (tempoFoco > 1f) {
					tempoFoco = 1f;
				}
				pai.position = Vector3.Lerp (posi0, foco.position, tempoFoco);
				if (tempoFoco == 1f) {
					focado = true;
				}
			}
		}		
		if (quantoTer > 0f) {
			quantoTer -= Time.deltaTime;
			Vector3 posiF = new Vector3 (posAjust.x + forcaTer * Random.value, posAjust.y + forcaTer * Random.value, posAjust.z);
			transform.localPosition = posiF;
		} else if (transform.localPosition != posAjust) {
            transform.localPosition = posAjust;
		} 
	}

	public void Focar(Transform onde, bool logo,float demora){
		foco = onde;

		if (logo) {
			focado = true;
		} else {
			tempoFoco = 0f;
			speedFoco = 1f / demora;
			focado = false;
		}
	}

	public void Tremer(float forca,float dura){
		quantoTer = dura;
		forcaTer = forca;
	}

	public void ParaTremer(){
		quantoTer = 0f;
        transform.position = posi0;
	}
}
