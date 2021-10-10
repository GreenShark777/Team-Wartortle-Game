//Si occupa delle collisioni tra il nemico e qualsiasi altro oggetto
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesCollisionsManager : MonoBehaviour
{
    //riferimento al Rigidbody2D di questo nemico
    [SerializeField]
    private Rigidbody2D enemyRb = default;
    //riferimento allo script della vita di questo nemico
    [SerializeField]
    private EnemiesHealth eh = default;
    //riferimento al(possibile) script di comportamento del nemico gufo
    [SerializeField]
    private GufoBehaviour gb = default;
    //riferimento al collider di questo nemico
    private Collider2D enemyColl;
    /*
    //riferimento al WeaponContainer del giocatore
    [SerializeField]
    private WeaponsContainer wc = default;
    //riferimento statico al WeaponContainer del giocatore
    private static WeaponsContainer staticWc = default;
    */


    private void Awake()
    {
        //ottiene il riferimento al Rigidbody2D di questo nemico, se non esiste gi�
        if(!enemyRb) enemyRb = GetComponentInParent<Rigidbody2D>();
        //ottiene il riferimento allo script della vita di questo nemico, se non esiste gi�
        if (!eh) eh = GetComponentInParent<EnemiesHealth>();
        //ottiene il riferimento al collider di questo nemico
        enemyColl = GetComponent<Collider2D>();

        //se non esiste ancora un riferimento statico al WeaponContainer del giocatore, lo ottiene dal riferimento non statico
        //if (staticWc == null && wc != null) { staticWc = wc; }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //se il nemico non � ancora stato sconfitto, controlla i tag dell'oggetto con cui collide
        if (!eh.IsEnemyDefeated())
        {
            //se si sta collidendo con un'arma...
            if (collision.CompareTag("Weapon"))
            {
                //...ottiene il riferimento alle info dell'arma...
                WeaponStats ws = collision.GetComponent<WeaponStats>();
                //...il nemico viene colpito da quest'arma, se pu� essere colpito
                if (CanBeHit(ws)) { StartCoroutine(GotHit(ws)); }
            
            }

        }
        //Debug.Log(collision.tag);
    }

    private bool CanBeHit(WeaponStats ws)
    {
        //variabile che indicher� se il nemico potr� essere colpito o meno
        bool canBeHit = true;
        //se questo nemico � un gufo...
        if(gb != null)
        {
            //...se il suo collider non � solido, potr� essere colpito solo se l'arma che l'ha colpito � un proiettile
            if (enemyColl.isTrigger) { canBeHit = ws.GetWeaponType() == WeaponStats.WeaponType.Bullet; }

        }
        //infine, ritorna se il nemico pu� essere colpito o meno
        return canBeHit;

    }

    private IEnumerator GotHit(WeaponStats ws)
    {
        //Debug.Log("Enemy Hit: " + transform.parent.name);
        //il nemico subisce danni in base alla potenza dell'arma da cui � stato colpito
        eh.Damage(ws.GetAttack());

        //FARE IN MODO CHE IL NEMICO NON SI MUOVA O ATTACCHI(O DISATTIVARE SUO SCRIPT DI LOGICA O CAMBIARE UNA VARIABILE CHE INDICA DI ESSERE STORDITI)
        GetStunned(true);

        //ogni forza che agisce sul rigidbody del nemico viene azzerata
        enemyRb.velocity = Vector2.zero;
        //calcola come spingere il nemico in base alla potenza di spinta dell'arma
        Vector2 calculatedPush = -(ws.transform.position - transform.position).normalized * ws.GetPushForce() * enemyRb.mass;
        //il nemico viene spinto via, in base alla potenza di spinta calcolata
        enemyRb.AddForce(calculatedPush);
        //Debug.Log("POtenza spinta calcolata: " + calculatedPush);
        //aspetta un po' di tempo, in base al tempo di stordimento inflitto dall'arma
        yield return new WaitForSeconds(ws.GetStunTime());
        //il nemico si riprende e torna a camminare come di consueto
        enemyRb.velocity = Vector2.zero;
        //fare in modo che il nemico si comporti come di consueto, se la sua vita non � a zero
        if(!eh.IsEnemyDefeated()) GetStunned(false);

    }

    private void GetStunned(bool gotStunned)
    {
        //se questo nemico � un gufo, lo stordisce se deve essere stordito(o se � stato sconfitto)
        if (gb) { gb.IsStunned(gotStunned); }

    }

}
