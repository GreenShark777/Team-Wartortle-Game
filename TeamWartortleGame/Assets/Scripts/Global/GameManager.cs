using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static UnityEngine.Experimental.Rendering.Universal.Light2D;

public class GameManager : MonoBehaviour
{
    //Singleton pattern
    public static GameManager inst;

    //Riferimento al weaponcontainer per ottenere alcune sue informazioni sulla direzione
    [SerializeField]
    private WeaponsContainer weaponContainer;

    //Riferimento al player
    [SerializeField]
    private GameObject player;

    //Nemico corrente per la decisione(esecuzione o purificazione)
    [HideInInspector]
    public GameObject currentEnemy = null;

    //GameObject della purificazione(spada) e esecuzione(croce)
    [SerializeField]
    private GameObject spada, croce;

    //Animator del messaggio che appare in alto quando aumenta il danno o il personaggio subisce una trasformazione
    [SerializeField]
    private Animator msgAn;

    //Riferimenti alle weaponStats della pistola e della spada
    [SerializeField]
    private WeaponStats gunStats, swordStats;

    //Array degli sprite correnti delle teste del giocatore
    [SerializeField]
    private SpriteRenderer[] currentHeads;

    //Array delle faccie angelo e demone
    [SerializeField]
    private Sprite[] angelHeads, demonHeads;

    //Booleana che dice se il personaggio si � transformato o no
    [SerializeField]
    private bool transformated = false;

    //Booleane delle transformazioni
    public bool angel = false, demon = false;

    //Riferimento allo UI manager per effettuare i cambi visivi delle statistiche
    [SerializeField]
    private PlayerUIManager playerUI;

    //Riferimento alla couroitine della decisione del nemico per poterla interrompere quella precedente per richiamarla di nuovo cos� da evitare conflitti
    private Coroutine enemyDecisionCor = default;

    private void Start()
    {
        //Singleton pattern
        inst = this;
    }

    public Vector2 GetGunDirection()
    {
        //Se lo script weapon container � presente ritorno la direzione dell'arma chiamando a sua volta un'altro metodo
        if (weaponContainer)
            return weaponContainer.GetGunDirection();
        //Altrimenti ritorno un valore default di vector2
        else return default;
    }

    //Metodo per la scelta che il giocatore prende per il nemico, viene passato il nemico e il valore(0 purificazione e 1 per esecuzione)
    public void SceltaPerNemico(GameObject enemy, int value, EnemiesHealth enHealth)
    {
        if (enemyDecisionCor == null)
            enemyDecisionCor = StartCoroutine(IEnemyDecision(enemy, value, enHealth));
    }

