using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitInput : MonoBehaviour
{

    public void exitButtonCallback(string scene) {
        SceneManager.unloadSceneToMainMenu(scene);
    }
}
