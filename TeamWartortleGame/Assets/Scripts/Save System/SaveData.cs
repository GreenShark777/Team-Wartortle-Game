//Si occupa di salvare i dati di gioco

[System.Serializable]
public class SaveData {

    //costante che indica il numero di stanze presenti nel gioco
    private const int N_ROOMS = 50;

    public float savedMasterVolume, //indica il valore del volume generale scelto dal giocatore l'ultima volta che è stato salvato
        savedMusicVolume, //indica il valore del volume della musica scelto dal giocatore l'ultima volta che è stato salvato
        savedSfxVolume; //indica il valore del volume degli effetti sonori scelto dal giocatore l'ultima volta che è stato salvato

    public int savedLanguage = 0, //indica la lingua che è stata messa l'ultima volta dal giocatore
        activeCheckpoint = 0, //indica l'ultimo checkpoint in cui il giocatore ha salvato
        maliciousness = 100; //indica quanta malizia aveva il giocatore l'ultima volta che ha salvato(indicherà di conseguenza anche la sua bontà)

    //indica l'ID dell'ultima stanza in cui il giocatore ha salvato la partita
    public int lastRoomID = 0;
    //array che indica per ogni stanza se è stata visitata dal giocatore o meno(l'indice dell'array indica l'ID della stanza a cui si riferisce)
    public bool[] seenRooms = new bool[N_ROOMS];
    //array che indica per ogni stanza se tutti i nemici sono stati sconfitti o meno(l'indice dell'array indica l'ID della stanza a cui si riferisce)
    public bool[] defeatedAllEnemies = new bool[N_ROOMS];


    public SaveData(GameManag g)
    {
        //aggiorna i dati da salvare in base ai valori dentro GameManag
        savedMasterVolume = g.savedMasterVolume;
        savedMusicVolume = g.savedMusicVolume;
        savedSfxVolume = g.savedSfxVolume;
        savedLanguage = g.savedLanguage;
        activeCheckpoint = g.activeCheckpoint;
        maliciousness = g.maliciousness;
        lastRoomID = g.lastRoomID;
        seenRooms = g.seenRooms;
        defeatedAllEnemies = g.defeatedAllEnemies;

    }

}
