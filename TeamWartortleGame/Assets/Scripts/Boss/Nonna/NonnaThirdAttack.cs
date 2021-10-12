using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonnaThirdAttack : NonnaAbstract
{
    //Prendo il manager
    [HideInInspector]
    public NonnaContext nonnaManager;
    public override void StateEnter()
    {

    }

    public override void StateUpdate()
    {

    }

    public override void StateFixedUpdate() { }

    public override void StateTriggerEnter(Collider2D collision) { }

    public override void StateTriggerExit(Collider2D collision) { }

    public override void StateCollisionEnter(Collision2D collision) { }

    public override void StateCollisionExit(Collision2D collision) { }

    public override void StateExit() { }
}
