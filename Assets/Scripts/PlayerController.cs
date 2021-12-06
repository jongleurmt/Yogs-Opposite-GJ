using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
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

    [Header("Links")]
    [SerializeField]
    private Animator m_MeleeAnimator = null;

    // The mesh renderer components.
    private MeshRenderer[] m_MeshRenderers = {};

    // Whether or not the player is grounded.
    private bool m_IsGrounded = true;

    // The input direction vectors.
    private Vector3 m_Direction = Vector3.zero;

    // The target rotation.
    private Vector3 m_TargetRotation = Vector3.zero;

    // The rigidbody component.
    private Rigidbody m_Rigidbody = null;

    // The default drag.
    private float m_DefaultDrag = 0f;

    /// <summary>
    /// Component caching.
    /// </summary>
    void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_MeshRenderers = GetComponentsInChildren<MeshRenderer>();

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

        // Fall handler.
        if (m_Rigidbody.velocity.y < 0 && !m_IsGrounded)
            m_Rigidbody.drag = m_DescentDrag;
        else
            m_Rigidbody.drag = m_DefaultDrag;
    }

    // Collision Detection.
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            m_IsGrounded = true;
        else if (collision.gameObject.CompareTag("MeleeArm"))
        {
            if (collision.contactCount == 0) return;

            ContactPoint contact = collision.GetContact(0);
            Vector3 direction = transform.position - contact.point;
            direction.y = 1f;
            direction.x *= 10f;

            StartCoroutine(Invincibility(3, 0.15f));
        }
    }

    // Collision detection.
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            m_IsGrounded = false;
    }

    // Direction handler.
    protected override void Direction(Vector2 value)
    {
        m_Direction = new Vector3(value.x, 0, value.y);
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
        if (m_MeleeAnimator == null) return;
        m_MeleeAnimator.SetBool("MeleeAlternate", !m_MeleeAnimator.GetBool("MeleeAlternate"));
        m_MeleeAnimator.SetTrigger("Melee");
    }

    // Temporary invincibility.
    protected IEnumerator Invincibility(int cycles, float transitionDuration)
    {
        gameObject.layer = LayerMask.NameToLayer("InvinciblePlayer");
        for (int i = 0; i < cycles; i++)
        {
            foreach (MeshRenderer mr in m_MeshRenderers)
                mr.enabled = false;

            yield return new WaitForSeconds(transitionDuration);

            foreach (MeshRenderer mr in m_MeshRenderers)
                mr.enabled = true;
            
            yield return new WaitForSeconds(transitionDuration);
        }
        gameObject.layer = LayerMask.NameToLayer("Player");
    }
}
