//Si occupa della gestione delle statistiche delle armi
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStats : MonoBehaviour
{

    [SerializeField]
    private float attackStat = 1, //indica quanto danno fa quest'arma
        pushForceStat = 10, //indica quanto spinge indietro i nemici con un colpo
        stunTime = 0.2f; //indica per quanto tempo un nemico colpito da quest'arma viene stordito


    /// <summary>
    /// Permette ad altri script di ottenere la potenza d'attacco di quest'arma
    /// </summary>
    /// <returns></returns>
    public float GetAttack() { return attackStat; }
    /// <summary>
    /// Permette ad altri script di ottenere la potenza di spinta di quest'arma
    /// </summary>
    /// <returns></returns>
    public float GetPushForce() { return pushForceStat; }
    /// <summary>
    /// Permette ad altri script di sapere per quanto tempo un nemico deve essere stordito da un colpo
    /// </summary>
    /// <returns></returns>
    public float GetStunTime() { return stunTime; }
    /// <summary>
    /// Permette ad altri script di cambiare la potenza d'attacco di quest'arma
    /// </summary>
    /// <param name="newValue"></param>
    public void ChangeAttackStat(float newValue) { attackStat += newValue; }
    /// <summary>
    /// Permette ad altri script di cambiare la potenza di spinta di quest'arma
    /// </summary>
    /// <param name="newValue"></param>
    public void ChangePushForceStat(float newValue) { pushForceStat += newValue; }
    /// <summary>
    /// Permette ad altri script di cambiare per quanto tempo un nemico deve essere stordito da un colpo
    /// </summary>
    /// <param name="newValue"></param>
    public void ChangeStunTimeStat(float newValue) { stunTime += newValue; }

}
