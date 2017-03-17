using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class infInimigos {
    private float posX, posY;
    public bool morto;

	public infInimigos(Vector2 posNow,bool die)
    {
        morto = die;
        posX = posNow.x;
        posY = posNow.y;
    }

    public Vector2 PosGravada()
    {
        return new Vector2(posX, posY);
    }
}
