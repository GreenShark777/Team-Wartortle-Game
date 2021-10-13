using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heart : MonoBehaviour
{
    //Sprite cuore completo e cuore a met�, verr� fatto un random � verr� scelto uno dei due che comparir�
    [SerializeField]
    private Sprite fullHeart, halfHeart;

    //Reference allo sprite renderer del cuore
    private SpriteRenderer spriteRend;

    //Reference al player health per controllare se ha la vita piena o no, nel caso il cuore non sar� ottenibile
    private PlayerHealth playerHealth;

    private void Awake()
    {
        //Ottengo lo sprite renderer del cuore
        spriteRend = transform.GetComponentInChildren<SpriteRenderer>(true);
        //Ottengo lo script playerHealth
        playerHealth = GameObject.FindObjectOfType<PlayerHealth>();
    }
    private void OnEnable()
    {
        //Con una probabilit� del 70% esce il cuore a met�
        if (Random.value < .7) spriteRend.sprite = halfHeart;
        //mentre col 30% esce il cuore completo
        else spriteRend.sprite = fullHeart;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collisione");
        //Se sto toccando il player
        if (collision.CompareTag("Player"))
        {
            Debug.Log(playerHealth == null);
            //Se la vita non � piena posso curare il player
            if (!playerHealth.GetFullHealth()) HealPlayer(spriteRend.sprite);
        }
    }

    private void HealPlayer(Sprite heartSprite)
    {
        //Se il cuore � a met� lo curo di 1(mezzo cuore)
        if (heartSprite == halfHeart) playerHealth.Damage(-1);
        //altrimenti lo curo di 2(un cuore)
        else playerHealth.Damage(-2);

        //Dopo di che lo riaggiungo all'object pooling
        ObjectPooling.inst.ReAddObjectToPool("Hearts", this.gameObject);
    }
}
