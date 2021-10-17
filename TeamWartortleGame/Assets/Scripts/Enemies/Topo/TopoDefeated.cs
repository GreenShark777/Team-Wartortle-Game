using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopoDefeated : TopoAbstract
{
    //GameObject padre dei testi di scelta del nemico
    [SerializeField]
    private GameObject chooseGB;

    //Prendo i fillImage delle due scelte(purificazione e esecuzione)
    private Image fillPurifica, fillEsecuzione;

    //Timer iniziale per il caricamento della barra del pulsante che si decide premere per il nemico
    float timerConverted = 0;

    //Booleana che decide se il GameObject della scelta può essere attivata, verrà impostata a false quando la scelta verrà effettuata
    private bool canActiveChoose = true;

    //Reference manager
    [HideInInspector]
    public TopoManagerSTM topoManager;

    public override void StateEnter()
    {
        topoManager.rb.velocity = Vector3.zero;
        topoManager.rb.angularVelocity = 0;

    }

    public override void StateUpdate()
    {
        topoManager.rb.velocity = Vector3.zero;
        topoManager.rb.angularVelocity = 0;
        Debug.Log("Prova");

    }

    public override void StateFixedUpdate() { }

    public override void StateTriggerEnter(Collider2D collision) {

    }

    public override void StateTriggerExit(Collider2D collision) {
    }

    public override void StateCollisionEnter(Collision2D collision) { }

    public override void StateCollisionExit(Collision2D collision) { }

    public override void StateExit() { }
}
