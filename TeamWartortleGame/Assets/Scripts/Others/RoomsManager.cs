using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsManager : MonoBehaviour, IUpdateData
{
    //lista di tutte le stanze
    private static List<RoomsBehaviour> rooms = new List<RoomsBehaviour>();
    //riferimento al giocatore
    [SerializeField]
    private Transform player = default, 
        cam = default;

    private static Transform staticCam, 
        staticPlayer;

    private static Vector3 camStartPosition;
    //riferimento al GameManag di scena
    [SerializeField]
    private GameManag g = default;
    //riferimento allo script della minimappa
    [SerializeField]
    private MiniMap miniMap = default;
    private static MiniMap staticMiniMap = default;
    //indica l'ultima stanza in cui il giocatore � entrato
    private static int lastEnteredRoom;


    private void Awake()
    {
        //da alle stanze il riferimento al giocatore
        RoomsBehaviour.player = player;

        staticPlayer = player;

        staticCam = cam;

        camStartPosition = staticCam.localPosition;
        //per ogni figlio del manager delle stanze, ne ottiene lo script da stanza
        rooms.Clear();
        foreach (Transform child in transform) { rooms.Add(child.GetComponent<RoomsBehaviour>()); }
        //riordina la lista di stanze in base all'ID
        RearrangeRoomsBasedOnID();

    }

    private void Start()
    {
        //ottiene l'ID dell'ultima stanza in cui il giocatore era entrato quando ha salvato il gioco
        lastEnteredRoom = g.lastRoomID;
        //attiva solo la stanza in cui il giocatore � entato per l'ultima volta prima di salvare
        ActivateOnlyThisRoom(lastEnteredRoom);
        //ottiene il riferimento statico alla minimappa
        staticMiniMap = miniMap;
        //fa muovere il puntino del personaggio nella stanza in cui � entrato
        staticMiniMap.MovePlayerDot(lastEnteredRoom);





        //DEBUG------------------------------------------------------------------------------------------------------------------------
        for (int i = 0; i < rooms.Count; i++)
        {

            for (int j = i; j < rooms.Count; j++)
            {

                if (rooms[i] != rooms[j] && rooms[i].GetRoomID() == rooms[j].GetRoomID())
                {
                    Debug.LogError("Le stanze " + rooms[i].transform.name + " e " + rooms[j].transform.name
                    + " hanno lo stesso ID: " + rooms[i].GetRoomID());
                }

            }

        }

    }

    /// <summary>
    /// Riordina la lista in base all'ID delle stanze
    /// </summary>
    private void RearrangeRoomsBasedOnID()
    {
        //crea una lista di stanze non ordinata
        List<RoomsBehaviour> unorderedRooms = rooms;
        //lista che conterr� le stanze in modo ordinato
        List<RoomsBehaviour> orderedRooms = rooms;
        //cicla ogni stanza
        for (int i = 0; i < unorderedRooms.Count; i++)
        {

            for (int j = i + 1; j < unorderedRooms.Count; j++)
            {
                //ottiene l'ID delle stanza ad indici i e j
                int x = unorderedRooms[i].GetRoomID();
                int y = unorderedRooms[j].GetRoomID();
                //se il numero della stanza dietro � maggiore di quello della stanza avanti...
                if (x > y)
                {
                    //...cambia l'ordine nella lista ordinata
                    RoomsBehaviour temp = unorderedRooms[i];
                    orderedRooms[i] = unorderedRooms[j];
                    orderedRooms[j] = temp;

                }

            }

        }
        //aggiorna la lista di tutte le stanze con la lista ordinata
        rooms = orderedRooms;

    }

    private void ActivateOnlyThisRoom(int roomToActivate)
    {
        //attiva la stanza con l'indice ricevuto come parametro e disattiva tutte le altre
        foreach (RoomsBehaviour room in rooms)
        {

            bool isPlayerRoom = room.GetRoomID() == roomToActivate;

            room.gameObject.SetActive(isPlayerRoom);

            if (isPlayerRoom && room.IsSmallRoom()) { cam.parent = null; cam.position = room.transform.position; }

        }

    }

    public static void ChangeRoom(DoorsBehaviour openedDoor)
    {

        //FARE PRIMA DISSOLVENZA

        //disattiva la stanza da cui si sta uscendo
        rooms[openedDoor.GetOwnRoomID()].gameObject.SetActive(false);
        //ordina alla stanza della porta accanto di posizionare il giocatore nella posizione della porta da cui si entra
        DoorsBehaviour newRoomDoor = openedDoor.GetNextDoor();
        rooms[newRoomDoor.GetOwnRoomID()].PositionPlayer(newRoomDoor.GetDoorID());
        //ottiene l'indice della stanza in cui si � entrati
        lastEnteredRoom = newRoomDoor.GetOwnRoomID();

        if (rooms[newRoomDoor.GetOwnRoomID()].IsSmallRoom())
        {
            
            staticCam.parent = null;
            staticCam.position = new Vector3(rooms[newRoomDoor.GetOwnRoomID()].transform.position.x,
                rooms[newRoomDoor.GetOwnRoomID()].transform.position.y, staticCam.position.z);

        }
        else
        {

            staticCam.parent = staticPlayer;
            staticCam.localPosition = camStartPosition;

        }
        //fa muovere il puntino del personaggio nella stanza in cui � entrato
        staticMiniMap.MovePlayerDot(lastEnteredRoom);

    }
    /// <summary>
    /// Permette ad altri script di ottenere la lista completa delle stanze
    /// </summary>
    /// <returns></returns>
    public static List<RoomsBehaviour> GetRoomsList() { return rooms; }

    public void UpdateData()
    {
        //aggiorna l'ID dell'ultima stanza in cui il giocatore � entrato
        g.lastRoomID = lastEnteredRoom;
        //Debug.Log("Aggiornata ultima stanza entrata: " + lastEnteredRoom);
    }
}
