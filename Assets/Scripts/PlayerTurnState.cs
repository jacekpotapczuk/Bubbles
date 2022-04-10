using System.Collections.Generic;
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
            var tile = gameManager.Grid.GetRandomEmptyTile();
            gameManager.CircleFactory.SpawnAt(tile);
            CheckForPoints(tile.Shape);
        }
        return this;
    }

    public IGameState Update()
    {
        //Debug.Log("Player Turn state update");

        return HandleInput();
      
        // handle input
        // exit when turn complete
        // check for points5
        // show feature circles
    }

    private IGameState HandleInput()
    {
        if (moving)
            return this;
        if (moved)
        {
            CheckForPoints(lastTryingToMoveShape);
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
                lastTryingToMoveShape = selectedShape;
                selectedShape = null;
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

    private void CheckForPoints(Shape originShape)
    {
        var west = gameManager.Grid.GetMatchingShapesInDirection(originShape, -1, 0);
        var east = gameManager.Grid.GetMatchingShapesInDirection(originShape, 1, 0);
        var north = gameManager.Grid.GetMatchingShapesInDirection(originShape, 0, 1);
        var south = gameManager.Grid.GetMatchingShapesInDirection(originShape, 0, -1);
        var northWest = gameManager.Grid.GetMatchingShapesInDirection(originShape, -1, 1);
        var northEast = gameManager.Grid.GetMatchingShapesInDirection(originShape, 1, 1);
        var southWest = gameManager.Grid.GetMatchingShapesInDirection(originShape, -1, -1);
        var southEast = gameManager.Grid.GetMatchingShapesInDirection(originShape, 1, -1);
        
        bool removeCurrent = false;

        removeCurrent |= CheckWholeDirection(west, east);           // horizontal
        removeCurrent |= CheckWholeDirection(north, south);         // vertical
        removeCurrent |= CheckWholeDirection(northWest, southEast); // first diagonal
        removeCurrent |= CheckWholeDirection(northEast, southWest); // second diagonal
        
        if (removeCurrent)
            RemoveShapesAndAssignPoints(originShape);
    }

    // return true if points are scored
    private bool CheckWholeDirection(List<Shape> dirPart1, List<Shape> dirPart2)
    {
        var count = dirPart1.Count + dirPart2.Count + 1; // add one, because we don't have mid tile in lists
        if (count >= gameManager.RequiredInARow)
        {
            RemoveShapesAndAssignPoints(dirPart1);
            RemoveShapesAndAssignPoints(dirPart2);
            return true;
        }

        return false;
    }
    

    private void RemoveShapesAndAssignPoints(List<Shape> shapes)
    {
        foreach (var shape in shapes)
            RemoveShapesAndAssignPoints(shape);
    }
    
    private void RemoveShapesAndAssignPoints(Shape shape)
    {
        shape.Remove();
        gameManager.AddOneToScore();
    }
    

    private bool MatchColor(Tile tile, Color color)
    {
        return tile != null && tile.Shape != null && tile.Shape.MatchColor(color);
    }
    
    
}
