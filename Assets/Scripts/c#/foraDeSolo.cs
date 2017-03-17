using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class foraDeSolo : MonoBehaviour {
    private moveCPU move;
    private Rigidbody2D esteCorpo;
    private Vector2 direcao = Vector2.zero;
    public bool voar = false;
	// Use this for initialization
	void Awake () {
        move = GetComponent<moveCPU>();
	}
	
	// Update is called once per frame
	void Update () {
        if (esteCorpo.velocity != move.speed*direcao && voar)
        {
            Vector2 vAux = move.speed * direcao - esteCorpo.velocity;

            esteCorpo.AddForce(vAux, ForceMode2D.Impulse);
        }
	}

    public void VoarDire(Vector2 dire)
    {
        direcao = dire;
    }

}
