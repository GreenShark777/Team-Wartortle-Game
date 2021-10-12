using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    //GameObject che conterrà tutti i GameObject creati dall'object pooling
    private GameObject parentGB;

    //Dizionario che conterrà delle queue di GameObject(bullet esempio) con una stringa assocciata come Key("bullet" esempio)
    public Dictionary<string, Queue<GameObject>> dict_pool = new Dictionary<string, Queue<GameObject>>();

    //Sigleton pattern
    public static ObjectPooling inst;

    //Classe con il numero di GameObject e tag associato da inserire nel dizionario
    [System.Serializable]
    public class Pool
    {
        //Stringa associata all'istanza pool("bullet")
        public string tag;
        //GameObject corrispettivo
        public GameObject obj;
        //Quantitativo di GameObject da inserire
        public int size;
    }

    //Lista di istanza di tipo pools che poi verrà passata alla Queue
    public List<Pool> pools;

    private void Awake()
    {
        //Istanzio il gameobject parent
        parentGB = new GameObject();
        //e gli do un nome adeguato
        parentGB.name = "ObjectPoolingObjects";
    }
    void Start()
    {
        //Sigleton pattern
        inst = this;
        //Per ogni istanza Pool
        foreach (Pool pool in pools)
        {
            //Creo una queue 
            Queue<GameObject> obj_queue = new Queue<GameObject>();

            //Istanzio per il numero di grandezza stabilito nell'istanza(esempio bullet, 20 di size)
            for (int i = 0; i < pool.size; i++)
            {
                //Lo istanzio
                GameObject pref = Instantiate(pool.obj);
                //Lo disattivo
                pref.SetActive(false);
                //Gli assegno il parent
                pref.transform.parent = parentGB.transform;
                //Lo aggiungo alla fine della queue
                obj_queue.Enqueue(pref);
            }

            //Dopo di che aggiungo la queue al dizionario
            dict_pool.Add(pool.tag, obj_queue);
        }
    }

    //Metodo che prende un GameObject dal dizionario in base ai parametri inseriti
    public GameObject SpawnObjectFromPool(string tag, Vector3 pos, Quaternion rot)
    {
        //Se il dizionario contiene la Key passata
        if (dict_pool.ContainsKey(tag))
        {
            //Viene preso il primo GameObject della queue
            GameObject obj = dict_pool[tag].Dequeue();
            //Lo disattivo per poterlo posizionare
            obj.SetActive(false);
            //Lo posiziono
            obj.transform.position = pos;
            //Lo ruoto
            obj.transform.rotation = rot;
            //Lo riattivo
            obj.SetActive(true);

            //Lo reinserisco alla fine della queue
            dict_pool[tag].Enqueue(obj);
            //e lo ritorno
            return obj;
        }
        //Se non trovo la Key ritorno null
        else return null;
    }

    //Metodo overload
    public GameObject SpawnFromPool(string tag, Vector3 pos, Quaternion rot)
    {
        //Se è presente il tag inserito
        if (dict_pool.ContainsKey(tag))
        {
            //Prendo il primo GameObject della queue
            GameObject obj = dict_pool[tag].Dequeue();
            //Lo disattivo per poterlo posizionare e ruotare
            obj.SetActive(false);

            obj.transform.position = pos;
            obj.transform.rotation = rot;
            //Lo riattivo
            obj.SetActive(true);
            //e lo ritorno
            return obj;
        }
        //Altrimenti ritorno null
        else return null;
    }
    //Metodo che serve per reinserire un GameObject nella queue del dizionario che ha come Key il tag associato
    public void ReAddObjectToPool(string tag, GameObject obj)
    {
        //Se la Key esiste
        if (dict_pool.ContainsKey(tag))
        {
            //Il gameobject si disattiva e lo si inserisce alla fine della queue
            obj.SetActive(false);
            dict_pool[tag].Enqueue(obj);
        }
    }
}