//Si occupa del comportamento del gufo
using System.Collections;
using UnityEngine;

public class GufoBehaviour : MonoBehaviour
{
    //riferimento al Rigidbody2D del gufo
    private Rigidbody2D rbGufo;
    //riferimento allo sprite del gufo
    [SerializeField]
    private Transform spriteGufo = default;
    //riferimento alla posizione in cui il gufo deve essere mentre � in volo
    [SerializeField]
    private Transform flyingPoint = default;
    //riferimento alla posizione in cui il gufo deve essere quando � per terra
    [SerializeField]
    private Transform groundPoint = default;
    //riferimento al giocatore
    [SerializeField]
    private Transform player = default;
    //riferimento statico al giocatore
    private static Transform staticPlayer = default;
    //riferimento al collider del gufo
    [SerializeField]
    private Collider2D collGufo = default;
    //riferimento al collider del giocatore
    //[SerializeField]
    //private Collider2D collPlayer = default;
    //riferimento al collider del punto di volo del gufo
    private Collider2D collFlyingPoint;

    [Header("Animators")]

    private Animator flyingPointAnim, //riferimento all'Animator del punto di volo
        gufoAnimator; //riferimento all'Animator del gufo


    [Header("Speed")]
    //indica la velocit� del nemico in volo
    [SerializeField]
    private float flyingSpeed = 4;
    //indica la velocit� del nemico nel tuffo
    [SerializeField]
    private float diveSpeed = 8;
    //variabili che indicano lo stato del gufo
    private bool isFlying = false, //indica se il gufo sta volando o meno
        jumped = false, //indica se il gufo � in salto
        isDiving = false, //indica se il gufo si � tuffato
        isAttacking = false; //indica se il gufo sta attaccando o meno

    //indica se il nemico � stato stordito o meno
    private bool isStunned = false;

    [Header("Timers")]
    //vari timer del gufo
    private float jumpAnticipationTimer, //indica quanto tempo bisogna aspettare prima di alzarsi in volo
        diveAnticipationTimer; //indica quanto tempo bisogna aspettare prima di tuffarsi
    [SerializeField]
    private float startAttackMaxTimer = 2, //indica il tempo massimo che il gufo deve aspettare prima di attaccare
        startAttackMinTimer = 1, //indica il tempo minimo che il gufo deve aspettare prima di attaccare
        afterLandingCD = 1; //indica il tempo che il gufo deve aspettare per attaccare di nuovo dopo essere atterrato

    [Header("Distances")]
    [SerializeField]
    private float acceptableDistanceToPoint = 0.2f, //indica quanto vicino pu� essere al massimo lo sprite al punto di volo o di atterraggio per continuare
        distanceToSpot = 5; //indica quanto

    //indica il punto in cui il gufo deve atterrare
    private Vector2 divingPoint;


    private void Awake()
    {
        //riferimento al Rigidbody2D del gufo
        rbGufo = GetComponent<Rigidbody2D>();
        //se il riferimento statico al giocatore � ancora nullo e questo script ha un riferimento al giocatore
        if (staticPlayer == null && player != null) { staticPlayer = player; }
        //ottiene il riferimento all'Animator del punto di volo
        flyingPointAnim = flyingPoint.GetComponent<Animator>();
        //ottiene il riferimento al collider del punto di volo del gufo
        collFlyingPoint = flyingPoint.GetComponent<Collider2D>();
        //fa in modo che le collisioni tra il punto di volo del gufo e il giocatore vengano ignorate
        //Physics2D.IgnoreCollision(collFlyingPoint, collPlayer);

        //OTTIENE IL TEMPO DI ANTICIPAZIONE AL VOLO E DELLE ALTRE ANIMAZIONI
        jumpAnticipationTimer = 2;
        diveAnticipationTimer = 1;

    }
    
