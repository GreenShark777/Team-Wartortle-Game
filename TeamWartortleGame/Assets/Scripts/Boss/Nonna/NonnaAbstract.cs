using UnityEngine;

public abstract class NonnaAbstract : MonoBehaviour
{
    public abstract void StateEnter();

    public abstract void StateUpdate();

    public abstract void StateFixedUpdate();

    public abstract void StateTriggerEnter(Collider2D collision);

    public abstract void StateTriggerExit(Collider2D collision);

    public abstract void StateCollisionEnter(Collision2D collision);

    public abstract void StateCollisionExit(Collision2D collision);

    public abstract void StateExit();
}
