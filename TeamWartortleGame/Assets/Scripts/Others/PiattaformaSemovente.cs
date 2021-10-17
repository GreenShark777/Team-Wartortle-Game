//si occupa del movimento e controllo delle piattaforme semoventi
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiattaformaSemovente : MonoBehaviour
{
    //riferimento al buco su cui questa piattaforma sta volando
    [SerializeField]
    private HoleBehaviour hole = default;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //ottiene il riferimento al GroundCheck del collisore
        GroundCheck gc = collision.GetComponent<GroundCheck>();
        //se esiste il riferimento appena ottenuto...
        if (gc)
        {
            //...comunica al buco che il giocatore è sopra la piattaforma e non può cadere...
            hole.SetPlayerOnPlatform(true);
            //...e comunica al GroundCheck che è per terra
            gc.IsThereNoGround(false);
            //Debug.LogError("Giocatore nella piattaforma");
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //ottiene il riferimento al GroundCheck del collisore
        GroundCheck gc = collision.GetComponent<GroundCheck>();
        //se il riferimento al GroundCheck esiste...
        if (gc)
        {
            //...se il giocatore può cadere nel buco...
            if (hole.CouldPlayerFall())
            {
                //...al GroundCheck viene comunicato che non è per terra...
                gc.IsThereNoGround(true, hole);
                //...e se anche l'altro GroundCheck non è per terra...
                if (gc.GetOtherNoFloor())
                {
                    //...comunica al buco che il giocatore non è più nella piattaforma
                    hole.SetPlayerOnPlatform(false, collision);
                    //Debug.LogError("Giocatore non più nella piattaforma");
                }

            } //altrimenti, il giocatore non può cadere nel buco, quindi...
            else
            {
                //...se l'altro GroundCheck è per terra...
                if (!gc.GetOtherNoFloor())
                {
                    //...comunica al buco che il giocatore non è più nella piattaforma, ma non gli passa il collisore(impedendogli di far cadere il giocatore)
                    hole.SetPlayerOnPlatform(false);
                    //Debug.LogError("Giocatore non più nella piattaforma");
                }

            }

        }

        //if (hole.CouldPlayerFall())
        //{
        //    //se esiste il riferimento appena ottenuto, se anche l'altro groundcheck non è per terra,  comunica al buco che il giocatore può cadere
        //    if (gc)
        //    {

        //        gc.IsThereNoGround(true, hole);

        //        if (gc.GetOtherNoFloor())
        //        {

        //            hole.SetPlayerOnPlatform(false, collision);

        //            Debug.LogError("Giocatore non più nella piattaforma");

        //        }

        //    }
        //    Debug.Log("Player Could Fall");
        //}
        //else if(gc)
        //{

        //    if (!gc.GetOtherNoFloor())
        //    {

        //        hole.SetPlayerOnPlatform(false);

        //        Debug.LogError("Giocatore non più nella piattaforma");

        //    }

        //}

    }

}
