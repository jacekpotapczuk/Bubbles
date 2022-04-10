using UnityEngine;

public class GameStartState : IGameState
{
    private GameManager gameManager;

    public GameStartState(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }
    
    public IGameState Enter()
    {
        Debug.Log("Game Start State Enter");
        gameManager.Grid.CleanUpTiles();
        // reset score
        return new PlayerTurnState(gameManager);
    }

    public IGameState Update()
    {
        return new PlayerTurnState(gameManager);;
    }
}
