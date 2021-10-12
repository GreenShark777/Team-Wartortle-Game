//si occupa di tenere conto delle variabili salvate quando viene caricato il gioco
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManag : MonoBehaviour
{
    //indica se vogliamo caricare i dati ad inizio scena o meno(PER DEBUG, COMMENTARE A GIOCO FINITO)
    [SerializeField]
    private bool loadData = true;

    public float savedMasterVolume = 0, //indica il valore del volume generale scelto dal giocatore l'ultima volta che è stato salvato
        savedMusicVolume = -10, //indica il valore del volume della musica scelto dal giocatore l'ultima volta che è stato salvato
        savedSfxVolume = 0; //indica il valore del volume degli effetti sonori scelto dal giocatore l'ultima volta che è stato salvato

    public int savedLanguage = 0, //indica la lingua che è stata messa l'ultima volta dal giocatore
        activeCheckpoint = -1, //indica l'ultimo checkpoint in cui il giocatore ha salvato
        maliciousness = 100; //indica quanta malizia aveva il giocatore l'ultima volta che ha salvato(indicherà di conseguenza anche la sua bontà)

    //indica l'ID dell'ultima stanza in cui il giocatore ha salvato la partita
    public int lastRoomID = 0;

    //riferimento a tutti gli script che usano l'interfaccia per l'aggiornamento dei dati nel GameManag
    public static List<IUpdateData> dataToSave = new List<IUpdateData>();


    private void Awake()
    {
        /*
        //carica i dati nel GameManag, se non stanno venendo cancellati
        if (!SaveSystem.isDeleting) { OnGameLoad(SaveSystem.LoadGame()); }
        //altrimenti, comunica che è stato caricato il gioco di nuovo, quindi il cancellamento dei dati è finito
        else { SaveSystem.isDeleting = false; OnGameLoad(SaveSystem.LoadGame()); }//SaveSystem.DataSave(this); OnGameLoad(SaveSystem.LoadGame()); }
        */
        //se i dati stavano venendo cancellati, indica che il cancellamento è finito in quanto si stanno per caricare i dati
        if (SaveSystem.isDeleting) { SaveSystem.isDeleting = false; }
        //carica i dati salvati
        if(loadData) OnGameLoad(SaveSystem.LoadGame());
        //dopo il caricamento dei dati controlla se gli array sono vuoti, nel qual caso li inizializza
        InizializeEmptyArrays();

    }

    private void Start()
    {
        //viene svuotata la lista di script che devono salvare i dati
        dataToSave.Clear();
        //viene creato un'array recipiente con tutti gli script che devono salvare dati(anche quelli inattivi)
        var recipient = FindObjectsOfType<MonoBehaviour>(true).OfType<IUpdateData>();
        //inizializza la lista di script che devono salvare i dati, aggiungendo tutti gli elementi nella lista recipiente
        foreach (IUpdateData elem in recipient) { dataToSave.Add(elem); }

        //DEBUG PER SAPERE GLI OGGETTI NELLA LISTA DI CHI DEVE AGGIORNARE DATI PRIMA DEL SALVATAGGIO
        //for (int i = 0; i < dataToSave.Count; i++ ) { Debug.LogError("Aggiunto: " + dataToSave[i]); }
        //Debug.Log("Oggetti nella lista di dati da salvare: " + dataToSave.Count);
    }
    /// <summary>
    /// Carica i dati salvati in SaveData
    /// </summary>
    /// <param name="sd"></param>
    public void OnGameLoad(SaveData sd)
    {
        //se esistono dati di salvataggio
        if (sd != null)
        {
            //aggiorna i dati in base ai dati salvati su SaveData
            savedMasterVolume = sd.savedMasterVolume;
            savedMusicVolume = sd.savedMusicVolume;
            savedSfxVolume = sd.savedSfxVolume;
            savedLanguage = sd.savedLanguage;
            activeCheckpoint = sd.activeCheckpoint;
            maliciousness = sd.maliciousness;
            lastRoomID = sd.lastRoomID;

            Debug.Log("Caricati dati salvati");
        } //altrimenti, tutti i dati vengono messi al loro valore originale, in quanto non si è trovato un file di salvataggio
        else { DataErased(); }

    }
    /// <summary>
    /// Riporta tutti i dati salvati al loro valore originale
    /// </summary>
    public void DataErased()
    {
        //tutte le variabili vengono riportate al loro valore originale
        savedMasterVolume = 0;
        savedMusicVolume = -10;
        savedSfxVolume = 0;
        savedLanguage = 0;
        activeCheckpoint = -1;
        maliciousness = 100;
        lastRoomID = 0;
        
        //tutti gli array vengono svuotati
        EmptyArrays();

        Debug.Log("Caricati dati iniziali");
    }
    /// <summary>
    /// Riporta tutti gli array al loro valore originale
    /// </summary>
    private void EmptyArrays()
    {
        //variabile di controllo che indicherà quanti cicli hanno fatto i cicli for sottostanti
        //int nControl = 0;
        //for (int i = 0; i < CountDellaLista; i++) { Svuotare Lista /*nControl++;*/ }
        //Debug.Log("Cicli fatti per i frammenti ottenuti: " + nControl); nControl = 0;
    }
    /// <summary>
    /// Controlla, per ogni array, se è nullo, nel qual caso inizializza l'array nel modo necessario
    /// </summary>
    private void InizializeEmptyArrays()
    {
        //se l'array delle stelle ottenute nei livelli è nullo, lo inizializza come array int di n livelli
        //if (lista == null) { lista = new tipoLista[NCONTENUTI]; /*Debug.Log("Lista");*/ }

    }
    /// <summary>
    /// Aggiorna i dati da salvare nel GameManag prima di salvare i dati
    /// </summary>
    private void UpdateDataBeforeSave()
    {
        //variabile di controllo, per vedere quante volte viene svolto il ciclo foreach
        //int n = 0;
        //viene richiamata la funzione dell'interfaccia per aggiornare i dati di ogni elemento nella lista
        foreach (IUpdateData elem in dataToSave) { elem.UpdateData(); /*n++;*/ }
        //Debug.Log("Aggiornati dati nel GameManag. Il numero di elementi aggiornati sono: " + n);
    }

    public void SaveDataAfterUpdate()
    {
        //aggiorna i dati se la scena non è un livello o, se lo è, se il livello è stato completato
        UpdateDataBeforeSave();
        //salva i dati ogni volta che si va da una scena all'altra, se i dati non stanno venendo cancellati
        if (!SaveSystem.isDeleting) { SaveSystem.DataSave(this); Debug.Log("Dati aggiornati e salvati"); }
        else Debug.LogError("Dati non aggiornati, perchè stanno venendo cancellati");

    }

    private void OnDestroy()
    {
        //salva i dati di gioco dopo aver ottenuto gli aggiornamenti dalla lista di script che devono aggiornare i dati
        //SaveDataAfterUpdate();

    }

}
