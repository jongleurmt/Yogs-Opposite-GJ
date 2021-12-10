using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    public enum PlayerState { Registered, Ready }

    [SerializeField]
    private LobbyPlayer[] m_PlayerControls = { };

    protected Dictionary<int, PlayerState> m_PlayerStates = new Dictionary<int, PlayerState>();

    [SerializeField]
    private GameObject m_Countdown = null;

    [SerializeField]
    private Text m_CountdownText = null;

    private Coroutine m_CountdownRoutine = null;

    // Syncs with the player manager.
    void Start()
    {
        PlayerManager.Instance.OnPlayerJoined.AddListener(OnPlayerJoined);
        m_Countdown.SetActive(false);
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
        if (m_PlayerControls.Length <= playerInfo.ID) return;

        // Activate the player.
        m_PlayerControls[playerInfo.ID].Enable();
        m_PlayerControls[playerInfo.ID].Bind(playerInfo.Input);
        
        int index = playerInfo.ID;

        if (m_PlayerStates.ContainsKey(index)) return;
        m_PlayerStates.Add(index, PlayerState.Registered);


        if (m_CountdownRoutine != null)
        {
            StopCoroutine(m_CountdownRoutine);
            m_Countdown.SetActive(false);
        }
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

        if (m_CountdownRoutine != null)
        {
            StopCoroutine(m_CountdownRoutine);
            m_Countdown.SetActive(false);
        }
    }

    // Checks the status of the players.
    protected void CheckPlayers()
    {
        // Do nothing if there are no players.
        if (m_PlayerStates.Count < 2) return;

        // Do nothing if any player isn't ready.
        foreach (PlayerState state in m_PlayerStates.Values)
            if (!state.Equals(PlayerState.Ready)) return;

        m_CountdownRoutine = StartCoroutine(Countdown());
        IEnumerator Countdown()
        {
            m_Countdown.SetActive(true);
            for (int t = 3; t > -1; t--)
            {
                if (t > 0) m_CountdownText.text = t.ToString();
                else m_CountdownText.text = "GO!";
                
                yield return new WaitForSeconds(1f);
            }
            
            SceneManager.LoadScene("Player_Test");
        }
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
