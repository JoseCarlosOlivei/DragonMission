using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class giroAcordoSpeed : MonoBehaviour {

    private Rigidbody2D estecorpo;
	// Use this for initialization
	void Awake () {
        estecorpo = GetComponent<Rigidbody2D>();
	}

    // Update is called once per frame
    void LateUpdate()
    {
        float anguloZ = Mathf.Atan2(estecorpo.velocity.y, estecorpo.velocity.x) * Mathf.Rad2Deg;

        transform.eulerAngles = new Vector3(0f, 0f, anguloZ);
    }
}
