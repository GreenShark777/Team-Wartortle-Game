//Si occupa delle stanze e di ciò che è contenuto al loro interno(nemici, porte, colliders, props, ecc...)
using System.Collections.Generic;
using UnityEngine;

public class RoomsBehaviour : MonoBehaviour
{
    //lista contenente tutti gli script delle porte di questa stanza
    private List<DoorsBehaviour> doors = new List<DoorsBehaviour>();
    
    private Transform doorsContainer, //riferimento al contenitore di tutte le porte
        collidersContainer, //riferimento al contenitore dei collider per i vari tipi di stanze
        enemiesContainer; //riferimento al contenitore di tutti i nemici nella stanza

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
    //indica di quanto i limiti della telecamera devono essere minori di quelli della stanza
    [SerializeField]
    private float xLimitsOffset = default,
        yLimitsOffset = default;

    //indica il tipo di questa stanza
    // 0 - quadrata
    // 1 - rettangolare
    // 2 - T
    // 3 - L
    // 4 - boss
    //private int roomType;

    //indice da dare ad ogni stanza ad Awake(in modo che sia sempre differente)
    //private static int newRoomID = -1;


    private void Awake()
    {
        //viene incrementato il nuovo ID da dare alle stanze
        //newRoomID++;
        //la stanza ottiene un nuovo ID unico
        //roomID = newRoomID;

        //ottiene il riferimento al contenitore dei collider delle stanze
        collidersContainer = transform.GetChild(1);
        //ottiene il riferimento al contenitore dei nemici
        enemiesContainer = transform.GetChild(2);
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
    public void ActivateThisRoom(int doorIndex)
    {
        Debug.Log("Door" + doorIndex);
        Debug.Log("Nome Porta" + doors[doorIndex].name);
        Debug.Log("Room" + roomID);
        //attiva questa stanza
        gameObject.SetActive(true);
        //posiziona il giocatore nella posizione di spawn della porta da cui sta entrando
        player.position = doors[doorIndex].GetSpawnPosition();

        //L'INIZIALIZZAMENTO DEI NEMICI VIENE FATTO DENTRO IL LORO SCRIPT NELLA FUNZIONE ONENABLE
        /*
        //se ci sono nemici attivi nella stanza...
        if (AreThereEnemies())
        {
            //...ne inizializza lo stato
            foreach () { }

        }
        */

    }
    /// <summary>
    /// Attiva i collider di questa stanza in base allo sprite che sta usando
    /// </summary>
    private void ActivateRoomColliderAndDoors()
    {
        //ottiene il nome dello sprite della stanza
        string roomSpriteName = roomSprite.sprite.name;
        //indice che indica il collider da attivare
        int indexToActivate;
        //se lo sprite della stanza è quello della stanza quadrata, bisogna attivare il collider ad indice 0
        if (roomSpriteName.Contains("quadrata")) { indexToActivate = 0; }
        //altrimenti,se lo sprite della stanza è quello della stanza rettangolare, bisogna attivare il collider ad indice 1(se la stanza è gigante sarà invece 5)
        else if (roomSpriteName.Contains("rettangolare")) { indexToActivate = (!roomSpriteName.Contains("gigante")) ? 1 : 5; }
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
    /// Permette di ottenere il riferimento allo spriteRenderer dello sprite di questa stanza
    /// </summary>
    /// <returns></returns>
    public SpriteRenderer GetThisRoomSpriteRend() { return roomSprite; }
    /// <summary>
    /// Permette di ottenere i limiti di questa stanza
    /// </summary>
    /// <returns></returns>
    public Vector2 GetRoomBounds() { return new Vector2(maxCameraLookX, maxCameraLookY); }
    /// <summary>
    /// Permette di ottenere l'offset dei limiti della telecamera nell'asse X
    /// </summary>
    /// <returns></returns>
    public float GetRoomBoundsOffsetX() { return xLimitsOffset; }
    /// <summary>
    /// Permette di ottenere l'offset dei limiti della telecamera nell'asse Y
    /// </summary>
    /// <returns></returns>
    public float GetRoomBoundsOffsetY() { return yLimitsOffset; }
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
    /// <summary>
    /// Comunica se in questa stanza ci sono nemici o meno
    /// </summary>
    /// <returns></returns>
    public bool AreThereEnemies()
    {
        //indica se ci sono nemici nella stanza
        bool thereAreEnemies = false;
        //se ci sono dei nemici nel container dei nemici...
        if (enemiesContainer.childCount > 0)
        {
            //...cicla tutti i nemici e, se almeno uno di loro è attivo, comunica che ci sono nemici
            foreach (Transform enemy in enemiesContainer) { if (enemy.gameObject.activeSelf) { thereAreEnemies = true; break; } }

        }
        //ritorna il valore ottenuto dal controllo
        return thereAreEnemies;

    }
    /// <summary>
    /// Disattiva tutti i nemici in questa stanza
    /// </summary>
    public void DeactivateAllEnemies()
    {
        //disattiva tutti nemici nel container di nemici
        foreach (Transform enemy in enemiesContainer) { enemy.gameObject.SetActive(false); }
        //Debug.LogError("Disattivati nemici stanza: " + name);
    }

    private void OnDrawGizmos()
    {
        if (!staticCamera)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireCube(roomSprite.transform.position, new Vector3(maxCameraLookX, maxCameraLookY));
        }

    }

}
