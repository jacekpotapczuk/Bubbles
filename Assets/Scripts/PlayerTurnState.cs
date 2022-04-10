using System.Threading.Tasks;
using UnityEngine;

public class PlayerTurnState : IGameState
{
    private Shape selectedShape;
    private Shape lastTryingToMoveShape; // shape that last moved (or at least tried to), used for score check

    private bool moving = false;
    private bool moved = false;

    private GameManager gameManager;
    public PlayerTurnState(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }
    
    public IGameState Enter()
    {
        Debug.Log("Player Turn state enter");
        // "grow" feature circles
        
        for (int i = 0; i < gameManager.BubblesToSpawnPerTurn; i++)
        {
            gameManager.CircleFactory.SpawnAt(gameManager.Grid.GetRandomEmptyTile());    
        }
        return this;
    }

    public IGameState Update()
    {
        //Debug.Log("Player Turn state update");

        return HandleInput();
      
        // handle input
        // exit when turn complete
        // check for points
        // show feature circles
    }

    private IGameState HandleInput()
    {
        if (moving)
            return this;
        if (moved)
        {
            CheckForPoints(gameManager);
            return new PlayerTurnState(gameManager);
        }
            
        var currentTile = gameManager.Grid.GetTile(Input.mousePosition);
        if (currentTile == null)
            return this;
        
        if (Input.GetMouseButtonDown(0))
        {
            if (currentTile.Shape != null)
            {
                currentTile.Shape.Select();
                
                if(selectedShape != null)
                    selectedShape.DeSelect();
                selectedShape = currentTile.Shape;

            }
            else
            {
                if (selectedShape != null)
                {
                    selectedShape.DeSelect();
                    selectedShape = null;
                }
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            if (selectedShape == null)
                return this;

            if (selectedShape is IMovable movable)
            {
                selectedShape.DeSelect();
                selectedShape = null;
                lastTryingToMoveShape = selectedShape;
                TryMove(movable, gameManager, currentTile);    
            }
        }

        return this;
    }

    private async Task TryMove(IMovable movable, GameManager gameManager, Tile endTile)
    {
        moving = true;
        var moveTask = movable.TryMoveTo(endTile, gameManager.Grid);
        while(!moveTask.IsCompleted)
            await Task.Yield();
        
        moving = false;
        moved = moveTask.Result;
    }

    private void CheckForPoints(GameManager gameManager)
    {
            
    }
}
