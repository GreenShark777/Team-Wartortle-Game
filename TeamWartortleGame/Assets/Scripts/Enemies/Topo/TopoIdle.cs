using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopoIdle : TopoAbstract
{
    //Variabili di timer di attesa prima di muoversi
    private float startTime, shootStartTimer, timerToReach = 2;

    private GameObject[] projectiles = new GameObject[2];

    //Script manager da cui chiamare i suoi metodi
    private TopoManagerSTM topoManager;

    //Game object che controller� quale tra orizzontale e verticale � attivo e decidera di conseguenza la direzione dei proiettili
    //In direzione verticale spara in basso e in alto e a destra e sinistra in direzione orizzontale
    private bool horizontal;

    private Transform shootPos;

    public override void StateEnter() {
        //Inizializzo lo startTime al tempo corrente
        startTime = shootStartTimer = Time.time;
        //Prendo lo script manager
        topoManager = GetComponent<TopoManagerSTM>();
        //Il gameobject orizzontale � attivo, se si lo prendo come current
        if (transform.GetChild(0).GetChild(0).gameObject.activeSelf)
            horizontal = true;
        else horizontal = false;

        // prima ottengo il figlio "Parent" dopo di che ottengo il suo ultimo figlio, "Fiamma ratto"
        shootPos = transform.GetChild(0).GetChild(transform.GetChild(0).transform.childCount - 1);

    }

    public override void StateUpdate() {

        Shoot();

        if (Time.time - startTime > timerToReach)
        {
            //Passo allo stato movement del topo
            topoManager.SwitchState(topoManager.topoMovement);
            //Reimposto il timer a quello corrente cos� che la condizione si riazzeri
            startTime = Time.time;
        }
    }

    public override void StateFixedUpdate() { }

    public override void StateTriggerEnter(Collider2D collision) { }

    public override void StateTriggerExit(Collider2D collision) { }

    public override void StateCollisionEnter(Collision2D collision) { }

    public override void StateCollisionExit(Collision2D collision) { }

    public override void StateExit() { }

    private void Shoot()
    {
        //Quando il tempo corrente azzerato � maggiore del tempo da raggiungere
        if (Time.time - shootStartTimer > timerToReach / 2)
        {
            //Instanzio i proiettili dall'object pooling
            projectiles[0] = ObjectPooling.inst.SpawnFromPool("Fiamma", shootPos.position, Quaternion.identity);
            projectiles[1] = ObjectPooling.inst.SpawnFromPool("Fiamma", shootPos.position, Quaternion.identity);
            //Se sono in orizzontale sparo a destra e a sinistra
            if (horizontal)
            {
                projectiles[0].GetComponent<Rigidbody2D>().AddForce(Vector2.left * 5f, ForceMode2D.Impulse);
                projectiles[1].GetComponent<Rigidbody2D>().AddForce(Vector2.right * 5f, ForceMode2D.Impulse);
            }
            //Altrimenti sparo sopra e in basso
            else
            {
                projectiles[0].GetComponent<Rigidbody2D>().AddForce(Vector2.up * 5f, ForceMode2D.Impulse);
                projectiles[1].GetComponent<Rigidbody2D>().AddForce(Vector2.down * 5f, ForceMode2D.Impulse);
            }
            //Imposto il timer ad un valore irraggiungibile
            shootStartTimer = 9999;
        }
    }
}
