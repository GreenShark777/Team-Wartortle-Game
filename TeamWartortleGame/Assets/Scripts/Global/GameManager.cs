using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private void Start()
    {
        //Singleton pattern
        inst = this;
    }

    public Vector2 GetGunDirection()
    {
        //Se lo script weapon container è presente ritorno la direzione dell'arma chiamando a sua volta un'altro metodo
        if (weaponContainer)
            return weaponContainer.GetGunDirection();
        //Altrimenti ritorno un valore default di vector2
        else return default;
    }

    //Metodo per la scelta che il giocatore prende per il nemico, viene passato il nemico e il valore(0 purificazione e 1 per esecuzione)
    public void SceltaPerNemico(GameObject enemy, int value, EnemiesHealth enHealth)
    {
        StartCoroutine(IEnemyDecision(enemy, value, enHealth));
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
            anPlayer.SetTrigger("Purify");
            //Attivo la spada
            spada.SetActive(true);
            //La posiziono sopra il nemico
            spada.transform.parent.position = enemy.transform.position + Vector3.up * 3;
            while (timer < 1)
            {      
                //Aumento il timer, il tutto verrà effettuato in un secondo
                timer += Time.deltaTime / 1f;
                //Assegno il colore a quello corrente
                currentColor = Color.Lerp(enHealth.startColor, Color.black, timer);
                //e assegno il colore raggiunto allo sprite corrente del nemico

                //Per ogni sprite del nemico
                for (int i = 0; i < enemySprites.Length; i++)
                {
                    //Assegnoi il colore raggiunto
                    enemySprites[i].color = currentColor;

                }
                yield return null;
            }

            //Assegno il colore corrente al nemico
            enHealth.currentColor = currentColor;
            //Riazzero il timer per poterlo riutilizzare
            timer = 0;
   
        }
        //Altrimenti se ho scelto esecuzione
        else 
        {
            anPlayer.SetTrigger("Execution");
            //Attivo la spada
            croce.SetActive(true);
            //La posiziono sopra il nemico
            croce.transform.parent.position = enemy.transform.position + Vector3.up * 3;
            while (timer < 1)
            {
                //Aumento il timer, il tutto verrà effettuato in un secondo
                timer += Time.deltaTime / 1f;
                //Assegno il colore a quello corrente
                currentColor = Color.Lerp(enHealth.startColor, Color.red, timer);
                //e assegno il colore raggiunto allo sprite corrente del nemico

                //Per ogni sprite del nemico
                for (int i = 0; i < enemySprites.Length; i++)
                {
                    //Assegnoi il colore raggiunto
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
        while (timer < .5)
        {
            //Aumento il timer, il tutto verrà effettuato in un mezzo secondo
            timer += Time.deltaTime / .5f;
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
        yield return null;
    }
}
