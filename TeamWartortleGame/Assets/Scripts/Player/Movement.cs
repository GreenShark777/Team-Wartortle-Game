using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    #region variabili
    //Variabile per la velocità di movimento
    [SerializeField]
    private float moveSpeed;
    //Riferimento al RigidBody per la gestione del movimento
    [SerializeField]
    private Rigidbody2D rb;
    //Posizione da raggiungere
    private Vector2 movePos;

    //Riferimento all'animator
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private WeaponsContainer weaponContainer;

    #endregion

    void Update()
    {
        //Prendo il valore input degli assi orizzontali e verticali
        float horizontal = Input.GetAxisRaw("Horizontal"), vertical = Input.GetAxisRaw("Vertical");

        //Passo i valori input di movimento alla posizione da raggiungere
        movePos = new Vector2(horizontal, vertical);

        //Setto l'animazione in base alla velocità di movimento
        SetAnimator();

    }

    private void FixedUpdate()
    {
        //Muovo il Personaggio sommando la posizione da raggiungere a quella corrente
        rb.MovePosition(rb.position + (movePos * moveSpeed * Time.fixedDeltaTime));
    }

    //Metodo che imposta i valori float dell'animatore e decide la direzione, se in idle o in running
    private void SetAnimator()
    {
        //Setto la velocità via animator per passare tra idle e movimento
        animator.SetFloat("Velocity", movePos.magnitude);

        //Ottengo li assi degli arrow keys, per cambiare direzione di animazione in caso in cui voglia attaccare
        float horArrow = Input.GetAxisRaw("HorizontalArrowKeys"), verArrow = Input.GetAxisRaw("VerticalArrowKeys");
        //Se sto premendo gli arrow keys orizzontali
        if (Mathf.Abs(horArrow) > 0)
        {
            //Attacco orizzontalmente e imposto la direzione di movimento in base al valore horArrow(-1 o 1), e inoltre azzero il moveY per evitare conflitti di animazioni
            EstablishMovement("ArrowDirX", "ArrowDirY", horArrow);
            EstablishMovement("moveX", "moveY", horArrow);
        }
        //Altrimenti se sto premendo gli arrow keys verticali
        else if (Mathf.Abs(verArrow) > 0)
        {
            //Attacco verticalmente e imposto la direzione di movimento in base al valore verArrow(-1 o 1), e inoltre azzero il moveX per evitare conflitti di animazioni
            EstablishMovement("ArrowDirY", "ArrowDirX", verArrow);
            EstablishMovement("moveY", "moveX", verArrow);
        }
        //Altrimenti se non sto attaccando e se mi sto muovendo con i tasti di movimento e controllo inoltre se il secondo layer di animazione non sta effettuando animazioni di attacco
        else if (movePos.magnitude > 0 && animator.GetCurrentAnimatorStateInfo(1).IsName("Idle"))
        {
            //Controllo se mi sto muovendo sulla X
            if (Mathf.Abs(movePos.x) > 0)
            {
                //Applico il movimento sul orizzontale all'animator e azzero moveY per evitare conflitti di animazioni
                EstablishMovement("moveX", "moveY", movePos.x);
            }
            //Altrimenti se non mi sto muovendo sulla X ma sulla Y
            else if (Mathf.Abs(movePos.y) > 0)
            {
                //Applico il movimento sul verticale all'animator e azzero moveX per evitare conflitti di animazioni
                EstablishMovement("moveY", "moveX", movePos.y);
            }
        }
    }
    

    //Metodo che prende una stringa a, gli applica il valore value e alla stringa b gli applica 0(Usato dentro il metodo SetAnimator)
    private void EstablishMovement(string a,string b,float value)
    {
        //Attivo animazione a
        animator.SetFloat(a, value);
        //Elimino la velocità di b sull'animazione visto che non voglio conflitti di animazioni
        animator.SetFloat(b, 0);
    }

    public void SpawnBullet()
    {
        weaponContainer.SpawnBullet();
    }

    public void EquipWeapon(int value)
    {
        weaponContainer.EquipWeapon(value);
    }

}
