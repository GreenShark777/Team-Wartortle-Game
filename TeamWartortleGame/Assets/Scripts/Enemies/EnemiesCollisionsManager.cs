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

    //Booleana per capire se lo script si trova in un boss o no
    [SerializeField]
    bool isBoss = false, trap = false;

    [SerializeField]
    private Transform player;

    //Int del danno che infligge questo nemico, normalmente togliera metà cuore(1) mentre il danno per il cuore intero è 2
    [SerializeField]
    private int dmg = 1;

    private static Transform staticPlayer;
    /*
    //riferimento al WeaponContainer del giocatore
    [SerializeField]
    private WeaponsContainer wc = default;
    //riferimento statico al WeaponContainer del giocatore
    private static WeaponsContainer staticWc = default;
    */


    private void Awake()
    {
        //ottiene il riferimento al Rigidbody2D di questo nemico, se non esiste già
        if(!enemyRb) enemyRb = GetComponentInParent<Rigidbody2D>();
        //ottiene il riferimento allo script della vita di questo nemico, se non esiste già
        if (!eh) eh = GetComponentInParent<EnemiesHealth>();
        //ottiene il riferimento al collider di questo nemico
        enemyColl = GetComponent<Collider2D>();

        if (player != null && staticPlayer == null) { staticPlayer = player; }
        //se non esiste ancora un riferimento statico al WeaponContainer del giocatore, lo ottiene dal riferimento non statico
        //if (staticWc == null && wc != null) { staticWc = wc; }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        //se il nemico non è ancora stato sconfitto, controlla i tag dell'oggetto con cui collide
        if (!trap)
        {
            if (!eh.IsEnemyDefeated())
            {
                //se si sta collidendo con un'arma...
                if (collision.CompareTag("Weapon"))
                {
                    //...ottiene il riferimento alle info dell'arma...
                    WeaponStats ws = collision.GetComponent<WeaponStats>();
                    //...il nemico viene colpito da quest'arma, se può essere colpito
                    if (CanBeHit(ws)) { StartCoroutine(GotHit(ws)); }

                }

            }
        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        DamageTo(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //Se non è una trappola vuole dire che devo controllare se il nemico non è stato sconfitto prima
        if (!trap)
        {
            //Se il nemico non è sconfitto
            if (!eh.IsEnemyDefeated())
                DamageTo(collision);
        }
        //Altrimenti se è una trappola faccio continuamente danno
        else
            DamageTo(collision);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Se non è una trappola vuole dire che devo controllare se il nemico non è stato sconfitto prima
        if (!trap)
        {
            //Se il nemico non è sconfitto
            if (!eh.IsEnemyDefeated())
                DamageTo(collision);
        }
        //Altrimenti se è una trappola faccio continuamenter danno
        else
            DamageTo(collision);
    }

    private bool CanBeHit(WeaponStats ws)
    {
        //variabile che indicherà se il nemico potrà essere colpito o meno
        bool canBeHit = true;
        //se questo nemico è un gufo...
        if(gb != null)
        {
            //...se il suo collider non è solido, potrà essere colpito solo se l'arma che l'ha colpito è un proiettile
            if (enemyColl.isTrigger) { canBeHit = ws.GetWeaponType() == WeaponStats.WeaponType.Bullet; }

        }
        //infine, ritorna se il nemico può essere colpito o meno
        return canBeHit;

    }

    private IEnumerator GotHit(WeaponStats ws)
    {
        //Debug.Log("Enemy Hit: " + transform.parent.name);
        //il nemico subisce danni in base alla potenza dell'arma da cui è stato colpito
        eh.Damage(ws.GetAttack());

        //FARE IN MODO CHE IL NEMICO NON SI MUOVA O ATTACCHI(O DISATTIVARE SUO SCRIPT DI LOGICA O CAMBIARE UNA VARIABILE CHE INDICA DI ESSERE STORDITI)
        GetStunned(true);

        if (!isBoss)
        {
            //ogni forza che agisce sul rigidbody del nemico viene azzerata
            enemyRb.velocity = Vector2.zero;
            //calcola come spingere il nemico in base alla potenza di spinta dell'arma
            Vector2 calculatedPush = -(/*ws.transform*/staticPlayer.position - transform.position).normalized * ws.GetPushForce()/* * enemyRb.mass*/;
            //il nemico viene spinto via, in base alla potenza di spinta calcolata
            enemyRb.velocity = calculatedPush;
            //Debug.Log("POtenza spinta calcolata: " + calculatedPush);
            //aspetta un po' di tempo, in base al tempo di stordimento inflitto dall'arma
            yield return new WaitForSeconds(ws.GetStunTime());
            //il nemico si riprende e torna a camminare come di consueto
            enemyRb.velocity = Vector2.zero;
        }
        //fare in modo che il nemico si comporti come di consueto, se la sua vita non è a zero
        if(!eh.IsEnemyDefeated()) GetStunned(false);

    }

    private void GetStunned(bool gotStunned)
    {
        //se questo nemico è un gufo, lo stordisce se deve essere stordito(o se è stato sconfitto)
        if (gb) { gb.IsStunned(gotStunned); }

    }

    //Overload in collision
    private void DamageTo(Collision2D collision)
    {
        //Se collido con il player
        if (collision.transform.CompareTag("Player"))
        {
            //Prendo la sua interface IDamageable che ha il metodo Damage
            IDamageable temp = collision.transform.GetComponentInChildren<IDamageable>();
            //e se non è null
            if (temp != null)
            {
                //Richiamo il metodo damage e gli passo il danno di questo nemico
                temp.Damage(dmg, true, transform.position, 3f);
            }
        }
    }

    //Overload in trigger
    private void DamageTo(Collider2D collision)
    {
        //Se collido con il player
        if (collision.CompareTag("Player"))
        {
            //Prendo la sua interface IDamageable che ha il metodo Damage
            IDamageable temp = collision.transform.GetComponentInChildren<IDamageable>();
            //e se non è null
            if (temp != null)
            {
                //Richiamo il metodo damage e gli passo il danno di questo nemico
                temp.Damage(dmg, true, transform.position, 3f);
            }
        }
    }

}
