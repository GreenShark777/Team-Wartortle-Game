//Si occupa della vita del nemico
using UnityEngine;

public class EnemiesHealth : MonoBehaviour
{
    //indica quanta vita questo nemico ha
    [SerializeField]
    private float enemyHp = default;
    //indica se questo nemico è stato sconfitto e non deve più essere colpito
    private bool defeated = false;
    //riferimento al collider di questo nemico
    [SerializeField]
    private Collider2D enemyCollider = default;


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
        //comunica che questo nemico è stato sconfitto
        defeated = true;
        //il collider del nemico non è più solido, in modo da non poter essere più colpito da armi
        enemyCollider.isTrigger = true;

        //NON FAR SCOMPARIRE IL NEMICO, FALLO STORDIRE E DARE POSSIBILITA' AL GIOCATORE DI UCCIDERLO

        Debug.Log(gameObject.name + " sconfitto");
    }
    /// <summary>
    /// Permette ad altri script di sapere se questo nemico è stato sconfitto o meno
    /// </summary>
    /// <returns></returns>
    public bool IsEnemyDefeated() { return defeated; }

}
