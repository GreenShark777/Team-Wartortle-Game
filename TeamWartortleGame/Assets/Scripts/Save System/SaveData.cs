//Si occupa di salvare i dati di gioco

[System.Serializable]
public class SaveData {

    public float savedMasterVolume, //indica il valore del volume generale scelto dal giocatore l'ultima volta che è stato salvato
        savedMusicVolume, //indica il valore del volume della musica scelto dal giocatore l'ultima volta che è stato salvato
        savedSfxVolume; //indica il valore del volume degli effetti sonori scelto dal giocatore l'ultima volta che è stato salvato

    public int savedLanguage = 0, //indica la lingua che è stata messa l'ultima volta dal giocatore
        activeCheckpoint = 0, //indica l'ultimo checkpoint in cui il giocatore ha salvato
        maliciousness = 100; //indica quanta malizia aveva il giocatore l'ultima volta che ha salvato(indicherà di conseguenza anche la sua bontà)


    public SaveData(GameManag g)
    {
        //aggiorna i dati da salvare in base ai valori dentro GameManag
        savedMasterVolume = g.savedMasterVolume;
        savedMusicVolume = g.savedMusicVolume;
        savedSfxVolume = g.savedSfxVolume;
        savedLanguage = g.savedLanguage;
        activeCheckpoint = g.activeCheckpoint;
        maliciousness = g.maliciousness;

    }

}
