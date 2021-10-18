using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushRandom : MonoBehaviour
{
    //Riferimento al rigidbody
    [SerializeField]
    private Rigidbody2D rb;
    //Booleana che controlla se effettivamente voglio spingere questo item quando appare verso una direzione randomica
    [SerializeField]
    private bool toPush = true;
    //Valore float per il rallentamento dell'item dopo esser stato spinto
    [SerializeField]
    private float slowSpeed = 1f;

    private void OnEnable()
    {
        //Controllo se è un item che posso spingere
        if (toPush)
        {
            StartCoroutine(IPush());
        }
    }

    private IEnumerator IPush()
    {
        //Creo un numero randomico da associare alla x e alla y
        float randomValue = Random.Range(1, 3);
        //Creo un vettore randomico
        Vector2 pushDir = new Vector2(randomValue, randomValue);
        //Inverto la direzione nel 50% dei case
        pushDir.x *= (Random.value > .5f) ? -1 : 1;
        pushDir.y *= (Random.value > .5f) ? -1 : 1;
        //e intanto lo aggiungo all'addforce
        rb.velocity = pushDir;
        //Lerpo il la velocity il vettore della direzione a zero nel mentre lo assegno all'addForce
        while (rb.velocity.magnitude > .1f)
        {
            //Lerpo a 0 il vettore di spinta
            rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, slowSpeed * Time.deltaTime);
            yield return null;
        }
        rb.velocity = Vector2.zero;
    }
}
