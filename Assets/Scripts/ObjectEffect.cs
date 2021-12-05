
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectEffect : MonoBehaviour
{
    public GameObject[] children;

    public int damageValue;
    public int heavyValue;

    private bool held;
    private void start()
    {
        held = false;
    }

    private void Update()
    {
        //if it was picked up by player set held to true
    }

    private void OnCollisionEnter(Collision coll)
    {
        if(held)
        {
            //put the code below here else object will be destroyed when spawning
        }
        foreach (var child in children)
        {
            child.AddComponent<Rigidbody>();
            child.transform.parent = null;

            Destroy(child, 2f);
        }

        Destroy(this.gameObject , 1f);
    }

}
