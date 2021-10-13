using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsBehaviour : MonoBehaviour
{
    //lista contenente tutti gli script delle porte di questa stanza
    [SerializeField]
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
    //indica se la telecamera deve essere ferma o meno
    [SerializeField]
    private bool staticCamera = false;
    //indica i limiti della telecamera
    [SerializeField]
    private float maxCameraLookX = default, 
        maxCameraLookY = default;
    //indica il tipo di questa stanza
    // 0 - quadrata
    // 1 - rettangolare
    // 2 - T
    // 3 - L
    // 4 - boss
    //private int roomType;


    private void Awake()
    {
        //ottiene il riferimento al contenitore dei collider delle stanze
        collidersContainer = transform.GetChild(1);
        //attiva il collider adatto alla stanza e ottiene il riferimento al suo contenitore di porte
        ActivateRoomColliderAndDoors();
        //indicherà il nuovo indice delle porte di questa stanza
        int newID = 0;
        //per ogni figlio nel contenitore delle porte...
        foreach(Transform child in doorsContainer)
        {
            //...se il figlio è attivo...
            if (child.gameObject.activeSelf)
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

    private void ActivateRoomColliderAndDoors()
    {
        //ottiene il nome dello sprite della stanza
        string roomSpriteName = roomSprite.sprite.name;
        //indice che indica il collider da attivare
        int indexToActivate;
        //se lo sprite della stanza è quello della stanza quadrata, bisogna attivare il collider ad indice 0
        if (roomSpriteName.Contains("quadrata")) { indexToActivate = 0; }
        //altrimenti,se lo sprite della stanza è quello della stanza rettangolare, bisogna attivare il collider ad indice 1
        else if (roomSpriteName.Contains("rettangolare")) { indexToActivate = 1; }
        //altrimenti,se lo sprite della stanza è quello della stanza a T, bisogna attivare il collider ad indice 2
        else if (roomSpriteName.Contains("T")) { indexToActivate = 2; }
        //altrimenti,se lo sprite della stanza è quello della stanza a L, bisogna attivare il collider ad indice 3
        else if (roomSpriteName.Contains("L")) { indexToActivate = 3; }
        //altrimenti, sarà la stanza del boss, quindi attiva il collider all'indice 4
        else { indexToActivate = 4; }
        //attiva il collider figlio del contenitore dei collider all'indice ottenuto
        collidersContainer.GetChild(indexToActivate).gameObject.SetActive(true);
        //ottiene il riferimento al contenitore di tutte le porte di questo tipo di stanza
        doorsContainer = transform.GetChild(0).GetChild(indexToActivate);
        //ottiene il tipo di questa stanza
        //roomType = collToActivate;
        Debug.Log("Nome sprite: " + roomSpriteName + " -> coll: " + indexToActivate);
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
    /// <summary>
    /// Permette ad altri script di sapere se questa stanza è piccola abbastanza da rendere la telecamera statica
    /// </summary>
    /// <returns></returns>
    public bool IsSmallRoom() { return staticCamera; }

    private void OnDrawGizmos()
    {
        if (!staticCamera)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireCube(roomSprite.transform.position, new Vector3(maxCameraLookX, maxCameraLookY));
        }

    }

}
