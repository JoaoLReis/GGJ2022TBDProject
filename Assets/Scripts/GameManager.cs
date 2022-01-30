using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SceneBoundSingletonBehaviour<GameManager>
{
    [HideInInspector]
    public int CurrentPlayerIndex;

    public CameraFollow Camera;
    [SerializeField]
    private GameResultsUI gameResultsUI;

    public PlayerController player;

    private void Start()
    {

    }

    public void ShowGameResults()
    {
        gameResultsUI.Setup(player);
        gameResultsUI.gameObject.SetActive(true);
        FindObjectOfType<SoundManager>().playCheer();
    }

    private bool IsPlayerMoving()
    {
        return player.IsMoving;
    }

    public void RunCallbackWhenAllPlayerStopMoving(Action callback)
    {
        StartCoroutine(nameof(CheckIfPlayerStopped), callback);
    }

    private IEnumerator CheckIfPlayerStopped(Action callback)
    {
        while (IsPlayerMoving())
        {
            yield return new WaitForSeconds(0.2f);
        }

        callback();
    }
}