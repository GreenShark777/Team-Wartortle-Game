//Si occupa di gestire la UI del giocatore
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{

    [SerializeField]
    private Slider maliciousnessSlider = default,
        goodwillSlider = default;


    public void ChangeMaliciousnessBar(float newMaliciousness)
    {

        Debug.Log("Goodwill calculated: " + newMaliciousness + " - " + maliciousnessSlider.value);

        goodwillSlider.value -= newMaliciousness - maliciousnessSlider.value;

        
        maliciousnessSlider.value = newMaliciousness;

    }

    private void Update()
    {
        //DA METTERE NELLO SCRIPT DI DEBUGGING
        if (Input.GetKeyDown(KeyCode.K)) { ChangeMaliciousnessBar(maliciousnessSlider.value + 1); }
        if (Input.GetKeyDown(KeyCode.L)) { ChangeMaliciousnessBar(maliciousnessSlider.value - 1); }

    }

}