    private void Update()
    {
        //crea una variabile che indica quanto � lontano lo sprite del gufo dal punto
        float distanceToPoint = 100;
        //se il gufo sta saltando...
        if (jumped)
        {
            //...lo sprite del gufo viene messo lentamente nella posizione in cui deve essere per essere in volo...
            spriteGufo.position = Vector2.Lerp(spriteGufo.position, flyingPoint.position, flyingSpeed * Time.deltaTime);
            //...se non si � in volo...
            if (!isFlying)
            {
                //...calcola la distanza dal punto in cui deve essere lo sprite del gufo per essere in volo...
                distanceToPoint = Mathf.Abs(spriteGufo.position.y - flyingPoint.position.y);
                //...se la distanza tra sprite e punto � accettabile, fa partire la coroutine di volo
                if (distanceToPoint < acceptableDistanceToPoint) { StartCoroutine(Flying()); }

            }

        } //altrimenti, se il nemico si sta tuffando verso il giocatore...
        else if (isDiving)
        {
            //...lo sprite del gufo viene messo lentamente nella posizione in cui deve essere per essere in volo...
            spriteGufo.position = Vector2.Lerp(spriteGufo.position, groundPoint.position, flyingSpeed * Time.deltaTime);

            //...calcola la distanza dal punto in cui deve essere lo sprite del gufo per essere per terra...
            //distanceToPoint = Mathf.Abs(spriteGufo.position.y - groundPoint.position.y);

            //...crea una variabile che indica se il gufo � atterrato o meno...
            bool landed = false;
            //...calcola la distanza dal punto in cui deve essere lo sprite del gufo per essere per terra...
            distanceToPoint = Vector2.Distance(transform.position, divingPoint);
            //...se la distanza tra sprite e punto � accettabile, comunica che il gufo � atterrato...
            if (distanceToPoint < acceptableDistanceToPoint /*&& Mathf.Abs(spriteGufo.position.y - groundPoint.position.y) < acceptableDistanceToPoint*/)
            { landed = true; }
            //...e se il gufo � atterrato, lo ferma
            if (landed) { StartCoroutine(Landed()); }
            //Debug.Log("Calculating Diving: " + distanceToPoint);
        } //altrimenti, se non si sta n� volando n� si sta gi� attaccando n� il gufo � stordito...
        else if (!isFlying && !isAttacking && !isStunned)
        {
            //...calcola se il giocatore � abbastanza vicino...
            distanceToPoint = Vector2.Distance(transform.position, staticPlayer.position);
            //...se lo �, comunica che il giocatore � stato avvistato
            if (distanceToPoint < distanceToSpot) { PlayerSpotted(); }
            //Debug.Log(distanceToPoint);
        } //altrimenti, se non si sta volando o il gufo � stordito...
        else if (!isFlying || isStunned)
        {
            //...e calcolando la distanza tra lo sprite e l'ombra...
            distanceToPoint = Mathf.Abs(spriteGufo.position.y - flyingPoint.position.y);
            //...risulta che lo sprite � lontano dall'ombra, lo avvicina
            if (distanceToPoint > acceptableDistanceToPoint)
            { spriteGufo.position = Vector2.Lerp(spriteGufo.position, groundPoint.position, flyingSpeed * Time.deltaTime); }

        }

    }

    public void PlayerSpotted()
    {
        //inizia la coroutine di inizio volo
        StartCoroutine(StartFlying());
        //comunica che il gufo sta attaccando
        isAttacking = true;
        //Debug.Log("player was spotted");
    }

    private IEnumerator StartFlying()
    {
        //Debug.Log("anticipazione volo");
        //FA PARTIRE L'ANIMAZIONE DI ANTICIPAZIONE AL VOLO

        //aspetta che l'animazione di anticipazione finisca
        yield return new WaitForSeconds(jumpAnticipationTimer);

        //FA PARTIRE L'ANIMAZIONE DI INNALZAMENTO IN VOLO

        //rende non solido il collider del gufo
        collGufo.isTrigger = true;
        //rimuove ogni forza che agisce sul Rigidbody del gufo
        rbGufo.velocity = Vector2.zero;
        //indica che il gufo sta saltando in aria
        jumped = true;
        //Debug.Log("salto");
    }

