//Si occupa della vita del nemico
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesHealth : MonoBehaviour, IDamageable
{
    //indica quanta vita questo nemico ha
    public float enemyHp = default;
    [HideInInspector]
    public float maxHP;
    //indica se questo nemico è stato sconfitto e non deve più essere colpito
    [HideInInspector]
    public bool defeated = false;
    //riferimento al collider di questo nemico
    public Collider2D enemyCollider = default;

    //Riferimento a tutte le componenti sprite renderer nei GameObject per effettuare il cambio colore
    [HideInInspector]
    public SpriteRenderer[] enemySprites;

    public string spriteToIgnore = "ombra";

    //Colore iniziale da far ritornare al nemico dopo aver ricevuto danno e colore di damage
    public Color startColor;
    public Color dmgColor;

    //Colore corrente
    public Color currentColor;

    //Riferimento all'animator del nemico per fargli avviare l'animazione di sconfitta
    public Animator enAnim;
    public virtual void Awake()
    {
        //Memorizzo la vita massima
        maxHP = enemyHp;
        //Prendo il colore corrente
        currentColor = startColor;
        //DEBUGGING-------------------------------------------------------------------------------------------------------------------------------
        if (enemyHp == default) { Debug.LogError("I punti vita di " + gameObject.name + " non sono stati ancora impostati!"); }

        //Prendo tutti gli sprite renderer per potergli poi cambiare colore quando il nemico viene colpito
        enemySprites = GetComponentsInChildren<SpriteRenderer>(true);
        //rimuove lo sprite da ignorare(l'ombra), cercando nella lista di sprite appena creata uno sprite con il nome dello sprite da ignorare
        for (int i = 0; i < enemySprites.Length; i++)
        { if (enemySprites[i].sprite.name == spriteToIgnore) { enemySprites[i] = enemySprites[i-1]; break; } }

    }

    //public void TakeDmg(float value)
    //{
    //    //il nemico subisce danni in base al valore ricevuto
    //    enemyHp -= value;
    //    //se la vita del nemico è a 0 o meno, è stato sconfitto
    //    if (enemyHp <= 0) { EnemyDefeated(); }

    //}

    public virtual void EnemyDefeated()
    {
        //comunica che questo nemico è stato sconfitto
        defeated = true;
        //il collider del nemico non è più solido, in modo da non poter essere più colpito da armi
        enemyCollider.isTrigger = true;

        //Attivo animazione sconfitta
        if(enAnim) enAnim.SetTrigger("Defeat");

    }
    /// <summary>
    /// Permette ad altri script di sapere se questo nemico è stato sconfitto o meno
    /// </summary>
    /// <returns></returns>
    public virtual bool IsEnemyDefeated() { return defeated; }

    //Interfaccia di danno
    public virtual void Damage(float value)
    {
        //il nemico subisce danni in base al valore ricevuto
        enemyHp -= value;
        //se la vita del nemico è a 0 o meno, è stato sconfitto
        if (enemyHp <= 0) { EnemyDefeated(); }

        StartCoroutine(IHitColor());
    }

    public virtual IEnumerator IHitColor()
    {
        //Per ogni sprite renderer del nemico
        for (int i = 0; i < enemySprites.Length; i++)
        {
            for (int j = 0; j < enemySprites.Length; j++)
            {
                //imposto il colore di damage
                enemySprites[j].color = dmgColor;
            }
        }

        yield return new WaitForSeconds(.2f);

        for (int i = 0; i < enemySprites.Length; i++)
        {
            for (int j = 0; j < enemySprites.Length; j++)
            {
                //imposto il colore di damage
                enemySprites[j].color = startColor;
            }
        }


        yield return null;
    }
}
