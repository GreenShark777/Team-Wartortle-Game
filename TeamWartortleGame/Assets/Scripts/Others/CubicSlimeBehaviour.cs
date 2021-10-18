//Si occupa del comportamento dello slime
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CubicSlimeBehaviour : MonoBehaviour
{
    //riferimento al giocatore da seguire
    [SerializeField]
    private Transform player = default;
    //riferimento allo script di movimento del giocatore
    private Movement playerMov;
    //riferimento allo script di vita del giocatore
    private PlayerHealth ph;
    //array di tutti gli sprite del cubo
    [SerializeField]
    private GameObject[] spriteSheet;
    //riferimento al collider di questo slime
    private Collider2D slimeColl;
    //riferimento al sorting group dello slime
    private SortingGroup slimeSG;

    [SerializeField]
    private float followTime = 2, //indica per quanto tempo lo slime sta sotto il giocatore prima di
        attackCD = 3, //indica quanto tempo deve aspettare lo slime prima e dopo l'attacco
        speed = 20, //indica quanto velocemente il cubo slime va verso il giocatore
        attackSpeed = 0.2f; //indica quanto velocemente va l'animazione d'attacco del cubo
    
    //private float startSpeed = 0;

    //indica se il cubo deve seguire il giocatore o meno
    private bool followPlayer = true;
    //indica lo stato d'animazione attuale del cubo
    private int actualAnimState = -1;


    // Start is called before the first frame update
    void Start()
    {
        //ottiene il riferimento al contenitore degli sprite
        Transform spritesContainer = transform.GetChild(0);
        //viene inizializzato l'array degli sprite
        spriteSheet = new GameObject[spritesContainer.childCount];
        //cicla ogni figlio nel contenitore degli sprite e li aggiunge alla lista
        foreach (Transform child in spritesContainer) { spriteSheet[child.GetSiblingIndex()] = child.gameObject; }
        //ottiene il riferimento al collider di questo slime
        slimeColl = GetComponent<Collider2D>();
        //ottiene il riferimento al sorting group dello slime
        slimeSG = GetComponent<SortingGroup>();

        //ottiene la velocità iniziale dello slime
        //startSpeed = speed;

        //ottiene i riferimenti agli script del giocatore
        playerMov = player.GetComponent<Movement>();
        ph = player.GetComponentInChildren<PlayerHealth>();

        //fa partire la coroutine per l'inseguimento del giocatore
        StartCoroutine(Follow());

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //se bisogna seguire il giocatore...
        if (followPlayer)
        {
            //...il cubo slime si muove verso il giocatore
            transform.position = Vector3.Lerp(transform.position, player.position, speed * Time.fixedDeltaTime);

        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //se lo slime colide con il giocatore, ferma tutte le coroutine e fa partire quella di cattura
        if (collision.CompareTag("Player")) { StopAllCoroutines(); StartCoroutine(CapturedPlayer()); }

    }

    /// <summary>
    /// Si occupa dei tempismi di inseguimento e attacco
    /// </summary>
    /// <returns></returns>
    private IEnumerator Follow()
    {
        //segue il giocatore per un po'
        yield return new WaitForSeconds(followTime);
        //smette di seguirlo
        followPlayer = false;
        //aspetta un po'
        yield return new WaitForSeconds(attackCD / 2);
        //fa avanzare lo stato d'animazione del cubo
        actualAnimState++;
        //inizia la coroutine d'attacco
        StartCoroutine(Attack(1));
        //attiva il collider dello slime
        slimeColl.enabled = true;
        //aspetta un po'
        yield return new WaitForSeconds(attackCD);
        //torna a seguire il giocatore
        followPlayer = true;
        //disabilita il collider dello slime
        slimeColl.enabled = false;

        //riporta lo slime allo sprite iniziale
        //spriteSheet[actualAnimState - 1].SetActive(false);
        //spriteSheet[0].SetActive(true);

        //fa indietreggiare lo stato d'animazione del cubo
        actualAnimState--;
        //fa ripartire la coroutine d'attacco, ma al contrario
        StartCoroutine(Attack(-1));
        //fa ripartire la coroutine
        StartCoroutine(Follow());

    }

    private IEnumerator Attack(int animationSpeed)
    {
        //incrementa lo stato d'animazione in base al valore ricevuto
        actualAnimState += animationSpeed;
        //aspetta il tempo d'animazione
        yield return new WaitForSeconds(attackSpeed);
        //se lo stato d'animazione non finisce nei limiti...
        if (!(actualAnimState < 0) && !(actualAnimState >= spriteSheet.Length))
        {
            //Debug.Log("Slime Actual Anim State = " + actualAnimState);
            //...fa continuare l'animazione...
            spriteSheet[actualAnimState - animationSpeed].SetActive(false);
            spriteSheet[actualAnimState].SetActive(true);
            //...e richiama se stesso per continuare il ciclo
            StartCoroutine(Attack(animationSpeed));

        }

    }

    private IEnumerator CapturedPlayer()
    {
        //aumenta la profondità del SortingGroup dello slime
        slimeSG.sortingOrder++;
        //fa partire la coroutine d'attacco
        StartCoroutine(Attack(1));
        //segue il giocatore
        followPlayer = true;
        //impedisce al giocatore di muoversi
        playerMov.enabled = false;
        //aspetta un po'
        yield return new WaitForSeconds(attackCD);
        //fa indietreggiare lo stato d'animazione del cubo
        actualAnimState--;
        //fa ripartire la coroutine d'attacco, ma al contrario
        StartCoroutine(Attack(-1));
        //il collider dello slime viene disabilitato
        slimeColl.enabled = false;
        //fa muovere di nuovo il giocatore
        playerMov.enabled = true;
        //infligge danno al giocatore
        ph.Damage(1, true);
        //riporta lo slime alla sua profondità originale
        slimeSG.sortingOrder--;
        //fa ripartire la coroutine di inseguimento
        StartCoroutine(Follow());

    }

}
