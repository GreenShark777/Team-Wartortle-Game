using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    //Direzione
    [SerializeField]
    private int direction = 1;
    //velocità
    [SerializeField]
    private float speed = 1000f;
    void Update()
    {
        transform.Rotate(0, 0, (direction * speed) * Time.deltaTime);
    }
}
