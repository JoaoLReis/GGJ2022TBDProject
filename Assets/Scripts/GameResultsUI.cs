using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameResultsUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text numLevel;
        
    [SerializeField]
    private TMP_Text timeToEnd;
    [SerializeField]
    private TMP_Text polarityChangesToEnd;
    [SerializeField]
    private TMP_Text numDeaths;

    void Awake()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
    }

    public void Setup(PlayerController player)
    {
        numLevel.text = "Level " + (SceneManager.CurrentLevelIndex +1).ToString();
        float fractionsOfASecond = (player.LevelTime % 1);
        float restOfFractions = (fractionsOfASecond * 100) % 1;
        timeToEnd.text = (player.LevelTime - fractionsOfASecond).ToString() + ":" + (fractionsOfASecond*100 - restOfFractions);
        polarityChangesToEnd.text = player.NumPolaritySwitches.ToString();
        numDeaths.text = player.NumDeaths.ToString();
    }
}