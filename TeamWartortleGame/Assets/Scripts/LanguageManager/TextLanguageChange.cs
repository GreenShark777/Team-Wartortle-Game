//Si occupa di cambiare la lingua del testo a cui questo script è attaccato in base al valore nel LanguageManager
using UnityEngine;
using UnityEngine.UI;

public class TextLanguageChange : MonoBehaviour
{
    //riferimento al manager delle lingue di gioco
    //[SerializeField]
    private static LanguageManager lm = default;
    //riferimento al testo da cambiare
    [SerializeField]
    private Text textToChange = default;
    //stringhe che indicano il testo da mostrare in base alla lingua scelta
    [SerializeField]
    private string italianText = default, //testo che deve essere scritto nel testo quando la lingua è italiana
        englishText = default; //testo che deve essere scritto nel testo quando la lingua è inglese

    //indica se l'oggetto attaccato a questo script è un bottone senza testo, nel qual caso le proprietà nell'Inspector cambieranno
    public bool isNoTextButton = default;
    //riferimento al bottone da cambiare
    [SerializeField]
    private Image buttonImageToChange = default;
    //riferimenti alle immagini del bottone senza testo in tutte le lingue
    [SerializeField]
    private Sprite italianButton = default,
        englishButton = default;


    private void Awake()
    {
        //se questo script deve cambiare un testo e non è stato già messo come riferimento nell'editor, prende il componente Text dal gameObject
        if (!isNoTextButton && textToChange == null) { textToChange = GetComponent<Text>(); }
        //altrimenti, se bisogna cambiare l'immagine di un bottone e non è stato già messo nell'editor, prende il componenente Button dal gameObject
        else if(isNoTextButton && buttonImageToChange == null){ buttonImageToChange = GetComponent<Image>(); }

    }

    private void Start()
    {
        //se il riferimento al LanguageManager è ancora nullo, viene preso
        if (lm == null) { lm = FindObjectOfType<LanguageManager>(); }
        //si aggiunge alla lista dei testi o bottoni da cambiare al cambio di lingua
        lm.textsToChangeLanguage.Add(this);
        //se non si è messo il testo da cambiare come riferimento nell'editor, prende il componente Text dentro il gameObject
        //if (textToChange == null) { textToChange = GetComponent<Text>(); }
        //infine, cambia il testo o il bottone in base alla lingua corrente
        ChangeLanguage(lm.GetCurrentLanguage());

    }
    /// <summary>
    /// Cambia la lingua del testo in base al valore ricevuto come parametro
    /// 0 = italiano
    /// 1 = inglese
    /// </summary>
    /// <param name="currentLanguage"></param>
    public void ChangeLanguage(int currentLanguage)
    {
        //In base al valore ricevuto, cambia la lingua del testo
        switch (currentLanguage)
        {
            //LINGUA ITALIANA
            case 0:
                {
                    //se non è un bottone, cambia il testo in italiano
                    if (!isNoTextButton) { textToChange.text = italianText; }
                    //altrimenti, cambia il bottone in italiano
                    else { buttonImageToChange.sprite = italianButton; }

                    break;

                }
            //LINGUA INGLESE
            case 1:
                {
                    //se non è un bottone, cambia il testo in inglese
                    if (!isNoTextButton) { textToChange.text = englishText; }
                    //altrimenti, cambia il bottone in inglese
                    else { buttonImageToChange.sprite = englishButton; }

                    break;

                }
            //Nel caso viene dato un valore errato, viene segnalato nella console come errore
            default: /*Debug.LogError("Aggiunto valore di lingua sbagliato: " + currentLanguage);*/ break;

        }

    }
    /*
    /// <summary>
    /// Permette ad altri script, senza riferimento alla lingua corrente, di cambiare la lingua del testo in base alla lingua corrente
    /// </summary>
    public void ChangeLanguage()
    {
        //In base al valore ricevuto, cambia la lingua del testo
        switch (lm.GetCurrentLanguage())
        {
            //LINGUA ITALIANA
            case 0:
                {
                    //cambia il testo in italiano
                    textToChange.text = italianText;
                    break;

                }
            //LINGUA INGLESE
            case 1:
                {
                    //cambia il testo in inglese
                    textToChange.text = englishText;
                    break;

                }
            //Nel caso viene dato un valore errato, viene segnalato nella console come errore
            default: Debug.LogError("Aggiunto valore di lingua sbagliato: " + lm.GetCurrentLanguage()); break;

        }

    }
    */
    /// <summary>
    /// Permette ad altri script di cambiare il testo da scrivere, prendendo come parametro una stringa per ogni lingua
    /// </summary>
    /// <param name="newItalianText"></param>
    /// <param name="newEnglishText"></param>
    public void UpdateText(string newItalianText, string newEnglishText)
    {
        //aggiorna il testo italiano al parametro del testo italiano ricevuto
        italianText = newItalianText;
        //aggiorna il testo inglese al parametro del testo inglese ricevuto
        englishText = newEnglishText;
        //se il riferimento al LanguageManager è ancora nullo, viene preso
        if (lm == null) { lm = FindObjectOfType<LanguageManager>(); }
        //infine, cambia il testo da mostrare in base alla lingua corrente
        ChangeLanguage(lm.GetCurrentLanguage());

    }

    //Ogni volta che viene attivato il gameObject con questo script, viene cambiato il testo in base alla lingua corrente
    //private void OnEnable() { ChangeLanguage(lm.GetCurrentLanguage()); }

}