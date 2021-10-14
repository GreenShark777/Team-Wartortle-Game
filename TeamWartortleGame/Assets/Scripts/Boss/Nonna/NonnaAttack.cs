using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonnaAttack : NonnaAbstract
{
    //Prendo il manager
    [HideInInspector]
    public NonnaContext nonnaManager;
    //Prendo l'animator per eseguire l'animazione di attacco
    private Animator bossAn;
    //Timer per tenere chiusa la bocca prima di sparare
    private float startTime, timerToReach = .6f;
    //Bocca chiusa e bocca aperta reference
    private GameObject boccaDefault, boccaChiusa, boccaAperta;
    public override void StateEnter()
    {
        //Inizializzo il timer a quello corrente
        startTime = Time.time;
        //Assegno l'animatore di questo script prendendolo dal manager
        this.bossAn = nonnaManager.bossAn;
        //Assegno le bocche
        this.boccaDefault = nonnaManager.boccaDefault;
        this.boccaChiusa = nonnaManager.boccaChiusa;
        this.boccaAperta = nonnaManager.boccaAperta;
        //Imposto la bossa chiusa come attiva e disattivo la bocca aperta visto che il boss sta caricando i proiettili
        boccaChiusa.SetActive(true);
        boccaDefault.SetActive(false);
        boccaAperta.SetActive(false);
        //Attivo animazione di sparo
        bossAn.SetTrigger("Shoot");
    }

    public override void StateUpdate()
    {
        //Se il timer è stato raggiunto
        if (Time.time - startTime >= timerToReach)
        {
            //Faccio aprire la bocca al boss, quindi attivo il gameobject bocca aperta e disattivo quello con la bocca chiusa
            boccaChiusa.SetActive(false);
            boccaAperta.SetActive(true);
            //Posso sparare a attivare l'animazione di sparo
            //bossAn.SetTrigger("Shoot");
            ObjectPooling.inst.SpawnObjectFromPool("Scheggia", nonnaManager.shootPos.position, Quaternion.identity);
            //Ritorno allo stato idle della nonna
            nonnaManager.SwitchState(nonnaManager.nonnaIdle);
        }
    }

    public override void StateFixedUpdate() { }

    public override void StateTriggerEnter(Collider2D collision) { }

    public override void StateTriggerExit(Collider2D collision) { }

    public override void StateCollisionEnter(Collision2D collision) { }

    public override void StateCollisionExit(Collision2D collision) { }

    public override void StateExit() 
    {
        nonnaManager.Invoke("DefaultMouth", 1f);
    }

}
