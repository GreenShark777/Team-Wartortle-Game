using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    //indica se questo collider è per terra
    private bool noFloor = false;
    //riferimento all'altro GroundCheck
    [SerializeField]
    private GroundCheck otherGC = default;
    //riferimento al giocatore
    [SerializeField]
    private Transform player = default;
    //riferimento all'Animator del giocatore
    private Animator playerAnim;
    //riferimento allo script di movimento del giocatore
    private Movement playerMovement;
    //indica quanto tempo bisogna aspettare per respawnare dopo essere caduti
    [SerializeField]
    private float fallingTime = 1.4f;
    //indica se si sta cadendo o meno
    private bool isFalling = false;
    //riferimento all'Animator dell'immagine che funge da dissolvenza
    [SerializeField]
    private Animator blackScreenAnim = default;


    private void Awake()
    {
        //ottiene il riferimento all'Animator del giocatore
        playerAnim = player.GetComponent<Animator>();
        //ottiene il riferimento allo script di movimento del giocatore
        playerMovement = player.GetComponent<Movement>();

    }

    public void IsThereNoGround(bool noGround, HoleBehaviour hole = null)
    {
        //se non si sta già cadendo, esegue i vari controlli
        if (!isFalling)
        {
            //imposta la variabile di caduta al valore ottenuto
            noFloor = noGround;
            //se non si è per terra...
            if (noGround)
            {
                //...controlla se anche l'altro non è per terra, nel qual caso fa partire la coroutine di caduta
                if (otherGC.GetNoFloor()) { StartCoroutine(Falling(hole)); }

            }

        }

    }
    /// <summary>
    /// Ritorna lo stato di caduta
    /// </summary>
    /// <returns></returns>
    public bool GetNoFloor() { return noFloor; }
    /// <summary>
    /// Ritorna lo stato di caduta dell'altro GroundCheck
    /// </summary>
    /// <returns></returns>
    public bool GetOtherNoFloor() { return otherGC.noFloor; }
    /// <summary>
    /// Si occupa del tempismo di caduta
    /// </summary>
    /// <param name="hole"></param>
    /// <returns></returns>
    private IEnumerator Falling(HoleBehaviour hole)
    {
        //fa partire l'animazione di dissolvenza dello schermo in nero
        blackScreenAnim.SetTrigger("Dissolve");
        //impedisce al giocatore di muoversi
        playerMovement.enabled = false;
        //fa cominciare l'animazione di caduta
        playerAnim.SetBool("Falling", true);
        //comunica che si sta cadendo
        isFalling = true;
        //aspetta del tempo
        yield return new WaitForSeconds(fallingTime);
        //fa finire l'animazione di caduta
        playerAnim.SetBool("Falling", false);
        //permette al giocatore di camminare nuovamente
        playerMovement.enabled = true;
        //fa respawnare il giocatore
        hole.Respawn(player);
        //comunica che non si sta più cadendo
        isFalling = false;
        //comunica ad entrambi i GroundCheck che si sta toccando di nuovo terra
        noFloor = false;
        otherGC.IsThereNoGround(false);

    }
    /// <summary>
    /// Ritorna il Transform del giocatore
    /// </summary>
    /// <returns></returns>
    public Transform GetPlayer() { return player; }

}
