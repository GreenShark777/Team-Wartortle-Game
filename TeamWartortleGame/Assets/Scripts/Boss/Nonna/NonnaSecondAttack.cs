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
    private float timer, timerToReach;
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
        //Inizializzio il timer a 0
        timer = 0;
        //Inizializzo il timer da raggiungere
        timerToReach = .8f;
        //Attivo animazione di sparo della nube
        bossAn.SetTrigger("ShootNube");
    }

    public override void StateUpdate()
    {
        //Controllo se il timer è stato raggiunto
        if (timer < timerToReach)
        {
            timer += Time.deltaTime / 1;

        }
        //Altrimenti se il timer è stato raggiunto
        else
        {
            //Creo un loop per sparare due nubi
            for (int i = 0; i < 2; i++)
            {
                //La istanzio
                GameObject temp = ObjectPooling.inst.SpawnObjectFromPool("Nube", shootPos.position, Quaternion.identity);
                //La posiziono prima a sinistra dello shootPos se i è a 0 mentre se è superiore alla destra
                temp.transform.position = shootPos.transform.TransformPoint(new Vector3(i == 0 ? -5 : 5, 0));
                //Imposto la direzione a destra o a sinistra in base all'indice i
                temp.GetComponent<Rotation>().direction = i == 0 ? -1 : 1;
                Rigidbody2D rb = temp.GetComponent<Rigidbody2D>();
                //Gli applico una piccola spinta con il RigidBody
                rb.AddForce(i == 0 ? -transform.right * 5 : transform.right * 5, ForceMode2D.Impulse);
                //Fermo tutte le velocità del rigidBody e passo l'indice corrente per capire quale nube sto lanciando(La prima o la seconda)
                StartCoroutine(IStopRigidBodyVelocity(rb, 1, i));

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

    private IEnumerator IStopRigidBodyVelocity(Rigidbody2D rb, float time, int i)
    {
        yield return new WaitForSeconds(time);
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
        rb.angularDrag = 0;
        //Imposto un'offset per le nubi così da non farle sovrapporre
        rb.GetComponent<NubeHealth>().offset = new Vector3(i == 0 ? -1 : 1, 0);
    }
}
