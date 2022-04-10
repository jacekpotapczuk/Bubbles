using UnityEngine;

public class GameStartState : IGameState
{ 
    public IGameState Enter(GameManager gameManager)
    {
        Debug.Log("Game Start State Enter");
        gameManager.Grid.CleanUpTiles();
        // reset score
        return new PlayerTurnState();
    }

    public IGameState Update(GameManager gameManager)
    {
        return new PlayerTurnState();;
    }
}
