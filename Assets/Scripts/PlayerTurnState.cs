using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurnState : GameState
{
    public override void Setup(GameStateMachine gameStateMachine)
    {
        base.Setup(gameStateMachine);
        totalDuration = 20;

        PlayerController.PlayerShoot += OnPlayerShoot;
    }

    private void OnPlayerShoot()
    {
        GameManager.Instance.RunCallbackWhenAllPlayerStopMoving(() => TriggerEndState());
    }

    public override void OnEnter()
    {
        base.OnEnter();
        StartPlayerTurn();
    }

    protected override void TriggerEndState()
    {
        gameStateMachine.SetState<TurnEndState>();
    }

    private void StartPlayerTurn()
    {
        GameManager gameManager = GameManager.Instance;
        List<PlayerController> players = gameManager.players;
        gameManager.CurrentPlayerIndex = GetNextPlayerIndex();
        PlayerController currentPlayer = players[gameManager.CurrentPlayerIndex];
        for (int i = 0; currentPlayer.HasFinishedTrack && i <= players.Count; i++) 
        {
            gameManager.CurrentPlayerIndex = GetNextPlayerIndex();
            currentPlayer = players[gameManager.CurrentPlayerIndex];
        }
        
        Debug.Log($"Player {gameManager.CurrentPlayerIndex} Turn Start");
        
        gameManager.Camera.SetTarget(currentPlayer.gameObject);
        currentPlayer.StartTurn(totalDuration);
    }

    private int GetNextPlayerIndex()
    {
        GameManager gameManager = GameManager.Instance;
        List<PlayerController> players = gameManager.players;
        return ++gameManager.CurrentPlayerIndex % players.Count;
    }

    public override void OnLeave()
    {
        base.OnLeave();
        GameManager.Instance.CurrentPlayer.EndTurn();
    }
}
