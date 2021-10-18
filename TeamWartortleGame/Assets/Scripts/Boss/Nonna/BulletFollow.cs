using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFollow : MonoBehaviour
{
    //Transform da direzione
    [HideInInspector]
    private Transform direction;
    //Velocità di movimento
    [SerializeField]
    private float speed;
    //Riferimento al RigidBody
    [SerializeField]
    private Rigidbody2D rb;
    //Booleana per capire se deve sparare una direzione fissa 
    [SerializeField]
    private bool isFollowPlayer = true;
    //Nome del proiettile perché ci sono due varianti e bisogna sapere il nome per re inserirle poi nell'object pooling
    [SerializeField]
    private string bulletName = "Scheggia";

    //Danno del proiettile
    private int dmg = 1;

    private void Awake()
    {
        //Prendo la posizione del player come direzione
        direction = GameObject.FindGameObjectWithTag("Player").transform;

    }

    private void OnEnable()
    {
        //Disattivo il gameobject dopo tot
        Invoke("Disable", 3f);
        //Calcolo la direzione
        Vector3 targetPos = (direction.position - transform.position);
        //Applico la rotazione adeguata che punta quindi nella direzione calcolata
        transform.right = isFollowPlayer ? targetPos : Vector3.down;
        //Applico la velocità al RigidBody nella direzione calcolata
        rb.velocity = isFollowPlayer ? targetPos.normalized * speed : Vector3.down * speed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Se collido con il target
        if (collision.CompareTag("Player"))
        {
            //Chiamo il suo Damage
            IDamageable temp = collision.transform.parent.GetComponentInChildren<IDamageable>();
            if (temp != null)
                temp.Damage(dmg, true, transform.position, 3f);
            Disable();
        }
        if (bulletName == "Scheggia2")
        {
            if (LayerMask.LayerToName(collision.transform.gameObject.layer) == "Obstacle") Disable();
            Debug.Log("Ostacolo");
        }
    }

    private void Disable()
    {
        Debug.Log("Disable");

        //Re inserisco il gameobject nell'object pooling passando il nome registrato
        if (gameObject.activeSelf)
            ObjectPooling.inst.ReAddObjectToPool(bulletName, gameObject);
    }

}