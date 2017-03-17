using UnityEngine;
using System.Collections;

public class itens : MonoBehaviour {
	public float hpPorcetagemRecuperada;
	public int scores;
	public bool resetarTempo;
	public GameObject buff;
	private bool pegado = false;
	private float tempoX = 0f;
    private Collider2D esteColl;
    private AudioSource au;

	void Awake(){
        esteColl = GetComponent<Collider2D>();
        au = GetComponent<AudioSource>();
	}

	void Update () {
		if (pegado) {
			if (tempoX == 0f) {
				esteColl.enabled = false;
				GetComponent<SpriteRenderer> ().enabled = false;
				au.PlayOneShot (fases.faseX.pegandoItem);
			}
			if (tempoX >= fases.faseX.pegandoItem.length) {
				gameObject.SetActive (false);
			} else {
				tempoX += Time.deltaTime;
			}
        }else
        {
            ColidiuPlay();
        }
	}

    private void ColidiuPlay()
    {
        Vector2 a = esteColl.bounds.center;
        Vector2 b = a;
        a.x += esteColl.bounds.extents.x;
        a.y += esteColl.bounds.extents.y;
        b.x -= esteColl.bounds.extents.x;
        b.y -= esteColl.bounds.extents.y;

        Collider2D coll = Physics2D.OverlapArea(a, b, fases.faseX.playMask);

        if (coll)
        {
            atributosHeroi atr = coll.gameObject.GetComponent<atributosHeroi>();
            atr.hpAtual += atr.hp * hpPorcetagemRecuperada;
            if (resetarTempo)
            {
                fases.faseX.ZerarTempo();
            }
            upamento.AddScores(scores);
            buffs buffX = buff.GetComponent<buffs>();
            if (buffX)
            {
                atr.Buffa(buffX);
            }
            buffsSpeed speedBuff = buff.GetComponent<buffsSpeed>();
            if (speedBuff)
            {
                atr.SpeedBuff(speedBuff);
            }
            buffsRecupera cura = buff.GetComponent<buffsRecupera>();
            if (cura)
            {
                atr.AddRecupera(cura);
            }
            pegado = true;
        }
    }
}
