using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonnaContext : MonoBehaviour
{
    //STM corrente basato sullo script padre da cui ereditano tutti gli stm
    [HideInInspector]
    public NonnaAbstract currentState;
    //Stato di idle
    [HideInInspector]
    public NonnaIdle nonnaIdle;
    //Stato attacco
    [HideInInspector]
    public NonnaAttack nonnaAttack;
    //Stato secondo attacco
    [HideInInspector]
    public NonnaSecondAttack nonnaSecondAttack;
    //Stato di transizione alla seconda fase
    [HideInInspector]
    public NonnaTransition nonnaTransition;
    //Stato terzo attacco
    [HideInInspector]
    public NonnaThirdAttack nonnaThirdAttack;
    //Stato di sconfitta
    [HideInInspector]
    public NonnaSconfitta nonnaDefeated;

    //LayerMask dei muri da cui dovrà rimbalzare
    public LayerMask groundMask;

    //Velocità di movimento
    public float speed = 8;

    //Reference animator
    public Animator bossAn;
    //Reference RigidBody per muoversi
    [SerializeField]
    private Rigidbody2D rb;

    //Riferimento allo script della vita per capire se il boss è stato sconfitto o no
    [HideInInspector]
    public BossHealth bossHealth;

    //Riferimento alla posizione di sparo(bocca)
    public Transform shootPos;

    //Riferimenti alla bocca chiusa e alla bocca aperta
    public GameObject boccaDefault, boccaChiusa, boccaAperta;

    //Riferimento alle pietre da far cadere nel momento della transizione
    public GameObject pietre;

    //Raycast per far cambiare direzione di movimento al boss
    private RaycastHit2D ray;

    //Posione iniziale del boss per poterla riposizionare dopo averlo sconfitto
    public Vector3 startPos;

    //Direzione di movimento del boss, inzialmente a destra e un timer per aspettare un po' per cominciare a muovere il boss
    int dir = 1;
    float timer = 0;

    private void Awake()
    {
        //Salvo la posizione iniziale
        startPos = transform.position;
        //Aggiungo tutti gli stm al gameObject 
        nonnaSecondAttack = GetComponent<NonnaSecondAttack>();
        nonnaThirdAttack = GetComponent<NonnaThirdAttack>();
        nonnaTransition = GetComponent<NonnaTransition>();
        nonnaDefeated = GetComponent<NonnaSconfitta>();
        nonnaAttack = GetComponent<NonnaAttack>();
        nonnaIdle = GetComponent<NonnaIdle>();

        //Passo questo script agli altri stati che ne hanno bisogno
        nonnaSecondAttack.nonnaManager = nonnaThirdAttack.nonnaManager = nonnaTransition.nonnaManager = nonnaDefeated.nonnaManager
        = nonnaAttack.nonnaManager = nonnaIdle.nonnaManager = this;

        //Prendo il riferimento dello script BossHealth
        bossHealth = GetComponent<BossHealth>();
    }

    private void Start()
    {
        //Assegno come STM iniziale l'idle
        currentState = nonnaIdle;
        //Chiamo il suo metodo start visto che mi trovo nel metodo start
        currentState.StateEnter();
    }

    private void Update()
    {
        //Chiamo il metodo Update dello stato corrente visto che mi trovo nell'update
        currentState.StateUpdate();

        CheckForMoving();

        //Se il boss è stato sconfitto cambio lo stato in quello di sconfitta
        if (bossHealth.IsEnemyDefeated() && currentState != nonnaDefeated)
        {
            SwitchState(nonnaDefeated);
        }
  
    }

    private void FixedUpdate()
    {
        //Chiamo il metodo FixedUpdate dello stato corrente visto che mi trovo nel FixedUpdate
        currentState.StateFixedUpdate();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Quando collido chiamo il metodo di collisione dello stato corrente
        currentState.StateTriggerEnter(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Quando esco dalla collisione chiamo il metodo di uscita di collisione dello stato corrente
        currentState.StateTriggerExit(collision);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Quando collido chiamo il metodo di collisione dello stato corrente
        currentState.StateCollisionEnter(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //Quando esco dalla collisione chiamo il metodo di uscita di collisione dello stato corrente
        currentState.StateCollisionExit(collision);
    }

    //Metodo che serve per cambiare stm
    public void SwitchState(NonnaAbstract stm)
    {
        //Chiamo il metodo di uscita prima di aggiornarlo
        currentState.StateExit();

        //Aggiorno lo stm
        currentState = stm;
        //chiamo il suo metodo StateEnter visto che è stato appena modificato lo stm
        currentState.StateEnter();
    }

    //Metodo che ritorna la seconda fase
    public bool GetSecondPhase()
    {
        return bossHealth.secondPhase;
    }

    //Metodo che controlla se il boss si trova alla seconda fase, e in caso si muove a destra e a sinistra attraverso raycast
    private void CheckForMoving()
    {
        //Se sono alla secnda fase e sono tornato all'animazione di idle
        if (GetSecondPhase())
        {
            //Aspetto un secondo
            if (timer < 2f)
            {
                timer += Time.deltaTime / 2;
            }
            //Dopo di che comincio a muovere il boss
            else
            {
                //Posso cominciare a muovermi verso una direzione, inizialmente destra e creare il raycast per controllare se sto toccando un muro
                rb.velocity = new Vector3(dir * speed, 0, 0);
                //Controllo se il raycast sta toccando un muro e in caso cambio direzione e lo flippo
                ray = Physics2D.Raycast(transform.position, Vector2.right * dir, 1f, groundMask);
                //Se ray è a true, quindi sta toccando un muro
                if (ray)
                {
                    //Flippo la direzione
                    dir *= -1;
                }
            }
        }
    }

    //Muove la transform verso una posizione
    public void MoveAbove()
    {
        StartCoroutine(IMoveAbove());
    }

    //Chiude la bocca della nonna
    public void DefaultMouth()
    {
        boccaDefault.SetActive(true);
        boccaChiusa.SetActive(false);
        boccaAperta.SetActive(false);
    }

    private IEnumerator IMoveAbove()
    {
        //Inizializzo il timer a 0
        float timer = 0, speed = 1.3f;
        //Prendo la posizione iniziale e la posizione finale
        Vector3 startPos = transform.position, targetPos = new Vector3(transform.position.x,3,transform.position.z);
        //Finché sono sotto il primo secondo
        while (timer < 1f)
        {
            //Aumento il timer secondo la formula secondi
            timer += Time.deltaTime / 1f;
            //Lerpo la posizione del boss a quella finale
;           transform.position = Vector3.Lerp(startPos, targetPos, timer * speed);
            yield return null;
        }
        yield return null;
    }
}
