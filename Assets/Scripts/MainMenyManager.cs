using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenyManager : MonoBehaviour
{
    public void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void LoadEasyDifficulty()
    {
        DifficultySettings.stumbleAmount = 2.5f;
        SceneManager.LoadScene("MainGame");
    }

    public void LoadNormalDifficulty()
    {
        DifficultySettings.stumbleAmount = 5f;
        SceneManager.LoadScene("MainGame");
    }
    public void LoadHardDifficulty()
    {
        DifficultySettings.stumbleAmount = 7.5f;
        SceneManager.LoadScene("MainGame");
    }
}
