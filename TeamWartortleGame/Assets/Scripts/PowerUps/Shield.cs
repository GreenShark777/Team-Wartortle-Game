using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    //Animator dello scudo
    [SerializeField]
    private Animator shieldAn;
    //Scudo del player
    private GameObject playerShield;
    private void Start()
    {
        //Ottengo lo scudo dal player(il primo figlio)
        playerShield = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && shieldAn.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !shieldAn.IsInTransition(0) && !playerShield.activeSelf)
        {
            //Attivo lo scudo
            playerShield.SetActive(true);
            //Lo ri aggiungo alla sua pool
            ObjectPooling.inst.ReAddObjectToPool("Shields", gameObject);
        }
    }
}
