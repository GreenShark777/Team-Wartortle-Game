//Si occupa della vita del nemico
using UnityEngine;

public class EnemiesHealth : MonoBehaviour
{
    //indica quanta vita questo nemico ha
    [SerializeField]
    private float enemyHp = default;


    private void Awake()
    {

        //DEBUGGING-------------------------------------------------------------------------------------------------------------------------------
        if (enemyHp == default) { Debug.LogError("I punti vita di " + gameObject.name + " non sono stati ancora impostati!"); }

    }

    public void TakeDmg(float value)
    {
        //il nemico subisce danni in base al valore ricevuto
        enemyHp -= value;
        //se la vita del nemico è a 0 o meno, è stato sconfitto
        if (enemyHp <= 0) { EnemyDefeated(); }

    }

    private void EnemyDefeated()
    {

        //NON FAR SCOMPARIRE IL NEMICO, FALLO STORDIRE E DARE POSSIBILITA' AL GIOCATORE DI UCCIDERLO

    }

}
