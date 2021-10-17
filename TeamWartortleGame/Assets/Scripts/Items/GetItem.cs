using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItem : MonoBehaviour
{
    //Enum degli oggetti collezionabili,
    [System.Serializable]
    private enum EItems { 
        Soul,
        Key
    }
    //Valore numerico dell'anima o chiave che viene presa, potremmo mettere anime che danno più di 1 quantità
    [SerializeField]
    private int value = 1;

    //Istanza dell'enum che conterrà da inspector l'item assegnato
    [SerializeField]
    private EItems itemType;

    //Animator per controllare se l'animazione è finita e posso prendere l'oggetto
    [SerializeField]
    private Animator itemAn;

    private void OnTriggerEnter2D(Collider2D col)
    {
        //Se collido con il player, allora posso ottenere questo oggetto
        if (col.CompareTag("Player") && itemAn.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !itemAn.IsInTransition(0))
        {
            //Controllo che item è, se è un'anima gli aggiungo il valore a quello corrente nel gameManager
            if (itemType == EItems.Soul)
            {
                //Sommo al valore complessivo
                GameManager.inst.soul += value;
                //Controllo se sono arrivato a 30 anime e in caso droppo un cuore
                if (GameManager.inst.soul >= GameManager.inst.soulValue)
                {
                    //Soul value di aumenta a 30 ogni volta
                    GameManager.inst.soulValue += 30;
                    //Faccio apparire un cuore
                    ObjectPooling.inst.SpawnObjectFromPool("Hearts", transform.position, Quaternion.identity);
                }
                //Lo ri aggiungo alla sua pool
                ObjectPooling.inst.ReAddObjectToPool("Souls", gameObject);
            }
            //altrimenti lo faccio per la chiave
            else if (itemType == EItems.Key)
            {
                GameManager.inst.key += value;
                ObjectPooling.inst.ReAddObjectToPool("Keys", gameObject);
            }

            //Chiamo il metodo di GameManger che aggiorna l'HUD degli item
            GameManager.inst.UpdateItemHUG();
        }
    }

}
