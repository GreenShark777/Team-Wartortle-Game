using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonnaIdle : NonnaAbstract
{
    //Manager della nonna
    [HideInInspector]
    public NonnaContext nonnaManager;

    //Timer per passare all'attacco
    private float startTime, timerToReach;

    //Booleana che controlla se si è arrivati alla seconda fase
    private bool secondPhase = false;
    public override void StateEnter()
    {
        //Inizializzo randomicamente il timer da raggiungere tra 1 a 2 secondi
        timerToReach = Random.Range(2, 3);
        //Imposto lo startTime a quello corrente del Time.time
        startTime = Time.time;
    }

    public override void StateUpdate()
    {

    
        //Se ancora non sono alla seconda fase ma la vita mi dice che è arrivata alla seconda fase
        if (!secondPhase && nonnaManager.GetSecondPhase())
        {
            //Eseguo lo state machine della transizione
            nonnaManager.SwitchState(nonnaManager.nonnaTransition);
            //Imposto a true la seconda fase così che per la prossima esecuzione di questo stm ci sarà l'attacco della seconda fase
            secondPhase = true;
        }
        //Controllo se il timer di attacco è stato raggiunto
        else if (Time.time - startTime >= timerToReach)
        {
          
            //Chiamo l'attacco passando la seconda fase come booleana
            Attack(secondPhase);
        }
    }

    public override void StateFixedUpdate() { }

    public override void StateTriggerEnter(Collider2D collision) { }

    public override void StateTriggerExit(Collider2D collision) { }

    public override void StateCollisionEnter(Collision2D collision) { }

    public override void StateCollisionExit(Collision2D collision) { }

    public override void StateExit() { }

    private void Attack(bool secondPhase)
    {
        //Se non sono alla seconda fase
        if (!secondPhase)
        {
            //con la probabilità del 80%
            if (Random.value < .8)
            {
                //Eseguo l'attacco scheggia(primo attacco)
                nonnaManager.SwitchState(nonnaManager.nonnaAttack);
            }
            else
            {
                //Eseguo l'attacco veleno(secondo attacco)
                nonnaManager.SwitchState(nonnaManager.nonnaSecondAttack);
            }
        }
        else
        {
            //con la probabilità del 30%
            if (Random.value < .3)
            {
                //Eseguo l'attacco scheggia(primo attacco)
                nonnaManager.SwitchState(nonnaManager.nonnaAttack);
            }
            else if (Random.value < .5)
            {
                //Eseguo l'attacco veleno(secondo attacco)
                nonnaManager.SwitchState(nonnaManager.nonnaSecondAttack);
            } else
            {
                //Eseguo l'attacco pioggia di scheggie(terzo attacco)
                nonnaManager.SwitchState(nonnaManager.nonnaThirdAttack);
            }
        }
    }

}
