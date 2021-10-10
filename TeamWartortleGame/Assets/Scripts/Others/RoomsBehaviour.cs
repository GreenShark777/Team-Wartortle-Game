using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsBehaviour : MonoBehaviour
{
    //lista contenente tutti gli script delle porte di questa stanza
    private List<DoorsBehaviour> doors = new List<DoorsBehaviour>();
    //riferimento al contenitore di tutte le porte
    private Transform doorsContainer;
    //riferimento al giocatore
    public static Transform player;
    //identificativo della stanza
    [SerializeField]
    private int roomID = 0;


    private void Start()
    {
        //ottiene il riferimento al contenitore di tutte le porte
        doorsContainer = transform.GetChild(0);
        //per ogni figlio nel contenitore delle porte...
        foreach(Transform child in doorsContainer)
        {
            //...ne ottiene lo script da porta...
            DoorsBehaviour door = child.GetComponent<DoorsBehaviour>();
            //...lo aggiunge alla lista di porte...
            doors.Add(door);
            //...e gli comunica a quale stanza appartiene
            door.SetOwnRoomID(roomID);
        
        }



        //DEBUG------------------------------------------------------------------------------------------------------------------------
        for (int i = 0; i < doors.Count; i++)
        {

            for (int j = i; j < doors.Count; j++)
            {

                if (doors[i] != doors[j] && doors[i].GetDoorID() == doors[j].GetDoorID())
                {
                    Debug.LogError("Le porte " + doors[i].transform.parent + " e " + doors[j].transform.parent
                    + " hanno lo stesso ID: " + doors[i].GetDoorID());
                }

            }

        }

    }

    public void PositionPlayer(int doorIndex)
    {
        //posiziona il giocatore nella posizione di spawn della porta da cui deve entrare
        player.position = doors[doorIndex].GetSpawnPosition();

    }

    public int GetRoomID() { return roomID; }

}
