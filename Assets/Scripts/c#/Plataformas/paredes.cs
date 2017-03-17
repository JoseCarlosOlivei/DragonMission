using UnityEngine;
using System.Collections;

public class paredes : MonoBehaviour {
	private BoxCollider2D esteColider;
	//private float comecoX,fimX,comecoY,fimY;
	/*void Start () {
		esteColider = GetComponent<BoxCollider2D> ();
	}*/
	void Start () {
		esteColider = GetComponent<BoxCollider2D> ();
	}

	void OnCollisionStay2D(Collision2D coll) {
		 if (coll.gameObject.tag == "Inimigo") {
			moveCPU move = coll.gameObject.GetComponent<moveCPU> ();

            if (coll.contacts[0].normal.y == -1f)
            {
                EstaNasPontas(move);
            }else if(coll.contacts[0].normal.y == 0f)
            {
                if (coll.contacts[0].normal.x > 0f)
                {
                    move.obstaculoDir = true;
                }
                else if (coll.contacts[0].normal.x < 0f)
                {
                    move.obstaculoEsq = true;
                }
            }
		}else if (coll.gameObject.tag == "Player")
        {
            moverJogador move = coll.gameObject.GetComponent<moverJogador>();
            if (coll.contacts[0].normal.y == 0f)
            {
                if (coll.contacts[0].normal.x > 0f)
                {
                    move.obstaculoDir = true;
                }
                else if (coll.contacts[0].normal.x < 0f)
                {
                    move.obstaculoEsq = true;
                }
            }
        }
	}

	void OnCollisionExit2D(Collision2D coll){
		if (coll.gameObject.tag == "Inimigo") {
			moveCPU move = coll.gameObject.GetComponent<moveCPU> ();
			if(move.obstaculoEsq){
				move.obstaculoEsq = false;
			}
			if(move.obstaculoDir){
				move.obstaculoDir = false;
			}
		}else if (coll.gameObject.tag == "Player")
        {
            moverJogador move = coll.gameObject.GetComponent<moverJogador>();
            if (move.obstaculoDir)
            {
                move.obstaculoDir = false;
            }
            if (move.obstaculoEsq)
            {
                move.obstaculoEsq = false;
            }
        }
    }

    public static float PontaX(GameObject objtX,bool dirt)
    {
        float ret = objtX.transform.position.x;
        Collider2D collX = objtX.GetComponent<Collider2D>();
        if (collX)
        {
            float ajust = collX.bounds.extents.x;
            if (dirt)
            {
                ret += ajust;
            }else
            {
                ret -= ajust;
            }
        }

        return ret;
    }

    public static float PontaInversa(GameObject objX,float ponta)
    {
        float ajust = objX.GetComponent<Collider2D>().bounds.extents.x;
        float ponta0 = objX.transform.position.x - ajust;
        float pontaF = objX.transform.position.x + ajust;

        if (ponta == ponta0)
        {
            pontaF = ponta0;
        }

        return pontaF;
    }

	private void EstaNasPontas(moveCPU moveX){
        float auxX = moveX.collid.bounds.extents.x;
        float pontaDirt = transform.position.x + esteColider.bounds.extents.x - auxX;
        float pontaEsq = transform.position.x - esteColider.bounds.extents.x + auxX;
		float distX = moveX.transform.position.x;

		if (distX >= pontaDirt) {
            if (!moveX.pontaDrt)
            {
                moveX.pontaDrt = true;
            }
		} else if(moveX.pontaDrt)
        {
			moveX.pontaDrt = false;
		}

		if (distX <= pontaEsq) {
            if (!moveX.pontaEsq)
            {
                moveX.pontaEsq = true;
            }
		} else if(moveX.pontaEsq)
        {
			moveX.pontaEsq = false;
		}
	}
}
