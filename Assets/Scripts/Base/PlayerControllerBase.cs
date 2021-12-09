using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Base class for character controls.
/// </summary>
public abstract class PlayerControllerBase : MonoBehaviour
{
    // The player input component.
    private PlayerInput m_PlayerInput = null;

    // The player index.
    public int ID => m_PlayerInput == null ? -1 : m_PlayerInput.playerIndex;

    // Input actions to bind.
    private InputAction m_DirectionAction = null;
    private InputAction m_InteractAction = null;
    private InputAction m_JumpAction = null;
    private InputAction m_MeleeAction = null;
    private InputAction m_ThrowAction = null;

    /// <summary>
    /// Handles the input actions.
    /// </summary>
    /// <param name="context">The callback context given by each action.</param>
    private void ActionTriggeredHandler(InputAction.CallbackContext context)
    {
        // The direction action.
        if (context.action.Equals(m_DirectionAction) && context.performed)
            Direction(context.ReadValue<Vector2>());
        
        // The interact action.
        else if (context.action.Equals(m_InteractAction) && !context.performed && !context.canceled)
            Interact();

        // The jump action.
        else if (context.action.Equals(m_JumpAction) && !context.performed && !context.canceled)
            Jump();

        // The melee action.
        else if (context.action.Equals(m_MeleeAction) && !context.performed && !context.canceled)
            Melee();
            
        // The throw action.
        else if (context.action.Equals(m_ThrowAction) && !context.performed && !context.canceled)
            Throw();
    }

    /// <summary>
    /// Binds the player input controls.
    /// </summary>
    /// <param name="playerInput">The player input controls.</param>
    public void Bind(PlayerInput playerInput)
    {
        // Input setup.
        m_PlayerInput = playerInput;
        m_PlayerInput.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;

        // In case the current action map is null.
        if (m_PlayerInput.currentActionMap == null)
            m_PlayerInput.SwitchCurrentActionMap(m_PlayerInput.defaultActionMap);

        // Action references.
        m_DirectionAction = m_PlayerInput.currentActionMap.FindAction("Direction");
        m_InteractAction = m_PlayerInput.currentActionMap.FindAction("Interact");
        m_JumpAction = m_PlayerInput.currentActionMap.FindAction("Jump");
        m_MeleeAction = m_PlayerInput.currentActionMap.FindAction("Melee");
        m_ThrowAction = m_PlayerInput.currentActionMap.FindAction("Throw");

        // Bind the actions.
        m_PlayerInput.onActionTriggered += ActionTriggeredHandler;
    }

    /// <summary>
    /// Removes all references to the player input.
    /// </summary>
    public void Unbind()
    {
        if (m_PlayerInput == null) return;
        
        // Clear the function.
        m_PlayerInput.onActionTriggered -= ActionTriggeredHandler;

        // Clean up the actions.
        m_DirectionAction = null;
        m_InteractAction = null;
        m_JumpAction = null;
        m_MeleeAction = null;
        m_ThrowAction = null;

        // Remove the input reference.
        m_PlayerInput = null;
    }

    /// <summary>
    /// The direction action.
    /// </summary>
    /// <param name="value">The vector value.</param>
    protected virtual void Direction(Vector2 value)
    { }

    /// <summary>
    /// The interact action.
    /// </summary>
    protected virtual void Interact()
    { }

    /// <summary>
    /// The jump action.
    /// </summary>
    protected virtual void Jump()
    { }

    /// <summary>
    /// The melee action.
    /// </summary>
    protected virtual void Melee()
    { }

    /// <summary>
    /// The throw action.
    /// </summary>
    protected virtual void Throw()
    { }
}
