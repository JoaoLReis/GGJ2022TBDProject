using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InFlightState : GameState
{
    public override void Setup(GameStateMachine gameStateMachine)
    {
        base.Setup(gameStateMachine);
        totalDuration = 12;

        ApplyGravityFromPlanets.PlayerOrbit += OnPlayerOrbit;
        ApplyGravityFromPlanets.PlayerCrash += OnPlayerRespawn;
        PlayerController.PlayerFinished += OnPlayerFinished;
        PlayerController.PlayerRespawn += OnPlayerRespawn;
    }

    private void OnPlayerOrbit()
    {
        gameStateMachine.SetState<InOrbitState>();
    }

    private void OnPlayerFinished()
	{
        TriggerEndState();
    }
    private void OnPlayerRespawn()
    {
        gameStateMachine.SetState<PreGameState>();
    }


    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("In Flight State");
    }

    protected override void TriggerEndState()
    {
        gameStateMachine.SetState<GameFinishState>();
    }
}
