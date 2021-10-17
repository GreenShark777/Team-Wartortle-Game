using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopoManagerSTM : MonoBehaviour
{
    //STM corrente basato sullo script padre da cui ereditano tutti gli stm
    [HideInInspector]
    public TopoAbstract currentState;
    //Stato di idle
    [HideInInspector]
    public TopoIdle topoIdle;
    //Stato di movimento
    [HideInInspector]
    public TopoMovement topoMovement;
    //Stato di sconfitta
    [HideInInspector]
    public TopoDefeated topoDefeated;

    //LayerMask del ground su cui potrà camminare e degli ostacoli che dovrà invece bloccarsi quando li toccherà con un raycast
    public LayerMask groundMask, obstacleMask;

    //Velocità di movimento
    public float speed = 8;

    //Reference animator
    [SerializeField]
    private Animator animator;
    //Reference RigidBody per muoversi
    public Rigidbody2D rb;
 

    //Direzione corrente
    private Vector2 movePos;

    //Posizione iniziale
    private Vector2 startPos;

    //Riferimento allo script della vita per capire se il nemico è stato sconfitto o no
    [HideInInspector]
    public EnemiesHealth enHealth;

    private void Awake()
    {
        //Memorizzo la posizione iniziale
        startPos = transform.position;

        //Aggiungo tutti gli stm al gameObject
        topoIdle = GetComponent<TopoIdle>();
        topoMovement = GetComponent<TopoMovement>();
        topoDefeated = GetComponent<TopoDefeated>();

        //Passo questo script agli altri stati che ne hanno bisogno
        topoIdle.topoManager = topoMovement.topoManager = topoDefeated.topoManager = this;

        //Prendo il riferimento dello script EnemiesHealth
        enHealth = GetComponent<EnemiesHealth>();
    }

    private void OnEnable()
    {
        //Lo sposto nella sua posizione inziale
        transform.position = startPos;
        //Assegno come STM iniziale l'idle
        currentState = topoIdle;
        //Chiamo il suo metodo start visto che mi trovo nel metodo start
        currentState.StateEnter();
    }

    private void Update()
    {
        //Chiamo il metodo Update dello stato corrente visto che mi trovo nell'update
        currentState.StateUpdate();


    }

    //private void FixedUpdate()
    //{
    //    //Chiamo il metodo FixedUpdate dello stato corrente visto che mi trovo nel FixedUpdate
    //    currentState.StateFixedUpdate();
    //}

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
    public void SwitchState(TopoAbstract stm)
    {
        //Chiamo il metodo di uscita prima di aggiornarlo
        currentState.StateExit();

        //Aggiorno lo stm
        currentState = stm;
        //chiamo il suo metodo StateEnter visto che è stato appena modificato lo stm
        currentState.StateEnter();
    }

    public void CheckAnimation(Vector2 movePos)
    {
        //Setto la velocità via animator per passare tra idle e movimento
        animator.SetFloat("Velocity", movePos.magnitude);

        //Se mi sto muovendo
        if (movePos.magnitude > 0)
        {
            //Aggiorno l'animator se mi muovo sulla X e azzero il movimento sulla Y
            if (Mathf.Abs(movePos.x) > Mathf.Abs(movePos.y))
            {
                animator.SetFloat("moveX", Mathf.Sign(movePos.x));
                animator.SetFloat("moveY", 0);
            }
            //Altrimenti lo aggiorno sulla Y e azzero il movimento sulla X
            else
            {
                animator.SetFloat("moveY", Mathf.Sign(movePos.y));
                animator.SetFloat("moveX", 0);
            }
        }
    }
   
}
