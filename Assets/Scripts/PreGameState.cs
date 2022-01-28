using UnityEngine;

public class PreGameState : GameState
{
    public override void Setup(GameStateMachine gameStateMachine)
    {
        base.Setup(gameStateMachine);
        totalDuration = 0.1f;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Preparing Game");
    }

    protected override void TriggerEndState()
    {
        gameStateMachine.SetState<PlayerTurnState>();
    }

    public override void OnLeave()
    {
        base.OnLeave();
        Debug.Log("Game is Starting");
    }
}
