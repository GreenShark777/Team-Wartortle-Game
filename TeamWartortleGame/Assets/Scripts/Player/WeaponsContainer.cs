//Si occupa del comportamento delle armi
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsContainer : MonoBehaviour
{
    //riferimenti alle armi e loro colpi(ci� che viene attivato quando si attacca)
    [SerializeField]
    private GameObject sword, //riferimento alla spada
        slash, //riferimento al colpo della spada
        gun; // riferimento alla pistola

    //riferimento allo sprite della spada
    private SpriteRenderer swordSprite;
    //array di tutti gli sprite per le sequenze di attacco
    [SerializeField]
    private Sprite[] swordAtkSequenceSprites;

    //riferimento al Rigidbody2D del proiettile
    //private Rigidbody2D rbBullet;
    //indica quanto velocemente i proiettili della pistola vanno
    //[SerializeField]
    //private float bulletSpeed = 2;

    //indica quale arma � attualmente in uso
    private bool gunOut = false;
    //countdown per gli attacchi di ogni arma
    public float swordAttackCD = 1, //indica quanto deve passare tra un attacco con spada e l'altro
        gunAttackCD = .3f; //indica quanto deve passare tra uno sparo e l'altro

    //Float di coolDown iniziale per poterlo poi riassegnare dopo aver finito l'effetto di un powerUp, e float del CoolDown diminuito per il power up della velocit�
    [HideInInspector]
    public float startSwordAttackCD, speedSwordAttackCD;

    //Posizione in cui apparir� lo slash e il proiettile
    [SerializeField]
    private Transform slashPos, shootPos;
    //indica quanto tempo deve passare tra un attacco con spada all'altro per interrompere la sequenza
    [SerializeField]
    private float slashSequenceResetCD = 0.2f;
    //indica a quale parte di sequenza dell'attacco con spada si � arrivati
    private int slashAtkSequence = 0;
    //indica il numero massimo di attacchi sequenziali con spada
    private int maxSlashAtkSequence;
    //indica se il giocatore pu� attaccare o meno
    private bool canAttack = true;
    //riferimento al manager della UI del giocatore
    [SerializeField]
    private PlayerUIManager playerUIManag = default;

    //Riferimento all'animator del player per eseguire le sue animazioni
    [SerializeField]
    private Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        //Memorizzo il cooldown default della spada
        startSwordAttackCD = swordAttackCD;

        //Assegno come valore di cooldown diminuito(power up velocit�) il valore del cooldown diviso per un valore vicino al due
        speedSwordAttackCD = swordAttackCD / 1.8f;
        //ottiene i riferimenti alle armi e loro colpi
        //sword = transform.GetChild(0).gameObject;
        //slash = sword.transform.GetChild(0).gameObject;
        //gun = transform.GetChild(1).gameObject;
        //bullet = gun.transform.GetChild(0).gameObject;
        //ottiene il riferimento allo sprite della spada
        //swordSprite = slash.GetComponent<SpriteRenderer>();
        //ottiene il numero massimo di sequenze d'attacco possibili con spada in base al numero di sprites nell'array
        maxSlashAtkSequence = swordAtkSequenceSprites.Length;
        //ottiene il riferimento al Rigidbody2D del proiettile
        //rbBullet = bullet.GetComponent<Rigidbody2D>();
        //disattiva all'inizio la pistola
        gun.SetActive(false);

    }

    //Convertito in LateUpdate a causa di ritardi 
    void LateUpdate()
    {
     
        //se il giocatore pu� attaccare, potr� anche cambiare arma
        if (canAttack)
        {
            //premendo Q, le armi vengono scambiate
            if (Input.GetKeyDown(KeyCode.Q)) { SwapWeapons(); }
            //Controllo gli arrow keys orizzontali e verticali
            float horArrow = Input.GetAxisRaw("HorizontalArrowKeys"), verArrow = Input.GetAxisRaw("VerticalArrowKeys");
            //Se sto premendo uno dei due, attacco
            if (Mathf.Abs(horArrow) > 0 || Mathf.Abs(verArrow) > 0)
            {
                Attack();
            }
            //premendo il tasto sinistro del mouse, attacca in base all'arma attualmente in uso
            //if (Input.GetKey(KeyCode.Mouse0)) { Attack(); }

        }
        //altrimenti, se non si sta usando la pistola e il colpo di spada � disattivo, il giocatore sta continuando la sequenza di attacchi, quindi...
        //else if(!gunOut && !slash.activeSelf)
        //{
        //    //...se il giocatore cerca di attaccare...
        //    if (Input.GetKey(KeyCode.Mouse0))
        //    {
        //        //... e non si � raggiunto il massimo numero di attacchi sequenziali...
        //        if (slashAtkSequence < maxSlashAtkSequence)
        //        {
        //            //...ferma tutte le coroutine di questo script(fermando cos� la coroutine dell'attacco con spada e non interrompere la sequenza)...
        //            StopAllCoroutines();
        //            //...e fa partire la coroutine dell'attacco con spada
        //            Attack();

        //        }

        //    }

        //}

    }
    /// <summary>
    /// Permette al giocatore di cambiare arma in uso
    /// </summary>
    private void SwapWeapons()
    {
        //inverte l'arma in uso
        gunOut = !gunOut;
        //la spada sar� attiva quando la pistola non � in uso...
        sword.SetActive(!gunOut);
        //...e viceversa
        gun.SetActive(gunOut);
        //cambia la UI in modo da indicare al giocatore l'avvenuto cambiamento d'armi
        playerUIManag.ChangeWeaponInUse(gunOut);

    }
    /// <summary>
    /// Fa partire l'attacco dell'arma attualmente in uso
    /// </summary>
    private void Attack()
    {
        //in base all'arma in uso, inizia la coroutine d'attacco dell'arma
        if (!gunOut) { StartCoroutine(SwordAttack()); }
        else { StartCoroutine(GunAttack()); }

    }
    /// <summary>
    /// Si occupa dell'attacco con spada
    /// </summary>
    /// <returns></returns>
    private IEnumerator SwordAttack()
    {
        //Eseguo l'animazione di attacco con la spada
        animator.SetTrigger("Sword");
        //il giocatore non potr� attaccare
        canAttack = false;
        //Ottengo un'istanza di slash dall'object pooling
        //ObjectPooling.inst.SpawnObjectFromPool("Slash", slashPos.position, transform.rotation);
        //cambia lo sprite del colpo con spada in base alla sequenza corrente
        //ChangeSwordSequenceSprite();
        //attiva il colpo della spada
        //slash.SetActive(true);
        //aspetta che finisca il countdown dell'attacco con spada
        //yield return new WaitForSeconds(swordAttackCD);
        //disattiva il colpo con spada
        //slash.SetActive(false);
        //aspetta un po' di tempo
        yield return new WaitForSeconds(swordAttackCD);
        //il giocatore potr� attaccare di nuovo
        canAttack = true;
        //la sequenza d'attacco viene resettata
        slashAtkSequence = 0;

    }
    /// <summary>
    /// Cambia lo sprite del colpo di spada in base alla sequenza attuale
    /// </summary>
    private void ChangeSwordSequenceSprite()
    {
        //cambia lo sprite del colpo di spada in base alla sequenza d'attacco corrente
        swordSprite = slash.transform.GetChild(1).GetComponent<SpriteRenderer>();
        swordSprite.sprite = swordAtkSequenceSprites[slashAtkSequence];
        //indica l'indice della sequenza che l'attacco di spada deve avere nel prossimo attacco
        slashAtkSequence++;
        /*
        switch (swordAtkSequence)
        {

            case 0: { break; }
            case 1: { break; }
            case 2: { break; }
            default: { Debug.Log("Raggiunto n massimo di sequenza -> " + swordAtkSequence + " >= " + maxSwordAtkSequence); swordAtkSequence = 0; break; }

        }
        */
    }
    /// <summary>
    /// Si occupa dell'attacco con pistola
    /// </summary>
    /// <returns></returns>
    private IEnumerator GunAttack()
    {
        //Eseguo l'animazione di attacco con la pistola
        animator.SetTrigger("Shooting");
        //il giocatore non potr� attaccare
        canAttack = false;
        //il proiettile torna alla posizione iniziale
        //ObjectPooling.inst.SpawnFromPool("Bullets", shootPos.position, transform.rotation);
        #region da cancellare
        //bullet.transform.position = gun.transform.position;
        //attiva il proiettile
        //bullet.SetActive(true);
        //azzera ogni forza che agisce sul rigidbody del proiettile
        //rbBullet = bullet.GetComponent<Rigidbody2D>();
        //rbBullet.velocity = Vector2.zero;
        //da una spinta al proiettile facendolo andare verso la direzione in cui la pistola � direzionata
        //rbBullet.AddForce((-gun.transform.right) * bulletSpeed);
        //Debug.Log("Bullet Added Force: " + (gun.transform.position - transform.parent.position) * bulletSpeed);
        #endregion
        //aspetta che finisca il countdown dello sparo
        yield return new WaitForSeconds(gunAttackCD);
        //il giocatore potr� attaccare di nuovo
        canAttack = true;

    }

    //Metodo che serve per ottenere la direzione corrente del personaggio, viene passata al proiettile per fargli seguire la direzione
    public Vector2 GetGunDirection()
    {
        return transform.right;
    }

    //public void SpawnSlash()
    //{
    //    //Ottengo un'istanza di slash dall'object pooling
    //    slash = ObjectPooling.inst.SpawnObjectFromPool("Slash", slashPos.position, transform.rotation);
    //}

    //public void DisableSlash()
    //{
    //    //Ottengo un'istanza di slash dall'object pooling
    //    ObjectPooling.inst.ReAddObjectToPool("Slash", slash);
    //}

    public void SpawnBullet()
    {
        ObjectPooling.inst.SpawnFromPool("Bullets", shootPos.position, transform.rotation);
        //Se sono un angelo sparo inoltre altri due proiettili in spread
        if (GameManager.inst.angel) {
            for (int i = 0; i < 2; i++)
            {
                Vector3 temp = transform.localEulerAngles;
                transform.rotation = (i == 0 ? Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z - 15) : Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z + 15));
                GameObject bullet = ObjectPooling.inst.SpawnFromPool("YellowBullets", shootPos.position, transform.rotation);
                bullet.GetComponent<WeaponStats>().SetAttackStat(2);
                transform.rotation = Quaternion.Euler(temp);
            }
        }
    }

    public void EquipWeapon(int value)
    {
        if (value == 0)
        {
            //inverte l'arma in uso
            gunOut = false;
            //la spada sar� attiva quando la pistola non � in uso...
            sword.SetActive(!gunOut);
            //...e viceversa
            gun.SetActive(gunOut);
            //cambia la UI in modo da indicare al giocatore l'avvenuto cambiamento d'armi
            playerUIManag.ChangeWeaponInUse(gunOut);
        } else if (value == 1)
        {
            //inverte l'arma in uso
            gunOut = true;
            //la spada sar� attiva quando la pistola non � in uso...
            sword.SetActive(!gunOut);
            //...e viceversa
            gun.SetActive(gunOut);
            //cambia la UI in modo da indicare al giocatore l'avvenuto cambiamento d'armi
            playerUIManag.ChangeWeaponInUse(gunOut);
        }
    }

    public void ShootFendente()
    {
        //Se sono un demone posso sparare il fendente dalla spada
        if (GameManager.inst.demon)
            ObjectPooling.inst.SpawnFromPool("Fendente", shootPos.position, transform.rotation);
    }

}
