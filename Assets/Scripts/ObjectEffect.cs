
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectEffect : MonoBehaviour
{
    public GameObject[] children;

    public enum DamageValue { Soft, Medium, Hard }
    public DamageValue Damage;
    public enum WeightValue { Light, Medium, Heavy }
    public WeightValue Weight;
    public float ForwordForce, UpwordForce;

    public int Health;

    public int ptsOnBrake, pts1stPlayerHit, pts2ndPlayerHit;

    private Collider[] _coll;
    private Rigidbody _rb;

    [SerializeField]
    private bool held = false, thrown = false;

    private void Awake()
    {
        _coll = GetComponentsInChildren<Collider>();
        _rb = GetComponent<Rigidbody>();

        // add 1 to items amount in game manager
    }

    public void OnInteract()
    {
        PickUp(transform);
    }

    public void PickUp(Transform hands)
    {
        //if it was picked up by player set held to true
        foreach(var parts in _coll)
        {
            parts.enabled = false;
        }
        _rb.isKinematic = true;
        
        Debug.Log("picked up");
        
        transform.parent = hands;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        held = true;
    }

    public void Throw()
    {
        foreach (var parts in _coll)
        {
            parts.enabled = true;
        }
        _rb.isKinematic = false;
        transform.parent = null;

        //add force
        Vector3 v3Force = (ForwordForce * transform.forward) + (UpwordForce * transform.up);
        _rb.velocity = v3Force;

        thrown = true;
    }

    private void OnCollisionEnter(Collision coll)
    {
        //Debug.Log(coll.gameObject.name);

        if(held)
        {
            Health--;

            if(coll.collider.tag == "player")
            {
                //reduse points from other player and add to the player that trew this objec
            }

            if(Health <= 0)
            {
                //put the code below here else object will be destroyed when spawning
                foreach (var child in children)
                {
                    child.AddComponent<Rigidbody>();
                    child.transform.parent = null;

                    Destroy(child, 2f);
                }

                // subtrect 1 to items amount in game manager
                Destroy(this.gameObject, 1f);
            }

        }

        if (thrown)
        {
            held = false;
            thrown = false;
        }
    }
}
