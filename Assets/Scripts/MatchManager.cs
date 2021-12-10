using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class MatchManager : MonoBehaviour
{
    [SerializeField]
    private PlayerController[] m_Controllers = {};
    private List<PlayerController> m_SpawnedPlayers = new List<PlayerController>();

    [SerializeField]
    private Transform[] m_SpawnPoints = {};

    // The player scores.
    private Dictionary<int, int> m_PlayerScores = new Dictionary<int, int>();

    // The set king event.
    public UnityEvent<int> SetRoyal { get; private set; } = new UnityEvent<int>();

    void Awake()
    {
        PlayerInput[] inputs = GameObject.FindObjectsOfType<PlayerInput>().OrderBy(input => input.playerIndex).ToArray();
        
        foreach (PlayerInput input in inputs)
        {
            if (m_Controllers.Length <= input.playerIndex) return;

            Vector3 pos = m_SpawnPoints[input.playerIndex].position;

            PlayerController player = Instantiate(m_Controllers[input.playerIndex], pos, Quaternion.identity);
            m_SpawnedPlayers.Add(player);

            player.Bind(input);
        }
    }

    void Start()
    {
        SetRoyal.Invoke(-1);
    }

    public void SetPlayerScore(int playerIndex, int score)
    {
        if (!m_PlayerScores.ContainsKey(playerIndex))
            m_PlayerScores.Add(playerIndex, score);
        else
            m_PlayerScores[playerIndex] = score;

        int highestScore = -100;
        int firstPlayer = -1;

        foreach (KeyValuePair<int, int> scoreData in m_PlayerScores)
        {
            if (scoreData.Value > highestScore)
            {
                highestScore = scoreData.Value;
                firstPlayer = scoreData.Key;
            }
            else if (scoreData.Value == highestScore)
            {
                firstPlayer = -1;
                break;
            }
        }

        SetRoyal.Invoke(firstPlayer);
    }
}
