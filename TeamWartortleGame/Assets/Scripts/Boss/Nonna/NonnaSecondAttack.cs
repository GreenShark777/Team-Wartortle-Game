using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonnaSecondAttack : NonnaAbstract
{    

    //Prendo il manager
    [HideInInspector]
    public NonnaContext nonnaManager;
    //Prendo l'animator per eseguire l'animazione di attacco
    private Animator bossAn;
    //Timer per tenere chiusa la bocca prima di sparare
    private float timer = 0, timerToReach;
    //Bocca chiusa e bocca aperta reference
    private GameObject boccaDefault, boccaChiusa, boccaAperta;
    //Riferimento alla shootPosition del boss(bocca)
    private Transform shootPos;

    public override void StateEnter()
    {
        //Assegno l'animatore di questo script prendendolo dal manager
        this.bossAn = nonnaManager.bossAn;
        //Assegnor riferimento alla shootPos del boss
        this.shootPos = nonnaManager.shootPos;
        //Imposto l'animazione in hold delle nubi
        bossAn.SetTrigger("HoldPoison");
        //Prendo la lunghezza dell'animazione corrente, quella in cui carica la nube quindi e la passo come tempo da raggiungere prima di sparare
        //timerToReach = bossAn.GetCurrentAnimatorStateInfo(0).length;
        timerToReach = 1;

        Debug.Log(timerToReach);
    }

    public override void StateUpdate()
    {
        //Controllo se il timer è stato raggiunto
        if (timer < timerToReach)
        {
            timer += Time.deltaTime / 1;
            //Attivo l'animazione di sparo delle nubi
            //bossAn.SetTrigger("NubeShoot");
            //Creo un loop per sparare due nubi
            for (int i = 0; i < 2; i++)
            {
                //La istanzio
                GameObject temp = ObjectPooling.inst.SpawnObjectFromPool("Nube", shootPos.position, Quaternion.identity);
                //La posiziono prima a sinistra dello shootPos se i è a 0 mentre se è superiore alla destra
                temp.transform.position = shootPos.transform.TransformPoint(new Vector3(i == 0 ? -5 : 5, 0));
                Rigidbody2D rb = temp.GetComponent<Rigidbody2D>();
                //Gli applico una piccola spinta con il RigidBody
                rb.AddForce(i == 0 ? -transform.right * 5: transform.right * 5, ForceMode2D.Impulse);
                //Fermo tutte le velocità del rigidBody
                StartCoroutine(IStopRigidBodyVelocity(rb, 1));
            
            }
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

    private IEnumerator IStopRigidBodyVelocity(Rigidbody2D rb, float time)
    {
        yield return new WaitForSeconds(time);
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
        rb.angularDrag = 0;
    }
}
