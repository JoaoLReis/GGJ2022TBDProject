using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InFlightState : GameState
{
    private bool alreadyEnded;

    Coroutine delayEnd;
    
    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("In Flight State");
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
        Debug.Log("Starting cor In Flight State");
        yield return new WaitForSeconds(PlayerController.TimeOutWhenFlying);
        Debug.Log("Ending cor In Flight State");
        TriggerEndState();
    }
}
