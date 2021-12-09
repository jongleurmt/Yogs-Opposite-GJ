using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    public enum PlayerState { Registered, Ready }

    [SerializeField]
    private GameObject[] m_PlayerObjects = { };

    [SerializeField]
    private LobbyPlayer[] m_Players = { };

    protected Dictionary<int, PlayerState> m_PlayerStates = new Dictionary<int, PlayerState>();

    // Syncs with the player manager.
    void Start()
    {
        PlayerManager.Instance.OnPlayerJoined.AddListener(OnPlayerJoined);
        foreach (GameObject camera in m_PlayerObjects) camera.SetActive(false);
    }

    // Desyncs with the player manager.
    void OnDestroy()
    {
        PlayerManager.Instance.OnPlayerJoined.RemoveListener(OnPlayerJoined);
    }

    // Player joined function.
    void OnPlayerJoined(PlayerInfo playerInfo)
    {
        // Stop if not enough objects!
        if (m_PlayerObjects.Length <= playerInfo.ID) return;

        // Activate the player.
        m_PlayerObjects[playerInfo.ID].SetActive(true);
        m_Players[playerInfo.ID].Bind(playerInfo.Input);
        
        int index = playerInfo.ID;

        if (m_PlayerStates.ContainsKey(index)) return;
        m_PlayerStates.Add(index, PlayerState.Registered);
    }

    // Cancels a player.
    public void CancelPlayer(int id)
    {
        // Do nothing if the player key isn't available.
        if (!m_PlayerStates.ContainsKey(id)) return;

        if (m_PlayerStates[id].Equals(PlayerState.Ready))
        {
            // Stop the countdown.
            m_PlayerStates[id] = PlayerState.Registered;
        }
        else if (m_PlayerStates[id].Equals(PlayerState.Registered))
        {
            // Remove the player state and kill the controls.
            m_PlayerStates.Remove(id);
            PlayerManager.Instance.DeregisterPlayer(id);

            // Check if the rest are still confirmed.
            CheckPlayers();
        }
    }

    // Checks the status of the players.
    protected void CheckPlayers()
    {
        // Do nothing if there are no players.
        if (m_PlayerStates.Count == 0) return;

        // Do nothing if any player isn't ready.
        foreach (PlayerState state in m_PlayerStates.Values)
            if (!state.Equals(PlayerState.Ready)) return;

        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }

    public void ConfirmPlayer(int id)
    {
        // Do nothing if the key doesn't exist.
        if (!m_PlayerStates.ContainsKey(id)) return;

        // // Push the player to Ready only.
        if (m_PlayerStates[id].Equals(PlayerState.Registered))
            m_PlayerStates[id] = PlayerState.Ready;
        else return;

        CheckPlayers();
    }
}
