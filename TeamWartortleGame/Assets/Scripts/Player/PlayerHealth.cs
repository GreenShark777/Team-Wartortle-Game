using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    //Parent dei contenitori dei cuori
    [SerializeField]
    private GameObject healthParent;

    //Lista di cuori interni(2 pezzi per ogni Contenitore)
    private List<GameObject> hearts = new List<GameObject>();

    //Vita corrente
    private int health;
    //Vita massima iniziale
    [SerializeField]
    private int maxHealth;
    //Cuori massimi
    private int maxTotalHealth;

    private void Start()
    {
        //Ottengo i cuori massimi con un childcount del parent
        maxTotalHealth = healthParent.transform.childCount;
        //Setto metodo iniziale che gestisce quanti container devono apparire
        SetHealthContainer(maxHealth);
    }

    //Metodo iniziale che gestisce quanti container devono apparire
    private void SetHealthContainer(int maxHealth)
    {
        //Libero la lista di cuori nel caso in cui c'era presente qualcosa in precedenza
        hearts.Clear();
        //Loopo ogni cuore
        for (int i=0; i<maxTotalHealth; i++)
        {
            //Se sono dentro la vita massima
            if (i < maxHealth)
            {
                //Attivo il Contenitore
                GameObject last = healthParent.transform.GetChild(i).gameObject;
                last.SetActive(true);
                //Aggiungo i suoi figli alla lista hearts
                for (int j=0; j<last.transform.childCount; j++)
                    hearts.Add(last.transform.GetChild(j).gameObject);
            }
            //Altrimenti se sono in cuori non ancora sbloccati li setto a false
            else
                healthParent.transform.GetChild(i).gameObject.SetActive(false);
       
        }

        //I cuori interni saranno logicamente la vita massima moltiplicata per due visto che ogni contenitori ha 2 pezzi
        //Imposto la vita al massimo
        health = maxHealth * 2;
        //Aggiorno l'HUD della vita
        SetHealth();
    }

    //Metodo per la gestione del danno
    public void Damage(float value)
    {
        //Converto in int il valore passato
        health -= Mathf.FloorToInt(value);
        health = Mathf.Clamp(health, 0, maxHealth * 2);
        //Se la vita è sotto zero muoio e reimposto sempre la vit a 0
        if (health <= 0)
        {
            Debug.Log("Morto");
        }
        //Chiamo metodo che gestisce i cuori interni ai container
        SetHealth();
    }
    //Metodo che gestisce i cuori interni ai container
    private void SetHealth()
    {
        //Per ogni cuore della lista
        for (int i=0; i < hearts.Count; i++)
        {
            //Se sono dentro la vita corrente li attivo
            if (i < health) hearts[i].SetActive(true);
            //Altrimenti li disattivo
            else hearts[i].SetActive(false);
        }
    }

    public void GetNewContainer()
    {
        this.maxHealth++;
        SetHealthContainer(maxHealth);
    }

    public bool GetFullHealth()
    {
        if (health == maxHealth * 2) return true;
        else return false;
    }
}
