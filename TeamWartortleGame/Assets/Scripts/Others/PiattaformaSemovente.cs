//si occupa del movimento e controllo delle piattaforme semoventi
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiattaformaSemovente : MonoBehaviour
{
    //riferimento al buco su cui questa piattaforma sta volando
    [SerializeField]
    private HoleBehaviour hole = default;
    //array di Transform dei punti in cui deve andare la piattaforma
    [SerializeField]
    private Transform[] route = default;

    //private Rigidbody2D platformRb;
    //array di float che indica quanto deve aspettare la piattaforma per andare da una posizione ad un'altra
    [SerializeField]
    private float[] timeToWait = default;
    //indica quanto velocemente la piattaforma va verso una nuova posizione
    [SerializeField]
    private float speed = 3;
    //indica quanto lontano può essere il centro della piattaforma alla posizione d'arrivo per andare a quella successiva
    [SerializeField]
    private float acceptableDistance = 0.2f;
    //indica verso quale posizione d'arrivo la piattaforma sta andando
    private int actualRoute = 0;
    //indica se si sta aspettando per una nuova posizione
    private bool waiting = false;



    private void Awake()
    {
        //sparenta tutte le posizioni, in modo che non seguano il movimento della piattaforma
        route[0].parent.parent = null;

        //platformRb = GetComponent<Rigidbody2D>();

        //platformRb.velocity = (transform.position + route[actualRoute].position) * speed;
        //Debug.Log(platformRb.velocity);
    }

    private void Update()
    {
        //crea un float per la distanza tra piattaforma e posizione d'arrivo
        float distance = 100;
        //se non si sta aspettando per una nuova posizione, calcola la distanza tra piattaforma e posizione d'arrivo
        if (!waiting) { distance = Vector2.Distance(transform.position, route[actualRoute].position); }
        //se la distanza è minore della distanza accettabile...
        if (distance < acceptableDistance)
        {
            //...sceglie il nuovo percorso in cui andare
            StartCoroutine(GoToNextRoute());
            Debug.Log(distance + " : " + acceptableDistance);
        }
        //altrimenti, continua a far avvicinare la piattaforma al punto d'arrivo
        else { transform.position = Vector3.Lerp(transform.position, route[actualRoute].position, speed * Time.deltaTime); }
        //{ platformRb.velocity = (transform.position + route[actualRoute].position) * speed; }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //ottiene il riferimento al GroundCheck del collisore
        GroundCheck gc = collision.GetComponent<GroundCheck>();
        //se esiste il riferimento appena ottenuto...
        if (gc)
        {
            //...comunica al buco che il giocatore è sopra la piattaforma e non può cadere...
            hole.SetPlayerOnPlatform(true);
            //...e comunica al GroundCheck che è per terra...
            gc.IsThereNoGround(false);
            //...infine, imparenta il giocatore a questa piattaforma
            gc.GetPlayer().parent = transform;
            //Debug.LogError("Giocatore nella piattaforma");
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //ottiene il riferimento al GroundCheck del collisore
        GroundCheck gc = collision.GetComponent<GroundCheck>();
        //se il riferimento al GroundCheck esiste...
        if (gc)
        {
            //...se il giocatore può cadere nel buco...
            if (hole.CouldPlayerFall())
            {
                //...al GroundCheck viene comunicato che non è per terra...
                gc.IsThereNoGround(true, hole);
                //...e se anche l'altro GroundCheck non è per terra...
                if (gc.GetOtherNoFloor())
                {
                    //...comunica al buco che il giocatore non è più nella piattaforma...
                    hole.SetPlayerOnPlatform(false, collision);
                    //...e sparenta il giocatore da questa piattaforma
                    gc.GetPlayer().parent = null;

                    //Debug.LogError("Giocatore non più nella piattaforma");
                }

            } //altrimenti, il giocatore non può cadere nel buco, quindi...
            else
            {
                //...se l'altro GroundCheck è per terra...
                if (!gc.GetOtherNoFloor())
                {
                    //...comunica al buco che il giocatore non è più nella piattaforma, ma non gli passa il collisore(impedendogli di far cadere il giocatore)
                    hole.SetPlayerOnPlatform(false);
                    //...e sparenta il giocatore da questa piattaforma
                    gc.GetPlayer().parent = null;
                    //Debug.LogError("Giocatore non più nella piattaforma");
                }

            }

        }

        //if (hole.CouldPlayerFall())
        //{
        //    //se esiste il riferimento appena ottenuto, se anche l'altro groundcheck non è per terra,  comunica al buco che il giocatore può cadere
        //    if (gc)
        //    {

        //        gc.IsThereNoGround(true, hole);

        //        if (gc.GetOtherNoFloor())
        //        {

        //            hole.SetPlayerOnPlatform(false, collision);

        //            Debug.LogError("Giocatore non più nella piattaforma");

        //        }

        //    }
        //    Debug.Log("Player Could Fall");
        //}
        //else if(gc)
        //{

        //    if (!gc.GetOtherNoFloor())
        //    {

        //        hole.SetPlayerOnPlatform(false);

        //        Debug.LogError("Giocatore non più nella piattaforma");

        //    }

        //}

    }

    private IEnumerator GoToNextRoute()
    {
        //comunica che sta aspettando
        waiting = true;

        //platformRb.velocity = Vector2.zero;
        //aspetta un po'
        yield return new WaitForSeconds(timeToWait[actualRoute]);
        //incrementa l'indice del punto d'arrivo in cui la piattaforma dovrà andare
        actualRoute++;
        //se l'indice supera il limite dell'array di posizioni, torna alla prima posizione
        if (actualRoute >= route.Length) { actualRoute = 0; }

        //platformRb.velocity = (transform.position + route[actualRoute].position) * speed;
        //comunica che non si sta più aspettando
        waiting = false;

    }

}
