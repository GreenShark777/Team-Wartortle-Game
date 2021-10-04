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

        if (movePos.magnitude > 0)
            //Controllo se mi sto muovendo sulla X
            if (Mathf.Abs(movePos.x) > 0)
            {
                //Attivo animazione orizzontale
                animator.SetFloat("moveX", movePos.x);
                //Elimino la velocità di Y sull'animazione visto che non voglio animazioni verticali
                animator.SetFloat("moveY", 0);
            }
            //Altrimenti se non mi sto muovendo sulla X ma sulla Y
            else if (Mathf.Abs(movePos.y) > 0)
            {
                //Attivo animazione verticale
                animator.SetFloat("moveY", movePos.y);
                //Elimino la velocità di X sull'animazione visto che non voglio animazioni orizzontali
                animator.SetFloat("moveX", 0);
            }
    }
    
}
