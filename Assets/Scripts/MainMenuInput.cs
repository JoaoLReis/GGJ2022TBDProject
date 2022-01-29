using UnityEngine;

public class MainMenuInput : MonoBehaviour
{
    public GameObject canvas;

    public void GoToMainMenu()
    {
        SceneManager.loadMainMenu();
    }
    
    public void playButtonCallback()
    {
        SceneManager.loadGame();
    }
    
    public void settingsCallback()
    {
        SceneManager.loadSettings(canvas);
    }
    
    public void creditCallback()
    {
        SceneManager.loadCredits(canvas);
    }
}
