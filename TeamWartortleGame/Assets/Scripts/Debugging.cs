using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugging : MonoBehaviour
{
    //riferimento allo script che si occupa della UI del giocatore
    private PlayerUIManager pUIm;
    //riferimento allo script di comportamento di un gufo nella scena
    private GufoBehaviour gb;


    // Start is called before the first frame update
    void Start()
    {
        //prende tutti i riferimenti agli script di cui deve fare debug
        pUIm = FindObjectOfType<PlayerUIManager>();
        gb = FindObjectOfType<GufoBehaviour>();

    }

    //LETTERE IN USO:
    //PlayerUIManager: K, L
    //GufoBehaviour: G
    void Update()
    {
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

    }

}
