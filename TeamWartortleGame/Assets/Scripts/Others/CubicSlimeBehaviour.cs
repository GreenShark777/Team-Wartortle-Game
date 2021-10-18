using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubicSlimeBehaviour : MonoBehaviour
{
    //riferimento al giocatore da seguire
    [SerializeField]
    private Transform player = default;
    //array di tutti gli sprite del cubo
    [SerializeField]
    private GameObject[] spriteSheet;

    [SerializeField]
    private float followTime = 2, //indica per quanto tempo lo slime sta sotto il giocatore prima di
        attackCD = 3,
        speed = 20, 
        attackSpeed = 0.2f;
    
    //private float startSpeed = 0;

    private bool followPlayer = true;

    private int actualAnimState = 0;


    // Start is called before the first frame update
    void Start()
    {
        //ottiene il riferimento al contenitore degli sprite
        Transform spritesContainer = transform.GetChild(0);
        //viene inizializzato l'array degli sprite
        spriteSheet = new GameObject[spritesContainer.childCount];
        //cicla ogni figlio nel contenitore degli sprite e li aggiunge alla lista
        foreach (Transform child in spritesContainer) { spriteSheet[child.GetSiblingIndex()] = child.gameObject; }

        //ottiene la velocità iniziale dello slime
        //startSpeed = speed;

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

        //yield return new WaitForSeconds(attackCD);

        //inizia la coroutine d'attacco
        StartCoroutine(Attack(1));
        //aspetta un po'
        yield return new WaitForSeconds(attackCD);
        //torna a seguire il giocatore
        followPlayer = true;
        //riporta lo slime allo sprite iniziale
        //spriteSheet[actualAnimState - 1].SetActive(false);
        //spriteSheet[0].SetActive(true);
        StartCoroutine(Attack(-1));

        //fa ripartire la coroutine
        StartCoroutine(Follow());

    }

    private IEnumerator Attack(int animationSpeed)
    {

        actualAnimState += animationSpeed;

        yield return new WaitForSeconds(attackSpeed);

        if (!(actualAnimState <= 0) && !(actualAnimState >= spriteSheet.Length))
        {
            
            spriteSheet[actualAnimState - animationSpeed].SetActive(false);

            spriteSheet[actualAnimState].SetActive(true);

            StartCoroutine(Attack(animationSpeed));

        }

    }

}
