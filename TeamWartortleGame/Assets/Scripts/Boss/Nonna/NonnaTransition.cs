using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonnaTransition : NonnaAbstract
{
    //Prendo il manager
    [HideInInspector]
    public NonnaContext nonnaManager;
    //Rereference all'animator
    private Animator bossAn;
    //Timer per prendere la fine dell'animazione e timer di durata dell'animazione
    private float timer = 0, timerToReach;
    //Riferimento alle pietre da far arrivare a terra
    private GameObject pietre;
    public override void StateEnter()
    {
        //Prendo l'animator dal manager
        this.bossAn = nonnaManager.bossAn;
        //Attivo il trigger d'animazione per la transizione
        //bossAn.SetTrigger("Transition");
        //Ottengo la durata dell'animazione
        timerToReach = bossAn.GetCurrentAnimatorStateInfo(0).length;
        //Prendo le pietre dal manager
        this.pietre = nonnaManager.pietre;
    }

    public override void StateUpdate()
    {
        //Se il timer non ha raggiunto il picco dell'animazione
        if (timer < timerToReach)
        {
            //Aumento il timer in formula di secondi
            timer += Time.deltaTime / 1f;
        }
        //altrimenti se il timer ha superato il picco
        else
        {
            //Attivo le pietre
            pietre.SetActive(true);
            //Ritorno allo stato Idle
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
