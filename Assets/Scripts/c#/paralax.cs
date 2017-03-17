using UnityEngine;
using System.Collections;

public class paralax : MonoBehaviour {
	private Transform camT;
	private Material mate;
	public float taxaX = 1f;
	public float taxaY = 1f;
	public float movimentoX,movimentoY;
	public bool alturaMinima = false,alturaMaxima = false;
	public bool limitarX, limitarY;
	private float limitX, limitY;
	public float altMn, altMx;
	private Vector2 offset,pos0;
	private float ajusteY;
	private bool maxAlt = false;
	//public float minimoY = 0f;
	private float mvX = 0f, mvY = 0f;
	//private Vector3 scalaOrig;
	// Use this for initialization
	void Start () {
		camT = FindObjectOfType<Camera> ().gameObject.transform;
		mate = GetComponent<MeshRenderer> ().material;
		limitX = 1f-mate.mainTextureScale.x;
		limitY = 1f-mate.mainTextureScale.y;
        pos0 = transform.localPosition;

		if (taxaX > 0f) {
			taxaX = 1f / taxaX;
		}
		if (taxaY > 0f) {
			taxaY = 1f / taxaY;
		}
		movimentoX /= 600f;
		movimentoY /= 300f;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (mate.mainTextureOffset.x >= 1f) {
			mvX -= 1f;
		}
		if (mate.mainTextureOffset.y >= 1f) {
			mvY -= 1f;
		}
		mvX += movimentoX * Time.deltaTime;
		mvY += movimentoY * Time.deltaTime;
		offset = new Vector2 (camT.position.x*taxaX,(camT.position.y-altMn)*taxaY);
		offset.x += mvX;
		offset.y += mvY;
		if (limitarX && limitX < offset.x) {
			offset.x = limitX;
		} else if(limitarX && offset.x<0f){
			offset.x = 0f;
		}
		if (limitarY && limitY < offset.y) {
			offset.y = limitY;
		} else if (limitarY && offset.y < 0f) {
			offset.y = 0f;
		}
		if (maxAlt) {
			offset.y = (altMx - altMn) * taxaY;
			maxAlt = false;
		}

		if (alturaMinima && camT.position.y < altMn) {
			ajusteY = altMn - camT.position.y;
		} else if (alturaMaxima && camT.position.y > altMx) {
			ajusteY = altMx - camT.position.y ;
			maxAlt = true;
		}

		transform.position = new Vector3 (camT.position.x+pos0.x, camT.position.y + ajusteY+pos0.y, transform.position.z);
		mate.mainTextureOffset = offset;

	}
		
}
