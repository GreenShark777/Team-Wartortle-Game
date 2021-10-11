using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : EnemiesHealth, IDamageable
{
    //Booleana che controlla se ci si trova alla seconda fase
    [HideInInspector]
    public bool secondPhase = false;

    //Colori per la seconda fase
    [SerializeField]
    private Color rageColor, dmgRageColor;
    public override void Damage(float value)
    {
        //il nemico subisce danni in base al valore ricevuto
        enemyHp -= value;
        //se la vita del boss � met� o sotto, secondPhase diventa true se � false
        if (enemyHp <= maxHP / 2 && !secondPhase) SecondPhase();
        //Altrimenti se la vita � sotto zero il boss � sconfitto
        else if (enemyHp <= 0) { EnemyDefeated(); }

        StartCoroutine(IHitColor());
    }

    //Metodo che si occupa di inizializzare la seconda fase
    private void SecondPhase()
    {
        //Imposto la booleana della seconda fase a true
        secondPhase = true;

        //Setto i nuovi colori
        StartCoroutine(ITransitionColor());
    }

    //IEnumerator che si occupa della transizione di colore alla seconda fase
    private IEnumerator ITransitionColor()
    {
        //Inizializzo il timer in cui verr� tutto eseguito a 0
        float timer = 0;
        //Imposto il nuovo Dmg color
        dmgColor = dmgRageColor;
        //Finch� sono sotto i 5 secondi
        while (timer < 5)
        {
            //Aumento il timer, il tutto verr� effettuato in 5 secondi
            timer += Time.deltaTime / 1f;
            //Assegno il colore a quello corrente
            currentColor = Color.Lerp(startColor, rageColor, timer);

            //e assegno il colore raggiunto allo sprite corrente del nemico
            //Per ogni sprite del nemico
            for (int i = 0; i < enemySprites.Length; i++)
            {
                //Assegnoi il colore raggiunto
                enemySprites[i].color = currentColor;

            }
            yield return null;
        }
    }
}
