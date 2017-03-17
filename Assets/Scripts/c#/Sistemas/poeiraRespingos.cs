using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class poeiraRespingos : MonoBehaviour {
    private Transform pai;
    private ParticleSystem particulas;

    void Awake()
    {
        particulas = GetComponent<ParticleSystem>();
    }

    public void AddPai(Transform paiT,BoxCollider2D coll)
    {
        pai = paiT;
        float sizeX = coll.bounds.size.y;
        transform.localScale = new Vector3(sizeX, sizeX, sizeX);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (particulas.isPlaying)
        {
            ColocarPosicao();
        }
    }

    public void AtivarDesativar(bool ativa)
    {
        if (ativa && !particulas.isPlaying)
        {
            particulas.Play();
            ColocarPosicao();
        }

        if(!ativa && !particulas.isStopped)
        {
            particulas.Stop();
        }
    }

    private void ColocarPosicao()
    {
        if (pai.lossyScale.x >= 0f && transform.rotation.eulerAngles.y != 180f)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }

        if (pai.lossyScale.x < 0f && transform.rotation.eulerAngles.y != 0f)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        transform.position = pai.transform.position;
    }
}
