//Si occupa di ciò che deve accadere quando il giocatore cade in un buco
using UnityEngine;

public class HoleBehaviour : MonoBehaviour
{
    //riferimento alla posizione in cui il giocatore deve respawnare dopo essere caduto
    [SerializeField]
    private Transform respawnPoint = default;
    //indica se il giocatore è sopra il buco o meno
    private bool playerIsOnPlatform = false;
    //indica se il giocatore è abbastanza vicino da poter cadere
    private bool playerCouldFall = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //se il giocatore non è in una piattaforma...
        if (!playerIsOnPlatform)
        {
            //...ottiene il riferimento al GroundCheck del collisore...
            GroundCheck gc = collision.GetComponent<GroundCheck>();
            //...e se esiste, gli comunica che non è più per terra...
            if (gc) { gc.IsThereNoGround(true, this); }
            //...infine, comunica che il giocatore può cadere
            playerCouldFall = true;
            //Debug.LogError("Controllo GroundCheck in entrata");
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //ottiene il riferimento al GroundCheck del collisore
        GroundCheck gc = collision.GetComponent<GroundCheck>();
        //se esiste il riferimento appena ottenuto, gli comunica di essere di nuovo per terra e che il giocatore non può più cadere
        if (gc) { gc.IsThereNoGround(false); playerCouldFall = false; /*Debug.Log("Controllo GroundCheck in uscita");*/ }

    }
    /// <summary>
    /// Cambia la posizione di respawn di questo buco
    /// </summary>
    /// <param name="newPosition"></param>
    public void SetRespawnPosition(Vector2 newPosition) { respawnPoint.position = newPosition; /*Debug.Log("Changed Respawn Position");*/ }
    /// <summary>
    /// Comunica al buco che il giocatore non può cadere perchè è sopra una piattaforma. O, se non è soprà una piattaforma, effettua l'OnTriggerEnter2D
    /// </summary>
    /// <param name="isOn"></param>
    public void SetPlayerOnPlatform(bool isOn, Collider2D coll = null)
    {
        //comunica che il giocatore è sopra una piattaforma(o non lo è più)
        playerIsOnPlatform = isOn;
        //se il giocatore non è più sopra la piattaforma, ed è stato passato come parametro il collisore...
        if (!isOn && coll)
        {
            //...effettua l'OnTriggerEnter2D
            OnTriggerEnter2D(coll);

        }
    
    }

    public bool CouldPlayerFall() { return playerCouldFall; }
    /// <summary>
    /// Mette il giocatore nella posizione di respawn
    /// </summary>
    /// <param name="player"></param>
    public void Respawn(Transform player) { player.position = respawnPoint.position; }

}