using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemiesDecision : MonoBehaviour
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

    //Reference allo script health del nemico
    [SerializeField]
    private EnemiesHealth enHealth;

    //Riferimento al player
    [SerializeField]
    private Transform playerTransform;
    private void Start()
    {
        //Ottengo i riferimenti via codice
        fillPurifica = chooseGB.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        fillEsecuzione = chooseGB.transform.GetChild(1).GetChild(0).GetComponent<Image>();
    }

    // Update is called once per frame
    private void Update()
    {
        //Se il nemico è stato sconfitto e controllo se posso attivare il pannello della decisione
        if (enHealth.IsEnemyDefeated() && GameManager.inst.CanActiveForDecision())
        {
            //Prendo il nemico corrente dal GameManager
            GameObject currentEnemy = GameManager.inst.currentEnemy;
            if (currentEnemy)
                if (!currentEnemy.activeSelf) GameManager.inst.currentEnemy = null;
            //Se sono dentro una distanza di 5 unità
            if (Vector2.Distance(transform.position, playerTransform.position) < 1f)
            {
                //Assegno il current enemy se non è già assegnato e attivo
                if (currentEnemy == null)
                {
                    //Assegno questo GameObject al current enemy 
                    GameManager.inst.currentEnemy = gameObject;
                }
                else if (GameObject.ReferenceEquals(gameObject, currentEnemy))
                {
                    //Se il nemico corrente è attivo
                    if (currentEnemy.activeSelf)
                    {
                        //Se posso attivare la scelta(verrà disattivata solo nel momento di fine dello script
                        if (canActiveChoose)
                        {
                            //Attivo la scelta
                            chooseGB.SetActive(true);
                        }
                    }
                }
            }
            else
            {
                //Altrimenti se sono fuori di 1 metri al GameObject corrente
                if (GameObject.ReferenceEquals(gameObject, currentEnemy))
                {
                    chooseGB.SetActive(false);
                    GameManager.inst.currentEnemy = null;
                }
            }
            //Controllo che il gameobject di scelta sia attivo e che quindi sia abbastanza vicino da effettuarla
            //e controllo inoltre che il gameObject sia quello corrente, quindi quello più vicino
            if (chooseGB.activeSelf && GameObject.ReferenceEquals(gameObject, GameManager.inst.currentEnemy))
            {
                //Registro il tempo corrente se sto iniziando a premere uno dei due pulsanti o li sto rilasciando
                if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.X) || Input.GetKeyUp(KeyCode.Z) || Input.GetKeyUp(KeyCode.X))
                {
                    //Azzero tutto
                    timerConverted = 0;
                    fillPurifica.fillAmount = 0;
                    fillEsecuzione.fillAmount = 0;
                }

                //Se sono sotto il primo secondo
                if (timerConverted < 1f)
                {
                    //Aumento la barra di Q se sto premendo Q(Purificazione)
                    if (Input.GetKey(KeyCode.Z))
                    {
                        //Lerpo il fill della purificazione e mezzo secondo
                        timerConverted += Time.deltaTime / .5f;
                        fillPurifica.fillAmount = Mathf.Lerp(0, 1, timerConverted);
                        //Altrimenti aumento la barra di E se sto premendo E(Esecuzione)
                    }
                    else if (Input.GetKey(KeyCode.X))
                    {
                        //Lerpo il fill della purificazione e mezzo secondo
                        timerConverted += Time.deltaTime / .5f;
                        fillEsecuzione.fillAmount = Mathf.Lerp(0, 1, timerConverted);
                    }
                }
                //altrimenti, se il timer è stato superato
                else
                {
                    //Disattivo il chooseGB
                    chooseGB.SetActive(false);
                    //Non si può riattivare l'HUD della scelta
                    canActiveChoose = false;
                    //Se la purifica è più alta dell'esecuzione
                    if (fillPurifica.fillAmount > fillEsecuzione.fillAmount) GameManager.inst.SceltaPerNemico(gameObject, 0, enHealth);
                    //Altrimenti esecuzione
                    else GameManager.inst.SceltaPerNemico(gameObject, 1, enHealth);

                    //Elimino in ogni caso il nemico corrente visto che non c'è più
                    GameManager.inst.currentEnemy = null;
                }
            }
        }
    }
}