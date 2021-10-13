using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFollow : MonoBehaviour
{
    //Transform da direzione
    [HideInInspector]
    private Transform direction;
    //Velocit� di movimento
    [SerializeField]
    private float speed;
    //Riferimento al RigidBody
    [SerializeField]
    private Rigidbody2D rb;
    //Booleana per capire se deve sparare una direzione fissa 
    [SerializeField]
    private bool isFollowPlayer = true;
    //Nome del proiettile perch� ci sono due varianti e bisogna sapere il nome per re inserirle poi nell'object pooling
    [SerializeField]
    private string bulletName = "Scheggia";

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
        //Applico la velocit� al RigidBody nella direzione calcolata
        rb.velocity = isFollowPlayer ? targetPos.normalized * speed : Vector3.down * speed;
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Disable();
        } 
        if (bulletName == "Scheggia2")
        {
            if (LayerMask.LayerToName(col.transform.gameObject.layer) == "Obstacle") Disable();
        }
    }

    private void Disable()
    {
        //Re inserisco il gameobject nell'object pooling passando il nome registrato
        if(gameObject.activeSelf)
            ObjectPooling.inst.ReAddObjectToPool(bulletName, gameObject);
    }

}