using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class auxiliares : MonoBehaviour {

    public static AudioClip botao, completoClick, erroSom,pegarItem;
    public static int idCena;

    public static void BotaoSom(AudioSource saida)
    {
        saida.PlayOneShot(botao);
    }

    public static void CompletoSom(AudioSource saida)
    {
        saida.PlayOneShot(completoClick);
    }

    public static void SomErro(AudioSource saida)
    {
        saida.PlayOneShot(erroSom);
    }

    public static bool PodeFalar(Collider2D coll)
    {
        bool ret = false;

        if (atributosHeroi.heroiX.GetComponent<moverJogador>().noChao)
        {
            Vector2 a = coll.bounds.center;
            Vector2 siz =coll.bounds.extents;
            siz += Vector2.right * 0.4f;
           
            Collider2D collX = Physics2D.OverlapBox(a,siz,0f,fases.faseX.playMask);
            if (collX)
            {
                ret = true;
            }
        }

        return ret;
    }

    public static string RandonFala(string[] falas)
    {
        int i = Random.Range(0, falas.Length);

        return falas[i];
    }

    public static float VantagemSpeedAreaT(float speed,float tempo,Vector2 areaSize,float vantSeguidor)
    {
        float vant = 0.5f+speed*vantSeguidor / (speed*vantSeguidor+2f);
        float tVant = tempo / (64f + tempo);
        vant = (1f - tVant) + tVant * vant;

        Vector2 sizeMax = moveCPU.limiteTela * 2f;
        float vantArea = 1f+areaSize.magnitude / sizeMax.magnitude;

        vant = vantArea + vant / 2f;

        return vant;
    }

    public static float VantagemArea(Vector2 areaSize)
    {
        Vector2 sizeMax = moveCPU.limiteTela * 2f;
        return areaSize.magnitude / sizeMax.magnitude;
    }

    public static void Falar(Text localFala,Text quemLocal,int exprecao,GameObject ator,string fala,string quem,GameObject textoCaixa)
    {
        textoCaixa.SetActive(true);
        atuacao atua = ator.GetComponent<atuacao>();
        quemLocal.text = quem;
        localFala.text = fala;
        atua.Atuando(exprecao, fala.Length);
    }

    public static void RemoverIgnoradosColiders(List<Collider2D> list,Collider2D collX)
    {
        for(int i = 0; i < list.Count; i++)
        {
            Physics2D.IgnoreCollision(collX, list[i], false);
        }

        list.Clear();
    }

    public static GameObject RetornaObj (List<GameObject> list)
    {
        GameObject objX = null;

        for(int i = 0; i < list.Count; i++)
        {
            if (!list[i].activeInHierarchy)
            {
                objX = list[i];
                i = list.Count;
            }
        }

        if (!objX)
        {
            objX = Instantiate(list[0]);
            list.Add(objX);
        }

        return objX;
    }

	public static void DeixarTudoInvisivel(GameObject obj)
    {
        Renderer[] redeFilhos = obj.GetComponentsInChildren<Renderer>();

        for(int i = 0; i < redeFilhos.Length; i++)
        {
            redeFilhos[i].enabled = false;
        }
    }

    public static void DeixarInvisivel(GameObject obj)
    {
        obj.GetComponent<Renderer>().enabled = false;
    }

    public static void DeixarVisivel(GameObject obj)
    {
        obj.GetComponent<Renderer>().enabled = true;
    }

    public static void DeixarTudoVisivel(GameObject obj)
    {
        Renderer[] redeFilhos = obj.GetComponentsInChildren<Renderer>();

        for (int i = 0; i < redeFilhos.Length; i++)
        {
            redeFilhos[i].enabled = true;
        }
    }
}
