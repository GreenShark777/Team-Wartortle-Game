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
    private float startTime, timerToReach = 1;
    public override void StateEnter()
    {
        //Inizializzo il timer a quello corrente
        startTime = Time.time;
        //Assegno l'animatore di questo script prendendolo dal manager
        this.bossAn = nonnaManager.bossAn;
        //Imposto l'animazione di chiusura della bocca
        //bossAn.SetTrigger("Hold");
    }

    public override void StateUpdate()
    {
        //Se il timer è stato raggiunto
        if (Time.time - startTime >= timerToReach)
        {
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

    public override void StateExit() { }
}
