//Si occupa dei checkpoint
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    //riferimento alla UI di salvataggio
    [SerializeField]
    private GameObject saveUI = default;
    //riferimento al Transform del giocatore
    [SerializeField]
    private Transform player = default;
    //riferimento alla posizione in cui bisogna mettere il giocatore per un respawn
    private Vector2 respawnPoint = default;
    //riferimento al GameManag di scena
    [SerializeField]
    private GameManag g = default;
    //riferimento al particellare da attivare ad avvenuto salvataggio
    private ParticleSystem psSave;
    //identificatore per ogni checkpoint
    [SerializeField]
    private int IDCheckpoint = default;
    //indica se il giocatore può salvare o meno
    private bool canSave = false;


    // Start is called before the first frame update
    private void Start()
    {
        //se l'ultimo checkpoint salvato nel GameManag è questo checkpoint, respawna il giocatore
        if (g.activeCheckpoint == IDCheckpoint) { RespawnPlayer(); }
        //ottiene il riferimento al particellare di salvataggio
        psSave = transform.GetChild(0).GetComponent<ParticleSystem>();

    }

    private void Update()
    {
        //se il giocatore può salvare in questo checkpoint...
        if (canSave)
        {
            //...se preme E, avviene il salvataggio dei dati
            if (Input.GetKeyDown(KeyCode.E)) { Save(); }

        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //se l'oggetto entrato nel collider è il giocatore, gli permette di salvare
        if (collision.CompareTag("Player")) { CanPlayerSave(true); }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //se l'oggetto uscito dal collider è il giocatore, non gli permette più di salvare
        if (collision.CompareTag("Player")) { CanPlayerSave(false); }

    }

    private void CanPlayerSave(bool canPlayerSave)
    {
        //permette o meno al giocatore di salvare in base al parametro ricevuto
        canSave = canPlayerSave;
        //attiva o disattiva la UI di salvataggio se il giocatore può salvare
        saveUI.SetActive(canSave);
        //Debug.Log("Can player save: " + canSave);
    }

    private void Save()
    {
        //aggiorna, nel GameManag, il checkpoint in cui è stato salvato il gioco
        g.activeCheckpoint = IDCheckpoint;
        //salva i dati nel GameManag
        g.SaveDataAfterUpdate();
        //non permette al giocatore di salvare fino a quando non esce dal collider del checkpoint
        CanPlayerSave(false);
        //fa partire il particellare di salvataggio del checkpoint
        psSave.Play();
        //fa partire l'audio di avvenuto salvataggio;
        MusicManager.PlaySound("Undertale Save SFX");
        //Debug.Log("Checkpoint in cui si è salvato: " + g.activeCheckpoint);
    }

    private void RespawnPlayer()
    {
        //ottiene il riferimento alla posizione in cui bisogna mettere il giocatore per un respawn
        respawnPoint = transform./*GetChild(0).*/position;
        //il giocatore verrà messo nella posizione di respawn di questo checkpoint
        player.position = respawnPoint;
        //Debug.Log("PlayerPos: " + player.position + ", respawnPos: " + respawnPoint);
    }


    //DEBUG, DA CANCELLARE---------------------------------------------------------------------------------------------------------------------------
    public int GetID() { return IDCheckpoint; }

}
