using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        SceneManager.LoadScene("Editor");
    }

    public void LevelToEditor()
    {
        Time.timeScale = 1;
        ModifEditorHandler.IsModif = true;
        SceneManager.LoadScene("Editor");
    }

    public void Import()
    {
        SceneManager.LoadScene("MenuLevel");
    }

    public void RestartLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
