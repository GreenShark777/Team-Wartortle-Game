using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Singleton pattern
    public static GameManager inst;

    //Riferimento al weaponcontainer per ottenere alcune sue informazioni sulla direzione
    [SerializeField]
    private WeaponsContainer weaponContainer;

    private void Start()
    {
        //Singleton pattern
        inst = this;
    }

    public Vector2 GetGunDirection()
    {
        //Se lo script weapon container è presente ritorno la direzione dell'arma chiamando a sua volta un'altro metodo
        if (weaponContainer)
            return weaponContainer.GetGunDirection();
        //Altrimenti ritorno un valore default di vector2
        else return default;
    }
}
