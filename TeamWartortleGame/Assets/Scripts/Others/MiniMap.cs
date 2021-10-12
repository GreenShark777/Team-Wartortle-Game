using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour
{
    //lista di tutte le stanze presenti
    private List<RoomsBehaviour> listOfRooms;
    //riferimento all'immagine di stanza
    private Image roomImage;
    //riferimento all'immagine di porta delle stanze
    private Image doorImage;
    //riferimento all'immagine che rappresenta la posizione del giocatore
    private Image playerDot;
    //lista di riferimenti alle immagini di stanza create
    private List<Image> allRoomImages = new List<Image>();
    //indica la nuova posizione dell'ancora di ogni nuova immagine di stanza
    private List<Vector2> newAnchorsPosition = new List<Vector2>();


    private void Start()
    {
        //ottiene la lista delle stanze dal manager delle stanze
        listOfRooms = RoomsManager.GetRoomsList();
        //ottiene il riferimento all'immagine da duplicare ogni volta che viene creata una stanza nella minimappa
        roomImage = transform.GetChild(0).GetComponent<Image>();
        doorImage = transform.GetChild(1).GetComponent<Image>();
        playerDot = transform.GetChild(2).GetComponent<Image>();
        //genera la mini mappa
        GenerateMiniMap();
        //disattiva le immagini iniziali di porta e stanza, in quanto non servono più
        roomImage.gameObject.SetActive(false);
        doorImage.gameObject.SetActive(false);

    }

    private void GenerateMiniMap()
    {
        //per ogni stanza nella lista di stanze...
        foreach (RoomsBehaviour room in listOfRooms)
        {
            //...crea una nuova immagine di stanza, ne calcola la posizione e rotazione e lo rende figlio di questo gameobject...
            Image newRoomImage = Instantiate(roomImage, Vector2.zero, room.transform.rotation, transform);

            //FORSE CONVIENE CALCOLARE LA POSIZIONE DELLE STANZE UNA VOLTA CHE SONO STATE CREATE TUTTE
            /*newRoomImage.transform.localPosition = */CalculateRoomPosition(room.GetRoomID(), newRoomImage.rectTransform);

            //Image newRoomImage = newRoom.GetComponent<Image>();
            //...alla nuova immagine viene dato lo sprite di questa stanza...
            newRoomImage.sprite = room.GetThisRoomSprite();
            //...aggiunge la nuova immagine alla lista...
            allRoomImages.Add(newRoomImage);
            //...ottiene il contenitore delle porte di questa stanza...
            Transform thisRoomDoorsContainer = room.GetThisRoomDoorsContainer();
            //...e per ogni suo figlio...
            for (int door = 0; door < thisRoomDoorsContainer.childCount; door++)
            {
                //...crea una nuova immagine di porta, ne calcola la posizione e rotazione e lo rende figlio dell'immagine di stanza
                Image newDoorImage = Instantiate(doorImage, Vector2.zero, thisRoomDoorsContainer.GetChild(door).rotation, newRoomImage.transform);

                CalculateDoorPosition(room.GetRoomID(), door, thisRoomDoorsContainer, newDoorImage.rectTransform);

                newAnchorsPosition.Add(newDoorImage.rectTransform.position);

            }
            Debug.Log("Creata, nella mini mappa, la stanza: " + room.name);
        }

    }

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

    private void CalculateDoorPosition(int roomID, int doorID, Transform roomDoorsContainer, RectTransform doorImageRect)
    {

        Vector2 newPos = Vector2.zero;

        //newPos = new Vector2(Random.Range(0, 500), Random.Range(0, 500));
        newPos = listOfRooms[roomID].transform.position - roomDoorsContainer.GetChild(doorID).position;

        doorImageRect.anchoredPosition = newPos;

    }

    public void MovePlayerDot(int roomID) { playerDot.transform.position = allRoomImages[roomID].rectTransform.position; Debug.Log("Mosso Dot nella stanza: " + roomID); }

}
