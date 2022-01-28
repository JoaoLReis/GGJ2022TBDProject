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

    public List<PlayerController> players;
    public List<PlayerController> FinishedPlayers;

    public PlayerController CurrentPlayer => players[CurrentPlayerIndex];

    private void Start()
    {
        FinishedPlayers = new List<PlayerController>(players.Count);
    }

    public void ShowGameResults()
    {
        gameResultsUI.gameObject.SetActive(true);
        gameResultsUI.Setup(FinishedPlayers);
        FindObjectOfType<SoundManager>().playCheer();
    }

    public void SetPlayerFinished(PlayerController playerController)
    {
        if (FinishedPlayers.Contains(playerController))
            return;

        FinishedPlayers.Add(playerController);

        if (FinishedPlayers.Count == players.Count - 1)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (!FinishedPlayers.Contains(players[i]))
                    FinishedPlayers.Add(players[i]);
            }
        }
    }

    private bool IsAnyPlayerMoving()
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].IsMoving)
                return true;
        }

        return false;
    }

    public void RunCallbackWhenAllPlayerStopMoving(Action callback)
    {
        StartCoroutine(nameof(CheckIfAllPlayersStopped), callback);
    }

    private IEnumerator CheckIfAllPlayersStopped(Action callback)
    {
        while (IsAnyPlayerMoving())
        {
            yield return new WaitForSeconds(0.2f);
        }

        callback();
    }
}