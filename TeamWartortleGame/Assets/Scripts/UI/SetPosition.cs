using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPosition : MonoBehaviour
{
    //Posizione da seguire
    [SerializeField]
    private Vector3 offset;
    private void LateUpdate()
    {
        //Aggiorno per ogni frame la posizione a quella da seguire
        transform.position = GameManager.inst.currentEnemy.transform.position + offset;
    }
}
