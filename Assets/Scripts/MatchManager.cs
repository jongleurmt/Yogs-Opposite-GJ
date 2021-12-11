using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MatchManager : MonoBehaviour
{
    [SerializeField]
    private PlayerController[] m_Controllers = {};
    private List<PlayerController> m_SpawnedPlayers = new List<PlayerController>();

    [SerializeField]
    private Transform[] m_SpawnPoints = {};

    [SerializeField]
    private Text[] m_ScoreText = {};

    // The player scores.
    private Dictionary<int, int> m_PlayerScores = new Dictionary<int, int>();

    // The set king event.
    public UnityEvent<int> SetRoyal { get; private set; } = new UnityEvent<int>();

    public UnityEvent OnGameEnd { get; private set; } = new UnityEvent();

    [SerializeField]
    private Text m_WinnerText = null;

    void Awake()
    {
        PlayerInput[] inputs = GameObject.FindObjectsOfType<PlayerInput>().OrderBy(input => input.playerIndex).ToArray();
        List<Text> scoreTextUsed = new List<Text>();
        
        foreach (PlayerInput input in inputs)
        {
            if (m_Controllers.Length <= input.playerIndex) return;

            Vector3 pos = m_SpawnPoints[input.playerIndex].position;

            PlayerController player = Instantiate(m_Controllers[input.playerIndex], pos, Quaternion.identity);
            player.SetScoreText(m_ScoreText[input.playerIndex]);
            scoreTextUsed.Add(m_ScoreText[input.playerIndex]);
            m_SpawnedPlayers.Add(player);

            player.Bind(input);
        }

        foreach (Text scoreText in m_ScoreText)
        {
            if (scoreTextUsed.Contains(scoreText)) continue;
            scoreText.text = "Not Playing";
        }

        OnGameEnd.AddListener(GameEnd);
    }

    void Start()
    {
        SetRoyal.Invoke(-1);
        m_WinnerText.text = "";
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

    void GameEnd()
    {
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

        m_WinnerText.text = $"Player {firstPlayer} wins!";

        StartCoroutine(GoBack());
        IEnumerator GoBack()
        {
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene("Title_Screen");
        }
    }
}
