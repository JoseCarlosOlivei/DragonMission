using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class saveLoad : MonoBehaviour {
    public static dataSave[] savedGames = new dataSave[9];
    public infSlots[] slotsInf = new infSlots[9];
    public static int idSlot;
    public Color corEscolhido;
    private AudioSource esteAudio;
    public AudioClip somBotao,somCompleto,somErro;
    public static bool gastarContiue = false;
    public GameObject load;

    void Awake()
    {
        Load();
        AudioListener.volume = dataSave.GetVolume();
        AtulizarSlots();
        EscolhidoPintar();
        esteAudio = GetComponent<AudioSource>();
        auxiliares.botao = somBotao;
        auxiliares.completoClick = somCompleto;
        auxiliares.erroSom = somErro;
        EscolherSlot(PlayerPrefs.GetInt("ultimoId"));
    }

    public void NewGame()
    {
        if(savedGames[idSlot] == null)
        {
           savedGames[idSlot] = new dataSave();
           SceneManager.LoadSceneAsync("dificuldade");
        }
    }

    public void Opcoes()
    {
        auxiliares.BotaoSom(esteAudio);
        auxiliares.idCena = SceneManager.GetActiveScene().buildIndex;
        load.SetActive(true);
        SceneManager.LoadSceneAsync("opcoes");
    }

    private void EscolhidoPintar()
    {
        slotsInf[idSlot].imagens.color = corEscolhido;
    }

    private void ResetarCorEscolhido()
    {
        slotsInf[idSlot].imagens.color = Color.white;
    }

    private void AtulizarSlots()
    {
        for (int i = 0; i < slotsInf.Length; i++)
        {
            if (savedGames[i] != null)
            {
                slotsInf[i].informacoes.text = savedGames[i].Informacoes();
            }else
            {
                slotsInf[i].informacoes.text = "";
            }
        }
    }

    public void BotaoJogar()
    {
        PlayerPrefs.SetInt("ultimoId", idSlot);
        if (savedGames[idSlot] == null)
        {
            esteAudio.PlayOneShot(somBotao);
            NewGame();
            slotsInf[idSlot].informacoes.text = savedGames[idSlot].Informacoes();
        }else
        {
            esteAudio.PlayOneShot(somCompleto);
            savedGames[idSlot].Carregar();
        }
        load.SetActive(true);
    }

    public void Deletar()
    {
        savedGames[idSlot] = null;
        slotsInf[idSlot].informacoes.text = "";
        Save();
        esteAudio.PlayOneShot(somBotao);
    }

    public void EscolherSlot(int id)
    {
        esteAudio.PlayOneShot(somBotao);
        ResetarCorEscolhido();
        idSlot = id;
        EscolhidoPintar();
    }

    //it's static so we can call it from anywhere
    public static void Save()
    {
        //Debug.Log(savedGames.Count);
        BinaryFormatter bf = new BinaryFormatter();
        //Application.persistentDataPath is a string, so if you wanted you can put that into debug.log if you want to know where save games are located
        FileStream file = File.Create(Application.persistentDataPath + "/savedGames.gd"); //you can call it anything you want
        bf.Serialize(file, saveLoad.savedGames);
        file.Close();
    }

    public static void Load()
    {
        //File.Delete(Application.persistentDataPath + "/savedGames.gd");
        if (File.Exists(Application.persistentDataPath + "/savedGames.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedGames.gd", FileMode.Open);
            saveLoad.savedGames = (dataSave[])bf.Deserialize(file);
            file.Close();
            AudioListener.volume = dataSave.GetVolume();
            if (gastarContiue)
            {
                savedGames[idSlot].continues -= 1;
                dataSave.jogoAtual = savedGames[idSlot];
                Save();
                gastarContiue = false;
            }
        }
    }
}
