//Cambia la profondità del giocatore in base alla posizione degli oggetti con cui sta per collider
using UnityEngine;
using UnityEngine.Rendering;

public class SetObjectsDepth : MonoBehaviour
{
    //riferimento allo sprite del giocatore
    private PlayerSpriteManager psm;
    //indica la profondità del giocatore
    public static int playerLayerOrder;
    //indica la profondità originale dell'oggetto di cui si è appena cambiata la profondità
    //private static int previousObjectIndex;
    //indica se questo script deve diminuire la profondità dell'oggetto con cui si collide o meno
    [SerializeField]
    private bool lowersDepth = default;
    //indica se il collider di questo oggetto è occupato o meno
    //[HideInInspector]
    public bool occupied = false;
    //riferimento al collider dell'altro manager
    [SerializeField]
    private SetObjectsDepth otherManager = default;
    //riferimento al collider di questo gameObject
    private Collider2D thisCol;


    private void Awake()
    {
        //ottiene il riferimento al collider di questo gameObject
        thisCol = GetComponent<Collider2D>();
        //uno dei controllori della profondità del giocatore disattiva il padre, per evitare che ci siano errori di mancati riferimenti derivanti da PlayerSpriteManager
        transform.gameObject.SetActive(false);
    
    }
    /*
    // Start is called before the first frame update
    void Start()
    {
        //ottiene il riferimento allo sprite del giocatore
        psm = transform.parent.parent.GetComponent<PlayerSpriteManager>();
        //imposta la variabile che indica la profondità del giocatore alla profondità dello sprite di uno dei figli del gameObject Player
        playerLayerOrder = psm.GetPlayerLayer();
        //imposta al valore di controllo la variabile che indica la profondità originale dell'oggetto di cui è stata cambiata
        //previousObjectIndex = 100;
        //Debug.Log("La profondità del giocatore è " + playerLayerOrder);
    }*/

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ////ottiene il riferimento allo SpriteRenderer dell'oggetto con cui si ha colliso
        //SpriteRenderer objectSprite = ObtainObjectSprite(collision.transform);
        ////se esiste il riferimento allo sprite dell'oggetto con cui si ha colliso...
        //if (objectSprite != null/* && previousObjectIndex == 100*/)
        //{
        //    //...ne ottiene la sua profondità...
        //    int objectOrderInLayer = /*ObtainObjectSprite(collision.transform)*/objectSprite.sortingOrder;
        //    //...se l'oggetto ha la stessa profondità del giocatore e il collider è fisico...
        //    if (/*playerSprite.sortingOrder == playerLayerOrder && */objectOrderInLayer == playerLayerOrder && !collision.isTrigger)
        //    {
        //        //...salva la profondità originale dell'oggetto...
        //        //previousObjectIndex = objectOrderInLayer;
        //        //...se bisogna ridurre la profondità dell'oggetto, la riduce...
        //        if (lowersDepth) { psm.ChangePlayerLayer(playerLayerOrder - 1); }
        //        //...altrimenti, la aumenta
        //        else { psm.ChangePlayerLayer(playerLayerOrder + 1); }
        //        //Debug.Log("Cambiata profondità in base a quella di " + objectSprite.gameObject.name);
        //    }

