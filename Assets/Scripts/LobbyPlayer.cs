using UnityEngine;

public class LobbyPlayer : PlayerControllerBase
{
    [SerializeField]
    private Vector3 m_EjectForce = Vector3.up;

    // The lobby mmanager.
    private LobbyManager m_LobbyManager = null;

    // The rigidbody.
    private Rigidbody m_Rigidbody = null;

    // The default transform values.
    private Vector3 m_DefaultPosition;
    private Quaternion m_DefaultRotation;

    private bool m_IsReady = false;

    [SerializeField]
    private GameObject m_RegisteredText;

    [SerializeField]
    private GameObject m_ReadyText;

    void Awake()
    {
        m_LobbyManager = FindObjectOfType<LobbyManager>();

        m_Rigidbody = GetComponent<Rigidbody>();
        m_DefaultPosition = m_Rigidbody.position;
        m_DefaultRotation = m_Rigidbody.rotation;
        m_Rigidbody.isKinematic = true;

        m_ReadyText.SetActive(false);
        m_RegisteredText.SetActive(false);
    }

    void Update()
    {
        if (transform.position.y > 15)
            Despawn();
    }

    void Despawn()
    {
        m_Rigidbody.velocity = Vector3.zero;
        m_Rigidbody.angularVelocity = Vector3.zero;
        m_Rigidbody.MovePosition(m_DefaultPosition);
        m_Rigidbody.MoveRotation(m_DefaultRotation);

        m_Rigidbody.isKinematic = true;
    }
    
    // Confirm
    protected override void Interact()
    {
        if (!m_IsReady)
        {
            m_LobbyManager.ConfirmPlayer(ID);
            m_IsReady = true;

            m_ReadyText.SetActive(true);
            m_RegisteredText.SetActive(false);
        }
    }

    // Disconnect
    protected override void Melee()
    {
        m_ReadyText.SetActive(false);
        if (m_IsReady)
        {
            m_IsReady = false;
            m_RegisteredText.SetActive(true);
        }
        else
        {
            m_RegisteredText.SetActive(false);
            m_Rigidbody.AddForce(m_EjectForce, ForceMode.Impulse);
        }
        
        m_LobbyManager.CancelPlayer(ID);
    }

    public void Enable()
    {
        m_Rigidbody.isKinematic = false;
        m_ReadyText.SetActive(false);
        m_RegisteredText.SetActive(true);
    }
}
