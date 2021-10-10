using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsManager : MonoBehaviour
{
    //lista di tutte le stanze
    private static List<RoomsBehaviour> rooms = new List<RoomsBehaviour>();
    //riferimento al giocatore
    [SerializeField]
    private Transform player = default;


    private void Start()
    {
        //per ogni figlio del manager delle stanze, ne ottiene lo script da stanza
        foreach (Transform child in transform) { rooms.Add(child.GetComponent<RoomsBehaviour>()); }
        //riordina la lista di stanze in base all'ID
        RearrangeRoomsBasedOnID();
        //da alle stanze il riferimento al giocatore
        RoomsBehaviour.player = player;




        //DEBUG------------------------------------------------------------------------------------------------------------------------
        for (int i = 0; i < rooms.Count; i++)
        {

            for (int j = i; j < rooms.Count; j++)
            {

                if (rooms[i] != rooms[j] && rooms[i].GetRoomID() == rooms[j].GetRoomID())
                {
                    Debug.LogError("Le stanze " + rooms[i].transform.parent + " e " + rooms[j].transform.parent
                    + " hanno lo stesso ID: " + rooms[i].GetRoomID());
                }

            }

        }

    }

    private void RearrangeRoomsBasedOnID()
    {

        List<RoomsBehaviour> unorderedRooms = rooms;

        List<RoomsBehaviour> orderedRooms = rooms;

        for (int i = 0; i < unorderedRooms.Count; i++)
        {

            for (int j = i + 1; j < unorderedRooms.Count; j++)
            {

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

        rooms = orderedRooms;

    }

    public static void ChangeRoom(DoorsBehaviour openedDoor)
    {

        rooms[openedDoor.GetNextDoor().GetOwnRoomID()].PositionPlayer(openedDoor.GetNextDoor().GetDoorID());

    }

}
