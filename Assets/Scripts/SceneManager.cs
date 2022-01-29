using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    private static GameObject canvasMainMenu;
    private static GameObject canvasSettings;

    private static int currentLevelIndex = 0;
    private static string[] levelNames = {"Level1", "Level2", "Level3" };

    // Start is called before the first frame update
    void Awake() {
        DontDestroyOnLoad(this);
        loadMainMenu();
        Debug.Log("SceneManager awake");
    }

    public static void loadMainMenu() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public static void unloadSceneToMainMenu(string scene) {
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(scene);
        if (scene == "Credits" && SceneManager.canvasSettings != null) 
        {
            SceneManager.canvasSettings.SetActive(true);
        }
        else
        {
            if (SceneManager.canvasMainMenu != null) 
            {
                SceneManager.canvasMainMenu.SetActive(true);
            }

        }
    }

    public static void loadGame() {
        UnityEngine.SceneManagement.SceneManager.LoadScene(levelNames[currentLevelIndex]);
    }
    
    public static void loadCredits(GameObject canvas) {
        SceneManager.canvasSettings = canvas;
        canvas.SetActive(false);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Credits", LoadSceneMode.Additive);
    }
    
    public static void loadSettings(GameObject canvas) {
        SceneManager.canvasMainMenu = canvas;
        canvas.SetActive(false);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Settings", LoadSceneMode.Additive);
    }

    public static void loadNextLevel()
    {
        currentLevelIndex = (++currentLevelIndex) % levelNames.Length;
        loadGame();
    }
}
