using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    //riferimento all'audio source della musica di sottofondo del gioco
    private static AudioSource backgroundMusic;
    //riferimenti alle varie musiche di gioco
    [SerializeField]
    private AudioClip mainBgMusic = default,
        battleMusic = default,
        bossMusic = default;

    //array di riferimenti alle clip audio del gioco
    private static AudioClip[] allBgMusics = new AudioClip[3];
    //indica la musica da mettere al posto di quella attuale
    private static AudioClip musicToPlay;
    //indica che bisogna cambiare musica di gioco
    private static bool hasToChange = false;
    //indica quanto velocemente si dissolve la musica di gioco quando si deve cambiare
    [SerializeField]
    private float dissolveRate = 0.1f;


    private void Start()
    {
        //inizializza al loro valore originale tutte le variabili statiche
        hasToChange = false;
        musicToPlay = null;
        //ottiene il riferimento all'audio source della musica di sottofondo del gioco
        backgroundMusic = GetComponent<AudioSource>();
        //crea un'array ordinato con tutte le musiche ottenute da editor
        allBgMusics[0] = mainBgMusic;
        allBgMusics[1] = battleMusic;
        allBgMusics[2] = bossMusic;
        
    }

    private void Update()
    {
        //se bisogna cambiare musica, da un effetto dissolvenza e cambia alla nuova musica
        if (hasToChange) { DissolveIntoNewMusic(); }
        //altrimenti, se la musica è a basso volume, vuol dire che si è appena finito di cambiare musica e bisogna aumentare il volume
        else if (backgroundMusic.volume < 0.9f) { backgroundMusic.volume += dissolveRate * Time.deltaTime; }

    }
    /// <summary>
    /// Permette globalmente di cambiare la musica di gioco in sottofondo
    /// </summary>
    /// <param name="musicIndex"></param>
    public static void ChangeBackgroundMusic(int musicIndex)
    {
        //ottiene il riferimento alla canzone da mettere a fine dissolvenza
        if (musicIndex < allBgMusics.Length) { musicToPlay = allBgMusics[musicIndex]; }
        else { Debug.LogError("Inserito indice di musica errato: " + musicIndex); }
        //comunica che bisogna cambiare musica, se la nuova canzone richiesta non è quella attualmente in ascolto
        if (backgroundMusic.clip != musicToPlay) { hasToChange = true; }

    }

    private void DissolveIntoNewMusic()
    {
        //se il volume della musica è ancora troppo alto, lo abbassa
        if (backgroundMusic.volume > 0.1f) { backgroundMusic.volume -= dissolveRate * Time.deltaTime; }
        //altrimenti...
        else
        {
            //...cambia musica...
            backgroundMusic.clip = musicToPlay;
            //...la fa ripartire...
            backgroundMusic.Play();
            //...e comunica che non bisogna più cambiare musica
            hasToChange = false;

        }

    }

}
