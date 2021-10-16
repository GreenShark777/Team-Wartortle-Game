using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPivot : MonoBehaviour
{

    [SerializeField]
    private RectTransform otherImage;

    [SerializeField]
    private RectTransform myNewParent;

    private RectTransform myRect;


    private void Awake()
    {
        //myNewParent.SetParent(transform.parent);
    }

    private void Start()
    {

        myRect = GetComponent<RectTransform>();

        myRect.SetParent(myNewParent);
        //transform.parent.SetParent(myNewParent);

        myNewParent.position = otherImage.position;

    }

}
