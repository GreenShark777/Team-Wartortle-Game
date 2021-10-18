//Si occupa di quello che fanno i proiettili quando vengono sparati e quando collidono contro qualche oggetto
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsBehaviour : MonoBehaviour
{
    //riferimenti ai particellari del proiettile
    [SerializeField]
    private ParticleSystem shotPS = default, //Effetto di sparo
        hitPS = default; //Effetto di oggetto colpito

    //indica se questo proiettile può andare oltre gli oggetti fisici o meno
    [SerializeField]
    private bool goesThroughSolids = false;

    private Rigidbody2D rb;

    [SerializeField]
    private float speed = 1600;

    //Nome del proiettile per re inserirlo nel object pooling giusto
    [SerializeField]
    private string gbName = "Bullets";

    //Riferimento all'animatore
    [SerializeField]
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //se non si possono oltepassare oggetti solidi e si collide con un oggetto fisico, il proiettile viene disattivato
        if (!goesThroughSolids && !collision.isTrigger) {
            if (LayerMask.LayerToName(collision.gameObject.layer) != "Player")
                BulletHitSolid(); 
        }

    }

    private void OnEnable()
    {

        Invoke("AddToPool",3f);
        //Ottengo la direzione corrente dell'arma
        Vector2 direction = default;
        if (GameManager.inst)
            direction = GameManager.inst.GetGunDirection();
        //E la applico al rigidBody
        rb.velocity = (direction * (speed));
        //se esiste il riferimento al particellare di sparo...
        if (shotPS)
          {
             //...lo fa partire
             shotPS.Play();

          }
        //else { Debug.LogError("Al proiettile " + gameObject.name + " manca il riferimento al particellare di sparo"); }
        

    }

    private void Update()
    {
        //Se ho l'animatore disattivo il gameobject quando l'animazione finisce
        if (animator)
        {
            //Se il tempo normalizzato dell'animazione ha superato 1(durata massima) e inoltre non è in transizione, la riaggiungo nella sua pool
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))
                AddToPool();
        }
    }

    private void BulletHitSolid()
    {
        //se esiste il riferimento al particellare di oggetto colpito...
        if (hitPS)
        {
            //...lo fa partire...
            hitPS.Play();
            //...se non lo è già, lo sparenta al proiettile...
            if(hitPS.transform.parent) hitPS.transform.parent = null;
            //...e porta il particellare nella posizione in cui il proiettile ha colliso
            hitPS.transform.position = transform.position;

        }
        //else { Debug.LogError("Al proiettile " + gameObject.name + " manca il riferimento al particellare di oggetto colpito"); }
        //Riaggiungo il proiettile all'object pooling
        AddToPool();

    }

    private void AddToPool()
    {
        //Riposiziono il particellare come figlio
        if (hitPS) hitPS.transform.parent = transform;
        //Se il gameObject è attivo, quindi non è stato già inserito nell'object pooling lo inserisco
        if (gameObject.activeSelf)
            ObjectPooling.inst.ReAddObjectToPool(gbName, gameObject);
    }

}
