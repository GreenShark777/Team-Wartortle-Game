using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugging : MonoBehaviour
{
    //riferimento allo script che si occupa della UI del giocatore
    private PlayerUIManager pUIm;


    // Start is called before the first frame update
    void Start()
    {
        //prende tutti i riferimenti agli script di cui deve fare debug
        pUIm = FindObjectOfType<PlayerUIManager>();
        
    }

    //LETTERE IN USO:
    //PlayerUIManager: K, L
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

    }

}
