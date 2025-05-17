using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigationScript : MonoBehaviour
{
    public void Settings()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void MenuLevel()
    {
        SceneManager.LoadScene("MenuLevel");
    }

    public void Editor()
    {
        SceneManager.LoadScene("MenuLevel");
    }

    public void Import()
    {
        SceneManager.LoadScene("MenuLevel");
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
