//Si occupa della vita del nemico
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesHealth : MonoBehaviour, IDamageable
{
    //indica quanta vita questo nemico ha
    [SerializeField]
    private float enemyHp = default;
    //indica se questo nemico è stato sconfitto e non deve più essere colpito
    private bool defeated = false;
    //riferimento al collider di questo nemico
    [SerializeField]
    private Collider2D enemyCollider = default;

    //Riferimento a tutte le componenti sprite renderer nei GameObject per effettuare il cambio colore
    private SpriteRenderer[][] spriteRend = new SpriteRenderer[3][];

    //Colore iniziale da far ritornare al nemico dopo aver ricevuto danno e colore di damage
    [SerializeField]
    private Color startColor, dmgColor;

  
    private void Awake()
    {

        //DEBUGGING-------------------------------------------------------------------------------------------------------------------------------
        if (enemyHp == default) { Debug.LogError("I punti vita di " + gameObject.name + " non sono stati ancora impostati!"); }

        //Prendo tutti gli sprite renderer per potergli poi cambiare colore
        spriteRend[0] = transform.GetChild(0).GetChild(0).GetComponentsInChildren<SpriteRenderer>();
        spriteRend[1] = transform.GetChild(0).GetChild(1).GetComponentsInChildren<SpriteRenderer>();
        spriteRend[2] = transform.GetChild(0).GetChild(2).GetComponentsInChildren<SpriteRenderer>();

    }

    //public void TakeDmg(float value)
    //{
    //    //il nemico subisce danni in base al valore ricevuto
    //    enemyHp -= value;
    //    //se la vita del nemico è a 0 o meno, è stato sconfitto
    //    if (enemyHp <= 0) { EnemyDefeated(); }

    //}

    private void EnemyDefeated()
    {
        //comunica che questo nemico è stato sconfitto
        defeated = true;
        //il collider del nemico non è più solido, in modo da non poter essere più colpito da armi
        enemyCollider.isTrigger = true;

        //NON FAR SCOMPARIRE IL NEMICO, FALLO STORDIRE E DARE POSSIBILITA' AL GIOCATORE DI UCCIDERLO

        Debug.Log(gameObject.name + " sconfitto");
    }
    /// <summary>
    /// Permette ad altri script di sapere se questo nemico è stato sconfitto o meno
    /// </summary>
    /// <returns></returns>
    public bool IsEnemyDefeated() { return defeated; }

    //Interfaccia di danno
    public void Damage(float value)
    {
        //il nemico subisce danni in base al valore ricevuto
        enemyHp -= value;
        //se la vita del nemico è a 0 o meno, è stato sconfitto
        if (enemyHp <= 0) { EnemyDefeated(); }

        StartCoroutine(IHitColor());
    }

    private IEnumerator IHitColor()
    {
        //Per ogni sprite renderer del nemico
        for (int i = 0; i < spriteRend.Length; i++)
        {
            for (int j = 0; j < spriteRend[i].Length; j++)
            {
                //imposto il colore di damage
                spriteRend[i][j].color = dmgColor;
            }
        }

        yield return new WaitForSeconds(.2f);

        for (int i = 0; i < spriteRend.Length; i++)
        {
            for (int j = 0; j < spriteRend[i].Length; j++)
            {
                //imposto il colore di damage
                spriteRend[i][j].color = startColor;
            }
        }


        yield return null;
    }
}
