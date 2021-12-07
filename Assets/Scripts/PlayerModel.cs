using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    // The player's score.
    private int m_Score = 0;
    public int Score => m_Score;

    /// <summary>
    /// Adds points to this player.
    /// </summary>
    /// <param name="value">The player value.</param>
    public void AddScore(int value)
    {
        m_Score += value;
    }
}
