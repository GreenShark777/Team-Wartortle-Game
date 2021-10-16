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
    private float minSlidersValue = 4.5f;
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

    private void Start()
    {
        lastMaliciounessValue = goodwillSlider.value;
    }
    //private void Update()
    //{
    //    Debug.Log(goodwillSlider.value);
    //}
    /// <summary>
    /// Cambia il valore della barra della malizia, cambiando di conseguenza anche quella della bontà
    /// </summary>
    /// <param name="newMaliciousness"></param>
    public void ChangeMaliciousnessBar(float newMaliciousness)
    {

        //Debug.Log("Goodwill calculated: " + newMaliciousness + " - " + maliciousnessSlider.value + " -> "
        //+ (goodwillSlider.value - (newMaliciousness - maliciousnessSlider.value)));

        //calcola il valore che la barra della bontà deve avere
        goodwillSlider.value -= newMaliciousness - maliciousnessSlider.value;
        goodwillSlider.value = Mathf.Clamp(goodwillSlider.value, minSlidersValue, goodwillSlider.maxValue);

        //calcola il valore che la barra della malizia deve avere
        maliciousnessSlider.value = newMaliciousness;
        maliciousnessSlider.value = Mathf.Clamp(maliciousnessSlider.value, minSlidersValue, maliciousnessSlider.maxValue);

        //Se la nuova malizia è maggiore dell'ultima vuol dire che sto potenziando la malizia
        if (newMaliciousness > lastMaliciounessValue)
        {
            //Se la barra del cattivo è maggiore posso aumentare la caratteristica da demone
            if (maliciousnessSlider.value > goodwillSlider.value)
            {
                Debug.Log("Divisione: " + maliciousnessSlider.maxValue / 1.5f);
                //Se il valore di bontà superà i tre quarti e non sono già un demone, posso transformarmi  in un'demone
                if (maliciousnessSlider.value > maliciousnessSlider.maxValue / 1.5f && !GameManager.inst.demon)
                {
                    //Mi posso transformare in un demone e aumento il danno della spada
                    GameManager.inst.ChangeStats(0, .5f, false, true, "You became an demon!");
                }
                //Altrimenti aumento solo la statistica di danno della spada
                else
                {
                    GameManager.inst.ChangeStats(0, .5f, false, false, "Your sword gets stronger!");
                }
            }
        }
        else if (newMaliciousness < lastMaliciounessValue)
        {
            //Se il valore di bontà e maggiore di quello cattivo
            if (goodwillSlider.value > maliciousnessSlider.value)
            {
                Debug.Log("Divisione: " + goodwillSlider.maxValue / 1.5f);
                //Se il valore di bontà superà i tre quarti e non sono già un angelo, posso transformarmi  in un'angelo
                if (goodwillSlider.value > goodwillSlider.maxValue / 1.5f && !GameManager.inst.angel)
                {
                    //Mi posso transformare in un angelo e aumento il danno della pistola
                    GameManager.inst.ChangeStats(.5f, 0, true, false, "You became an angel!");
                }
                //Altrimenti aumento solo la statistica di danno della pistola
                else
                {
                    GameManager.inst.ChangeStats(.5f, 0, false, false, "Your gun gets stronger!");
                }
            }
        }

        lastMaliciounessValue = newMaliciousness;
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
