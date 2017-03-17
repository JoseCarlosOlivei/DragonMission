using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveTextura : MonoBehaviour {
    private MeshRenderer render;
    public float speedX = 0.05f;
    private float x =0f;
	// Use this for initialization
	void Awake () {
        render = GetComponent<MeshRenderer>();
	}
	
	// Update is called once per frame
	void LateUpdate () {
        x += Time.deltaTime * speedX;

        if (x > 1f)
        {
            x -= 1f;
        }

        render.material.mainTextureOffset = Vector2.right * x;
	}
}
