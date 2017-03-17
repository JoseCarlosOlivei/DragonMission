using UnityEngine;
using System.Collections;

public class plataElipse : MonoBehaviour {
	public float raioX = 1f;
	public float raioY = 1f;
	public float tempoDeCiclo = 10f;
	public bool sentidoHorario = true;
	private float speedAngulo;
	private float anguloAgoraSpeed = 0f;
	private Vector2 pos0;
	private BoxCollider2D box;
	//private Vector3 posF;
	// Use this for initialization
	void Start () {
		speedAngulo = Mathf.PI * 2f/ tempoDeCiclo;
		//speedX = -diametroX / tempoDeCiclo;
		//speedY = diametroY / tempoDeCiclo;
		if (sentidoHorario) {
			speedAngulo *= -1f;
		}
		pos0 = transform.position;
		if (!sentidoHorario) {
			raioX = -raioX;
		}
		pos0.x -= raioX;
		box = GetComponent<BoxCollider2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		anguloAgoraSpeed += speedAngulo * Time.deltaTime;
		//Debug.Log (anguloAgoraSpeed*360f/(2f*Mathf.PI));
		if (anguloAgoraSpeed >= Mathf.PI * 2f) {
			anguloAgoraSpeed -= Mathf.PI * 2f;
		} else if (anguloAgoraSpeed <= -Mathf.PI * 2f) {
			anguloAgoraSpeed += Mathf.PI * 2f;
		}
		transform.position = new Vector3(pos0.x+raioX*Mathf.Cos(anguloAgoraSpeed),pos0.y+Mathf.Sin(anguloAgoraSpeed)*raioY,transform.position.z);
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

	void OnCollisionEnter2D(Collision2D coll) {
		//Debug.Log (esteCorpo.velocity.y);
		if (coll.gameObject.tag == "Player") {
			//Debug.Log ("Entrou");
			VerificarJogador(coll.gameObject.GetComponent<moverJogador> ());

		} else if (coll.gameObject.tag == "Inimigo") {
			VerificarCPU(coll.gameObject.GetComponent<moveCPU> ());

		}
	}
}
