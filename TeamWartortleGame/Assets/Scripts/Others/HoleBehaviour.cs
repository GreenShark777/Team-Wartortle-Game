//Si occupa di ciò che deve accadere quando il giocatore cade in un buco
using UnityEngine;

public class HoleBehaviour : MonoBehaviour
{
    //riferimento alla posizione in cui il giocatore deve respawnare dopo essere caduto
    [SerializeField]
    private Transform respawnPoint = default;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //ottiene il riferimento al GroundCheck del collisore
        GroundCheck gc = collision.GetComponent<GroundCheck>();
        //se esiste il riferimento appena ottenuto, gli comunica che non è più per terra e ottiene il riferimento al giocatore
        if (gc) { gc.IsThereNoGround(true, this); }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //ottiene il riferimento al GroundCheck del collisore
        GroundCheck gc = collision.GetComponent<GroundCheck>();
        //se esiste il riferimento appena ottenuto, gli comunica di essere di nuovo per terra
        if (gc) { gc.IsThereNoGround(false); }

    }
    /// <summary>
    /// Cambia la posizione di respawn di questo buco
    /// </summary>
    /// <param name="newPosition"></param>
    public void SetRespawnPosition(Vector2 newPosition) { respawnPoint.position = newPosition; Debug.Log("Changed Respawn Position"); }
    /// <summary>
    /// Mette il giocatore nella posizione di respawn
    /// </summary>
    /// <param name="player"></param>
    public void Respawn(Transform player) { player.position = respawnPoint.position; }

}