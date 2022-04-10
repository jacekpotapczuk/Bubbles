
using System.Threading.Tasks;
using UnityEngine;

public class PlayerTurnState : IGameState
{
    private Circle selectedCircle;
    private Tile selectedTile;

    private bool moving = false;
    private bool moved = false;
    
    public IGameState Enter(GameManager gameManager)
    {
        Debug.Log("Player Turn state enter");
        // "grow" feature circles
        
        for (int i = 0; i < gameManager.BubblesToSpawnPerTurn; i++)
        {
            gameManager.CircleFactory.SpawnAt(gameManager.Grid.GetRandomEmptyTile());    
        }
        return this;
    }

    public IGameState Update(GameManager gameManager)
    {
        //Debug.Log("Player Turn state update");

        return HandleInput(gameManager);
      
        // handle input
        // exit when turn complete
        // check for points
        // show feature circles
    }

    private IGameState HandleInput(GameManager gameManager)
    {
        if (moving)
            return this;
        if (moved)
            return new PlayerTurnState();
        var currentTile = gameManager.Grid.GetTile(Input.mousePosition);
        if (currentTile == null)
            return this;

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Select");
            var shape = currentTile.Shape;
            if (shape == null)
                return this;
            if (shape is Circle circle)
            {
                selectedCircle = circle;
                selectedTile = currentTile;
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Move");
            if (selectedCircle == null)
                return this;

            moving = true;
            MoveAlong(gameManager, currentTile);
        }

        return this;
    }

    private async Task MoveAlong(GameManager gameManager, Tile endTile)
    {
        await selectedCircle.MoveAlong(gameManager.Grid.GetPath(selectedTile, endTile));
        moving = false;
        moved = true;
    }
}
