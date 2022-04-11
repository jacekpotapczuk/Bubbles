using UnityEngine;

public class GameStartState : IGameState
{
    private readonly GameManager gameManager;

    public GameStartState(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }
    
    public IGameState Enter()
    {
        gameManager.Grid.CleanUpTiles();
        gameManager.Score = 0;
        gameManager.UpdateScore();
        gameManager.MoveCircleIndicators();
        return new PlayerTurnState(gameManager);
    }

    public IGameState Update()
    {
        return new PlayerTurnState(gameManager);;
    }

    public void Exit()
    {
    }
}
