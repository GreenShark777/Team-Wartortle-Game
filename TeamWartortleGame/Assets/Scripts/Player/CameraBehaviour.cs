using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    //riferimento al giocatore, che all'inizio è parent della camera
    private Transform player;
    //riferimanro alla posizione in cui la telecamera deve essere quando è figlia del giocatore
    private Vector3 startLocalPosition;
    //indicano i limiti in cui la telecamera può andare
    private float rightLimit, leftLimit, topLimit, bottomLimit;
    //riferimento al centro della stanza
    private Transform camBoundsCenter;


    private void Awake()
    {
        //ottiene il riferimento al giocatore
        player = transform.parent;
        //ottiene la posizione in cui la telecamera deve essere quando è figlia del giocatore
        startLocalPosition = transform.localPosition;

    }

    private void Update()
    {
        //se il riferimento al centro della stanza non è nullo...
        if (camBoundsCenter != null)
        {

            //float XLimit = Mathf.Abs(/*Mathf.Abs(camBoundsCenter.position.x) +*/ camBoundsLimit.x);
            //float YLimit = Mathf.Abs(/*Mathf.Abs(camBoundsCenter.position.y) +*/ camBoundsLimit.y);

            //transform.position = new Vector3(Mathf.Clamp(player.position.x, -/*camBoundsLimit.x*/XLimit, /*camBoundsLimit.x*/XLimit), 
            //    Mathf.Clamp(player.position.y, -/*camBoundsLimit.y*/YLimit, /*camBoundsLimit.y*/YLimit), transform.position.z);

            //...la posizione della telecamera viene limitata dai limiti imposti, seguendo tuttavia la posizione del giocatore
            transform.position = new Vector3(Mathf.Clamp(player.position.x, leftLimit, rightLimit),
                Mathf.Clamp(player.position.y, bottomLimit, topLimit), transform.position.z);

            //Debug.Log(player.position + " : " + transform.position + " : " + XLimit + " : " + YLimit);
            //if (transform.position.x > camBoundsLimit.x) { transform.position = new Vector3(camBoundsLimit.x, transform.position.y, transform.position.z); }
            //if (transform.position.x < -camBoundsLimit.x) { transform.position = new Vector3(-camBoundsLimit.x, transform.position.y, transform.position.z); }
            //if (transform.position.x > camBoundsLimit.y) { transform.position = new Vector3(transform.position.x, camBoundsLimit.y, transform.position.z); }
            //if (transform.position.x < -camBoundsLimit.y) { transform.position = new Vector3(transform.position.x, -camBoundsLimit.y, transform.position.z); }
            /*
            if (transform.position.x > XLimit)
            { Debug.Log("Xlimit: " + XLimit + " camPosX: " + transform.position.x); transform.position = new Vector3(XLimit, transform.position.y, transform.position.z); }

            if (transform.position.x < -XLimit)
            { Debug.Log("-Xlimit: " + -XLimit + " camPosX: " + transform.position.x); transform.position = new Vector3(-XLimit, transform.position.y, transform.position.z); }

            if (transform.position.y > YLimit)
            { Debug.Log("Ylimit: " + YLimit + " camPosY: " + transform.position.y); transform.position = new Vector3(transform.position.x, YLimit, transform.position.z); }

            if (transform.position.y < -YLimit)
            { Debug.Log("-Ylimit: " + -YLimit + " camPosY: " + transform.position.y); transform.position = new Vector3(transform.position.x, -YLimit, transform.position.z); }
            */


        }

    }

    /// <summary>
    /// Permette di cambiare il parent della telecamera
    /// </summary>
    /// <param name="newParent"></param>
    public void ChangeCamParent(Transform newParent) { transform.parent = newParent; }
    /// <summary>
    /// Fa in modo che il giocatore torni ad essere parent della telecamera, e che quest'ultima torni alla posizione giusta per seguire il giocatore
    /// </summary>
    public void MakePlayerParent()
    {
        //rende la telecamera figlia del giocatore
        transform.parent = player;
        //riporta la telecamera nella posizione iniziale
        transform.localPosition = startLocalPosition;
        //Debug.Log("Player is parent again");
    }
    /// <summary>
    /// Permette di cambiare la posizione della telecamera tramite una data coordinara
    /// </summary>
    /// <param name="newPos"></param>
    public void ChangeCamPosition(Vector3 newPos) { transform.position = newPos; }
    /// <summary>
    /// Permette di cambiare la posizione della telecamera tramite la posizione di un dato Transform
    /// </summary>
    /// <param name="newPos"></param>
    public void ChangeCamPosition(Transform newPos)
    {
        //se il transform ricevuto non è nulla, cambia la posizione della telecamera
        if (newPos) { transform.position = newPos.position; }
        //altrimenti, comunica l'errore
        else { Debug.LogError("Non è stato dato in riferimento alcuna nuova posizione per la telecamera"); }
    
    }

    public void LimitCameraBounds(Vector2 newBounds, Transform boundsCenter, float xBoundsOffset, float yBoundsOffset)
    {
        //imposta i limiti della telecamera
        //camBoundsLimit = newBounds;

        //ottiene il riferimento al centro della stanza in cui si è appena entrati
        camBoundsCenter = boundsCenter;
        //calcola i limiti in cui la telecamera può andare
        rightLimit = boundsCenter.position.x + (newBounds.x / xBoundsOffset);
        leftLimit = boundsCenter.position.x - (newBounds.x / xBoundsOffset);
        topLimit = boundsCenter.position.y + (newBounds.y / yBoundsOffset);
        bottomLimit = boundsCenter.position.y - (newBounds.y / yBoundsOffset);

        //camBoundsLimit += new Vector2(camBoundsCenter.position.x, camBoundsCenter.position.y);
        //camBoundsLimit = camBoundsCenter.position + camBoundsLimit;

    }
    /// <summary>
    /// Rimuove i limiti ai movimenti della telecamera
    /// </summary>
    public void StopCameraLimits() { camBoundsCenter = null; }


    private void OnDrawGizmos()
    {

        if (camBoundsCenter != null)
        {
            Gizmos.color = Color.red;
            //Gizmos.DrawWireCube(camBoundsCenter.position, camBoundsLimit);

            Gizmos.DrawLine(new Vector2(leftLimit, topLimit), new Vector2(leftLimit, bottomLimit));
            Gizmos.DrawLine(new Vector2(leftLimit, bottomLimit), new Vector2(rightLimit, bottomLimit));
            Gizmos.DrawLine(new Vector2(rightLimit, bottomLimit), new Vector2(rightLimit, topLimit));
            Gizmos.DrawLine(new Vector2(rightLimit, topLimit), new Vector2(leftLimit, topLimit));

        }
        

    }

}
