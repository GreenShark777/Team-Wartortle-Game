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
        //indicherà il nuovo indice delle porte di questa stanza
        int newID = 0;
        //per ogni figlio nel contenitore delle porte...
        foreach(Transform child in doorsContainer)
        {
            //...ne ottiene lo script da porta...
            DoorsBehaviour door = child.GetComponent<DoorsBehaviour>();
            //...lo aggiunge alla lista di porte...
            doors.Add(door);
            //...gli da un ID unico...
            door.SetDoorID(newID);
            //...e gli comunica a quale stanza appartiene...
            door.SetOwnRoomID(roomID);
            Debug.Log("Porta: " + door.transform.name + " con indice: " + newID);
            //...infine, incrementa l'indice in modo che la prossima porta abbia un ID diverso 
            newID++;

        }



        //DEBUG------------------------------------------------------------------------------------------------------------------------
        for (int i = 0; i < doors.Count; i++)
        {

            for (int j = i; j < doors.Count; j++)
            {

                if (doors[i] != doors[j] && doors[i].GetDoorID() == doors[j].GetDoorID())
                {
                    Debug.LogError("Le porte " + doors[i].transform.name + " e " + doors[j].transform.name
                    + " della stanza " + transform.name + " hanno lo stesso ID: " + doors[i].GetDoorID());
                }

            }

        }

    }
    /// <summary>
    /// Posiziona il giocatore davanti la porta indicata dal parametro indice ricevuto
    /// </summary>
    /// <param name="doorIndex"></param>
    public void PositionPlayer(int doorIndex)
    {
        //attiva questa stanza
        gameObject.SetActive(true);
        //posiziona il giocatore nella posizione di spawn della porta da cui sta entrando
        player.position = doors[doorIndex].GetSpawnPosition();

    }
    /// <summary>
    /// Permette ad altri script di ottenere l'ID di questa stanza
    /// </summary>
    /// <returns></returns>
    public int GetRoomID() { return roomID; }

}
