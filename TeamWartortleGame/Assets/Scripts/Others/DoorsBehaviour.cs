//Si occupa del comportamento delle porte, che faranno da tramite tra la propria stanza e il manager delle stanze
using UnityEngine;

public class DoorsBehaviour : MonoBehaviour
{
    //identificatore per ogni porta
    private int doorID = 0;
    //riferimento allo script della porta a cui questa porta deve portare
    [SerializeField]
    private DoorsBehaviour nextDoor = default;
    //riferimento alla posizione in cui il giocatore viene messo quando entra da questa porta
    [SerializeField]
    private Transform spawnPoint = default;
    //indica la posizione in cui il giocatore viene messo quando entra da questa porta
    private Vector2 spawnPosition;
    //indica a quale stanza appartiene questa porta
    private int ownRoomID;
    //riferimento al buco che si trova nella stanza di cui questa porta fa parte
    [SerializeField]
    private HoleBehaviour hole = default;


    private void Awake()
    {
        //ottiene la posizione in cui il giocatore deve essere messo quando entra da questa porta
        spawnPosition = spawnPoint.position;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //se il giocatore entra in questa porta...
        if (collision.CompareTag("Player"))
        {
            //...comunica al manager delle stanze che bisogna cambiare stanza...
            StartCoroutine(RoomsManager.ChangeRoom(this));
            //...e comunica al buco di cui si ha riferimento(se esiste) dove mettere il suo punto di respawn
            if (nextDoor.GetHoleInTheRoom()) { nextDoor.GetHoleInTheRoom().SetRespawnPosition(nextDoor.GetSpawnPosition()); }
        
        }

    }

    /// <summary>
    /// Ritorna l'ID di questa porta
    /// </summary>
    /// <returns></returns>
    public int GetDoorID() { return doorID; }
    /// <summary>
    /// Permette di cambiare l'ID di questa porta
    /// </summary>
    /// <param name="newID"></param>
    public void SetDoorID(int newID) { doorID = newID; }
    /// <summary>
    /// Ritorna la posizione in cui il giocatore deve essere messo quando entra da questa porta
    /// </summary>
    /// <returns></returns>
    public Vector2 GetSpawnPosition() { return spawnPosition; }
    /// <summary>
    /// Ritorna la porta a cui questa porta deve portare
    /// </summary>
    /// <returns></returns>
    public DoorsBehaviour GetNextDoor() { return nextDoor; }
    /// <summary>
    /// Permette di impostare l'ID della stanza d'appartenenza
    /// </summary>
    /// <param name="roomID"></param>
    public void SetOwnRoomID(int roomID) { ownRoomID = roomID; }
    /// <summary>
    /// Ritorna l'ID della stanza d'appartenenza
    /// </summary>
    /// <returns></returns>
    public int GetOwnRoomID() { return ownRoomID; }
    /// <summary>
    /// Ritorna il riferimento al buco nella stanza di questa porta, se c'è
    /// </summary>
    /// <returns></returns>
    public HoleBehaviour GetHoleInTheRoom() { return hole; }

}
