using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : EnemiesHealth, IDamageable
{
    //Parent della vita dell'HUD della vita del boss
    [SerializeField]
    private GameObject bossHealth;
    //Slider della vita del boss
    private Slider fillBarSlider;

    //Booleana che controlla se ci si trova alla seconda fase
    [HideInInspector]
    public bool secondPhase = false;

    //Colori per la seconda fase
    [SerializeField]
    private Color rageColor, dmgRageColor;

    private void Start()
    {
        //Attivo la vita del boss
        bossHealth.SetActive(true);

        //Trovo lo slider della fillbar del boss attraverso i suoi figli
        fillBarSlider = bossHealth.GetComponentInChildren<Slider>();

        //Setto i valori dello slider della vita del boss
        fillBarSlider.maxValue  = maxHP;
        fillBarSlider.value = maxHP;


    }
    public override void Damage(float value, bool knockBack = false, Vector3 knockPos = default, float knockPower = 1)
    {
        //il nemico subisce danni in base al valore ricevuto
        enemyHp -= value;
        //Assegno la vita allo slider
        fillBarSlider.value = enemyHp;
        //se la vita del boss è metà o sotto, secondPhase diventa true se è false
        if (enemyHp <= maxHP / 2 && !secondPhase) SecondPhase();
        //Altrimenti se la vita è sotto zero il boss è sconfitto
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
        //Inizializzo il timer in cui verrà tutto eseguito a 0
        float timer = 0;
        //Imposto il nuovo Dmg color
        dmgColor = dmgRageColor;
        //Finchè sono sotto i 5 secondi
        while (timer < 5)
        {
            //Aumento il timer, il tutto verrà effettuato in 5 secondi
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
