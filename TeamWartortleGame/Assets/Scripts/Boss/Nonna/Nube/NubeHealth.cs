using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NubeHealth : EnemiesHealth, IDamageable
{
    //Transform del player per seguire la sua direzione
    private Transform playerTrans;

    //Velocit� da inserire come reference al metodo smoothDamp
    private Vector3 velocity = Vector3.zero;

    //Velocit� dello smooth
    [SerializeField]
    private float smoothTime = 0.3F;

    public override void Awake()
    {
        base.Awake();
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        //Ottento la direzione del player
        Vector3 targetPosition = playerTrans.position;
        //Muovo in modo smooth la nube verso il player
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
    public override void EnemyDefeated()
    {
        //comunica che questo nemico � stato sconfitto
        defeated = true;
        //il collider del nemico non � pi� solido, in modo da non poter essere pi� colpito da armi
        enemyCollider.isTrigger = true;

        //Disattivo il GameObject e lo mando nell'object pooling
        ObjectPooling.inst.ReAddObjectToPool("Nube", gameObject);

    }

    public override void Damage(float value)
    {
        base.Damage(value);
        //Se sono ancora attivo posso scalare in basso quando prendo danno
        if (gameObject.activeSelf)
            StartCoroutine(IScaleDown());
    }

    private IEnumerator IScaleDown()
    {
        //Calcolo il valore normalizzato tra 0 e la vita massima della vita corrente
        float lifeParam = Mathf.InverseLerp(0, maxHP, enemyHp);
        //Creo un vettore della scala come target da raggiungere
        Vector3 targetScale = default;
        //Associo il valore normalizzato della vita corrente ai x e y del vettore
        targetScale.x = lifeParam;
        targetScale.y = lifeParam;

        //Vettore della scala iniziale
        Vector3 startScale;
        //Serve un'altro vettore per salvare il lerp
        Vector3 currentScale = startScale = transform.localScale;

        //Timer della discesa di scala
        float timer = 0;    

        //Finch� la scala di x divisa 1.5f e minore della scala corrente vuol dire che deve ancora scendere
        while(timer < 1)
        {
            //Aumento il timer(sar� 1 in un secondo)
            timer += Time.deltaTime / 1;
            //Inserisco alla scala corrente il lerp della scala iniziale, con targetScale da raggiungere e come indice di proseguimento il timer
            currentScale = Vector3.Lerp(startScale, targetScale, timer);
            //Aggiorno la localScale con quella corrente
            transform.localScale = currentScale;
            yield return null;
        }

        yield return null;
    }
}
