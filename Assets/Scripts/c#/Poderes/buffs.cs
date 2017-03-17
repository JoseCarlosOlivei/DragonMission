using UnityEngine;
using System.Collections;

public class buffs : MonoBehaviour {
	public string nome;
	public float[] ondeQuanto = new float[3];//0 dano/defesa, 1 resistencia,2 agilidade
	public float duracao = 0f;
	public float durou = 0f;
	public bool verificado = false;
    public Vector2 sizeArea;
	// Use this for initialization
	void Awake () {
        float ajust = auxiliares.VantagemArea(sizeArea);
	    for(int i = 0; i < ondeQuanto.Length; i++)
        {
            ondeQuanto[i] /= ajust;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public float[] SomaBuff(float[] x){
		float[] resp = x;
		for (int i = 0; i < ondeQuanto.Length; i++) {
			x [i] += ondeQuanto [i];
		}
		return resp;
	}

	public float[] SubtraBuff(float[] x){
		float[] resp = x;
		for (int i = 0; i < ondeQuanto.Length; i++) {
			x [i] -= ondeQuanto [i];
		}
		return resp;
	}
}
