using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameResultsUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text[] playerResultsNode;
    [SerializeField]
    private Image[] playerResultsAvatars;
    
    public void Setup(List<PlayerController> players)
    {
        for (int i = 0; i < playerResultsNode.Length; i++)
        {
            playerResultsNode[i].text = players[i].gameObject.name;
            playerResultsAvatars[i].overrideSprite = players[i].PlayerAvatar;
        }
    }
}