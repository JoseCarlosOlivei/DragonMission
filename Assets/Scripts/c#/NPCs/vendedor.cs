using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vendedor : MonoBehaviour {
    private Collider2D esteColl;
    private Animator anima;
    private string[] falasChegada,falasSaida;
    public GameObject interacao;
    private AudioSource esteAudio;
    private float tempoFala = 0f;
    public AudioClip fala;
    public float velocidadeDeFala = 1.75f;
    public shop canvasShop;
    public static vendedor quemVende;
    public static bool primeiraVez = true;
    private bool passou = false;
    private bool despedir = false;
    private bool interagiu = false;
    private int cena = 0;
    private atuacao calangoAtua;
    private bool especialCena = false;
    private bool especialCena2 = false;
	// Use this for initialization
	void Awake () {
        anima = GetComponent<Animator>();
        esteColl = GetComponent<Collider2D>();
        esteAudio = GetComponent<AudioSource>();
        falasChegada = new string[10];
        falasSaida = new string[10];
        falasChegada[0] = "É perigoso ir sozinho, leve tudo !!!";
        falasSaida[0] = "Volte se sobreviver.";
        falasChegada[1] = "Você denovo ?";
        falasSaida[1] = "Até o próximo mapa.";
        falasChegada[2] = "Seja feliz, compre mais !!!";
        falasSaida[2] = "Você comprou tão pouco... acho que meus filhos vão passar fome hoje.";
        falasChegada[3] = "Nada aqui é roubado.";
        falasSaida[3] = "Vou ficar triste quando você morrer... mas feliz por ficar com suas coisas.";
        falasChegada[4] = "Os mais espertos compram tudo, os burros só olham.";
        falasSaida[4] = "Um dia quero ser tão rico quanto você.";
        falasChegada[5] = "O lixo para uns é ouro para mim, mas tem coisa que é só lixo mesmo.";
        falasSaida[5] = "Cuida para não perder seus itens em um lugar que eu não possa encontrar.";
        falasChegada[6] = "Está um lindo dia ,os pássaros estão cantando, as flores desabrochando, em dias como este pessoas como você deveriam comprar tudo da minha loja !!!";
        falasSaida[6] = "Despedidas me entristecem.";
        falasChegada[7] = "Tudo pela metade do preço para quem é caolho !!!";
        falasSaida[7] = "Qual quer dia eu deveria dar um desconto para você...";
        falasChegada[8] = "O de sempre hoje ?";
        falasSaida[8] = "Que Dragonicos lhe proteja.";
        falasChegada[9] = "Quem compra os itens da minha loja vive mais feliz enquanto o item dura.";
        falasSaida[9] = "Quem compra sempre ganha, mas quem vende ganha mais.";

        esteAudio.pitch = velocidadeDeFala;
        quemVende = this;
        calangoAtua = atributosHeroi.heroiX.GetComponent<atuacao>();

        if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "prologo" && dataSave.jogoAtual.vendorPrimeiro)
        {
            especialCena = true;
        }

        if (dataSave.jogoAtual.vendorDica)
        {
            especialCena2 = true;
        }
	}

	// Update is called once per frame
	void Update () {
        if (!fases.podeMover)
        {
            if (interacao.activeInHierarchy)
            {
                interacao.SetActive(false);
            }

            if (interagiu && !especialCena && !especialCena2)
            {
                if (Input.GetButtonDown("Passar"))
                {
                    fases.faseX.caixaMensagens.SetActive(false);
                    if (despedir)
                    {
                        fases.podeMover = true;
                        passou = false;
                        interacao.SetActive(false);
                        tempoFala = 0f;
                        interagiu = false;
                        despedir = false;
                    }
                    else if (!passou)
                    {
                        passou = true;
                        canvasShop.AparecerShop();
                        tempoFala = 0f;
                    }
                }
            }
        }
        else if (auxiliares.PodeFalar(esteColl))
        {
            if (!interacao.activeInHierarchy)
            {
                interacao.SetActive(true);
            }
            if (!especialCena && !especialCena2)
            {
                Interagir();
            }
        }
        else
        {
            if (interacao.activeInHierarchy)
            {
                interacao.SetActive(false);
            }
        }

        if (tempoFala > 0f)
        {
            tempoFala -= Time.deltaTime;
            if (!anima.GetBool("fala"))
            {
                anima.SetBool("fala", true);
                esteAudio.clip = fala;
                esteAudio.Play();
            }
        }
        else if (anima.GetBool("fala"))
        {
            anima.SetBool("fala", false);
            esteAudio.Stop();
        }

        //cenas especiais
        if (auxiliares.PodeFalar(esteColl))
        {
            if (especialCena)
            {
                if (Input.GetButtonDown("Passar"))
                {
                    if (cena == 0)
                    {
                        VirarHeroi();
                        cena += 1;
                        fases.podeMover = false;
                        fases.faseX.caixaMensagens.SetActive(true);
                        fases.faseX.quemfala.text = "Calanguito";
                        fases.faseX.fala.text = "O que você está você fazendo aqui ???";
                        calangoAtua.Atuando(2, fases.faseX.fala.text.Length);
                    }
                    else if (cena == 1)
                    {
                        cena += 1;
                        calangoAtua.Atuando(2, 0);
                        fases.faseX.quemfala.text = "Vendor";
                        fases.faseX.fala.text = "Calma meu amigo, sou apenas um vendedor, estou aqui a negócios.";
                        tempoFala = fases.faseX.fala.text.Length / (40f * velocidadeDeFala);
                    }
                    else if (cena == 2)
                    {
                        cena += 1;
                        tempoFala = 0f;
                        fases.faseX.quemfala.text = "Calanguito";
                        fases.faseX.fala.text = "Estamos no meio de uma invasão !!!";
                        calangoAtua.Atuando(2, fases.faseX.fala.text.Length);
                    }
                    else if (cena == 3)
                    {
                        cena += 1;
                        calangoAtua.Atuando(2, 0);
                        fases.faseX.quemfala.text = "Vendor";
                        fases.faseX.fala.text = "Normal.";
                        tempoFala = fases.faseX.fala.text.Length / (40f * velocidadeDeFala);
                    }
                    else if (cena == 4)
                    {
                        cena += 1;
                        tempoFala = 0f;
                        fases.faseX.quemfala.text = "Calanguito";
                        fases.faseX.fala.text = "Você é louco ?";
                        calangoAtua.Atuando(2, fases.faseX.fala.text.Length);
                    }
                    else if (cena == 5)
                    {
                        cena += 1;
                        calangoAtua.Atuando(2, 0);
                        fases.faseX.quemfala.text = "Vendor";
                        fases.faseX.fala.text = "Louco é quem perde minhas promoções !";
                        tempoFala = fases.faseX.fala.text.Length / (40f * velocidadeDeFala);
                    }
                    else if (cena == 6)
                    {
                        cena += 1;
                        tempoFala = 0f;
                        fases.faseX.quemfala.text = "Calanguito";
                        fases.faseX.fala.text = "Não tenho tempo para isso tenho que salvar meu rei !!!";
                        calangoAtua.Atuando(1, fases.faseX.fala.text.Length);
                    }
                    else if (cena == 7)
                    {
                        calangoAtua.Atuando(1, 0);
                        cena += 1;
                        fases.faseX.quemfala.text = "Vendor";
                        fases.faseX.fala.text = "Mas antes de ir, dê só uma olha no que eu tenho a lhe oferecer.";
                        tempoFala = fases.faseX.fala.text.Length / (40f * velocidadeDeFala);
                    }
                    else if (cena == 8)
                    {
                        canvasShop.AparecerShop();
                        tempoFala = 0f;
                        cena += 1;
                    }
                    else if (cena == 9 && despedir)
                    {
                        cena += 1;
                        fases.faseX.quemfala.text = "Vendor";
                        fases.faseX.fala.text = "Ele se chamado de crital do retorno, dizem que quem o ativa pode retorna do ponto em que ativou o cristal se tiver um certo artefato... por acaso eu vendo este artefato.";
                        tempoFala = fases.faseX.fala.text.Length / (40f * velocidadeDeFala);
                    }
                    else if (cena == 10)
                    {
                        cena = 0;
                        tempoFala = 0f;
                        especialCena = false;
                        despedir = false;
                        dataSave.jogoAtual.vendorPrimeiro = false;
                        fases.podeMover = true;
                        calangoAtua.Atuando(0, 0);
                        fases.faseX.caixaMensagens.SetActive(false);
                    }
                }
            }
        }else if (especialCena2)
        {

        }
    }

    private void Interagir()
    {
        if (Input.GetButtonDown("Passar"))
        {
            VirarHeroi();
            fases.podeMover = false;
            fases.faseX.caixaMensagens.SetActive(true);
            fases.faseX.quemfala.text = "Vendor";
            fases.faseX.fala.text = auxiliares.RandonFala(falasChegada);
            tempoFala = fases.faseX.fala.text.Length / (40f * velocidadeDeFala);
            interagiu = true;
        }
    }

    private void VirarHeroi() { 
        //virar para heroi
        if (transform.position.x <= atributosHeroi.heroiX.transform.position.x)
        {
            if (transform.eulerAngles.y != 0f)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
        }
        else if (transform.eulerAngles.y != 180f)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
    }

    public void Despedindo()
    {
        fases.faseX.caixaMensagens.SetActive(true);

        if(especialCena)
        {
            fases.faseX.fala.text = "Esta vendo aquele cristal ali.";
            if (transform.eulerAngles.y != 0f)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
            tempoFala = fases.faseX.fala.text.Length / (40f * velocidadeDeFala);
        }
        else
        {           
            fases.faseX.fala.text = auxiliares.RandonFala(falasSaida);
            tempoFala = fases.faseX.fala.text.Length / (40f * velocidadeDeFala);
        }

        despedir = true;
    }
}
