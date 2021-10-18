//Si occupa di controllare se il giocatore sta cadendo o se ha i piedi per terra
using System.Collections;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    //indica se questo collider è per terra
    [SerializeField]
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
    //riferimento allo script della vita del giocatore
    [SerializeField]
    private PlayerHealth ph = default;
    //indica quanto tempo bisogna aspettare per respawnare dopo essere caduti
    [SerializeField]
    private float fallingTime = 1.4f;
    //indica se si sta cadendo o meno
    private bool isFalling = false;
    //riferimento all'Animator dell'immagine che funge da dissolvenza
    [SerializeField]
    private Animator blackScreenAnim = default;
    //indica la direzione in cui il giocatore deve cadere
    private Vector2 fallTo;


    private void Awake()
    {
        //ottiene il riferimento all'Animator del giocatore
        playerAnim = player.GetComponent<Animator>();
        //ottiene il riferimento allo script di movimento del giocatore
        playerMovement = player.GetComponent<Movement>();

    }

    private void FixedUpdate()
    {
        //se si sta cadendo, il giocatore andrà verso il centro del buco
        if (isFalling) { player.position = Vector2.MoveTowards(player.position, fallTo, Time.deltaTime); }

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
        //ottiene la velocità iniziale dell'animazione di fadeIn
        float startSpeedAnim = blackScreenAnim.speed;
        //dimezza la velocità d'animazione del fadeIn
        blackScreenAnim.speed = 0.5f;
        //impedisce al giocatore di muoversi
        playerMovement.enabled = false;
        //fa cominciare l'animazione di caduta
        playerAnim.SetBool("Falling", true);
        //ottiene la posizione centrale del buco
        fallTo = hole.GetHoleCenterPosition();
        //comunica che si sta cadendo
        isFalling = true;
        //aspetta del tempo
        yield return new WaitForSeconds(fallingTime);
        //riporta la velocità d'animazione di fadeIn alla velocità normale
        blackScreenAnim.speed = startSpeedAnim;
        //fa finire l'animazione di caduta
        playerAnim.SetBool("Falling", false);
        //permette al giocatore di camminare nuovamente
        playerMovement.enabled = true;
        //fa respawnare il giocatore
        hole.Respawn(player);
        //fa subire danno al giocatore
        ph.Damage(1);
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
