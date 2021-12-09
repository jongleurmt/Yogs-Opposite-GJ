using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIControls : MonoBehaviour
{
    public void GoToGame()
        => SceneManager.LoadScene("Game");

    public void GoToLobby()
        => SceneManager.LoadScene("Player_Lobby");

    public void GoToTitle()
        => SceneManager.LoadScene("Title_Screen");

    public void Quit()
        => Application.Quit();
}
