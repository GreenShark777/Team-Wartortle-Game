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
    private Vector2 movePos, movePosRb;

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

        //Passo i valori input di movimento alla posizione da raggiungere, questo servirà principalmente per gestire le animazioni
        movePos = new Vector2(horizontal, vertical);

        //Altro vettore per inviare un movimento smooth
        movePosRb = Vector2.Lerp(movePosRb, movePos, 14 * Time.deltaTime);

        //Setto l'animazione in base alla velocità di movimento
        SetAnimator();

    }

    private void FixedUpdate()
    {
        //Muovo il Personaggio sommando la posizione da raggiungere a quella corrente
        rb.MovePosition(rb.position + (movePosRb * moveSpeed * Time.fixedDeltaTime));
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

    //Metodo per knockback in cui chiamo la coroutine e gli passo i parametri
    public void Knockback(Vector3 pos, float knockPower)
    {
        StartCoroutine(IKnockback(pos, knockPower));
    }

    //Coroutine di knockback
    private IEnumerator IKnockback(Vector3 pos, float knockPower)
    {
        //Inizializzo un timer a 0
        float timer = 0;
        //Calcolo la direzione opposta all'attacco
        Vector2 dir = -(pos-transform.position).normalized;

        //Per 0.2 millesimi di secondo
        while (timer < .2f)
        {
            //aumento il timer
            timer += Time.deltaTime / 1f;
            //Modifico verso la direzione di knockback il vettore di movimento
            movePosRb = Vector2.Lerp(movePosRb, dir * knockPower, knockPower * Time.deltaTime);
            yield return null;
        }

        yield return null;
    }
}
