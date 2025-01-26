using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScene2 : MonoBehaviour

{
    public string gameSceneName = "EndScene2"; // Change this to your actual scene name

    public void NextScene()
    {
        SceneManager.LoadScene("EndScene2");
    }
}