    private IEnumerator Flying()
    {
        //Debug.Log("in salto");

        //float distanceToPoint = Mathf.Abs(spriteGufo.position.y - flyingPoint.position.y);

        //Debug.Log("Distanza: " + distanceToPoint);

        //se lo sprite non ha raggiunto il punto in cui deve essere in volo...
        //if (/*!isFlying && */distanceToPoint > acceptableDistanceToPoint)
        /*{
            //...lo fa avvicinare alla posizione in cui deve essere mentre vola...
            //spriteGufo.position = Vector2.Lerp(spriteGufo.position, flyingPoint.position, flyingSpeed);
            //StopAllCoroutines();
            //...ricomincia la coroutine...
            StartCoroutine(Flying());
            //...e chiude questa coroutine
            yield break;
            //StopCoroutine("Flying"); NON FUNZIONEREBBE, PERCHE' FERMA ALTRE ISTANZE DI QUESTA COROUTINE E NON QUESTA
        }*/

        //altrimenti, comunica che il gufo sta volando
        //else { isFlying = true; }

        //il gufo non sta pi� saltando
        jumped = false;
        //il gufo � in volo
        isFlying = true;
        //il punto volante andr� in cerchio
        flyingPointAnim.SetBool("FlyingAround", true);
        //il collider del punto volante diventa solido, in modo che il gufo non vada fuori dai limiti della mappa durante il volo
        collFlyingPoint.isTrigger = false;
        //il gufo si muove verso il giocatore in volo
        StartCoroutine(FlyAround());
        //ottiene il tempo d'aspettare entro il range dei timer minimo e massimo
        float randWaitTime = Random.Range(startAttackMinTimer, startAttackMaxTimer);
        //Debug.Log("In punto di volo. Tempo per attacco: " + randWaitTime);
        //aspetta il tempo calcolato
        yield return new WaitForSeconds(randWaitTime);
        //il collider del punto volante diventa di nuovo non solido, dato che ha finito di volare
        collFlyingPoint.isTrigger = true;
        //il gufo si tuffa verso il giocatore
        StartCoroutine(DiveAttack());

    }

    private IEnumerator FlyAround()
    {
        //se il gufo sta ancora volando...
        if (isFlying)
        {
            //...lo fa avvicinare alla posizione di volo(che sta andando in giro per la mappa)...
            rbGufo.velocity = (new Vector2((flyingPoint.position.x - transform.position.x) * (flyingSpeed / 2), rbGufo.velocity.y));
            //...sposta anche lo sprite verso il punto di volo(facendolo muovere anche nell'asse y)...
            spriteGufo.position = Vector2.Lerp(spriteGufo.position, flyingPoint.position, flyingSpeed * Time.deltaTime);
            //...aspetta del tempo...
            yield return new WaitForSeconds(0.01f);
            //...e fa ricominciare la coroutine
            StartCoroutine(FlyAround());

        } //altrimenti, il punto volante non andr� pi� in cerchio
        else { flyingPointAnim.SetBool("FlyingAround", false); }

    }

    private IEnumerator DiveAttack()
    {

        //FA PARTIRE L'ANIMAZIONE DI ANTICIPAZIONE AL TUFFO

        //aspetta che l'animazione di anticipazione al tuffo finisca
        yield return new WaitForSeconds(diveAnticipationTimer);

        //FA PARTIRE L'ANIMAZIONE DI TUFFO

        //il gufo si tuffa verso il giocatore
        rbGufo.velocity = (staticPlayer.position - transform.position).normalized * diveSpeed;
        //salva il punto in cui il gufo deve tuffarsi
        divingPoint = staticPlayer.position;
        //comunica che il gufo si sta tuffando
        isDiving = true;
        //comunica che non sta pi� volando
        isFlying = false;
        //Debug.Log("Dive attack");
    }

    private IEnumerator Landed()
    {
        //ferma il nemico
        rbGufo.velocity = Vector2.zero;
        //comunica che ha smesso di tuffarsi
        isDiving = false;
        //rende di nuovo solido il collider del gufo
        collGufo.isTrigger = false;
        Debug.Log("Landed");
        //aspetta del tempo e...
        yield return new WaitForSeconds(afterLandingCD);
        //...il gufo potr� attaccare di nuovo
        isAttacking = false;
        //Debug.Log("Can attack again -> diving: " + isDiving + " jumped: " + jumped + " isFlying: " + isFlying);
    }

    public void IsStunned(bool stunned)
    {
        //fema tutte le coroutine di questo script
        StopAllCoroutines();
        //resetta lo stato del gufo
        isDiving = false;
        isAttacking = false;
        isFlying = false;
        //comunica se il nemico � stordito o meno
        isStunned = stunned;

    }

}
