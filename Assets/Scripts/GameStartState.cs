using UnityEngine;

public class GameStartState : IGameState
{ 
    public IGameState Enter(GameManager gameManager)
    {
        Debug.Log("Game Start State Enter");
        for (int i = 0; i < gameManager.InitialBubblesCount; i++)
        {
            gameManager.CircleFactory.SpawnAt(gameManager.Grid.GetRandomEmptyTile());    
        }
        // show feature circles
        return new PlayerTurnState();
    }

    public IGameState Update(GameManager gameManager)
    {
        return this;
    }
}
