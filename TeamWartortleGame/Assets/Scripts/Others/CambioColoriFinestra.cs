using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class CambioColoriFinestra : MonoBehaviour
{
    //Colore da cattivo
    [SerializeField]
    private Color colorEvil;

    //Luce corrente
    [SerializeField]
    private UnityEngine.Experimental.Rendering.Universal.Light2D currentLight;

    private void Update()
    {
        //Se il giocatore è un demone e le luci non sono rosse le fa diventare rosse
        if (GameManager.inst.demon && currentLight.color != colorEvil)
        {
            //Aggiorno il colore
            currentLight.color = colorEvil;
        }
    }
}
