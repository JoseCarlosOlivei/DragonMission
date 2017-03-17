using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class gameOver : MonoBehaviour {
    public Text textGameOver,textFrases;
    private bool mostrou = false;
    private string[] frases;
    public AudioClip musicaGameOver;

	// Use this for initialization
	void Awake () {
        frases = new string[64];
		if(idioma.idiomaEscolhido == "Portugueis")
        {
            textGameOver.text = "Fim de Jogo";
            frases[0] = "É preciso ter um caos dentro de si para dar à luz uma estrela cintilante." + "\n" + "-Friedrich Nietzsche";
            frases[1] = "No fim tudo dá certo, e se não deu certo é porque ainda não chegou ao fim."+"\n"+"-Fernado Sabino";
            frases[2] = "Não devemos ter medo dos confrontos... até os planetas se chocam e do caos nascem as estrelas." + "\n" + "-Desconhecido";
            frases[3] = "Nunca ria de dragões vivos." + "\n" + "-J.R.R. Tolkien";
            frases[4] = "Primordial é vencer nossos próprios defeitos, derrotar os dragões da alma, conquistar o amor próprio, e virar o grande herói de si mesmo!" + "/n" + "-Sterlina";
            frases[5] = "O que não provoca minha morte faz com que eu fique mais forte." + "\n" + "-Friedrich Nietzsche";
            frases[6] = "Para quê preocuparmo-nos com a morte? A vida tem tantos problemas que temos de resolver primeiro." + "\n" + "-Confúcio";
            frases[7] = "Os covardes morrem várias vezes antes da sua morte, mas o homem corajoso experimenta a morte apenas uma vez." + "\n" + "-William Shakespeare";
            frases[8] = "A esperança é o sonho do homem acordado." + "\n" + "-Aristóteles";
            frases[9] = "A esperança seria a maior das forças humanas, se não existisse o desespero." + "\n" + "-Victor Hugo";
            frases[10] = "É bom ter esperança, mas é ruim depender dela." + "\n" + "-Textos Judaicos";
            frases[11] = "Num certo momento da vida, não é a esperança a última a morrer, mas a morte é a última esperança." + "\n" + "-Leonardo Sciascia";
            frases[12] = "Quem se vence a si mesmo é um herói maior do que quem enfrenta mil batalhas contra muitos milhares de inimigos." + "\n" + "-Textos Budistas";
            frases[13] = "Um herói é um indivíduo comum que encontra a força para perseverar e resistir apesar dos obstáculos devastadores." + "\n" + "-Christopher Reeve";
            frases[14] = "Todo o herói torna-se chato." + "\n" + "-Ralph Waldo Emerson";
            frases[15] = "O verdadeiro herói é aquele que faz o que pode." + "\n" + "-Romain Rolland";
            frases[16] = "O que é um dragão sem suas asas... um calango ?" + "\n" + "-José Carlos Oliveira dos Santos Júnior";
            frases[17] = "Os fracos morrem encasa e os guerreirros morrem lutando, e sao chamados de \"heroi\". . ."+"\n"+"-Ernesto Mabone Carlos";
            frases[18] = "As únicas pessoas que nunca fracassam são as que nunca tentam." + "\n" + "-Desconhecido";
            frases[19] = "Se você vai tentar, vá até o fim, caso contrário, nem comece." + "\n" + "-Charles Bukowski";
            frases[20] = "O sucesso é ir de fracasso em fracasso sem perder entusiasmo." + "\n" + "-Winston Churchill";
            frases[21] = "O campo da derrota não está povoado de fracassos, mas de homens que tombaram antes de vencer." + "\n" + "-Abraham Lincoln";
            frases[22] = "Cada fracasso ensina ao homem algo que ele precisava aprender." + "\n" + "-Charles Dickens";
            frases[23] = "O fracasso quebra as almas pequenas e engrandece as grandes, assim como o vento apaga a vela e atiça o fogo da floresta." + "\n" + "-Benjamin Franklin";
            frases[24] = "Há duas coisas que ninguém perdoa: nossas vitórias e nossos fracassos." + "\n" + "-Millôr Fernandes";
            frases[25] = "A alegria está na luta, na tentativa, no sofrimento envolvido e não na vitória propriamente dita." + "/n" + "-Mahatma Gandhi";
            frases[26] = "Sonha e serás livre de espírito... luta e serás livre na vida." + "\n" + "-Che Guevara";
            frases[27] = "Quem abandona a luta não poderá nunca saborear o gosto de uma vitória." + "-Textos Judaicos";
            frases[28] = "Só é lutador quem sabe lutar consigo mesmo." + "\n" + "-Carlos Drummond de Andrade";
            frases[29] = "A luta não é vencida quando seu inimigo cai ao chão,mas sim quando você se mantém de pé." +"\n"+ "-Gregory Barros";
            frases[30] = "Quem perde seus bens perde muito; quem perde um amigo perde mais; mas quem perde a coragem perde tudo."+"\n"+ "-Miguel de Cervantes";
            frases[31] = "O pessimista vê dificuldade em cada oportunidade; o otimista vê oportunidade em cada dificuldade." + "\n" + "-Winston Churchill";
            frases[32] = "Paciência e perseverança tem o efeito mágico de fazer as dificuldades desaparecerem e os obstáculos sumirem." + "\n" + "-John Quincy Adams";
            frases[33] = "Não olhe as dificuldades como obstáculo, mas como uma maneira de provar que você é capaz!" + "\n" + "-Pr.Ilson";
            frases[34] = "A facilidade enjoa, a dificuldade cansa." + "\n" + "-Fabrício Carpinejar";
            frases[35] = "O homem fraco teme a morte, o desgraçado chama-a; o valente procura-a. Só o sensato a espera." + "\n" + "-Benjamin Franklin";
            frases[36] = "O homem não teria alcançado o possível se, repetidas vezes, não tivesse tentado o impossível." + "\n" + "-Max Weber";
            frases[37] = "A única forma de chegar ao impossível é acreditar que é possível." + "\n" + "-Alice no País das Maravilhas";
            frases[38] = "É impossível para um homem aprender aquilo que ele acha que já sabe." + "\n" + "-Epicteto";
            frases[39] = "A persistência realiza o impossível." + "\n" + "-Provérbio Chinês";
            frases[40] = "Nossas dúvidas são traidoras e nos fazem perder o que, com frequência, poderíamos ganhar, por simples medo de arriscar." + "\n" + "-William Shakespeare";
            frases[41] = "A persistência é o caminho do êxito." + "\n" + "-Charles Chaplin";
            frases[42] = "As pessoas entram em nossa vida por acaso, mas não é por acaso que elas permanecem." + "\n" + "-Lilian Tonet";
            frases[43] = "A desordem é o melhor servidor da ordem estabelecida."+"\n"+ "-Jean-Paul Sartre";
            frases[44] = "O progresso não é mais do que o desenvolvimento da ordem." + "\n" + "-Auguste Comte";
            frases[45] = "Se a regra é a desordem, pagarás por instituir a ordem." + "\n" + "-Paul Valéry";
            frases[46] = "Estamos ameaçados por duas calamidades: a ordem e a desordem." + "\n" + "-Paul Valéry";
            frases[47] = "Eu trouxe ordem a partir do caos." + "\n" + "-Arthur Conan Doyle";
            frases[48] = "Era uma vez um calango... e morreu." + "\n" + "-K.K e Jr";
            frases[49] = "O desejo da ordem é a única ordem do mundo." + "\n" + "-Georges Duhamel";
            frases[50] = "Se matamos uma pessoa somos assassinos. Se matamos milhões de homens, celebram-nos como heróis."+"\n"+ "-Charles Chaplin";
            frases[51] = "Se você pudesse voltar uma vez no passado, você voltaria para reviver algum momento ou para mudar alguma coisa?" + "-Tumblr";
            frases[52] = "Os povos têm os governantes que merecem." + "\n" + "-Textos Judaicos";
            frases[53] = "Governantes há que se acham deuses. Outros, têm certeza." + "\n" + "-Antonio Gomes Lacerda";
            frases[54] = "Ninguém pode salvar ninguém.Temos de nos salvar a nós próprios." + "\n" + "-Herman Melville";
            frases[55] = "O desejo de salvar a humanidade é quase sempre um disfarce para o desejo de controlá-la." + "\n" + "-H. L. Mencken";
            frases[56] = "Uma espada empunhada mantém a outra na bainha."+"\n"+ "-George Herbert";
            frases[57] = "A missão é seu escudo, e a verdade sua espada."+"\n"+ "-Dia do Alívio - Forfun";
            frases[58] = "Lutar sem ter esperança, é dar o pescoço à espada." + "\n" + "-Isaac Marinho";
            frases[59] = "Os guerreiros também são feridos e chegam até ser derrotados, mas jamais largam a espada!"+"\n"+ "-Sid Aguiar";
            frases[60] = "Não se mede o valor de um guerreiro pelo tamanho de sua espada, mais sim pela habilidade e capacidade com a qual ele a desempenha." + "\n" + "-Leonardo Cardielly";
            frases[61] = "Quem ama a sua pátria tem o dever de estar sempre com a espada na mão."+"\n"+ "-Juahrez Alves";
            frases[62] = "Não perguntes o que a tua pátria pode fazer por ti. Pergunta o que tu podes fazer por ela." + "\n" + "-John Kennedy";
            frases[63] = "A pátria é nos lugares onde a alma está acorrentada." + "\n" + "-Voltaire";
        }
        else if(idioma.idiomaEscolhido == "Ingles")
        {
            textGameOver.text = "Game Over";
        }

        textFrases.text = auxiliares.RandonFala(frases);
	}

	// Update is called once per frame
	void Update () {
        if (Input.anyKeyDown && mostrou)
        {
            saveLoad.gastarContiue = true;
            saveLoad.Load();
            dataSave.jogoAtual.Carregar();
        }
	}

    public void Mostrou()
    {
        mostrou = true;
    }

    void OnEnable()
    {
        fases.faseX.TrocarMusica(musicaGameOver);
        pause.pausaX.enabled = false;
    }
}