    private IEnumerator IEnemyDecision(GameObject enemy, int value, EnemiesHealth enHealth)
    {
        //Riferimento all'animator del player
        Animator anPlayer = player.GetComponent<Animator>();
        //Ottengo tutti i suoi sprite renderer per colorarli di nero
        SpriteRenderer[] enemySprites = enemy.GetComponentsInChildren<SpriteRenderer>(true);
        //Timer per il cambio dei colori
        float timer = 0;
        //Colore corrente
        Color currentColor = enHealth.startColor;
        //Se ho scelto purificazione
        if (value == 0)
        {
            //Diminuisco la malizia di 25
            playerUI.ChangeMaliciousnessBar(playerUI.GetMaliciousness() - 25);
            //Eseguo l'animazione
            anPlayer.SetTrigger("Execution");
            //Attivo la croce
            croce.SetActive(true);
            //La posiziono sopra il nemico
            croce.transform.parent.position = enemy.transform.position + Vector3.up * 3;
            while (timer < 1)
            {
                //Aumento il timer, il tutto verr� effettuato in un secondo
                timer += Time.deltaTime / 1f;
                //Assegno il colore a quello corrente
                currentColor = Color.Lerp(enHealth.startColor, Color.cyan, timer);
                //e assegno il colore raggiunto allo sprite corrente del nemico

                //Per ogni sprite del nemico
                for (int i = 0; i < enemySprites.Length; i++)
                {
                    //Assegno il colore raggiunto
                    enemySprites[i].color = currentColor;

                }
                yield return null;
            }

            //Assegno il colore corrente al nemico
            enHealth.currentColor = currentColor;
            //Riazzero il timer per poterlo riutilizzare
            timer = 0;
        }
        //Altrimenti se ho scelto Esecuzione
        else 
        {
            //Aumento la malizia di 25
            playerUI.ChangeMaliciousnessBar(playerUI.GetMaliciousness() + 25);
            anPlayer.SetTrigger("Purify");
            //Attivo la spada
            spada.SetActive(true);
            //La posiziono sopra il nemico
            spada.transform.parent.position = enemy.transform.position + Vector3.up * 3;
            while (timer < 1)
            {
                //Aumento il timer, il tutto verr� effettuato in un secondo
                timer += Time.deltaTime / 1f;
                //Assegno il colore a quello corrente
                currentColor = Color.Lerp(enHealth.startColor, Color.black, timer);
                //e assegno il colore raggiunto allo sprite corrente del nemico

                //Per ogni sprite del nemico
                for (int i = 0; i < enemySprites.Length; i++)
                {
                    //Assegno il colore raggiunto
                    enemySprites[i].color = currentColor;

                }
                yield return null;
            }

            //Assegno il colore corrente al nemico
            enHealth.currentColor = currentColor;
            //Riazzero il timer per poterlo riutilizzare
            timer = 0;
            


        }

        //Aspetto un secondo
        yield return new WaitForSeconds(1);

        //Colore corrente
        currentColor = enHealth.currentColor;
        //Colore corrente ma con trasparenza
        Color fadeColor = currentColor;
        fadeColor.a = 0;
        //In mezzo secondo
        while (timer < 1)
        {
            //Aumento il timer, il tutto verr� effettuato in un mezzo secondo
            timer += Time.deltaTime / 1f;
            //Assegno il colore a quello corrente
            currentColor = Color.Lerp(enHealth.currentColor, fadeColor, timer);
            //e assegno il colore raggiunto allo sprite corrente del nemico

            //Per ogni sprite del nemico
            for (int i = 0; i < enemySprites.Length; i++)
            {
                //Assegnoi il colore raggiunto
                enemySprites[i].color = currentColor;

            }
            yield return null;
        }
        //Disattivo la spada e la croce se attivi
        if (spada.activeSelf) spada.SetActive(false);
        if (croce.activeSelf) croce.SetActive(false);
        //Disattivo il nemico
        enemy.SetActive(false);
        //Resetto la coroutine corrente a null cos� da poter essere richiamata
        enemyDecisionCor = null;
        yield return null;
    }

    //Gestisce il cambio di statistiche e la transformazione del personaggio in angelo o demone
    public void ChangeStats(float gunDmg = 0, float swordDmg = 0, bool angel = false, bool demon = false, string msg = default)
    {
        //Se non mi devo transformare
        if (!angel && !demon)
        {
            //Se il danno della pistola passato � pi� forte della spada vuol dire che sto modificando il danno della pistola
            if (gunDmg > swordDmg)
            {
                //Aumento il danno della pistola
                gunStats.ChangeAttackStat(gunDmg);
            }
            //Altrimenti sto aumentando il danno della spada
            else 
            {
                //Aumento il danno della spada
                swordStats.ChangeAttackStat(swordDmg);
            }

        }
        //Altrimenti se sto subendo una transformazione e non sono gi� transformato
        else if ((angel || demon) && !transformated)
        {
    
            //La transformazione � avvenuta quindi non posso ripeterla
            transformated = true;
            //Se mi sto transformando in un angelo
            if (angel)
            {
                //Imposto la booleana della transformazione della angelo a true
                this.angel = angel;
                //Metto il demone al contrario invece
                this.demon = !angel;
                //Per ogni testa(side, front e back)
                for (int i=0; i<currentHeads.Length; i++)
                {
                    //Assegno in ordine le nuove teste angelo
                    currentHeads[i].sprite = angelHeads[i];
                }
            }
            //Altrimenti se mi sto transformando in un demone
            else
            {
                //Imposto la booleana della transformazione della angelo a true
                this.demon = demon;
                //Metto il demone al contrario invece
                this.angel = !demon;
                //Per ogni testa(side, front e back)
                for (int i = 0; i < currentHeads.Length; i++)
                {
                    //Assegno in ordine le nuove teste demone
                    currentHeads[i].sprite = demonHeads[i];
                }
            }
        }

        //Mostro il messaggio passando la stringa 
        msgAn.GetComponentInChildren<TextMeshProUGUI>().text = msg;
        //Attivo inoltre l'animazione del texto
        msgAn.SetTrigger("On");
    }

    //Metodo chiamato dallo script enemy decision per controllare se la coroutine che applica la sorte di un nemico e finita o no
    //se � finita ritorner� true e potr� quindi apparire di nuovo il pannello per la decisione
    public bool CanActiveForDecision()
    {
        //Se la coroutine non c'� in questo momento ritorna true
        if (enemyDecisionCor == null)
            return true;
        //Altrimenti ritorna false perch� vuol dire che si sta eseguendo
        else return false;
    }
}
