using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class virarDeAcordoYrotation : MonoBehaviour {
    private Transform pai;
	// Use this for initialization
	void Awake () {
        pai = transform.parent;
	}
	
	// Update is called once per frame
	void Update () {
		if(pai.rotation.eulerAngles.y == transform.rotation.eulerAngles.y)
        {
            transform.rotation = Quaternion.Euler(0f, pai.eulerAngles.y, transform.rotation.eulerAngles.x);
        }
	}
}
