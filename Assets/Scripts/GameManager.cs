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
        currentGameState = new GameStartState();
        var gameState  = currentGameState.Enter(this);
        if (gameState != currentGameState)
            currentGameState = gameState.Enter(this);
    }
    

    private void Update()
    {
        var gameState = currentGameState.Update(this);
        if (gameState != currentGameState)
            currentGameState = gameState.Enter(this);
        
    }
}
