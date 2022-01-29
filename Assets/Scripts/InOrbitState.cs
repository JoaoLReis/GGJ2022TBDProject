using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InOrbitState : GameState
{
    public override void Setup(GameStateMachine gameStateMachine)
    {
        base.Setup(gameStateMachine);
        totalDuration = 0;

        //TODO add callback for when player leaves orbit
        //OrbitalController.PlayerOrbit += OnPlayerOrbit;
        PlayerController.PlayerFinished += OnPlayerFinished;
    }

    private void OnPlayerFinished()
    {
        TriggerEndState();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("In Orbit State");
    }

    protected override void TriggerEndState()
    {
        gameStateMachine.SetState<GameFinishState>();
    }
}
