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


    private void Start()
    {
        //ottiene la lista delle stanze dal manager delle stanze
        listOfRooms = RoomsManager.GetRoomsList();
        //ottiene il riferimento all'immagine da duplicare ogni volta che viene creata una stanza nella minimappa
        roomImage = transform.GetChild(0).GetComponent<Image>();
        doorImage = transform.GetChild(1).GetComponent<Image>();
        //genera la mini mappa
        GenerateMiniMap();
        //disattiva la prima immagine, in quanto di troppo
        roomImage.gameObject.SetActive(false);
        doorImage.gameObject.SetActive(false);

    }

    private void GenerateMiniMap()
    {

        List<Image> doorImages = new List<Image>();

        foreach (RoomsBehaviour room in listOfRooms)
        {

            Image newRoomImage = Instantiate(roomImage, CalculateRoomPosition(), room.transform.rotation, transform);

            //Image newRoomImage = newRoom.GetComponent<Image>();

            newRoomImage.sprite = room.GetThisRoomSprite();

            Transform thisRoomDoorsContainer = room.GetThisRoomDoorsContainer();

            for (int door = 0; door < thisRoomDoorsContainer.childCount; door++)
            {

                Image newDoorImage = Instantiate(doorImage, CalculateDoorPosition(), thisRoomDoorsContainer.GetChild(door).rotation, newRoomImage.transform);

            }
            Debug.Log("Creata, nella mini mappa, la stanza: " + room.name);
        }

    }

    private Vector2 CalculateRoomPosition()
    {

        Vector2 newPos = Vector2.zero;

        return newPos;

    }

    private Vector2 CalculateDoorPosition()
    {

        Vector2 newPos = Vector2.zero;

        return newPos;

    }

}
