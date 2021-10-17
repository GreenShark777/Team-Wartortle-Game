using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    //Struct delle chest con due frame ciascuno
    [System.Serializable]
    public struct ChestStruct
    {
        //Sprite che conterranno i due frame della cassa(chiusa e aperta)
        public Sprite[] sprites;
    }

    //Array dei ChestStruct che conterrà tutti i tipi di cassa
    [SerializeField]
    private ChestStruct[] chestStruct;

    //Array di SpriteRenderer a cui assegnare gli sprite che usciranno
    [SerializeField]
    private SpriteRenderer[] currentSpriteRend;

    int index = 0;

    private PlayerHealth playerHealth;

    void OnEnable()
    {
        float randValue = Random.value;

        //Con la probabilità del 15% trovo la cassa Leggendaria(massima)
        if (randValue <= .15f)
        {
            index = 2;
        }
        //Con la probabilità del 25% si trova quella Epica(media)
        else if (randValue <= .40)
        {
            index = 1;
        }
        //Mentre con la probilità del 60% si trova quella comune
        else
        {
            index = 0;
        }

        for (int i = 0; i < chestStruct[index].sprites.Length; i++)
        {
            currentSpriteRend[i].sprite = chestStruct[index].sprites[i];
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Se il gameobject cassa chiusa è attivo, quindi la cassa non è stata già aparta, posso aprirla
            if (currentSpriteRend[0].gameObject.activeSelf)
            {
                playerHealth = collision.GetComponentInChildren<PlayerHealth>(true);
                //Se la cassa ha bisogno di una chiave e il giocatore le possiede
                if (index > 0 && GameManager.inst.key > 0)
                {
                    //Consumo una chiave
                    GameManager.inst.key--;
                    //Apro la cassa
                    OpenChest();
                }
                //Altrimenti se la cassa è comune posso aprirla in ogni caso senza l'utilizzo di chiavi
                else if (index == 0)
                {
                    //Apro la cassa
                    OpenChest();
                }
            }
        }
    }

    //private void OnDisable()
    //{
    //    for (int i = 0; i < 2; i++)
    //        currentSpriteRend[i].gameObject.SetActive(i == 0 ? true : false);
    //}

    private void OpenChest()
    {
        //Attivo il gameObject di cassa aperta e disattivo quello della cassa chiusa
        for (int i = 0; i < 2; i++)
            currentSpriteRend[i].gameObject.SetActive(i == 0 ? false : true);

        //Se la cassa è Leggendaria
        if (index == 2)
        {
            Debug.Log("Leggendaria");
            //Se la vita totale non è stata raggiunta
            if (!playerHealth.isTotalHealthReached())
            {
                //Posso far spawnare il frutto che aumenta il container della vita
                ObjectPooling.inst.SpawnObjectFromPool("Frutta", transform.position, Quaternion.identity);
            }

            //Spawno anime randomicamente
            for(int i=0; i<Random.Range(5,8); i++)
            {
                //Spawno anime dall'Object pooling
                ObjectPooling.inst.SpawnObjectFromPool("Souls", transform.position, Quaternion.identity);
            }

            //Spawno chiavi randomicamente
            for (int i = 0; i < Random.Range(0, 2); i++)
            {
                //Spawno chiavi dall'Object pooling
                ObjectPooling.inst.SpawnObjectFromPool("Keys", transform.position, Quaternion.identity);
            }

            //Spawno cuori randomicamente
            for (int i = 0; i < Random.Range(1, 2); i++)
            {
                //Spawno cuori dall'Object pooling
                ObjectPooling.inst.SpawnObjectFromPool("Hearts", transform.position, Quaternion.identity);
            }
        }
        //Altrimenti se è la cassa epica
        else if (index == 1)
        {
            Debug.Log("Epica");
            //Spawno anime randomicamente
            for (int i = 0; i < Random.Range(3, 5); i++)
            {
                //Spawno anime dall'Object pooling
                ObjectPooling.inst.SpawnObjectFromPool("Souls", transform.position, Quaternion.identity);
            }

            //Spawno chiavi randomicamente
            for (int i = 0; i < Random.Range(0, 1); i++)
            {
                //Spawno chiavi dall'Object pooling
                ObjectPooling.inst.SpawnObjectFromPool("Keys", transform.position, Quaternion.identity);
            }

            //Spawno un cuore
            for (int i = 0; i < 1; i++)
            {
                //Spawno cuori dall'Object pooling
                ObjectPooling.inst.SpawnObjectFromPool("Hearts", transform.position, Quaternion.identity);
            }
        }
        //Altrimenti se è la cassa comune
        else
        {
            //Spawno anime randomicamente
            for (int i = 0; i < Random.Range(1, 3); i++)
            {
                //Spawno anime dall'Object pooling
                GameObject temp = ObjectPooling.inst.SpawnObjectFromPool("Souls", transform.position, Quaternion.identity);

            }

            //Spawno chiavi randomicamente
            for (int i = 0; i < Random.Range(1, 2); i++)
            {
                //Spawno chiavi dall'Object pooling
                ObjectPooling.inst.SpawnObjectFromPool("Keys", transform.position, Quaternion.identity);
            }
        }
    }
}
