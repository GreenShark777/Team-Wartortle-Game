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
    
    private float startSpeed = 0;

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
        startSpeed = speed;

        StartCoroutine(Follow());

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (followPlayer)
        {

            transform.position = Vector3.Lerp(transform.position, player.position, speed * Time.fixedDeltaTime);

        }
        
    }

    private IEnumerator Follow()
    {

        yield return new WaitForSeconds(followTime);

        followPlayer = false;

        yield return new WaitForSeconds(attackCD);

        StartCoroutine(Attack());

        yield return new WaitForSeconds(attackCD);

        followPlayer = true;

    }

    private IEnumerator Attack()
    {

        actualAnimState++;

        yield return new WaitForSeconds(attackSpeed);

        if (actualAnimState == spriteSheet.Length)
        {
            
            spriteSheet[actualAnimState - 1].SetActive(false);

            spriteSheet[actualAnimState].SetActive(true);
        
        }

    }

}
