using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Debugging : MonoBehaviour
{
    //riferimento allo script che si occupa della UI del giocatore
    private PlayerUIManager pUIm;
    //riferimento allo script di comportamento di un gufo nella scena
    private GufoBehaviour gb;
    //riferimento allo script che si occupa della lingua di gioco
    private LanguageManager lm;
    //riferimento alla lista dropdown per la lingua
    public Dropdown languageDropdownList;
    //riferimento al GameManag di scena
    private GameManag g;
    //Riferimento per test di animazione delle armi
    [SerializeField]
    private Animator playerAnim;
    //lista di riferimenti di tutti i checkpoint nella scena
    public Checkpoints[] allCheckpoints;
    //indica l'indice della musica di sottofondo da far mettere al MusicManager
    private int nMusic = 0;

    [SerializeField]
    PlayerHealth playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        //prende tutti i riferimenti agli script di cui deve fare debug
        pUIm = FindObjectOfType<PlayerUIManager>(true);
        gb = FindObjectOfType<GufoBehaviour>(true);
        lm = FindObjectOfType<LanguageManager>(true);
        //languageDropdownList = FindObjectOfType<Dropdown>(true);
        g = FindObjectOfType<GameManag>(true);
        allCheckpoints = FindObjectsOfType<Checkpoints>();

        //controlla se dei checkpoint hanno lo stesso ID
        if (allCheckpoints.Length > 0 && g)
        {

            for (int i = 0; i < allCheckpoints.Length; i++)
            {

                for (int j = i; j < allCheckpoints.Length; j++)
                {

                    if (allCheckpoints[i] != allCheckpoints[j] && allCheckpoints[i].GetID() == allCheckpoints[j].GetID())
                    {
                        Debug.LogError("I checkpoint " + allCheckpoints[i].transform.parent + " e " + allCheckpoints[j].transform.parent
                        + " hanno lo stesso ID: " + allCheckpoints[i].GetID());
                    }

                }

            }

        }
        //Fine controllo checkpoint
    }

    //LETTERE IN USO:
    //LanguageManager: T
    //PlayerUIManager: K, L
    //GufoBehaviour: G
    //Animator giocatore: U
    //MusicManager: M
    void Update()
    {
        //se esistono i riferimenti al GameManag e al LanguageManager...
        if (lm && g && languageDropdownList)
        {
            //...premendo il tasto T...
            if (Input.GetKeyDown(KeyCode.T))
            {
                //...cambia alla lingua successiva...
                g.savedLanguage++;
                //...se non esiste una lingua successiva, ritorna alla prima...
                if (g.savedLanguage > 1) { g.savedLanguage = 0; }
                //se esiste il riferimento alla dropdown list per il cambio della lingua, gli da il valore appena modificato al GameManag
                if (languageDropdownList != null) { languageDropdownList.value = g.savedLanguage; }
                //...infine, cambia la lingua di tutti i testi
                lm.LanguageChange();

            }

        }
        //se esiste il riferimento allo script che si occupa della UI del giocatore, ne fa il debug
        if (pUIm)
        {
            //AUMENTA MALIZIA PREMENDO K
            if (Input.GetKey(KeyCode.K)) { pUIm.ChangeMaliciousnessBar(pUIm.GetMaliciousness() + 1); }
            //DIMINUISCE MALIZIA PREMENDO L
            if (Input.GetKey(KeyCode.L)) { pUIm.ChangeMaliciousnessBar(pUIm.GetMaliciousness() - 1); }

        }
        //se esiste il riferimento allo script di comportamento di un gufo, ne fa il debug
        if (gb)
        {
            //INIZIA FASE D'ATTACCO GUFO
            if (Input.GetKeyDown(KeyCode.G)) { gb.PlayerSpotted(); }

        } //altrimenti, ottiene un nuovo riferimento ad uno script di un gufo
        else { if (Input.GetKeyDown(KeyCode.G)) { gb = FindObjectOfType<GufoBehaviour>(); } }

        //Sparo animazione
        if (Input.GetKeyDown(KeyCode.U))
        {
            playerHealth.Damage(1);
        } else if (Input.GetKeyDown(KeyCode.P)) playerHealth.Damage(-1);
        //CAMBIA MUSICA ANDANDO A QUELLA ALL'INDICE SUCCESSIVO(CONTROLLANDO CHE NON SI VADA FUORI DAI LIMITI DELL'ARRAY DELLE MUSICHE)
        if (Input.GetKeyDown(KeyCode.M)) { nMusic++; if (nMusic > 2) { nMusic = 0; } MusicManager.ChangeBackgroundMusic(nMusic); }

    }

}
