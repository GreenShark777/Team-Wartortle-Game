//Si occupa di gestire la UI del giocatore
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    //slider per le barre di malizia e bontà
    [SerializeField]
    private Slider maliciousnessSlider = default,
        goodwillSlider = default;

    //indica qual'è il valore minimo che gli slider possono avere(in modo da far rimanere almeno una piccola tacca anche quando si è al minimo)
    [SerializeField]
    private float minSlidersValue = 0f;
    //riferimenti alle immagini da cambiare per indicare al giocatore l'arma in uso
    [SerializeField]
    private Image weaponInUseImage = default,
        weaponNotInUseImage = default;
    //riferimenti agli sprite delle armi e ai loro contenitori per farli swappare
    [SerializeField]
    private Sprite swordSprite = default,
        gunSprite = default,
        swordContainer = default,
        gunContainer = default;

    float lastMaliciounessValue = default;

    //Boolenaa che controlla se il valore massimo di una delle due barre è stato raggiunto
    private bool maxValueReached = false;

    //Valori da raggiungere per effetuare un potenziamento
    float maliciousnessTarget, goodWillTarget;

    private void Start()
    {
        lastMaliciounessValue = goodwillSlider.value;
        //Il target da raggiungere per ottenere il potenziamento è del valore iniziale + 50
        maliciousnessTarget = maliciousnessSlider.value + 20;
        goodWillTarget = goodwillSlider.value + 20;


    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            ChangeMaliciousnessBar(GetMaliciousness() - 5);
        } else if (Input.GetKeyDown(KeyCode.N)) ChangeMaliciousnessBar(GetMaliciousness() + 5);
    }
    /// <summary>
    /// Cambia il valore della barra della malizia, cambiando di conseguenza anche quella della bontà
    /// </summary>
    /// <param name="newMaliciousness"></param>
    public void ChangeMaliciousnessBar(float newMaliciousness)
    {

        //Se una delle due barre non ha raggiunto il valore massimo posso ancora modificare le statistiche
        if (!maxValueReached)
        {
            //Debug.Log("Goodwill calculated: " + newMaliciousness + " - " + maliciousnessSlider.value + " -> "
            //+ (goodwillSlider.value - (newMaliciousness - maliciousnessSlider.value)));

            //calcola il valore che la barra della bontà deve avere
            goodwillSlider.value -= newMaliciousness - maliciousnessSlider.value;
            goodwillSlider.value = Mathf.Clamp(goodwillSlider.value, minSlidersValue, goodwillSlider.maxValue);

            //calcola il valore che la barra della malizia deve avere
            maliciousnessSlider.value = newMaliciousness;
            maliciousnessSlider.value = Mathf.Clamp(maliciousnessSlider.value, minSlidersValue, maliciousnessSlider.maxValue);

            Debug.Log(maliciousnessSlider.value);

      
            //Se la nuova malizia è maggiore dell'ultima vuol dire che sto potenziando la malizia
            if (newMaliciousness > lastMaliciounessValue)
            {
                //Se la malizia ha raggiunto il target per potenziare il personaggio
                if (maliciousnessSlider.value >= maliciousnessTarget)
                {
                    //Se la barra del cattivo è maggiore posso aumentare la caratteristica da demone
                    if (maliciousnessSlider.value > goodwillSlider.value)
                    {
                        Debug.Log("isDemon: " + GameManager.inst.demon);
                        //Se il valore di bontà superà è a metà e non sono già un demone, posso transformarmi  in un'demone
                        if (maliciousnessSlider.value >= 160f && !GameManager.inst.demon)
                        {
                            //Mi posso transformare in un demone e aumento il danno della spada
                            GameManager.inst.ChangeStats(0, 2f, false, true, "You turned into a demon!");
                        }
                        //Altrimenti aumento solo la statistica di danno della spada
                        else
                        {
                            GameManager.inst.ChangeStats(0, 2f, false, false, "Your sword gets stronger!");
                        }
                    }
                    //Imposto il prossimo target per raggiungere il potenziamento
                    maliciousnessTarget = maliciousnessSlider.value + 20;
                    //Cntrollo se la barra è al massimo
                    if (maliciousnessSlider.value == maliciousnessSlider.maxValue)
                        maxValueReached = true;
                }
            }
            else if (newMaliciousness < lastMaliciounessValue)
            {
                //Se il karma buono ha raggiunto il target per potenziare il personaggio

                Debug.Log("goodWill: " + goodwillSlider.value);
                Debug.Log("goodWillTarget: " + goodWillTarget);
                if (goodwillSlider.value >= goodWillTarget)
                {
                    //Se il valore di bontà e maggiore di quello cattivo
                    if (goodwillSlider.value > maliciousnessSlider.value)
                    {
                        Debug.Log("isAngel: " + GameManager.inst.angel);
                        //Se il valore di bontà superà i tre quarti e non sono già un angelo, posso transformarmi  in un'angelo
                        if (goodwillSlider.value >= 160f && !GameManager.inst.angel)
                        {
                            //Mi posso transformare in un angelo e aumento il danno della pistola
                            GameManager.inst.ChangeStats(1f, 0, true, false, "You turned into an angel!");
                        }
                        //Altrimenti aumento solo la statistica di danno della pistola
                        else
                        {
                            GameManager.inst.ChangeStats(1f, 0, false, false, "Your gun gets stronger!");
                        }
                    }
                    //Imposto il prossimo target per raggiungere il potenziamento
                    goodWillTarget = goodwillSlider.value + 20;
                }
                if (goodwillSlider.value == goodwillSlider.maxValue)
                    maxValueReached = true;
            }

            lastMaliciounessValue = newMaliciousness;
        }
    }
    /// <summary>
    /// Permette ad altri script di ottenere il riferimento alla attuale malizia del giocatore
    /// </summary>
    /// <returns></returns>
    public float GetMaliciousness() { return maliciousnessSlider.value; }
    /// <summary>
    /// Scambia gli sprite delle armi in uso in base al valore booleano ricevuto
    /// </summary>
    /// <param name="isUsingGun"></param>
    public void ChangeWeaponInUse(bool isUsingGun)
    {
        //cambia gli sprite in base al valore booleano
        weaponInUseImage.sprite = isUsingGun ? gunSprite : swordSprite;
        weaponNotInUseImage.sprite = !isUsingGun ? gunSprite : swordSprite;

        //Controllo che sprite quale sprite corrente ho per impostare lo swap del container e la rotazione dell'immagine della spada
        Image containerInUse = weaponInUseImage.rectTransform.parent.GetComponent<Image>();
        containerInUse.sprite = isUsingGun ? gunContainer : swordContainer;
        containerInUse.rectTransform.rotation = isUsingGun ? Quaternion.Euler(Vector3.zero) : Quaternion.Euler(new Vector3(0, 0, 20));
        //Faccio la stessa cosa per l'altro contenitore
        Image containerNotInUse = weaponNotInUseImage.rectTransform.parent.GetComponent<Image>();
        containerNotInUse.sprite = !isUsingGun ? gunContainer : swordContainer;
        containerNotInUse.rectTransform.rotation = !isUsingGun ? Quaternion.Euler(Vector3.zero) : Quaternion.Euler(new Vector3(0, 0, 20));

    }

}
