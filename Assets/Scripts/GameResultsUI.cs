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

    public void Setup(PlayerController player)
    {
        numLevel.text = "Level " + (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex - 3).ToString();
        
        timeToEnd.text = ((int) player.LevelTime).ToString();
        polarityChangesToEnd.text = player.NumPolaritySwitches.ToString();
        numDeaths.text = player.NumDeaths.ToString();
    }
}