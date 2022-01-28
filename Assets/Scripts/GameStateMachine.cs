using System;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMachine : MonoBehaviour
{
    private GameState currentState;
    
    private List<GameState> gameStates = new List<GameState>();
    
    private void Start()
    {
        RegisterStates();
    }

    private void RegisterStates()
    {
        gameStates.Add(new PreGameState());
        gameStates.Add(new PlayerTurnState());
        gameStates.Add(new TurnEndState());
        gameStates.Add(new GameFinishState());

        for (int i = 0; i < gameStates.Count; i++)
        {
            gameStates[i].Setup(this);
        }
        
        SetState<PreGameState>();
    }

    public void SetState<T>() where T : GameState, new()
    {
        currentState?.OnLeave();
        GameState newState = GetState<T>();
        newState.OnEnter();
        currentState = newState;
    }

    private GameState GetState<T>() where T : GameState
    {
        Type type = typeof(T);
        for (int i = 0; i < gameStates.Count; i++)
        {
            GameState gameState = gameStates[i];
            if (gameState.GetType() == type)
                return gameState;
        }
        return null;
    }

    private void Update()
    {
        currentState.OnUpdate();
    }
}
