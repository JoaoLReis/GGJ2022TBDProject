using UnityEngine;

public class GameFinishState : GameState
{
    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Game Finished");
        GameManager.Instance.ShowGameResults();
    }
    
    public override void OnUpdate()
    {
        //Override so it does not end after X seconds
    }

    protected override void TriggerEndState()
    {
        //No state after this
    }
}
