using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFollow : MonoBehaviour
{
    //Transform da direzione
    [SerializeField]
    private Vector3 direction;
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

    private void Awake()
    {
        //Prendo la posizione del player come direzione
        if (isFollowPlayer) direction = GameObject.FindGameObjectWithTag("Player").transform.position;
        else direction = Vector3.down;
  
    }
    private void OnEnable()
    {
        //Disattivo il gameobject dopo tot
        Invoke("Disable", 3f);
        //Calcolo la direzione
        Vector3 targetPos = (direction - transform.position);
        //Applico la rotazione adeguata che punta quindi nella direzione calcolata
        transform.right = isFollowPlayer ? targetPos : direction;
        //Applico la velocità al RigidBody nella direzione calcolata
        rb.velocity = isFollowPlayer ? targetPos.normalized * speed : direction * speed;
    }

    private void Disable()
    {
        //Re inserisco il gameobject nell'object pooling passando il nome registrato
        if(gameObject.activeSelf)
            ObjectPooling.inst.ReAddObjectToPool(bulletName, gameObject);
    }
}