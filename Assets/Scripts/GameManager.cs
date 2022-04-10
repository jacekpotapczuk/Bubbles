using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [field: SerializeField, Range(3, 10)] public int InitialBubblesCount { get; private set; } = 3;
    [field: SerializeField, Range(3, 10)] public int BubblesToSpawnPerTurn { get; private set; } = 3;
    
    [field: SerializeField]  public GameGrid Grid { get; private set; }
    [field: SerializeField] public CircleFactory CircleFactory {get; private set;}
    
    private IGameState currentGameState;

    private void Start()
    {
        currentGameState = new GameStartState(this);
        var gameState  = currentGameState.Enter();
        if (gameState != currentGameState)
            currentGameState = gameState.Enter();
    }
    

    private void Update()
    {
        var gameState = currentGameState.Update();
        if (gameState != currentGameState)
            currentGameState = gameState.Enter();

        // temporary, for easy testing
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentGameState = new TestTurnState(this);
            currentGameState.Enter();
        }   
    }
}
