using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    private static GameObject canvasMainMenu;
    private static GameObject canvasSettings;

    private static int currentLevelIndex = 0;
    public static int CurrentLevelIndex => currentLevelIndex;
    private static string[] levelNames = {"Level1", "Level2", "Level3", "Level4", "Level5", "Level6", "Level7", "Level8", "Level9", "Level10", "Level11", "Level12"};

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

    public static void loadGame(GameObject canvas)
    {
        SceneManager.canvasMainMenu = canvas;
        canvas.SetActive(false);
        UnityEngine.SceneManagement.SceneManager.LoadScene("TierSelector", LoadSceneMode.Additive);
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
        loadLevel(currentLevelIndex);
    }

    public static void loadLevel(int levelIndex)
    {
        if (levelIndex > levelNames.Length)
            levelIndex = 0;
        UnityEngine.SceneManagement.SceneManager.LoadScene(levelNames[levelIndex]);
    }
}
