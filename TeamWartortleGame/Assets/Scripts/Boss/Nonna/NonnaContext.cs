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
    [SerializeField]
    private Animator animator;
    //Reference RigidBody per muoversi
    [SerializeField]
    private Rigidbody2D rb;

    //Riferimento allo script della vita per capire se il boss è stato sconfitto o no
    [HideInInspector]
    public BossHealth bossHealth;

    private void Awake()
    {
        //Aggiungo tutti gli stm al gameObject 
        nonnaSecondAttack = GetComponent<NonnaSecondAttack>();
        nonnaThirdAttack = GetComponent<NonnaThirdAttack>();
        nonnaTransition = GetComponent<NonnaTransition>();
        nonnaDefeated = GetComponent<NonnaSconfitta>();
        nonnaAttack = GetComponent<NonnaAttack>();
        nonnaIdle = GetComponent<NonnaIdle>();

        //Passo questo script agli altri stati che ne hanno bisogno

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

}
