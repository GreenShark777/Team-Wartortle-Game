//Si occupa della creazione e gestione della minimappa
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour, IUpdateData
{
    //lista di tutte le stanze presenti
    private static List<RoomsBehaviour> listOfRooms;
    //riferimento all'immagine di stanza
    private GameObject roomImage;

    //riferimento all'immagine che rappresenta la posizione del giocatore
    //private Image playerDot;

    //riferimento al colore iniziale delle stanze(ovvero quello di quando il giocatore non è all'interno di quella stanza)
    private Color startImageRoomColor;
    //riferimento all'empty che fungerà da pivot per le immagini stanza durante il posizionamento
    private RectTransform newRoomPivot;

    //lista di tutte le grandezze che le immagini di stanze di diverso tipo devono avere
    [SerializeField]
    private Vector2 squareRoomScale = new Vector3(0.35f, 0.35f, 1), //STANZE QUADRATE
        rectangleRoomScale = new Vector3(0.64f, 0.64f, 1), //STANZE RETTANGOLARI
        LRoomScale = new Vector3(1.45f, 1.45f, 1); //STANZE A L

    //lista di riferimenti alle immagini di stanza create
    private List<Transform> allRoomImages = new List<Transform>();
    //lista di tutti i contenitori di porte attivi delle immagini stanza
    [SerializeField]
    private List<Transform> allRoomImagesDoorsContainer = new List<Transform>();
    
    //lista di tutte le posizioni delle porte
    //[SerializeField]
    //private List<Vector2> doorsPositions = new List<Vector2>();
    
    //lista degli ID delle porte che devono diventare per le proprie stanze(l'ID della stanza è uguale all'indice in cui si trova il valore)
    [SerializeField]
    private List<int> pivotDoors = new List<int>();
    //lista degli ID delle porte che devono fungere da punto d'ancoraggio per le stanze pivot(gli indici indicano la porta pivot a cui faranno da ancora)
    [SerializeField]
    private List<int> anchorDoors = new List<int>();
    //lista di tutti gli ID delle stanze delle porte d'ancoraggio
    [SerializeField]
    private List<int> anchorDoorsRooms = new List<int>();
    //riferimento al GameManag
    [SerializeField]
    private GameManag g = default;

    //indica la nuova posizione dell'ancora di ogni nuova immagine di stanza
    //private List<Vector2> newAnchorsPosition = new List<Vector2>();


    private void Awake()
    {
        //ottiene il riferimento al pallino che indica la posizione del giocatore
        //playerDot = transform.GetChild(1).GetComponent<Image>();
    }

    private void Start()
    {
        //ottiene la lista delle stanze dal manager delle stanze
        listOfRooms = RoomsManager.GetRoomsList();
        //ottiene il riferimento all'immagine da duplicare ogni volta che viene creata una stanza nella minimappa
        roomImage = transform.GetChild(0).gameObject/*.GetComponent<Image>()*/;
        //ottiene il riferimento al colore iniziale delle stanze
        startImageRoomColor = roomImage.transform.GetChild(0).GetComponent<Image>().color;
        //le dimensioni delle stanze vengono scalate alla grandezza dell'immagine stanza di riferimento
        squareRoomScale = new Vector3(squareRoomScale.x * roomImage.transform.localScale.y, squareRoomScale.y * roomImage.transform.localScale.y, 1);
        rectangleRoomScale = new Vector3(rectangleRoomScale.x * roomImage.transform.localScale.y, rectangleRoomScale.y * roomImage.transform.localScale.y, 1);
        LRoomScale = new Vector3(LRoomScale.x * roomImage.transform.localScale.y, LRoomScale.y * roomImage.transform.localScale.y, 1);
        //ottiene il riferimento all'empty che fungerà da pivot per le immagini stanza
        newRoomPivot = transform.GetChild(2).GetComponent<RectTransform>();

        //doorImage = transform.GetChild(1).GetComponent<Image>();

        //genera la mini mappa
        GenerateMiniMap();
        //disattiva le stanze non ancora esplorate dal giocatore
        ShowOnlySeenRooms();
        //disattiva le immagini iniziali di porta e stanza, in quanto non servono più
        roomImage.gameObject.SetActive(false);
        //posiziona il pallino del giocatore nella stanza attuale
        MovePlayerDot(RoomsManager.GetLastEnteredRoom());

    }
    /// <summary>
    /// Genera la minimappa
    /// </summary>
    private void GenerateMiniMap()
    {
        //per ogni stanza nella lista di stanze...
        foreach (RoomsBehaviour room in listOfRooms)
        {
            Debug.Log("RoomImage: " + roomImage);
            //...crea una nuova immagine di stanza, ne calcola la posizione e rotazione e lo rende figlio di questo gameobject...
            GameObject newRoomImage = Instantiate(roomImage, Vector2.zero, room.transform.rotation, transform);

            //FORSE CONVIENE CALCOLARE LA POSIZIONE DELLE STANZE UNA VOLTA CHE SONO STATE CREATE TUTTE
            //*newRoomImage.transform.localPosition = */CalculateRoomPosition(room.GetRoomID(), newRoomImage.rectTransform);

            //Image newRoomImage = newRoom.GetComponent<Image>();

            //...alla nuova immagine viene dato lo sprite di questa stanza...
            newRoomImage.transform.GetChild(0).GetComponent<Image>().sprite = room.GetThisRoomSprite();
            //...ottiene il nome dello sprite della stanza...
            string roomSpriteName = room.GetThisRoomSprite().name;
            //...cambia la grandezza dell'immagine in base al nome dello sprite(bisogna farlo altrimenti alcune stanze vengono viste più grandi di come sono veramente)
            if (roomSpriteName.Contains("quadrata"))
            { newRoomImage.transform.localScale = squareRoomScale; }
            else if (roomSpriteName.Contains("rettangolo"))
            { newRoomImage.transform.localScale = rectangleRoomScale; }
            else if (roomSpriteName.Contains("L"))
            { newRoomImage.transform.localScale = LRoomScale; }
            //...aggiunge la nuova immagine alla lista...
            allRoomImages.Add(newRoomImage.transform);
            //...ottiene il contenitore delle porte di questa stanza...
            Transform thisRoomDoorsContainer = room.GetThisRoomDoorsContainer();
            //...ottiene, in base al suo indice, il contenitore delle porte di questa immagine di stanza e la attiva...
            Transform thisImageRoomDoors = newRoomImage.transform.GetChild(1).GetChild(thisRoomDoorsContainer.GetSiblingIndex());
            //Debug.LogError("Contenitore Stanza: " + thisRoomDoorsContainer);
            thisImageRoomDoors.gameObject.SetActive(true);
            //...e per ogni porta nei contenitori...
            for (int door = 0; door < thisRoomDoorsContainer.childCount; door++)
            {
                //...ottiene il riferimento allo script di questa porta...
                DoorsBehaviour thisDoor = thisRoomDoorsContainer.GetChild(door).GetComponent<DoorsBehaviour>();
                //...controlla se la porta è attiva o meno...
                bool doorIsActive = thisDoor.gameObject.activeSelf;
                //...se è la porta è attiva viene mostrata nella minimappa, altrimenti viene disattivata
                thisImageRoomDoors.GetChild(door).gameObject.SetActive(doorIsActive);
                //..se la porta è attiva...
                if (doorIsActive)
                {

                    //bool isNewDoor = true;

                    //foreach (int ID in doorsX) { if (thisDoor.GetOwnRoomID() == ID) { isNewDoor = false; break; } }

                    if (/*isNewDoor*/pivotDoors.Count < allRoomImages.Count)
                    {
                        //...ottiene la posizione della porta nella UI...
                        //doorsPositions.Add(thisDoor.transform.position);
                        //...aggiunge, alla lista di indici di porte pivot, l'ID di questa porta...
                        pivotDoors.Add(thisDoor.transform.GetSiblingIndex());
                        //...aggiunge, alla lista di indici di porte d'ancoraggio, l'ID della porta dopo...
                        anchorDoors.Add(thisDoor.GetNextDoor().transform.GetSiblingIndex());
                        //...e di quest'ultima porta ottiene l'ID della stanza di cui fa parte
                        anchorDoorsRooms.Add(thisDoor.GetNextDoor().GetOwnRoomID()/*index*/);

                    }

                }

                //if (thisRoomDoorsContainer.GetChild(door).gameObject.activeSelf)
                //{
                //    //...crea una nuova immagine di porta, ne calcola la posizione e rotazione e lo rende figlio dell'immagine di stanza
                //    Image newDoorImage = Instantiate(doorImage, Vector2.zero, thisRoomDoorsContainer.GetChild(door).rotation, newRoomImage.transform);

                //    CalculateDoorPosition(room.GetRoomID(), door, thisRoomDoorsContainer, newDoorImage.rectTransform);

                //    newAnchorsPosition.Add(newDoorImage.rectTransform.position);

                //}

            }
            //infine, aggiunge alla lista di container di porte il contenitore di porte previamente preso in riferimento
            allRoomImagesDoorsContainer.Add(thisImageRoomDoors/*.GetComponent<RectTransform>()*/);
            Debug.Log("Creata, nella mini mappa, la stanza: " + room.name);
        }

        //roomImage.transform.SetAsLastSibling();
        //playerDot.transform.SetAsLastSibling();

        //infine, calcola la posizione di ogni stanza
        //foreach (Transform room in allRoomImages) { ChangeRoomImagePosition(room.GetComponent<RectTransform>()); }
        //mette le stanze nelle posizioni adeguate, usando le liste di porte pivot e ancoraggio
        ChangeRoomImagesPosition();
        Debug.Log("Finita minimappa");
    }
    /// <summary>
    /// Si occupa di posizionare correttamente le immagini di stanze nella minimappa
    /// </summary>
    private void ChangeRoomImagesPosition()
    {
        //cicla ogni immagine di stanza creata
        for (int i = 0; i < allRoomImages.Count; i++)
        {
            //l'empty che funge da pivot per le immagini di stanza viene posizionata nella porta che funge da pivot per questa stanza
            newRoomPivot.position = allRoomImagesDoorsContainer[i].GetChild(pivotDoors[i]).position;
            //Debug.LogError("Alla stanza " + allRoomImages[i] + " viene dato come pivot: " + allRoomImagesDoorsContainer[i].GetChild(pivotDoors[i]));
            //l'empty diventa parent della stanza ciclata
            allRoomImages[i].SetParent(newRoomPivot, true);
            //if(i+1 != allRoomImagesDoorsContainer.Count) newRoomPivot.position = allRoomImagesDoorsContainer[i+1].GetChild(doorsY[i]).position;
            //l'empty che funge da pivot viene messo nella posizione della porta d'ancoraggio
            newRoomPivot.position = allRoomImagesDoorsContainer[anchorDoorsRooms[i]].GetChild(anchorDoors[i]).position;
            //l'immagine di stanza torna ad essere figlio della minimappa
            allRoomImages[i].SetParent(newRoomPivot.parent, true);
            //Debug.LogError("Stanza indice " + i + " spostata nella posizione della porta " + allRoomImagesDoorsContainer[anchorDoorsRooms[i]].GetChild(anchorDoors[i]) +
            //    " del contenitore porte " + allRoomImagesDoorsContainer[anchorDoorsRooms[i]] + " della stanza ad indice "
            //    + allRoomImagesDoorsContainer[anchorDoorsRooms[i]].parent.parent.GetSiblingIndex());
        }
        //ogni immagine di stanza viene fatta diventare nuovamente figlia del pivot(bisogna farlo qua altrimenti nel ciclo qua sopra vengono cambiate le grandezze delle immagini)
        foreach (Transform thisRoomImage in allRoomImages) { thisRoomImage.SetParent(newRoomPivot); }
        //sposta il pivot, e tutte le immagini stanza ora sue figlie, al centro della minimappa
        newRoomPivot.position = transform.position;

    }

    private void ShowOnlySeenRooms()
    {
        //cicla ogni immagine di stanza nella lista
        for (int i = 0; i < allRoomImages.Count; i++)
        {
            //se la stanza a cui si riferisce non è stata ancora vista dal giocatore, viene disattivata
            if (!g.seenRooms[i]) { allRoomImages[i].gameObject.SetActive(false); }

        }

    }

    /*
    private void ChangeRoomImagePosition(RectTransform room)
    {

        RectTransform previousParent = room.GetComponentInParent<RectTransform>();

        int thisRoomIndex = room.GetSiblingIndex();
        Debug.LogError(thisRoomIndex);
        if (thisRoomIndex > 0)
        {

            //room.parent = allRoomImagesDoorsContainer[thisRoomIndex].GetChild(doorsY[thisRoomIndex]);
            
            //room.GetComponent<RectTransform>().parent

            //RectTransform roomPivot = allRoomImagesDoorsContainer[thisRoomIndex].GetChild(doorsY[thisRoomIndex]).GetComponent<RectTransform>();

            newRoomPivot.position = allRoomImagesDoorsContainer[thisRoomIndex].GetChild(doorsY[thisRoomIndex]).GetComponent<RectTransform>().position;

            //room.SetParent(null, true);

            //roomPivot.SetParent(previousParent);

            room.SetParent(newRoomPivot, false);

            //room.position = Vector2.zero;

            //room.SetAsLastSibling();

            //roomPivot.position = doorsPositions[thisRoomIndex];
            
            //roomPivot.position = /*doorsPositions[thisRoomIndex]allRoomImagesDoorsContainer[thisRoomIndex - 1].GetChild(doorsX[thisRoomIndex]).position;
            
            newRoomPivot.position = /*doorsPositions[thisRoomIndex]allRoomImagesDoorsContainer[thisRoomIndex - 1].GetChild(doorsX[thisRoomIndex]).GetComponent<RectTransform>().position;
            //else { roomPivot.position = /*doorsPositions[thisRoomIndex]allRoomImagesDoorsContainer[thisRoomIndex - 1].GetChild(doorsX[thisRoomIndex]).position; }
            //Debug.Log(roomPivot + " : " + room.parent);
        }
        else { room.position = new Vector2(); }
        Debug.Log("New Room Parent: " + room.parent);
        room.SetParent(previousParent);

        room.SetSiblingIndex(thisRoomIndex);

        room.localScale = Vector3.one;
        //Debug.LogError(thisRoomIndex);
        Debug.Log("New Room Pos: " + room.position);
    }
    */
    /*
    private void CalculateRoomPosition(int thisRoomID, RectTransform roomImageRect)
    {

        Vector2 newPos = Vector2.zero;

        if (newAnchorsPosition.Count > 0)
        {

            for (int anchorPos = 0; anchorPos < newAnchorsPosition.Count; anchorPos++)
            {

                if (thisRoomID == anchorPos - 1) { newPos = newAnchorsPosition[anchorPos]; break; }

            }
            Debug.Log("Immagine stanza: " + listOfRooms[thisRoomID] + " in posizione porta: " + newPos);
        }

        roomImageRect.position = newPos;

    }
    */

    /*
    private void CalculateDoorPosition(int roomID, int doorID, Transform roomDoorsContainer, RectTransform doorImageRect)
    {

        Vector2 newPos = Vector2.zero;

        //newPos = new Vector2(Random.Range(0, 500), Random.Range(0, 500));
        newPos = listOfRooms[roomID].transform.position - roomDoorsContainer.GetChild(doorID).position;

        doorImageRect.anchoredPosition = newPos;

    }
    */
    /// <summary>
    /// Permette di spostare il pallino del giocatore nella stanza specificata dall'ID ricevuto
    /// </summary>
    /// <param name="roomID"></param>
    public void MovePlayerDot(int roomID)
    {

        //playerDot.transform.position = allRoomImages[roomID].transform.position; Debug.Log("Mosso Dot nella stanza: " + roomID);

        //ottiene il riferimento al GameObject dell'immagine della stanza appena entrata
        GameObject enteredRoomImage = allRoomImages[roomID].gameObject;
        //se è disattiva, la attiva
        if (!enteredRoomImage.activeSelf) enteredRoomImage.SetActive(true);
        //indice che indica la porta a cui siamo arrivati nella lista
        int i = 0;
        //cicla ogni oggetto nella lista di immagini stanza
        foreach (Transform imageOfRoom in allRoomImages)
        {
            //se l'immagine di stanze è attiva...
            if (imageOfRoom.gameObject.activeSelf)
            {
                //...crea un nuovo colore, inizializzato al colore iniziale delle immagini stanza...
                Color newColor = startImageRoomColor;
                //...se questa stanza è quella in cui si trova il giocatore...
                if (i == roomID)
                {
                    //...il nuovo colore di questa stanza sarà il blu...
                    newColor = Color.blue;
                    //...e il suo alpha sarà uguale a quello di tutte le stanze
                    newColor.a = startImageRoomColor.a;

                }
                //...e il colore di questa stanza verrà cambiato al nuovo colore ottenuto
                imageOfRoom.GetChild(0).GetComponent<Image>().color = newColor;
            }
            //l'indice viene incrementato per continuare il controllo
            i++;

        }
    
    }

    public void UpdateData()
    {

        int index = 0;

        foreach (Transform imageRoom in allRoomImages)
        {

            g.seenRooms[index] = imageRoom.gameObject.activeSelf;

            index++;

        }

    }

}
