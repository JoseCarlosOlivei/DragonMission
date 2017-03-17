using UnityEngine;
using System.Collections;

public class plataLadoOutro : MonoBehaviour {
	public float distX = 2.5f;
	public float distY = 0f;
	public float frequecia = 5f;
	public bool esquerda = false,descendo = false;
	public float espera = 1f;
	private float esperando = 0;
	private float speedX, speedY;
	private bool indo = true;
	private Vector3 posi0;
	private bool xOk = false, yOk = false;
	private Vector2 move;
	private BoxCollider2D box;
	//private Rigidbody2D esteCorpo;
	// Use this for initialization
	void Start () {
		speedX = distX / frequecia;
		speedY = distY / frequecia;
		if (!esquerda) {
			speedX *=-1f;
		}
		if (descendo) {
			speedY *= -1f;
		}
		posi0 = transform.position;
		box = GetComponent<BoxCollider2D> ();
		//esteCorpo = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (esperando == 0f) {
			//Vector2 v = new Vector2 (speed, 0f);
			if (DistanciaDaOrigemX () < distX && indo) {
				//esteCorpo.velocity = new Vector2(-speed,esteCorpo.velocity.y);
				move.x = -speedX;
			} else if (SubDistX() > 0f && !indo) {  
				//esteCorpo.velocity = new Vector2(speed,esteCorpo.velocity.y);
				move.x = speedX;
			} else {
				xOk = true;
			}

			if (DistanciaDaOrigemY () < distY && indo) {
				move.y = speedY;
			} else if (SubDistY() > 0f && !indo) {  
				move.y = -speedY;
			} else {
				yOk = true;
			}

			if (xOk && yOk) {
				xOk = false;
				yOk = false;
				indo = !indo;
				esperando += Time.deltaTime;
			} else {
				transform.Translate (move * Time.deltaTime);
			}
		}else {
			esperando += Time.deltaTime;
			if (esperando > espera) {
				esperando = 0f;
			}
		}
	}

	private float DistanciaDaOrigemY(){
		float dist = SubDistY ();
		if (dist < 0f) {
			dist *= -1f;
		}

		return dist;
	}

	private float DistanciaDaOrigemX(){
		float dist = SubDistX ();
		if (dist < 0f) {
			dist *= -1f;
		}

		return dist;
	}

	private float SubDistY(){
		float dist = posi0.y - transform.position.y;
		if (!descendo) {
			dist *= -1f;
		}
		return dist;
	}


	private float SubDistX(){
		float dist = posi0.x - transform.position.x;
		if (!esquerda) {
			dist *= -1f;
		}
		return dist;
	}

	private void VerificarJogador(moverJogador moveX){
		if (moveX.MaisAltoPlata(box)) {
			//Debug.Log ("Goi");
			TornaParente(moveX.gameObject.transform);

		}
	}

	private void VerificarCPU(moveCPU moveX){
		if (moveX.MaisAltoPlata(box)) {
			//Debug.Log ("Goi");
			TornaParente(moveX.gameObject.transform);
		}
	}

	private void TornaParente(Transform traX){
		traX.SetParent (transform);
		//traX.localPosition = new Vector2 (traX.transform.localPosition.x, box.bounds.extents.y / (2f * transform.lossyScale.y));
	}

	void OnCollisionStay2D(Collision2D coll) {
		//Debug.Log (esteCorpo.velocity.y);
		if (coll.gameObject.tag == "Player" && coll.transform.parent != transform) {
			//Debug.Log ("Entrou");
			VerificarJogador(coll.gameObject.GetComponent<moverJogador> ());

		} else if (coll.gameObject.tag == "Inimigo" && coll.transform.parent != transform) {
			VerificarCPU(coll.gameObject.GetComponent<moveCPU> ());

		}
	}
}
