using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame2 : MonoBehaviour
{
    public string gameSceneName = "Jonas"; // Change this to your actual scene name

    public void PlayGame()
    {
        SceneManager.LoadScene("Jonas");
    }
    public void PlayGame1()
    {
        Debug.Log("Play button clicked!");
        SceneManager.LoadScene(gameSceneName);
    }
}
