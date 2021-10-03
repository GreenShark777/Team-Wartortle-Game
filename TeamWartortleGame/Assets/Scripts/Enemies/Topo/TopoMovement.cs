using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopoMovement : TopoAbstract
{
    //Posizione da raggiungere
    private Vector2 targetPos = default;

    private float speed; //Velocit� di movimento

    //Reference allo script manager da cui chiamare i suoi metodi
    private TopoManagerSTM topoManager;

    //Vettore che conterr� la destinazione
    private Vector2 movePos;

    //Reference al rigidBody per muoversi
    private Rigidbody2D rb;

    private bool checkPos = false;

    private LayerMask groundMask, obstacleMask;

    public override void StateEnter() {
        //Ottengo il manager
        topoManager = GetComponent<TopoManagerSTM>();

        groundMask = topoManager.groundMask;
        obstacleMask = topoManager.obstacleMask;

        //Ottengo la velocit� dal manager
        speed = topoManager.speed;
        //Ottengo il rigidBody2D
        rb = GetComponent<Rigidbody2D>();

        checkPos = false;


        StartCoroutine(IGetPosition());

    }

    public override void StateUpdate() { }

    public override void StateFixedUpdate() {
       
    }
    public override void StateTriggerEnter(Collider2D collision) { }

    public override void StateTriggerExit(Collider2D collision) { }

    public override void StateCollisionEnter(Collision2D collision) { }

    public override void StateCollisionExit(Collision2D collision) { }

    public override void StateExit() { }

    private IEnumerator IGetPosition() {
        float x = default;
        float y = default;
        while (!checkPos)
        {
            //Ottengo i punti di destinazione x e y
            x = (Random.value > .5 ? 2 : -2);
            y = (Random.value > .5 ? 2 : -2);
            checkPos = Physics2D.OverlapCircle(new Vector2(transform.position.x + x, transform.position.y + y), .1f, groundMask);
           
            yield return null;
        }
        StartMoving(x,y);

        yield return null;
    }

    private void StartMoving(float x,float y)
    {
        //Riempio il vettore della destinazione con i punti x e y
        targetPos = new Vector2(x, y);

        //Chiamo la coroutine che muove sulla destinazione il topo
        StartCoroutine(IMoveTo(targetPos));
    }

 

    private IEnumerator IMoveTo(Vector2 movePos)
    {
        //Setto l'animazione corrente
        this.movePos = movePos - (Vector2)transform.position;
        //Aggiorno inizialmente l'animazione verso la direzione che sto prendendo
        topoManager.CheckAnimation(this.movePos.normalized);
    
        //Calcolo la distanza tra me stesso e la destinazione e quando sono vicino esco dal loop
        while (Vector2.Distance(transform.position, movePos) > .5f)
        {
            RaycastHit2D hit = (Physics2D.Raycast(transform.position, this.movePos.normalized, 1, obstacleMask));
            if (hit.collider != null)
            {
                StopAllCoroutines();
                StartCoroutine(IGetPosition());
            }
            //Muovo il rigidBody nella posizione stabilita sommando alla posizione corrente la destinazione
            rb.MovePosition(rb.position + this.movePos.normalized * (speed * Time.fixedDeltaTime));
            yield return null;
        }
        //Blocco il movimento
        this.movePos = Vector2.zero;
        //Aggiorno l'animazione(si aggiorner� solo lo script velocit� in questo caso in cui il vettore diventa a 0, quindi si passer� solo da running ad idle) 
        topoManager.CheckAnimation(this.movePos);
        //Torno allo stato idle e ripeto il ciclo idle-running
        topoManager.SwitchState(topoManager.topoIdle);
        yield return null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(targetPos, .5f);
        Gizmos.color = Color.red;
    }
}
