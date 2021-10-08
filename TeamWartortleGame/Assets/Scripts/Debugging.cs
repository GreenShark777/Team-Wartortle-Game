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


    // Start is called before the first frame update
    void Start()
    {
        //prende tutti i riferimenti agli script di cui deve fare debug
        pUIm = FindObjectOfType<PlayerUIManager>(true);
        gb = FindObjectOfType<GufoBehaviour>(true);
        lm = FindObjectOfType<LanguageManager>(true);
        //languageDropdownList = FindObjectOfType<Dropdown>(true);
        g = FindObjectOfType<GameManag>(true);

    }

    //LETTERE IN USO:
    //LanguageManager: T
    //PlayerUIManager: K, L
    //GufoBehaviour: G
    //Animator giocatore: U
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
            playerAnim.SetTrigger("Shooting");
        }

    }

}
