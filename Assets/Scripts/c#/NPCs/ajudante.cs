using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ajudante : MonoBehaviour {
	private int idBonus = 0;
	private fases fase;
	private float proXMn,proXMx,proYMn,proYMx;
	private atributosHeroi hAtr;
	private bool falando = false;
	public Text boxFala,boxQuemFala;
	private GameObject ativaFala;
	private string[] falaBonus = new string[3];
	//private float posF;
	// Use this for initialization
	void Start () {
		ativaFala = boxQuemFala.transform.parent.gameObject;
		fase = FindObjectOfType<fases> ();
		hAtr = atributosHeroi.heroiX.GetComponent<atributosHeroi> ();
		SpriteRenderer redeP = GetComponent<SpriteRenderer> ();
		SpriteRenderer rede = GetComponent<SpriteRenderer> ();
		proXMn = transform.position.x-rede.bounds.extents.x-redeP.bounds.extents.x;
		proXMx = transform.position.x+rede.bounds.extents.x+redeP.bounds.extents.x;
		proYMn =transform.position.y-0.01f;
		proYMx =transform.position.y+rede.bounds.extents.y*2f;

		if (0.1f >= Random.value) {
			idBonus = Random.Range (0, 3);
			//Debug.Log (idBonus);
		} else {
			gameObject.SetActive (false);
		}

		IniciarFalas (PlayerPrefs.GetString("Idioma"));
	}
	
	// Update is called once per frame
	void Update () {
		if (PodeFalar (atributosHeroi.heroiX.transform.position) && !falando) {
			if (Input.GetButtonDown ("Passar")) {
				fases.podeMover = false;
				falando = true;
			}
		} else if (falando) {
			if (!ativaFala.activeInHierarchy) {
				boxQuemFala.text = "Ajudante";
				boxFala.text = falaBonus[idBonus];
				ativaFala.SetActive (true);
			}
			if (Input.GetButtonDown ("Passar")) {
				ativaFala.SetActive (false);
				falando = false;
				gameObject.SetActive (false);
				fases.podeMover = true;
				Bonus ();
			}
		}
	}

	private void Bonus(){
		if (idBonus == 0) {
			fase.ZerarTempo ();
		} else if (idBonus == 1) {
			upamento.AddScores(100);
		} else if (idBonus == 2) {
			hAtr.hpAtual = hAtr.hp;
		}
	}

	private bool PodeFalar(Vector2 pos){
		bool retorno = false;
		if (pos.x >= proXMn && pos.x <= proXMx && pos.y >= proYMn && pos.y <= proYMx) {
			retorno = true;
		}
		return retorno;
	}

	private void IniciarFalas(string idioma){
		if (idioma == "Portugueis") {
			falaBonus [0] = "Zerar tempo.";
			falaBonus [1] = "50 pontos.";
			falaBonus [2] = "Recuperar todo HP.";
		}else if(idioma=="Ingles"){
			falaBonus [0] = "Zerar tempo.";
			falaBonus [1] = "50 pontos.";
			falaBonus [2] = "Recuperar todo HP.";
		}
	}

}
