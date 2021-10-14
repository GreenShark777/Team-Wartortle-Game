using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spina : MonoBehaviour
{
    //Timer prima che la trappola si attivi, timer per riattivare invece uguale per tutto
    [SerializeField]
    private float timer = 1, defaultTimer = 3;

    //Animatore
    [SerializeField]
    private Animator spineAn;

    //Quando la spina si attiva
    private void OnEnable()
    {
        //si avvia l'animazione delle spine con un delay passato per parametro
        StartCoroutine(ITrapOn(timer));
    }

    private IEnumerator ITrapOn(float timer)
    {
        //Aspetto un tot di secondi passati per variabile
        yield return new WaitForSeconds(timer);

        //Attivo la booleana della trappola
        spineAn.SetBool("On", true);

        yield return new WaitForSeconds(3f);

        //Disattivo la booleana della trappola
        spineAn.SetBool("On", false);

        //Richiamo la coroutine
        StartCoroutine(ITrapOn(defaultTimer));

    }
}
