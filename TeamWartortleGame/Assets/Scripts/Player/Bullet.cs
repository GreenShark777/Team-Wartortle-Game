using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //Nome del tag del target da colpire
    [SerializeField]
    private string target;

    //Danno
    [SerializeField]
    private float dmg = 1;

    [SerializeField]
    private float timeToDestroy = 3;

    private void OnEnable()
    {
        Invoke("ReAddToPool",timeToDestroy);
    }

    private void ReAddToPool()
    {
        if (gameObject.activeSelf)
            ObjectPooling.inst.ReAddObjectToPool("Fiamma", this.gameObject);
    }

    //Con trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Se collido con il target
        if (collision.CompareTag(target))
        {
            //Chiamo il suo Damage
            IDamageable temp = collision.transform.parent.GetComponentInChildren<IDamageable>();
            if (temp != null)
                temp.Damage(dmg, true, transform.position, 3f);
            //Lo riaggiungo all'object pooling
            ReAddToPool();
        }
        else if (collision.CompareTag("Obstacle")) ReAddToPool();

    }

    //ma anche con collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Se collido con il target
        if (collision.gameObject.CompareTag(target))
        {
            //Chiamo il suo Damage
            IDamageable temp = collision.transform.parent.GetComponentInChildren<IDamageable>();
            if (temp != null)
                temp.Damage(dmg, true, transform.position, 3f);
        }
        //Lo riaggiungo all'object pooling
        ReAddToPool();
    }
}
