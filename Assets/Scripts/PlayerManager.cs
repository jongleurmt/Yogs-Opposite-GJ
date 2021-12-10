using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputManager))]
public class PlayerManager : Singleton<PlayerManager>
{
    // The player input manager.
    protected PlayerInputManager m_PlayerInputManager = null;

    // The player data.
    private Dictionary<int, PlayerInfo> m_Players = new Dictionary<int, PlayerInfo>();

    /// <summary>
    /// The player joined event.
    /// </summary>
    /// <typeparam name="int">The player index.</typeparam>
    /// <typeparam name="PlayerInput">The player input.</typeparam>
    public UnityEvent<PlayerInfo> OnPlayerJoined { get; private set; } = new UnityEvent<PlayerInfo>();

    /// <summary>
    /// The player leave event.
    /// </summary>
    /// <typeparam name="int">The player index.</typeparam>
    public UnityEvent<int> OnPlayerLeft { get; private set; } = new UnityEvent<int>();

    /// <summary>
    /// The number of players active in the game.
    /// </summary>
    public int PlayerCount => m_Players.Count();

    /// <summary>
    /// Loads the required components.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        
        m_PlayerInputManager = GetComponent<PlayerInputManager>();
        m_PlayerInputManager.onPlayerJoined += RegisterInput;
        m_PlayerInputManager.onPlayerLeft += DeregisterInput;
    }

    /// <summary>
    /// Deregisters a player's controller.
    /// </summary>
    /// <param name="id">The player index.</param>
    public void DeregisterInput(PlayerInput input)
        => DeregisterPlayer(input.playerIndex);

    /// <summary>
    /// Deregisters a player's controller.
    /// </summary>
    /// <param name="id">The player index.</param>
    public void DeregisterPlayer(int index)
    {
        // Stop here if player is not in list.
        if (!m_Players.ContainsKey(index))
            return;

        // Get the player info.
        PlayerInfo info = m_Players[index];

        // Destroy the associated components.
        m_Players.Remove(info.ID);
        Destroy(info.GameObject);

        // Enable joining.
        OnPlayerLeft.Invoke(index);
        EnableJoining();
    }

    /// <summary>
    /// Disables player joining.
    /// </summary>
    public void DisableJoining()
        => m_PlayerInputManager.DisableJoining();

    /// <summary>
    /// Enables player joining.
    /// </summary>
    public void EnableJoining()
        => m_PlayerInputManager.EnableJoining();

    /// <summary>
    /// Returns the registered players.
    /// </summary>
    public PlayerInfo[] GetPlayers()
        => m_Players.Values.ToArray();

    /// <summary>
    /// Register the joined player.
    /// </summary>
    /// <param name="input">The player input component.</param>
    public void RegisterInput(PlayerInput input)
    {
        // Rename and parent the object.
        input.gameObject.name = $"Player-{input.playerIndex}";
        input.transform.SetParent(transform);

        PlayerInfo info = new PlayerInfo{
            Input = input,
            GameObject = input.gameObject
        };

        // Register the player input.
        m_Players.Add(info.ID, info);

        // Invoke the associated functions.
        OnPlayerJoined.Invoke(info);

        if (m_Players.Count == 4)
            DisableJoining();
    }

    public void Clear()
    {
        PlayerInfo[] infos = m_Players.Values.ToArray();
        foreach (PlayerInfo info in infos)
            DeregisterInput(info.Input);
    }
}