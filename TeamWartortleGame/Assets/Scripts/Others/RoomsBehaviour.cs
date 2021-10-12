using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsBehaviour : MonoBehaviour
{
    //lista contenente tutti gli script delle porte di questa stanza
    private List<DoorsBehaviour> doors = new List<DoorsBehaviour>();
    //riferimento al contenitore di tutte le porte
    private Transform doorsContainer, 
        collidersContainer;
    //riferimento al giocatore
    public static Transform player;
    //identificativo della stanza
    [SerializeField]
    private int roomID = 0;
    //riferimento allo sprite della stanza
    [SerializeField]
    private SpriteRenderer roomSprite = default;
    //indica il tipo di questa stanza
    // 0 - quadrata
    // 1 - rettangolare
    // 2 - T
    // 3 - L
    // 4 - boss
    //private int roomType;


    private void Awake()
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
        //ottiene il riferimento al contenitore dei collider delle stanze
        collidersContainer = transform.GetChild(1);
        //attiva il collider adatto alla stanza
        ActivateRoomCollider();



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

    private void ActivateRoomCollider()
    {
        //ottiene il nome dello sprite della stanza
        string roomSpriteName = roomSprite.sprite.name;
        //indice che indica il collider da attivare
        int collToActivate;
        //se lo sprite della stanza è quello della stanza quadrata, bisogna attivare il collider ad indice 0
        if (roomSpriteName.Contains("quadrata")) { collToActivate = 0; }
        //altrimenti,se lo sprite della stanza è quello della stanza rettangolare, bisogna attivare il collider ad indice 1
        else if (roomSpriteName.Contains("rettangolare")) { collToActivate = 1; }
        //altrimenti,se lo sprite della stanza è quello della stanza a T, bisogna attivare il collider ad indice 2
        else if (roomSpriteName.Contains("T")) { collToActivate = 2; }
        //altrimenti,se lo sprite della stanza è quello della stanza a L, bisogna attivare il collider ad indice 3
        else if (roomSpriteName.Contains("L")) { collToActivate = 3; }
        //altrimenti, sarà la stanza del boss, quindi attiva il collider all'indice 4
        else { collToActivate = 4; }
        //attiva il collider figlio del contenitore dei collider all'indice ottenuto
        collidersContainer.GetChild(collToActivate).gameObject.SetActive(true);
        //ottiene il tipo di questa stanza
        //roomType = collToActivate;
        Debug.Log("Nome sprite: " + roomSpriteName + " -> coll: " + collToActivate);
    }
    /// <summary>
    /// Permette ad altri script di ottenere l'ID di questa stanza
    /// </summary>
    /// <returns></returns>
    public int GetRoomID() { return roomID; }

    /// <summary>
    /// Permette agli altri script di ottenere il tipo di questa stanza
    /// 0 - quadrata
    /// 1 - rettangolare
    /// 2 - T
    /// 3 - L
    /// 4 - boss
    /// </summary>
    /// <returns></returns>
    //public int GetThisRoomType() { return roomType; }

    /// <summary>
    /// Permette agli altri script di ottenere lo sprite di questa stanza
    /// </summary>
    /// <returns></returns>
    public Sprite GetThisRoomSprite() { return roomSprite.sprite; }
    /// <summary>
    /// Permette ad altri script di ottenere il riferimento al contenitore delle porte
    /// </summary>
    /// <returns></returns>
    public Transform GetThisRoomDoorsContainer() { return doorsContainer; }

}
