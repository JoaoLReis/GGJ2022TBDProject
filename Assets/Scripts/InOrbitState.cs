using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InOrbitState : GameState
{
    private bool alreadyEnded;

    Coroutine delayEnd;
    
    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("In Orbit State");
        alreadyEnded = false;
    }

    public override void OnLeave()
    {
        base.OnLeave();
        if (delayEnd != null)
		{
            gameStateMachine.StopCoroutine(delayEnd);
            delayEnd = null;
        }
    }

    protected override void TriggerEndState()
    {
        gameStateMachine.SetState<GameFinishState>();
    }

    public override void OnUpdate()
    {
        if (!alreadyEnded && GameManager.Instance.player.IsMoving)
            delayEnd = gameStateMachine.StartCoroutine(DelayTriggerEndState());
    }

    private IEnumerator DelayTriggerEndState()
    {
        alreadyEnded = true;
        yield return new WaitForSeconds(PlayerController.TimeOutWhenFlying);
        TriggerEndState();
    }
}
