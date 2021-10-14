using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerSpriteManager : MonoBehaviour
{
    //lista di tutti gli sprite del giocatore
    //private List<SpriteRenderer> playerSprites = new List<SpriteRenderer>();
    //riferimento al SortingGroup degli sprite del giocatore
    private SortingGroup playerSpritesSG;
    //lista di riferimenti degli sprite da disattivare
    private List<GameObject> spritesToDeactivate = new List<GameObject>();
    //riferimento al manager della profondità degli sprite del giocatore
    [SerializeField]
    private Transform playerDepthManager = default;
    //riferimento al PlayerControlls del gicoatore
    //[SerializeField]
    //private PlayerControlls pc = default;
    //[SerializeField]
    //private SwipeSimulation SwipeSimul = default;

    private void Start()
    {
        //SwipeSimul = FindObjectOfType<SwipeSimulation>();
        //ottiene il riferimento al contenitore di tutti gli sprite del giocatore
        //Transform spriteContainer = transform/*.GetChild(0)*/;
        //viene creato un recipiente di tutti gli sprite del giocatore
        //var spritesRecipient = spriteContainer.GetComponentsInChildren<SpriteRenderer>(true);
        //tutti gli sprite vengono inseriti nella lista finale
        //for (int i = 0; i < spritesRecipient.Length; i++) { /*Debug.Log(spritesRecipient[i]);*/ playerSprites.Add(spritesRecipient[i]); }
        //ottiene il riferimento al SortingGroup degli sprite del giocatore
        playerSpritesSG = /*spriteContainer.GetComponent<SortingGroup>()*/GetComponent<SortingGroup>();

        //ottiene il riferimento agli oggetti da disattivare, ciclando ogni sprite nella lista
        /*for (int obj = 0; obj < playerSprites.Count; obj++)
        {
            //ottiene il riferimento al padre dello sprite che si sta controllando
            GameObject thisSpriteParent = playerSprites[obj].transform.parent.gameObject;
            //se questo sprite non è già nella lista e non è figlio del primo figlio del giocatore, lo mette nella lista
            if (!spritesToDeactivate.Contains(thisSpriteParent) && thisSpriteParent.transform != spriteContainer.GetChild(0)) { spritesToDeactivate.Add(thisSpriteParent); }
            //Debug.Log("Sprite controllato: " + playerSprites[obj] + ", con padre: " + thisSpriteParent + " , da mettere in lista? -> "
            //    + (!spritesToDeactivate.Contains(thisSpriteParent) && thisSpriteParent.transform != spriteContainer));
        }*/
        //Debug.Log("Oggetti nella lista: " + spritesToDeactivate.Count);

        ////inizializza un riferimento per il giocatore originale, nel caso questo giocatore sia un duplicato
        //PlayerControlls otherPlayerPc = null;
        ////se questo giocatore è un duplicato...
        //if (GetComponent<DuplicatesBehaviour>())
        //{
        //    //...crea una lista con tutti gli script PlayerControlls dei possibili duplicati in scena...
        //    PlayerControlls[] pcFound = FindObjectsOfType<PlayerControlls>();
        //    //...e cicla ogni oggetto nella lista finchè non trova uno script adatto ed esce dal ciclo
        //    foreach (PlayerControlls newPc in pcFound)
        //    { if (newPc != null) { otherPlayerPc = newPc; break; } };

        //}
        //inizializza un int che indicherà quale sprite non bisogna disattivare
        //int spriteToNotDeactivate = -1;
        //se questo oggetto è un duplicato e si è riuscito a trovare un altro PlayerControlls...
        //if (otherPlayerPc != null)
        //{
        //    //...per ogni oggetto della lista, la variabile viene incrementata fino a quando non si trova uno sprite attivo
        //    foreach (GameObject sprite in spritesToDeactivate)
        //    { spriteToNotDeactivate++; /*if (sprite.activeSelf)*/if (sprite.transform.position == sprite.transform.parent.position) { break; } }
        //    //Debug.Log("Trovato giocatore originale e calcolato indice di oggetto da non disattivare: " + spriteToNotDeactivate);
        //    //Debug.Log(spritesToDeactivate[spriteToNotDeactivate]);
        //}

        //disattiva gli sprite da disattivare, tranne(se ce n'è) quello indicato dalla variabile "spriteToNotDeactivate"
        //for (int sprite = 0; sprite < spritesToDeactivate.Count; sprite++)
        ////{ if (sprite != spriteToNotDeactivate) spritesToDeactivate[sprite].SetActive(false); }
        //{
        //    if (sprite != spriteToNotDeactivate) { spritesToDeactivate[sprite].transform.position = new Vector3(-1000, -1000, 0); }
        //    else { spritesToDeactivate[sprite].transform.position = spritesToDeactivate[sprite].transform.parent.position; }
        
        //}

        //crea una lista di gameObject che conterrà tutti gli sprite animati
        //List<GameObject> animatedSprites = new List<GameObject>();
        //aggiungo gli oggetti nella lista
        //animatedSprites.Add(spriteContainer.GetChild(0).gameObject);

        //for(int j = 0; j < spritesToDeactivate.Count; j++) { animatedSprites.Add(spritesToDeactivate[j]); }
        //copia la lista su PlayerControlls
        //pc.PlayerPos = animatedSprites;
        //SwipeSimul.PlayerPos = animatedSprites;
        ////Debug.Log("La lista contiene " + pc.PlayerPos.Count + " sprite animati ordinati così: ");
        ////for (int k = 0; k < pc.PlayerPos.Count; k++) { Debug.Log(k + ") " + pc.PlayerPos[k]); }
        ////da a PlayerControlls il riferimento all'animator di questa skin del giocatore
        //pc.anim = animatedSprites[0].GetComponent<Animator>();
        //SwipeSimul.anim = animatedSprites[0].GetComponent<Animator>();

        //i manager della profondità del giocatore vengono attivati dopo aver ricevuto il riferimento a questo script
        //playerDepthManager.SetActive(true);
        for (int i = 0; i < playerDepthManager.childCount; i++)
        {
            //ottiene il riferimento al manager all'indice i...
            SetObjectsDepth depthManager = playerDepthManager.GetChild(i).GetComponent<SetObjectsDepth>();
            //...gli da questo script come riferimento...
            depthManager.GetPlayerSpriteManager(this);
            //...e lo attiva
            depthManager.gameObject.SetActive(true);
        
        }

    }
    ///// <summary>
    ///// Cambia il colore di ogni sprite con il nuovo colore ricevuto come parametro
    ///// </summary>
    ///// <param name="newColor"></param>
    //public void ChangePlayerSpritesColor(Color newColor)
    //{
    //    //cicla ogni sprite per cambiarne il colore
    //    for (int sprite = 0; sprite < playerSprites.Count; sprite++) { playerSprites[sprite].color = newColor; }

    //}
    ///// <summary>
    ///// Permette ad altri script di sapere colore attuale dello sprite ad indice "wantedSprite" del giocatore
    ///// </summary>
    ///// <param name="wantedSprite"></param>
    ///// <returns></returns>
    //public Color GetPlayerSpriteColor(int wantedSprite) { return playerSprites[wantedSprite].color; }
    ///// <summary>
    ///// Cambia il layer(profondità) degli sprite del giocatore
    ///// </summary>
    ///// <param name="newLayer"></param>
    public void ChangePlayerLayer(int newLayer) { playerSpritesSG.sortingOrder = newLayer; }
    /// <summary>
    /// Permette ad altri script di sapere il layer(profondità) degli sprite del giocatore
    /// </summary>
    /// <returns></returns>
    public int GetPlayerLayer() { return playerSpritesSG.sortingOrder; }

}