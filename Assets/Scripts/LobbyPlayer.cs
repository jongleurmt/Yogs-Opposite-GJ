using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyPlayer : PlayerControllerBase
{
    [SerializeField]
    private Vector3 m_EjectForce = Vector3.up;

    // The lobby mmanager.
    private LobbyManager m_LobbyManager = null;

    // The rigidbody.
    private Rigidbody m_Rigidbody = null;

    private Vector3 m_DefaultPosition;
    private Quaternion m_DefaultRotation;

    void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_LobbyManager = FindObjectOfType<LobbyManager>();

        m_DefaultPosition = m_Rigidbody.position;
        m_DefaultRotation = m_Rigidbody.rotation;
    }

    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(0.15f);
        
        m_Rigidbody.velocity = Vector3.zero;
        m_Rigidbody.angularVelocity = Vector3.zero;
        m_Rigidbody.MovePosition(m_DefaultPosition);
        m_Rigidbody.MoveRotation(m_DefaultRotation);

        gameObject.SetActive(false);
    }
    
    // Confirm
    protected override void Jump()
    {
        
    }

    // Disconnect
    protected override void Melee()
    {
        m_Rigidbody.AddForce(m_EjectForce, ForceMode.Impulse);
        StartCoroutine(Despawn());
        m_LobbyManager.CancelPlayer(ID);
    }
}
