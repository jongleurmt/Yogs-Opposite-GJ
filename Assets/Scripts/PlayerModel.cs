using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    // The player's score.
    private int m_Score = 0;
    public int Score => m_Score;

    // Adds points to player.
    public void AddScore(int value)
    {
        m_Score += value;
    }

    // Removes points from player.
    public void ReduceScore(int value)
    {
        m_Score -= value;
    }
}
