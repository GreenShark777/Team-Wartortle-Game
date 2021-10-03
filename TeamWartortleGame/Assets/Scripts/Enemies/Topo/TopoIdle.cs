using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopoIdle : TopoAbstract
{
    //Variabili di timer di attesa prima di muoversi
    private float startTime, timerToReach = 2;

    //Script manager da cui chiamare i suoi metodi
    private TopoManagerSTM topoManager;

    public override void StateEnter() {
        //Inizializzo lo startTime al tempo corrente
        startTime = Time.time;
        //Prendo lo script manager
        topoManager = GetComponent<TopoManagerSTM>();
    }

    public override void StateUpdate() {
        //Quando il tempo corrente azzerato è maggiore del tempo da raggiungere
        if (Time.time - startTime > timerToReach)
        {
            //Passo allo stato movement del topo
            topoManager.SwitchState(topoManager.topoMovement);
            //Reimposto il timer a quello corrente così che la condizione si riazzeri
            startTime = Time.time;
        }
    }

    public override void StateFixedUpdate() { }

    public override void StateTriggerEnter(Collider2D collision) { }

    public override void StateTriggerExit(Collider2D collision) { }

    public override void StateCollisionEnter(Collision2D collision) { }

    public override void StateCollisionExit(Collision2D collision) { }

    public override void StateExit() { }
}
