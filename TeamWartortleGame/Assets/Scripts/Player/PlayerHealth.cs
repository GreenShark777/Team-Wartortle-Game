using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    //Parent dei contenitori dei cuori
    [SerializeField]
    private GameObject healthParent;

    //Lista di cuori interni(2 pezzi per ogni Contenitore)
    private List<GameObject> hearts = new List<GameObject>();

    //Lista di cuori 
    private List<Image> heartsParent = new List<Image>();

    //Vita corrente
    private int health;
    //Vita massima iniziale
    [SerializeField]
    private int maxHealth;
    //Cuori massimi
    private int maxTotalHealth;
    //Tutti i tipi di lineart del cuore
    [SerializeField]
    Sprite emptyLineart, halfLineart, fullLineart;

    //Colore iniziale da far ritornare al nemico dopo aver ricevuto danno e colore di damage
    [SerializeField]
    private Color startColor;
    [SerializeField]
    private Color dmgColor;

    //Colore corrente
    [SerializeField]
    private Color currentColor;

    //Riferimento a tutte le componenti sprite renderer nei GameObject per effettuare il cambio colore
    private SpriteRenderer[] playerSprites;

    //Booleana che decide quando il player potrà essere danneggiato nuovamente
    private bool canDamage = true;

    //Riferimento al Rigidbody per fargli eseguire il knockback
    [SerializeField]
    private Rigidbody2D playerRb;

    //Riferimento allo script del player per bloccare il movimento per un attimo per effettuare il knockback
    [SerializeField]
    private Movement playerMovement;

    //Riferimento al GameObject dello scudo
    [SerializeField]
    private GameObject shield;
    private void Start()
    {
        //Ottengo i cuori massimi con un childcount del parent
        maxTotalHealth = healthParent.transform.childCount;
        //Setto metodo iniziale che gestisce quanti container devono apparire
        SetHealthContainer(maxHealth);

        //Prendo tutti gli sprite renderer per potergli poi cambiare colore quando il nemico viene colpito
        playerSprites = transform.parent.GetComponentsInChildren<SpriteRenderer>(true);
    }

    //Metodo iniziale che gestisce quanti container devono apparire
    private void SetHealthContainer(int maxHealth)
    {
        //Libero la lista di cuori nel caso in cui c'era presente qualcosa in precedenza
        hearts.Clear();
        //Libero la lista dei contenitori di cuori così da poterla re inserire in modo pulito
        heartsParent.Clear();
        //Ciclo ogni cuore
        for (int i=0; i<maxTotalHealth; i++)
        {
            //Se sono dentro la vita massima
            if (i < maxHealth)
            {
                //Attivo il Contenitore
                GameObject last = healthParent.transform.GetChild(i).gameObject;
                last.SetActive(true);
                //Asegno il contenitore alla lista di cuori
                heartsParent.Add(last.GetComponent<Image>());
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
    public void Damage(float value, bool knockBack = false, Vector3 knockPos = default, float knockPower = 1)
    {
        //Se lo scudo è attivo mi proteggo da un danno
        if (!shield.activeSelf)
        {
            //Se posso essere nuovamente danneggiato
            if (canDamage)
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

                //Metodo che gestisce la lineart dei cuori
                SetHeartLineart();

                //Controllo che non mi stia invece curando perché in caso non cambio colore
                if (value > 0)
                {
                    //Faccio il knockback se mi viene passato dal nemico
                    if (knockBack)
                    {

                        playerMovement.Knockback(knockPos, knockPower);
                    }
                    //Attiva la coroutine che cambia il colore in rosso per un attimol
                    StartCoroutine(IHitColor());
                }

            }
        }
        //Disattivo lo scudo se invece è attivo
        else {
            canDamage = false;
            shield.SetActive(false);
            Invoke("CanGetDamageAgain",.5f);
        }
    }

    private void CanGetDamageAgain()
    {
        canDamage = true;
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

    //Metodo che gestisce la lineart dei cuori
    private void SetHeartLineart()
    {
        //Per ogni cuore
        for (int i = 0; i < heartsParent.Count; i++)
        {
            //Se interamente il cuore imposto la lineart vuota
            if (!heartsParent[i].transform.GetChild(0).gameObject.activeSelf)
            {
                heartsParent[i].sprite = emptyLineart;
            }
            //Altrimenti se mi manca mezzo cuore imposto la lineart a metà
            else if (!heartsParent[i].transform.GetChild(1).gameObject.activeSelf)
            {
                heartsParent[i].sprite = halfLineart;
            }
            //Altrimenti vuol dire che ho un cuore completo quindi imposto la lineart piena
            else heartsParent[i].sprite = fullLineart;
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

    public bool isTotalHealthReached()
    {
        //Se la vita massima ha raggiunto il massimo totale ritorna true, così che non si possono sbloccare nuovi cuori
        return (maxHealth >= maxTotalHealth);
    }

    private IEnumerator IHitColor()
    {
        //Imposto che non posso essere danneggiato attraverso la booleana canDamage
        canDamage = false;
        //Per ogni sprite renderer
        for (int i = 0; i < playerSprites.Length; i++)
        {
            for (int j = 0; j < playerSprites.Length; j++)
            {
                //imposto il colore di damage
                playerSprites[j].color = dmgColor;
            }
        }

        //Aspetto un po'
        yield return new WaitForSeconds(.2f);

        for (int i = 0; i < playerSprites.Length; i++)
        {
            for (int j = 0; j < playerSprites.Length; j++)
            {
                //imposto il colore iniziale
                playerSprites[j].color = currentColor;
            }
        }

        //Aspetto mezzo secondo e poi potrò nuovamente essere danneggiato
        yield return new WaitForSeconds(.5f);
        canDamage = true;
        yield return null;
    }
}
