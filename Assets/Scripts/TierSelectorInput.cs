using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TierSelectorInput : MonoBehaviour
{
    public void selectTierCallback()
    {
        SceneManager.loadGame();
    }
}
