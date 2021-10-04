//Si occupa del comportamento delle armi
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsContainer : MonoBehaviour
{
    //riferimenti alle armi e loro colpi(ciò che viene attivato quando si attacca)
    private GameObject sword, //riferimento alla spada
        slash, //riferimento al colpo della spada
        gun, // riferimento alla pistola
        bullet; //riferimento al proiettile della pistola

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

    //indica quale arma è attualmente in uso
    private bool gunOut = false;
    //countdown per gli attacchi di ogni arma
    [SerializeField]
    private float swordAttackCD = default, //indica quanto deve passare tra un attacco con spada e l'altro
        gunAttackCD = default; //indica quanto deve passare tra uno sparo e l'altro

    //indica quanto tempo deve passare tra un attacco con spada all'altro per interrompere la sequenza
    [SerializeField]
    private float slashSequenceResetCD = 0.2f;
    //indica a quale parte di sequenza dell'attacco con spada si è arrivati
    private int slashAtkSequence = 0;
    //indica il numero massimo di attacchi sequenziali con spada
    private int maxSlashAtkSequence;
    //indica se il giocatore può attaccare o meno
    private bool canAttack = true;
    //riferimento al manager della UI del giocatore
    [SerializeField]
    private PlayerUIManager playerUIManag = default;


    // Start is called before the first frame update
    void Start()
    {
        //ottiene i riferimenti alle armi e loro colpi
        sword = transform.GetChild(0).gameObject;
        slash = sword.transform.GetChild(0).gameObject;
        gun = transform.GetChild(1).gameObject;
        //bullet = gun.transform.GetChild(0).gameObject;
        //ottiene il riferimento allo sprite della spada
        swordSprite = slash.GetComponent<SpriteRenderer>();
        //ottiene il numero massimo di sequenze d'attacco possibili con spada in base al numero di sprites nell'array
        maxSlashAtkSequence = swordAtkSequenceSprites.Length;
        //ottiene il riferimento al Rigidbody2D del proiettile
        //rbBullet = bullet.GetComponent<Rigidbody2D>();
        //disattiva all'inizio la pistola
        gun.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        //se il giocatore può attaccare, potrà anche cambiare arma
        if (canAttack)
        {
            //premendo Q, le armi vengono scambiate
            if (Input.GetKeyDown(KeyCode.Q)) { SwapWeapons(); }
            //premendo il tasto sinistro del mouse, attacca in base all'arma attualmente in uso
            if (Input.GetKeyDown(KeyCode.Mouse0)) { Attack(); }

        } //altrimenti, se non si sta usando la pistola e il colpo di spada è disattivo, il giocatore sta continuando la sequenza di attacchi, quindi...
        else if(!gunOut && !slash.activeSelf)
        {
            //...se il giocatore cerca di attaccare...
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                //... e non si è raggiunto il massimo numero di attacchi sequenziali...
                if (slashAtkSequence < maxSlashAtkSequence)
                {
                    //...ferma tutte le coroutine di questo script(fermando così la coroutine dell'attacco con spada e non interrompere la sequenza)...
                    StopAllCoroutines();
                    //...e fa partire la coroutine dell'attacco con spada
                    Attack();

                }

            }

        }

    }
    /// <summary>
    /// Permette al giocatore di cambiare arma in uso
    /// </summary>
    private void SwapWeapons()
    {
        //inverte l'arma in uso
        gunOut = !gunOut;
        //la spada sarà attiva quando la pistola non è in uso...
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
        //il giocatore non potrà attaccare
        canAttack = false;
        //cambia lo sprite del colpo con spada in base alla sequenza corrente
        ChangeSwordSequenceSprite();
        //attiva il colpo della spada
        slash.SetActive(true);
        //aspetta che finisca il countdown dell'attacco con spada
        yield return new WaitForSeconds(swordAttackCD);
        //disattiva il colpo con spada
        slash.SetActive(false);
        //aspetta un po' di tempo
        yield return new WaitForSeconds(slashSequenceResetCD);
        //il giocatore potrà attaccare di nuovo
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
        //il giocatore non potrà attaccare
        canAttack = false;
        //il proiettile torna alla posizione iniziale
        bullet = ObjectPooling.inst.SpawnBulletFromPool("Bullets", gun.transform.position, gun.transform.rotation);
        #region da cancellare
        //bullet.transform.position = gun.transform.position;
        //attiva il proiettile
        //bullet.SetActive(true);
        //azzera ogni forza che agisce sul rigidbody del proiettile
        //rbBullet = bullet.GetComponent<Rigidbody2D>();
        //rbBullet.velocity = Vector2.zero;
        //da una spinta al proiettile facendolo andare verso la direzione in cui la pistola è direzionata
        //rbBullet.AddForce((-gun.transform.right) * bulletSpeed);
        //Debug.Log("Bullet Added Force: " + (gun.transform.position - transform.parent.position) * bulletSpeed);
        #endregion
        //aspetta che finisca il countdown dello sparo
        yield return new WaitForSeconds(gunAttackCD);
        //il giocatore potrà attaccare di nuovo
        canAttack = true;

    }

    public Vector2 GetGunDirection()
    {
        //Se la pistola esiste ritorno la direzione
        if (gun)
            return -gun.transform.right;
        //Altrimenti ritorno Vector2 default
        else return default;
    }

}
