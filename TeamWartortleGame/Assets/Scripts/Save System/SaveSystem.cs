//si occupa di salvare e caricare i dati di gioco salvati
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem {

    //indica ad altri script se si stanno cancellando i dati
    public static bool isDeleting;

    /// <summary>
    /// Salva i dati di gioco
    /// </summary>
    /// <param name="g"></param>
	public static void DataSave(GameManag g)
    {
        //i dati vengono salvati se non stanno venendo cancellati
        if (!isDeleting)
        {
            //formattatore binario per criptare e decriptare i dati di salvataggio
            BinaryFormatter bf = new BinaryFormatter();
            //stringa che indica il percorso in cui creare il salvataggio
            string path = Application.persistentDataPath + "/saveData.ass";
            //flusso di dati per creare il salvataggio
            FileStream fs = new FileStream(path, FileMode.Create);
            //indica da che parte prendere i dati da salvare
            SaveData sd = new SaveData(g);
            //formatta i dati salvati
            bf.Serialize(fs, sd);
            //chiude il flusso di dati
            fs.Close();
            //Debug.Log("Dati salvati");
        }
        //else { Debug.Log("DATI NON SALVATI, IN QUANTO STANNO VENENDO CANCELLATI"); }

    }
    /// <summary>
    /// Carica i dati salvati
    /// </summary>
    /// <returns></returns>
    public static SaveData LoadGame() {
        //stringa che indica il percorso in cui cercare il salvataggio da caricare
        string path = Application.persistentDataPath + "/saveData.ass";
        //se il file esiste lo carica
        if (File.Exists(path))
        {
            //formattatore binario per criptare e decriptare i dati di salvataggio
            BinaryFormatter bf = new BinaryFormatter();
            //flusso di dati per aprire il file che contiene i dati di salvataggio
            FileStream fs = new FileStream(path, FileMode.Open);
            //ottiene i dati di salvataggio e li decripta
            SaveData sd = bf.Deserialize(fs) as SaveData;
            //chiude il flusso di dati
            fs.Close();
            //ritorna i dati di salvataggio
            return sd;

        }
        else //altrimenti, il file non esiste, quindi...
        {
            //...comunica l'errore...
            //Debug.LogError("Salvataggio non trovato in " + path);
            //...e non ritorna niente
            return null;

        }

    }
    /// <summary>
    /// Cancella i dati salvati
    /// </summary>
    public static void ClearData(GameManag g)
    {
        //comunica agli altri script che si stanno cancellando i dati di gioco
        isDeleting = true;
        //stringa che indica il percorso in cui cercare il salvataggio da cancellare
        string path = Application.persistentDataPath + "/saveData.ass";
        //cancella i dati di salvataggio se esistono, mantenendo il numero massimo di monete
        //if (File.Exists(path)) { File.Delete(path); g.MaintainMaxCoins(); g.DataErased(); Debug.Log("CANCELLATI DATI DI SALVATAGGIO"); }


        //invece di cancellare i dati li sovrascrive
        if (File.Exists(path))
        {
            //cancella i dati nel GameManag(tranne le monete massime nei livelli)
            g.DataErased();
            //comunica temporaneamente che non si stanno cancellando i dati(in questo modo può salvare i nuovi dati)
            isDeleting = false;
            //salva i nuovi dati(facendo così finta che sono stati cancellati i dati)
            DataSave(g);
            //comunica che si stanno cancellando i dati(in modo che altri script non provino a cancellare, salvare o caricare i dati)
            isDeleting = true;
            
        }
        //else { Debug.LogError("File non esiste"); }
        /*
        //comunica che i dati sono stati cancellati
        isDeleting = false;
        //salva nuovamente i dati, in modo da creare un nuovo SaveData
        DataSave(g);
        //infine, carica i dati nel GameManag
        g.OnGameLoad(g.sd);

        g.lastCompletedLevel = 0;

        DataSave(g);

        isDeleting = true;
        */
    }

}
