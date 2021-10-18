using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scarpa : MonoBehaviour
{
    [SerializeField]
    private Animator scarpaAn;
    private void OnTriggerEnter2D(Collider2D col)
    {
        //Se collido con il player e l'animazione di push è finita
        if (col.CompareTag("Player") && scarpaAn.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !scarpaAn.IsInTransition(0))
        {
            //Attivo il Power Up dal GameManager
            GameManager.inst.SpeedPowerUp();
            //Lo riaggiungo alla pool
            ObjectPooling.inst.ReAddObjectToPool("Scarpe", gameObject);
        }
    }
}