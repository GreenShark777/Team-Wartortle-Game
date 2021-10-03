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


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //se si collide con un oggetto fisico...
        if (!collision.isTrigger) { BulletHitSolid(); }

    }

    private void OnEnable()
    {
        //se esiste il riferimento al particellare di oggetto colpito...
        if (shotPS)
        {
            //...lo fa partire...
            shotPS.Play();
            //...se non lo è già, lo sparenta al proiettile
            //if (shotPS.transform.parent) hitPS.transform.parent = null;

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
        //disattiva questo proiettile
        gameObject.SetActive(false);

    }

}
