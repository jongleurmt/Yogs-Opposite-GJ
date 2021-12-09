
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

    private int m_Health = 3;

    public int ptsOnBrake, pts1stPlayerHit, pts2ndPlayerHit;

    private Collider[] _coll;
    private Rigidbody _rb;

    [SerializeField]
    private bool m_Held = false, m_Thrown = false;
    public bool IsHeld => m_Held;
    public bool WasThrown => m_Thrown;

    private PlayerController m_SourcePlayer = null;

    private void Awake()
    {
        _coll = GetComponentsInChildren<Collider>();
        _rb = GetComponent<Rigidbody>();

        // add 1 to items amount in game manager
    }

    void Start()
    {
        switch (Damage)
        {
            case DamageValue.Soft: m_Health = 1; break;
            case DamageValue.Medium: m_Health = 2; break;
        }
    }

    public void PickUp(PlayerController player)
    {
        //if it was picked up by player set held to true
        foreach(var parts in _coll)
        {
            parts.enabled = false;
        }
        _rb.isKinematic = true;
        
        m_SourcePlayer = player;
        
        transform.parent = player.Hands;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        m_Held = true;
    }

    public void Throw()
    {
        StartCoroutine(EnableColliders());
        _rb.isKinematic = false;
        transform.parent = null;

        float forwardForce = 10f;
        switch (Weight)
        {
            case WeightValue.Medium: forwardForce = 7f; break;
            case WeightValue.Heavy: forwardForce = 5f; break;
        }

        //add force
        Vector3 v3Force = (forwardForce * transform.forward) + (forwardForce * 0.25f * transform.up);
        _rb.velocity = v3Force;

        m_Thrown = true;

        IEnumerator EnableColliders()
        {
            yield return new WaitForSeconds(0.2f);
            foreach (var parts in _coll)
            {
                parts.enabled = true;
            }
        }
    }

    private void OnCollisionEnter(Collision coll)
    {
        if(m_Held)
        {
            Debug.Log(coll.collider.name);
            if(coll.collider.tag == "Player")
            {
                //reduse points from other player and add to the player that trew this objec
                PlayerController controller = coll.gameObject.GetComponent<PlayerController>();
                if (controller != null && !controller.Equals(m_SourcePlayer))
                {
                    m_SourcePlayer.Model.AddScore(10);
                    controller.StartInvincibility();
                    Dent();
                }
            }
        }

        if (m_Thrown)
        {
            m_Held = false;
            m_Thrown = false;
            m_SourcePlayer = null;
        }
    }

    public void Dent(bool deathFling = false)
    {
        m_Health--;
        if (m_Health <= 0)
        {
            if (deathFling)
                Throw();

            if (m_Held)
            {
                m_SourcePlayer.Drop();
                m_SourcePlayer = null;
                transform.SetParent(null);
            }

            //put the code below here else object will be destroyed when spawning
            foreach (var child in children)
            {
                child.AddComponent<Rigidbody>();
                child.transform.parent = null;

                child.layer = LayerMask.NameToLayer("InvinciblePlayer");
                Destroy(child, 5f);
            }

            // subtrect 1 to items amount in game manager
            gameObject.layer = LayerMask.NameToLayer("InvinciblePlayer");
            Destroy(this.gameObject, 8f);
        }
}
}
