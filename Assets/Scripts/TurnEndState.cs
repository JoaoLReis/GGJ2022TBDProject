using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnEndState : GameState
{
    private bool alreadyEnded;
    
    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Player turn Ended");
        alreadyEnded = false;
    }

    protected override void TriggerEndState()
    {
        if (CanGameFinish())
            gameStateMachine.SetState<GameFinishState>();
        else
            gameStateMachine.SetState<PlayerTurnState>();
    }

    public override void OnUpdate()
    {
        if (!GameManager.Instance.CurrentPlayer.IsMoving && !alreadyEnded)
            gameStateMachine.StartCoroutine(DelayTriggerEndState());
    }

    private IEnumerator DelayTriggerEndState()
    {
        alreadyEnded = true;
        yield return new WaitForSeconds(PlayerController.RespawnWaitTime);
        TriggerEndState();
    }

    private bool CanGameFinish()
    {
        List<PlayerController> players = GameManager.Instance.players;
        int playersThatFinished = 0;
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].HasFinishedTrack)
                playersThatFinished++;
        }
        
        return playersThatFinished >= (players.Count - 1);
    }
}
