using UnityEngine;

public abstract class GameState
{
    protected GameStateMachine gameStateMachine;
    
    protected float totalDuration = 1;
    private float currentDuration;
    
    public virtual void Setup(GameStateMachine gameStateMachine)
    {
        this.gameStateMachine = gameStateMachine;
    }

    public virtual void OnEnter()
    {
        currentDuration = 0;
    }

    public virtual void OnUpdate()
    {
        if (totalDuration <= 0) return;

        currentDuration += Time.deltaTime;

        if (currentDuration >= totalDuration)
        {
            Debug.Log("Timed out!");
            TriggerEndState();
        }
    }

    protected abstract void TriggerEndState();

    public virtual void OnLeave() { }
}