        //}
        //per ulteriore sicurezza, se non esiste il riferimento al PlayerSpriteManager lo ottiene privatamente
        if (psm == null) { GetPlayerSpriteManager(); }
        //se si è colliso con un oggetto fisico...
        if (!collision.isTrigger/* || collision.CompareTag("Player")*/)
        {
            Debug.LogError("Collider con cui si è colliso e che cambia il layer: " + collision.name);
            //...viene creata la variabile che indicherà la profondità dell'oggetto con cui si ha colliso...
            int objectLayerOrder = 0;
            //...viene inizializzato il riferimento allo sprite dell'oggetto con cui si ha colliso...
            SpriteRenderer objectSprite = null;
            //...ottiene il riferimento al SortingGroup dell'oggetto con cui si ha colliso...
            SortingGroup objectSG = ObtainObjectComponent<SortingGroup>(collision.transform);
            Debug.Log("FINITA RICERCA DI SORTING GROUP");
            //...e, se il riferimento è nullo, ne ottiene la profondità
            if (objectSG != null) { objectLayerOrder = objectSG.sortingOrder; Debug.LogError("SORTING GROUP"); }
            //altrimenti...
            else
            {
                //...ottiene il riferimento allo SpriteRenderer dell'oggetto con cui si ha colliso...
                objectSprite = ObtainObjectComponent<SpriteRenderer>(collision.transform);
                Debug.Log("FINITA RICERCA DI SPRITE");
                //...e ne ottiene la sua profondità, se si è trovato il riferimento
                if (objectSprite != null) { objectLayerOrder = objectSprite.sortingOrder; }
                else { Debug.LogError("Non è stato nemmeno trovato il riferimento allo sprite dell'oggetto"); }
                Debug.Log("SPRITE");
            }
            Debug.LogError("Layer PG: " + playerLayerOrder + " Layer OBJ: " + objectLayerOrder);
            //...infine, se la profondità dell'oggetto è uguale a quella del giocatore, ...
            if (objectLayerOrder == playerLayerOrder)
            {
                Debug.LogError("Stesso Layer");
                //...se bisogna ridurre la profondità...
                if (lowersDepth)
                {
                    //...e l'altro manager non è occupato, riduce quella del giocatore...
                    if (!otherManager.occupied) { psm.ChangePlayerLayer(playerLayerOrder - 1); }
                    //...altrimenti...
                    else
                    {
                        //...aumenta quella dell'oggetto con cui si è colliso(in base al componente che si è ottenuto)
                        if (objectSG != null) { objectSG.sortingOrder = psm.GetPlayerLayer() + 1; }
                        else { objectSprite.sortingOrder = psm.GetPlayerLayer() + 1; }
                        
                    }
                    Debug.LogError("Diminuisce Layer");
                }
                //...altrimenti, la aumenta
                else
                {
                    //...e l'altro manager non è occupato, aumenta quella del giocatore...
                    if (!otherManager.occupied) { psm.ChangePlayerLayer(playerLayerOrder + 1); }
                    //...altrimenti...
                    else
                    {
                        //...riduce quella dell'oggetto con cui si è colliso(in base al componente che si è ottenuto)
                        if (objectSG != null) { objectSG.sortingOrder = psm.GetPlayerLayer() - 1; }
                        else { objectSprite.sortingOrder = psm.GetPlayerLayer() - 1; }

                    }
                    Debug.LogError("Aumenta Layer");
                }
                //infine, comunica che questo manager è occupato
                occupied = true;
                //Debug.Log("Cambiata profondità in base a quella di " + collision.gameObject.name);
            }
            //Debug.Log("Player Layer = " + playerLayerOrder + " : Object Layer = " + objectLayerOrder);
        }
        
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        //creiamo un int che indicherà quanti collider sono all'interno del nostro collider
        int nCols = thisCol.Cast(Vector2.zero, new RaycastHit2D[1]);
        //Debug.Log("N colliders dentro " + transform.parent.parent.name + " -> " + nCols);
        //se l'oggetto uscito dal trigger era un oggetto fisico...
        if ((!collision.isTrigger || collision.CompareTag("Player")) && nCols == 0)
        {
            //...ottiene il riferimento al SortingGroup dell'oggetto...
            SortingGroup objectSG = ObtainObjectComponent<SortingGroup>(collision.transform);
            //...se non è nullo e la sua profondità è diversa da quella iniziale che dovrebbe avere, ritorna alla profondità iniziale
            if (!IsComponentOfTypeTNull(objectSG) && objectSG.sortingOrder != playerLayerOrder) { objectSG.sortingOrder = playerLayerOrder; }
            //altrimenti...
            else if(IsComponentOfTypeTNull(objectSG))
            {
                //...ottiene il riferimento allo sprite dell'oggetto...
                SpriteRenderer objectSprite = ObtainObjectComponent<SpriteRenderer>(collision.transform);
                //...se non è nullo e la sua profondità è diversa da quella iniziale che dovrebbe avere, ritorna alla profondità iniziale
                if (!IsComponentOfTypeTNull(objectSprite) && objectSprite.sortingOrder != playerLayerOrder)
                { objectSprite.sortingOrder = playerLayerOrder; }

            }
            //qualunque sia l'esito, questo manager non sarà più occupato
            occupied = false;

        }

    }
    /// <summary>
    /// Permette di ottenere il riferimento al PlayerSpriteManager, ottenendolo dal giocatore
    /// </summary>
    private void GetPlayerSpriteManager()
    {
        //ottiene il riferimento al maanger degli sprite del giocatore
        psm = transform.parent.parent.parent.GetComponent<PlayerSpriteManager>();
        //imposta la variabile che indica la profondità del giocatore alla profondità dello sprite di uno dei figli del gameObject Player
        playerLayerOrder = psm.GetPlayerLayer();
        //Debug.LogError("PSM PRIVATO");
    }
    /// <summary>
    /// Permette ad altri script di dare a questo script il riferimento al PlayerSpriteManager
    /// </summary>
    /// <param name="newPsm"></param>
    public void GetPlayerSpriteManager(PlayerSpriteManager newPsm)
    {
        //ottiene il riferimento al managwer degli sprite del giocatore
        psm = newPsm;
        //imposta la variabile che indica la profondità del giocatore alla profondità dello sprite di uno dei figli del gameObject Player
        playerLayerOrder = psm.GetPlayerLayer();
        //Debug.LogError("PSM PUBBLICO");
    }

    /*
    private void OnTriggerExit2D(Collider2D collision)
    {
        
        ////se la variabile della profondità originale non è al valore di controllo...
        //if (previousObjectIndex != 100 playerSprite.sortingOrder != playerLayerOrder)
        //{
        //    //...fa tornare l'oggetto di cui era stata cambiata la profondità alla sua profondità originale...
        //    //ObtainObjectSprite(collision.transform).sortingOrder = previousObjectIndex;
        //    playerSprite.sortingOrder = playerLayerOrder;//previousObjectIndex;
        //    //...infine, fa tornare al valore di controllo la variabile
        //    //previousObjectIndex = 100;

        //}
        
        //ottiene il riferimento all'oggetto uscito dal collider
        SpriteRenderer objectSprite = ObtainObjectSprite(collision.transform);
        //se l'oggetto con cui si è colliso ha uno sprite...
        if (objectSprite != null)
        {
            //...ottiene la profondità dell'oggetto uscito dal collider...
            int objectOrderInLayer = objectSprite.sortingOrder;
            //...e ha la stessa profondità del giocatore e il collider è fisico...
            if (objectOrderInLayer == playerLayerOrder && !collision.isTrigger)
            {
                //...se questo script ha abbassato o alzato la profondità del giocatore, la riporta a quella originale
                if ((lowersDepth && playerSprite.sortingOrder < playerLayerOrder) || (!lowersDepth && playerSprite.sortingOrder > playerLayerOrder))
                { playerSprite.sortingOrder = playerLayerOrder; /*Debug.Log("Tornato normale per colpa di: " + collision.name);* / }

            }

        }
        //Debug.Log("SAS -> " + " : " + lowersDepth + " : " + playerLayerOrder + " : " + playerSprite.sortingOrder);
    }
    */
    /*
    /// <summary>
    /// Ottiene, dell'oggetto restituito, lo sprite. Riesce a trovarlo fino ad un grado di parentela di 2 genitori
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    private SpriteRenderer ObtainObjectSprite(Transform obj)
    {
        //ottiene il riferimento allo sprite nell'oggetto stesso con cui si è colliso
        SpriteRenderer objectSprite = obj.GetComponent<SpriteRenderer>();
        //se il riferimento è nullo...
        if (objectSprite == null)
        {
            //...cerca di ottenere il riferimento allo sprite dai figli...
            objectSprite = obj.GetComponentInChildren<SpriteRenderer>();
            //...e se il riferimento è ancora nullo e l'oggetto ha un padre...
            if (objectSprite == null && obj.parent != null)
            {
                //...cerca di ottenere lo sprite dal suo genitore...
                objectSprite = obj.GetComponentInParent<SpriteRenderer>();
                //...e se il riferimento è ancora nullo...
                if (objectSprite == null)
                {
                    //ottiene il riferimento al padre dell'oggetto con cui si ha colliso
                    Transform objNextParent = obj.parent.parent;
                    //se questo padre ha un padre...
                    if (objNextParent != null)
                    {
                        //...cerca di ottenerlo dal genitore del suo genitore se esiste...
                        objectSprite = objNextParent.GetComponent<SpriteRenderer>();
                        //...e se il riferimento è ancora nullo, fa un ultimo tentativo con i figli del genitore del suo genitore
                        if (objectSprite == null) { objNextParent.GetComponentInChildren<SpriteRenderer>(); }

                    }
                
                }

            }

        }
        //infine ritorna lo spriteRenderer dell'oggetto con cui si è colliso, se è stato trovato
        return objectSprite;

    }
    
    private SortingGroup ObtainObjectSG(Transform obj)
    {

        SortingGroup objectSG = obj.GetComponent<SortingGroup>();

        if (objectSG == null)
        {

            objectSG = obj.GetComponentInChildren<SortingGroup>();

            if (objectSG == null) { }

        }

        return objectSG;

    }
    */
    private T ObtainObjectComponent<T>(Transform obj)
    {
        //ottiene il componente di tipo T dall'oggetto ricevuto come riferimento
        T obtainedComponent = obj.GetComponent<T>();
        //Debug.Log("Cercato componente nell'oggetto: " + obj);
        //se non si è ancora ottenuto il componente...
        if (IsComponentOfTypeTNull(obtainedComponent))
        {
            //...lo ottiene dai figli dell'oggetto
            //T[] recipientForComponents = obj.GetComponentsInChildren<T>(true);
            obtainedComponent = obj.GetComponentInChildren<T>(true);
            //Debug.Log("Cercato componente nei figli dell'oggetto: " + obj);
            //se non si è ancora ottenuto il componente...
            if (IsComponentOfTypeTNull(/*recipientForComponents*/obtainedComponent))
            {
                //...ottiene il padre dell'oggetto...
                Transform objParent = obj.parent;
                //...se esiste...
                if (objParent != null)
                {
                    //...ottiene il componente da lui...
                    obtainedComponent = objParent.GetComponent<T>();
                    //Debug.Log("Cercando componente nel padre dell'oggetto: " + objParent);
                    //...se il componente non è ancora stato preso...
                    if (IsComponentOfTypeTNull(obtainedComponent))
                    {
                        //...lo ottiene dai figli del padre dell'oggetto...
                        //recipientForComponents = objParent.GetComponentsInChildren<T>(true);
                        obtainedComponent = objParent.GetComponentInChildren<T>(true);
                        //Debug.Log("Cercato componente nei figli del padre dell'oggetto, che è: " + objParent);
                        //...se non ha ancora preso il componente, cerca nel padre del padre appena controllato e i suoi figli
                        if (IsComponentOfTypeTNull(obtainedComponent) && objParent.parent != null)
                        {
                            do
                            {
                                //ottiene il riferimento al padre
                                objParent = objParent.parent;
                                //se non è nullo...
                                if(objParent != null)
                                {
                                    //...cerca di ottenere il riferimento dal padre...
                                    obtainedComponent = ObtainObjectComponent<T>(objParent);
                                    //Debug.Log("Cercando componente nel nuovo padre ciclato: " + objParent);
                                    //...altrimenti, cerca di ottenerlo dai suoi figli
                                    if (IsComponentOfTypeTNull(obtainedComponent))
                                    {
                                        obtainedComponent = objParent.GetComponentInChildren<T>(true);
                                        //Debug.Log("Cercato componente nei figli del padre dell'oggetto, che è: " + objParent);
                                        if (IsComponentOfTypeTNull(obtainedComponent)) { Debug.LogError("Componente non trovato, si rifa il ciclo"); }
                                        else { Debug.Log("Componente ottenuto dai figli di: " + objParent); break; }
                                    }
                                    else { Debug.Log("Componente ottenuto dal padre ciclato: " + objParent); }
                                    //Debug.LogError("Ciclo provato");
                                }

                            } //il ciclo viene ripetuto fino a quando o il padre è nullo o si è ottenuto il riferimento
                            while (IsComponentOfTypeTNull(obtainedComponent) && objParent != null);
                        
                        }
                        else if (!IsComponentOfTypeTNull(obtainedComponent)) { Debug.Log("Componente ottenuto dai figli di: " + objParent); }
                        else { Debug.LogError("Non esiste un padre di: " + objParent + " -> " + objParent.parent); }
                    }
                    else { Debug.Log("Componente ottenuto da: " + objParent); }
                }
                else { Debug.Log("L'oggetto: " + obj + " non ha padri"); }
            }
            else { Debug.Log("Componente ottenuto dai figli di: " + obj); }
            /*
            foreach(T component in recipientForComponents)
            {
                if (IsComponentOfTypeTNull(component)) { obtainedComponent = component; break; }
            }
            */
        }
        else{ Debug.Log("Componente ottenuto da: " + obj); }

        /*
        if (!IsComponentOfTypeTNull(obtainedComponent)) { Debug.Log("Il componente è: " + obtainedComponent); }
        else { Debug.Log("Il componente " + obtainedComponent + " di tipo " + typeof(T) + " non è stato trovato"); }
        */

        //infine, ritorna il componente ottenuto
        return obtainedComponent;

    }

    /// <summary>
    /// Controlla, per il componente del tipo ricevuto, se è nullo o meno
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="component"></param>
    /// <returns></returns>
    private bool IsComponentOfTypeTNull<T>(T component)
    {
        //se il componente non è già nullo...
        if (component != null)
        {
            //Debug.LogError("E' nullo? -> " + component + " :" + component.Equals(null));
            //...ritorna il bool del controllo se il suo valore è effettivamente nullo
            return component.Equals(null);
        }
        //altrimenti, ritorna immediatamente true(indicando che è nullo)
        else { /*Debug.LogError("Il componente ricevuto era nullo");*/ return true; }
    
    }

}
