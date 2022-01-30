using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInitialTurnState : GameState
{
    public override void Setup(GameStateMachine gameStateMachine)
    {
        base.Setup(gameStateMachine);
        totalDuration = 0;

        PlayerController.PlayerShoot += OnPlayerShoot;
    }

    private void OnPlayerShoot()
    {
        TriggerEndState();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        StartPlayerTurn();
    }

    protected override void TriggerEndState()
    {
        gameStateMachine.SetState<InFlightState>();
    }

    private void StartPlayerTurn()
    {
        GameManager gameManager = GameManager.Instance;
        PlayerController currentPlayer = gameManager.player;
        
        Debug.Log("Turn Start");
        
        gameManager.Camera.SetTarget(currentPlayer.gameObject);
        currentPlayer.StartTurn();
    }


    public override void OnLeave()
    {
        base.OnLeave();
        GameManager.Instance.player.EndTurn();
    }
}
