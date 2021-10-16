//Si occupa di ciò che deve accadere quando il giocatore cade in un buco
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleBehaviour : MonoBehaviour
{
    //riferimento alla posizione in cui il giocatore deve respawnare dopo essere caduto
    [SerializeField]
    private Transform respawnPoint = default;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        


    }

}