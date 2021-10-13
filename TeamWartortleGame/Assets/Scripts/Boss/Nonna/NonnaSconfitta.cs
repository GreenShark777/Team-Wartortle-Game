using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonnaSconfitta : NonnaAbstract
{
    //Prendo il manager
    [HideInInspector]
    public NonnaContext nonnaManager;
    //Timer per riportare il boss alla sua posizione iniziale
    private float timer = 0;
    //Prendo l'animator per eseguire l'animazione di attacco
    private Animator bossAn;
    //Posizione iniziale
    Vector3 startPos;
    public override void StateEnter()
    {
        //Ottengo la posizione iniziale
        startPos = transform.position;
        //Disattivo la velocit� cos� il boss non pu� pi� muoversi
        nonnaManager.speed = 0;
        //Prendo l'animator
        this.bossAn = nonnaManager.bossAn;
        //Attivo l'animazione di sconfitta
        bossAn.SetTrigger("Defeat");
    }

    public override void StateUpdate()
    {
        //Finch� il timer � sotto il primo secondo posiziono il boss alla sua posizione iniziale
        if (timer < 1)
        {
            timer += Time.deltaTime / 1;
            transform.position = Vector3.Lerp(startPos, nonnaManager.startPos, timer);
        }
        else
        {
            //Faccio apparire la nonna fantasma
        }

        //Se l'animazione di morte � finita posso disattivare interamente questo GameObject
        if (bossAn.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !bossAn.IsInTransition(0))
            gameObject.SetActive(false);
    }

    public override void StateFixedUpdate() { }

    public override void StateTriggerEnter(Collider2D collision) { }

    public override void StateTriggerExit(Collider2D collision) { }

    public override void StateCollisionEnter(Collision2D collision) { }

    public override void StateCollisionExit(Collision2D collision) { }

    public override void StateExit() { }
}
