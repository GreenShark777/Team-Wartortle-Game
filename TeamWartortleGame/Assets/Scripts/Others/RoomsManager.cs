//Si occupa di gestire tutte le stanze presenti nella scena
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsManager : MonoBehaviour, IUpdateData
{
    //lista di tutte le stanze
    private static List<RoomsBehaviour> rooms = new List<RoomsBehaviour>();
    //riferimento al giocatore e alla telecamera
    [SerializeField]
    private Transform player = default, 
        cam = default;
    //riferimento allo script della telecamera
    private static CameraBehaviour camBehaviour;
    //riferimento al GameManag di scena
    [SerializeField]
    private GameManag g = default;
    //array statico di bool che indicano in quali stanze sono stati sconfitti tutti i nemici o meno
    private static bool[] defeatedRooms;
    //riferimento allo script della minimappa
    [SerializeField]
    private MiniMap miniMap = default;
    private static MiniMap staticMiniMap = default;
    //indica l'ultima stanza in cui il giocatore è entrato
    private static int lastEnteredRoom;
    //riferimento all'Animator dell'immagine che funge da dissolvenza
    [SerializeField]
    private Animator blackScreenAnim = default;
    //riferimento all'Animator dell'immagine che funge da transizione
    private static Animator staticBlackScreenAnim = default;
    //riferimento allo script di movimento del giocatore
    private static Movement playerMov;


    private void Awake()
    {
        //da alle stanze il riferimento al giocatore
        RoomsBehaviour.player = player;
        //ottiene il riferimento allo script della telecamera
        camBehaviour = cam.GetComponent<CameraBehaviour>();
        //per ogni figlio del manager delle stanze, ne ottiene lo script da stanza
        rooms.Clear();
        foreach (Transform child in transform) { rooms.Add(child.GetComponent<RoomsBehaviour>()); }
        //riordina la lista di stanze in base all'ID
        RearrangeRoomsBasedOnID();
        //rende il riferimento all'Animator statico
        staticBlackScreenAnim = blackScreenAnim;
        //ottiene il riferimento allo script di movimento del giocatore
        playerMov = player.GetComponent<Movement>();

    }

    private void Start()
    {
        //ottiene l'ID dell'ultima stanza in cui il giocatore era entrato quando ha salvato il gioco
        lastEnteredRoom = g.lastRoomID;
        //ottiene la lista salvata di bool di tutte le stanze in cui il giocatore ha sconfitto tutti i nemici
        defeatedRooms = g.defeatedAllEnemies;
        //controlla per ogni stanza se i nemici all'interno sono stati sconfitti o meno
        CheckRoomsEnemies();
        //attiva solo la stanza in cui il giocatore è entato per l'ultima volta prima di salvare
        ActivateOnlyThisRoom(lastEnteredRoom);
        //ottiene il riferimento statico alla minimappa
        staticMiniMap = miniMap;





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
        //lista che conterrà le stanze in modo ordinato
        List<RoomsBehaviour> orderedRooms = rooms;
        //cicla ogni stanza
        for (int i = 0; i < unorderedRooms.Count; i++)
        {

            for (int j = i + 1; j < unorderedRooms.Count; j++)
            {
                //ottiene l'ID delle stanza ad indici i e j
                int x = unorderedRooms[i].GetRoomID();
                int y = unorderedRooms[j].GetRoomID();
                //se il numero della stanza dietro è maggiore di quello della stanza avanti...
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

    private void CheckRoomsEnemies()
    {
        //indice che indica l'indice della stanza che si sta controllando
        int index = 0;
        //cicla ogni stanza nella lista di stanze
        foreach (RoomsBehaviour room in rooms)
        {
            //se nella stanza ciclata sono stati sconfitti tutti i nemici, vengono disattivati 
            if (g.defeatedAllEnemies[index]) { room.DeactivateAllEnemies(); }
            //incrementa l'indice per continuare il ciclo
            index++;

        }

    }

    /// <summary>
    /// Attiva solo la stanza in cui si trova il giocatore
    /// </summary>
    /// <param name="roomToActivate"></param>
    private void ActivateOnlyThisRoom(int roomToActivate)
    {
        //cicla ogni stanza in lista
        foreach (RoomsBehaviour room in rooms)
        {
            //controlla se il giocatore è nella stanza ciclata...
            bool isPlayerRoom = room.GetRoomID() == roomToActivate;
            //...se lo è, la stanza viene attivata, altrimenti viene disattivata
            room.gameObject.SetActive(isPlayerRoom);
            //se questa è la stanza in cui il giocatore si trova all'inizio ed è una stanza piccola...
            if (isPlayerRoom && room.IsSmallRoom())
            {

                //cam.parent = null;

                //cam.position = room.transform.position;
                //...la telecamera non è più figlio del giocatore...
                camBehaviour.ChangeCamParent(null);
                //...e viene posizionata al centro della stanza
                camBehaviour.ChangeCamPosition(room.transform.position);
            
            }//altrimenti, se è la stanza attiva ma non è piccola...
            else if (isPlayerRoom && !room.IsSmallRoom())
            {
                //...alla telecamera vengono applicati dei limiti
                camBehaviour.LimitCameraBounds(room.GetRoomBounds(), room.GetThisRoomSpriteRend().transform, 
                    room.GetRoomBoundsOffsetX(), room.GetRoomBoundsOffsetY());

            }

        }

    }
    /// <summary>
    /// Cambia la stanza in cui il giocatore si trova
    /// </summary>
    /// <param name="openedDoor"></param>
    public static IEnumerator ChangeRoom(DoorsBehaviour openedDoor)
    {
        //fa dissolvenza in entrata
        staticBlackScreenAnim.SetTrigger("Dissolve");
        //il giocatore non potrà muoversi
        playerMov.enabled = false;
        //aspetta che la dissolvenza finisca
        yield return new WaitForSeconds(1.4f);
        //il giocatore potrà nuovamente muoversi
        playerMov.enabled = true;
        //ottiene il riferimento alla stanza da cui il giocatore sta uscendo
        RoomsBehaviour previousRoom = rooms[openedDoor.GetOwnRoomID()];
        //disattiva la stanza da cui si sta uscendo
        previousRoom.gameObject.SetActive(false);
        //aggiorna la lista indicandogli all'indice corretto se tutti i nemici sono stati sconfitti o meno
        defeatedRooms[previousRoom.GetRoomID()] = !previousRoom.AreThereEnemies();
        //ottiene il riferimento alla porta da cui si sta entrando
        DoorsBehaviour newRoomDoor = openedDoor.GetNextDoor();
        //ottiene il riferimento alla stanza della porta da cui si sta entrando
        RoomsBehaviour newRoom = rooms[newRoomDoor.GetOwnRoomID()];
        //attiva la stanza e gli fa effettuare i dovuti controlli
        newRoom.ActivateThisRoom(newRoomDoor.GetDoorID());
        //ottiene l'indice della stanza in cui si è entrati
        lastEnteredRoom = newRoomDoor.GetOwnRoomID();
        //se questa stanza è una stanza piccola...
        if (newRoom.IsSmallRoom())
        {
            //staticCam.parent = null;
            //staticCam.position = new Vector3(rooms[newRoomDoor.GetOwnRoomID()].transform.position.x,
            //    rooms[newRoomDoor.GetOwnRoomID()].transform.position.y, staticCam.position.z);

            //...la telecamera non sarà figlia del giocatore
            camBehaviour.ChangeCamParent(null);
            //...non avrà limiti(in quanto non si muoverà comunque)...
            camBehaviour.StopCameraLimits();
            //...e viene posizionata al centro della stanza
            camBehaviour.ChangeCamPosition(new Vector3(newRoom.transform.position.x,
                newRoom.transform.position.y, camBehaviour.transform.position.z));

        }
        else //altrimenti è una stanza grande, quindi...
        {
            //staticCam.parent = staticPlayer;
            //staticCam.localPosition = camStartPosition;

            //...la telecamera sarà figlia del giocatore...
            camBehaviour.MakePlayerParent();
            //...e gli vengono imposti dei limiti
            camBehaviour.LimitCameraBounds(newRoom.GetRoomBounds(), newRoom.GetThisRoomSpriteRend().transform,
                newRoom.GetRoomBoundsOffsetX(), newRoom.GetRoomBoundsOffsetY());

        }
        //se ci sono nemici attivi nella stanza, cambia la musica di gioco con quella da battaglia
        if (newRoom.AreThereEnemies()) { MusicManager.ChangeBackgroundMusic(1); }
        //altrimenti, se non lo è già, la cambia in musica normale
        else { MusicManager.ChangeBackgroundMusic(0); }
        //fa muovere il puntino del personaggio nella stanza in cui è entrato
        staticMiniMap.MovePlayerDot(lastEnteredRoom);

    }
    /// <summary>
    /// Permette ad altri script di ottenere la lista completa delle stanze
    /// </summary>
    /// <returns></returns>
    public static List<RoomsBehaviour> GetRoomsList() { return rooms; }
    /// <summary>
    /// Permette di ottenere l'ID dell'ultima stanza in cui il giocatore è entrato
    /// </summary>
    /// <returns></returns>
    public static int GetLastEnteredRoom() { return lastEnteredRoom; }

    public void UpdateData()
    {
        //aggiorna l'ID dell'ultima stanza in cui il giocatore è entrato
        g.lastRoomID = lastEnteredRoom;
        //aggiorna la lista di nemici sconfitti nelle varie stanze
        g.defeatedAllEnemies = defeatedRooms;
        //Debug.Log("Aggiornata ultima stanza entrata: " + lastEnteredRoom);
    }
}
