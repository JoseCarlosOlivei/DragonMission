using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class naoEspelhar : MonoBehaviour {
	
	// Update is called once per frame
	void LateUpdate () {
		if(transform.lossyScale.x<0f )
        {
            transform.localScale = new Vector3(-transform.localScale.x,transform.localScale.y,transform.localScale.z);         
        }
	}
}
