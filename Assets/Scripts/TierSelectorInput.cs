using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TierSelectorInput : MonoBehaviour
{
    public void loadLevel(int levelIndex)
    {
        SceneManager.loadLevel(levelIndex);
    }
}
