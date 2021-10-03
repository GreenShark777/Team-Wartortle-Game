using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesCollisionsManager : MonoBehaviour
{

    private Rigidbody2D enemyRb;

    private EnemiesHealth eh;


    private void Awake()
    {

        enemyRb = GetComponentInParent<Rigidbody2D>();

        eh = GetComponentInParent<EnemiesHealth>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //se si sta collidendo con un'arma, il nemico viene colpito da quest'arma
        if (collision.CompareTag("Weapon")) { StartCoroutine(GotHit(collision.GetComponent<WeaponStats>())); }
        //Debug.Log(collision.tag);
    }

    private IEnumerator GotHit(WeaponStats ws)
    {
        //Debug.Log("Enemy Hit: " + transform.parent.name);
        //il nemico subisce danni in base alla potenza dell'arma da cui � stato colpito
        eh.TakeDmg(ws.GetAttack());

        //FARE IN MODO CHE IL NEMICO NON SI MUOVA O ATTACCHI(O DISATTIVARE SUO SCRIPT DI LOGICA O CAMBIARE UNA VARIABILE CHE INDICA DI ESSERE STORDITI)

        //ogni forza che agisce sul rigidbody del nemico viene azzerata
        enemyRb.velocity = Vector2.zero;
        //calcola come spingere il nemico in base alla potenza di spinta dell'arma
        Vector2 calculatedPush = -(ws.transform.position - transform.position).normalized * ws.GetPushForce();
        //il nemico viene spinto via, in base alla potenza di spinta calcolata
        enemyRb.AddForce(calculatedPush);
        //Debug.Log("POtenza spinta calcolata: " + calculatedPush);
        //aspetta un po' di tempo, in base al tempo di stordimento inflitto dall'arma
        yield return new WaitForSeconds(ws.GetStunTime());
        //il nemico si riprende e torna a camminare come di consueto
        enemyRb.velocity = Vector2.zero;

        //FARE IN MODO CHE IL NEMICO SI MUOVA O ATTACCHI DI NUOVO

    }

}
