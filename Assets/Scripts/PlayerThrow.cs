    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrow : MonoBehaviour
{
    public ObjectEffect items;
    public Transform holdingPoint;

    public int force;

    // Start is called before the first frame update
    void Start()
    {
        // items.PickUp(holdingPoint);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnThrow()
    {
        items.Throw();
    }
}
