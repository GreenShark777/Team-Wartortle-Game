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


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //se non si possono oltepassare oggetti solidi e si collide con un oggetto fisico, il proiettile viene disattivato
        if (!goesThroughSolids && !collision.isTrigger) { BulletHitSolid(); }

    }

    private void OnEnable()
    {
        //se esiste il riferimento al particellare di sparo...
        if (shotPS)
        {
            //...lo fa partire
            shotPS.Play();

        }
        else { Debug.LogError("Al proiettile " + gameObject.name + " manca il riferimento al particellare di sparo"); }

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
        else { Debug.LogError("Al proiettile " + gameObject.name + " manca il riferimento al particellare di oggetto colpito"); }
        //disattiva questo proiettile
        gameObject.SetActive(false);

    }

}
