using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFollow : MonoBehaviour
{
    //Transform da direzione
    [SerializeField]
    private Transform direction;
    //Velocità di movimento
    [SerializeField]
    private float speed;
    //Riferimento al RigidBody
    [SerializeField]
    private Rigidbody2D rb;
    void OnEnable()
    {

        //Calcolo la direzione
        Vector3 targetPos = (direction.position - transform.position);
        //Applico la rotazione adeguata che punta quindi nella direzione calcolata
        transform.right = targetPos;
        //Applico la velocità al RigidBody nella direzione calcolata
        rb.velocity = targetPos.normalized * speed;
    }
}
