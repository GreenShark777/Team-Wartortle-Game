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
