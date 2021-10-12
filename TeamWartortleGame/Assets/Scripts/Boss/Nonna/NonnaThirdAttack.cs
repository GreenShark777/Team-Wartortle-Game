using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonnaThirdAttack : NonnaAbstract
{

    //Prendo il manager
    [HideInInspector]
    public NonnaContext nonnaManager;
    //Prendo l'animator per eseguire l'animazione di attacco
    private Animator bossAn;
    //Timer per tenere chiusa la bocca prima di sparare
    private float startTime, timerToReach;
    //Bocca chiusa e bocca aperta reference
    private GameObject boccaDefault, boccaChiusa, boccaAperta;
    //Riferimento alla shootPosition del boss(bocca)
    private Transform shootPos;
    //Index di proiettili
    private int i, maxBullet;
    public override void StateEnter()
    {
        //Inizializzo l'indice del numero di proiettili a 0
        i = 0;
        //Inizializzo in modo random il numero massimo di proiettili da sparare 
        maxBullet = Random.Range(30, 32);
        //Inizializzo il timer a quello corrente
        startTime = Time.time;
        //Assegno il timer da raggiungere prima di ripetere lo sparo(meno di mezzo secondo)
        timerToReach = .2f;
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
    }

    public override void StateUpdate()
    {
        //Controllo se l'index di sparo è stato superato
        if (i < maxBullet)
        {
            //Se il timer è stato raggiunto
            if (Time.time - startTime >= timerToReach)
            {
                //Faccio aprire la bocca al boss, quindi attivo il gameobject bocca aperta e disattivo quello con la bocca chiusa
                boccaChiusa.SetActive(false);
                boccaAperta.SetActive(true);
                //Posso sparare a attivare l'animazione di sparo
                //bossAn.SetTrigger("Shoot");
                GameObject temp = ObjectPooling.inst.SpawnObjectFromPool("Scheggia2", nonnaManager.shootPos.position, Quaternion.identity);
                //Aumento l'index di sparo
                i++;
                //Resetto il timer
                startTime = Time.time;
            }
        }
        //altrimenti posso ritornare allo stato idle
        else
        {
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
        Invoke("DefaultMouth", 1f);

    }

    private void DefaultMouth()
    {
        boccaDefault.SetActive(true);
        boccaChiusa.SetActive(false);
        boccaAperta.SetActive(false);
    }
}
