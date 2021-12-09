// TODO: Receive Melee Attack
// TODO: Break object?

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(PlayerModel))]
public class PlayerController : PlayerControllerBase
{
    [Header("Testing")]
    [SerializeField]
    private PlayerInput m_BindInput = null;

    [Header("Transform Manipulation")]
    [SerializeField]
    private float m_MotionSpeed = 10f;

    [SerializeField]
    private float m_RotationSpeed = 90f;

    [SerializeField]
    private float m_JumpForce = 800f;

    [SerializeField]
    private float m_DescentDrag = -1.5f;

    [Header("References")]
    [SerializeField]
    private Transform m_Hands = null;
    public Transform Hands => m_Hands;
    
    [SerializeField]
    private Collider m_BoxCastCollider = null;

    // The character animator.
    private Animator m_Animator;

    // The object currently held.
    private ObjectEffect m_ObjectHeld = null;

    // The invincibility boolean.
    private bool m_IsInvincible = false;

    // Whether or not the player is grounded.
    private bool m_IsGrounded = true;

    // The input direction vectors.
    private Vector3 m_Direction = Vector3.zero;

    // The target rotation.
    private Vector3 m_TargetRotation = Vector3.zero;

    // The rigidbody component.
    private Rigidbody m_Rigidbody = null;

    // The player model component.
    private PlayerModel m_Model = null;
    public PlayerModel Model => m_Model;

    // The default drag.
    private float m_DefaultDrag = 0f;

    // The list of players triggered.
    private List<PlayerController> m_PlayerList = new List<PlayerController>();

    /// <summary>
    /// Component caching.
    /// </summary>
    void Awake()
    {
        m_Model = GetComponent<PlayerModel>();

        m_Rigidbody = GetComponent<Rigidbody>();
        m_Animator = GetComponentInChildren<Animator>();

        m_DefaultDrag = m_Rigidbody.drag;
    }

    /// <summary>
    /// Property setup.
    /// </summary>
    void Start()
    {
        if (m_BindInput != null) Bind(m_BindInput);
        m_TargetRotation = m_Rigidbody.rotation.eulerAngles;
    }

    // Physics handling.
    void FixedUpdate()
    {
        // Fall handler.
        if (m_Rigidbody.velocity.y < 0 && !m_IsGrounded)
            m_Rigidbody.drag = m_DescentDrag;
        else
            m_Rigidbody.drag = m_DefaultDrag;

        if (m_IsInvincible) return;
        
        // Character motion.
        Vector3 velocity = m_Direction * m_MotionSpeed;
        velocity.y = m_Rigidbody.velocity.y;
        m_Rigidbody.velocity = velocity;

        // Character rotation.
        if (m_Direction != Vector3.zero)
        {
            Quaternion lookRot = Quaternion.LookRotation(m_Direction, Vector3.up);
            m_Rigidbody.MoveRotation(Quaternion.Slerp(m_Rigidbody.rotation, lookRot, m_RotationSpeed * Time.fixedDeltaTime));
        }
    }

    // Collision Detection.
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            m_IsGrounded = true;
            m_Animator.SetBool("IsGrounded", m_IsGrounded);
        }
    }

    // Collision detection.
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            m_IsGrounded = false;
            m_Animator.SetBool("IsGrounded", m_IsGrounded);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController controller = other.GetComponent<PlayerController>();
            if (controller == null) return;
            m_PlayerList.Add(controller);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController controller = other.GetComponent<PlayerController>();
            if (controller == null) return;
            m_PlayerList.Remove(controller);
        }
    }

    // Direction handler.
    protected override void Direction(Vector2 value)
    {
        m_Direction = new Vector3(value.x, 0, value.y);
        m_Animator.SetBool("IsMoving", !m_Direction.Equals(Vector3.zero));
    }

    // Interact handler.
    protected override void Interact()
    {
        if (m_ObjectHeld == null)
        {
            Vector3 halfExtents = m_BoxCastCollider.bounds.extents;
            float distance = Vector3.Distance(transform.position, m_BoxCastCollider.transform.position) * 2f;
            RaycastHit hit;

            if (Physics.BoxCast(transform.position, halfExtents, transform.forward, out hit, Quaternion.identity, distance, 1 << LayerMask.NameToLayer("Object")))
            {
                ObjectEffect oe = hit.collider.GetComponent<ObjectEffect>();
                if (oe != null)
                {
                    m_Animator.SetBool("IsHoldingObject", true);
                    oe.PickUp(this);
                    m_ObjectHeld = oe;
                }
            }
        }
        else
        {
            // drop object?
        }
    }

    // Jump handler.
    protected override void Jump()
    {
        if (m_IsGrounded)
            m_Rigidbody.AddForce(Vector3.up * m_JumpForce, ForceMode.Impulse);
    }

    // Melee handler.
    protected override void Melee()
    {
        if (m_Animator == null || m_ObjectHeld == null) return;

        foreach (PlayerController controller in m_PlayerList)
        {
            Model.AddScore(10);
            controller.StartInvincibility();
        }

        // Dent the object once for all players hit.
        if (m_PlayerList.Count > 0)
        {
            m_ObjectHeld.Dent(true);
        }

        m_Animator.SetTrigger("Melee");
    }

    // Throw handler.
    protected override void Throw()
    {
        if (m_ObjectHeld == null) return;

        m_ObjectHeld.Throw();
        Drop();
    }

    // Toggles invincibility.
    public void SetInvincible(bool value)
    {
        m_IsInvincible = value;
        gameObject.layer = LayerMask.NameToLayer(m_IsInvincible ? "InvinciblePlayer" : "Player");
    }

    public void StartInvincibility()
        => m_Animator.SetTrigger("Invincibility");

    // Toggles invincibility ON.
    // Used by the Animator.
    public void EnableInvincibility()
        => SetInvincible(true);

    // Toggles invincibility ON.
    // Used by the Animator.
    public void DisableInvincibility()
        => SetInvincible(false);

    public void Drop()
    {
        m_ObjectHeld = null;
        m_Animator.SetBool("IsHoldingObject", false);
    }
}
