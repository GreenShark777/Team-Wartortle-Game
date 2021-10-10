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
        //Ottengo i riferimenti via codice
        //fillPurifica = chooseGB.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        //fillEsecuzione = chooseGB.transform.GetChild(1).GetChild(0).GetComponent<Image>();
    }

    public override void StateUpdate()
    {
        //Controllo che il gameobject di scelta sia attivo e che quindi sia abbastanza vicino da effettuarla
        //if (chooseGB.activeSelf)
        //{
        //    //Registro il tempo corrente se sto iniziando a premere uno dei due pulsanti o li sto rilasciando
        //    if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyUp(KeyCode.Q) || Input.GetKeyUp(KeyCode.E))
        //    {
        //        //Azzero tutto
        //        timerConverted = 0;
        //        fillPurifica.fillAmount = 0;
        //        fillEsecuzione.fillAmount = 0;
        //    }

        //    //Se sono sotto il primo secondo
        //    if (timerConverted < 1f)
        //    {
        //        //Aumento la barra di Q se sto premendo Q(Purificazione)
        //        if (Input.GetKey(KeyCode.Q))
        //        {
        //            //Lerpo il fill della purificazione e mezzo secondo
        //            timerConverted += Time.deltaTime / .5f;
        //            fillPurifica.fillAmount = Mathf.Lerp(0, 1, timerConverted);
        //        //Altrimenti aumento la barra di E se sto premendo E(Esecuzione)
        //        } else if (Input.GetKey(KeyCode.E))
        //        {
        //            //Lerpo il fill della purificazione e mezzo secondo
        //            timerConverted += Time.deltaTime / .5f;
        //            fillEsecuzione.fillAmount = Mathf.Lerp(0, 1, timerConverted);
        //        }
        //    }
        //    //altrimenti, se il timer è stato superato
        //    else
        //    {
        //        chooseGB.SetActive(false);
        //        canActiveChoose = false;
        //        if (fillPurifica.fillAmount > fillEsecuzione.fillAmount) GameManager.inst.SceltaPerNemico(this.gameObject,0, topoManager.enHealth);
        //        else GameManager.inst.SceltaPerNemico(this.gameObject, 1, topoManager.enHealth);
        //    }
        //}
    }

    public override void StateFixedUpdate() { }

    public override void StateTriggerEnter(Collider2D collision) {
        //    //Attivo il GameObject delle scelte, nel caso in cui non sia già attivato
        //    if (collision.CompareTag("Player") && canActiveChoose)
        //        if (!chooseGB.activeSelf) chooseGB.SetActive(true);
    }

    public override void StateTriggerExit(Collider2D collision) {
        //Dasattivo il GameObject delle scelte, nel caso in cui non sia già disattivato
        //if (collision.CompareTag("Player"))
        //    if (chooseGB.activeSelf) chooseGB.SetActive(false);
    }

    public override void StateCollisionEnter(Collision2D collision) { }

    public override void StateCollisionExit(Collision2D collision) { }

    public override void StateExit() { }
}
