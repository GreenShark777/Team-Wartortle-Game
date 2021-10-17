using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitHeartContainer : MonoBehaviour
{    
    //Animator per controllare se l'animazione è finita e posso prendere l'oggetto
    [SerializeField]
    private Animator fruitAn;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Controllo se sto collidendo con il player
        if (collision.CompareTag("Player") && fruitAn.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !fruitAn.IsInTransition(0))
        {
            //Ottengo il riferimento alla vita del player anche se è disattivato
            PlayerHealth playerHealth = collision.GetComponentInChildren<PlayerHealth>(true);
            //Se non ho la vita totale al massimo aggiungo un nuovo contenitore
            if (!playerHealth.isTotalHealthReached())
            {
                //Aggiungo un contenitore e riporto il GameObject alla sua pool
                playerHealth.GetNewContainer();
                //Riaggiungo alla pool
                ObjectPooling.inst.ReAddObjectToPool("Frutta", gameObject);
                //Attivo un messaggio, chiamo metodo del cambio statistica ma utilizzo soltanto la parte del messaggio che si attiva
                GameManager.inst.ChangeStats(0, 0, false, false, "HP up!");
            }
        }
    }
}
