using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerTurnState : IGameState
{
    // simple state machine inside state machine
    // this one is much simpler and i don't expect this one to change that much
    // so we simply use enum and stay inside one class
    private enum TurnAction
    {
        Idle,
        Moving,
        Moved
    }
    
    private Shape selectedShape;
    private Shape tryingToMoveShape;
    private TurnAction turnAction;
    private readonly GameManager gameManager;
    public PlayerTurnState(GameManager gameManager)
    {
        this.gameManager = gameManager;
        turnAction = TurnAction.Idle;
    }
    
    public IGameState Enter()
    {
        foreach (var circleIndicator in gameManager.CircleIndicators)
        {
            if(circleIndicator.Tile.Shape == null)
                gameManager.CircleFactory.SpawnAt(circleIndicator.Tile, circleIndicator.Color);
        }
        
        // do the point check after all shapes has been spawned
        foreach (var circleIndicator in gameManager.CircleIndicators)
        {
            if(circleIndicator.Tile.Shape != null) // could be null when tile before removed this shapes after scoring 
                CheckForPoints(circleIndicator.Tile.Shape);
        }
        gameManager.MoveCircleIndicators();
        
        return this;
    }

    public IGameState Update()
    {
        return HandleInput();
    }

    public void Exit()
    {
    }

    private IGameState HandleInput()
    {
        switch (turnAction)
        {
            case TurnAction.Moving:
                return this;
            case TurnAction.Moved:
                CheckForPoints(tryingToMoveShape); // here we know tryingToMoveShape actually moved
                return new PlayerTurnState(gameManager);
            case TurnAction.Idle:
                return HandleIdle();
            default:
                Debug.LogError("Turn state not supported in HandeInput()");
                return this;
        }
    }

    private IGameState HandleIdle()
    {
        var currentTile = gameManager.Grid.GetTile(Input.mousePosition);
        if (currentTile == null)
            return this;

        if (!Input.GetMouseButtonDown(0)) 
            return this;
        
        if (currentTile.Shape == null && selectedShape is IMovable movable)
        {
            selectedShape.DeSelect();
            tryingToMoveShape = selectedShape;
            selectedShape = null;
            TryMove(movable, gameManager, currentTile);    
        }
        else if (currentTile.Shape != null)
        {
            currentTile.Shape.Select();
            if(selectedShape != null)
                selectedShape.DeSelect();
            selectedShape = currentTile.Shape;
        }
        else if(currentTile.Shape == null && selectedShape != null)
        {
            selectedShape.DeSelect();
            selectedShape = null;
        }

        return this;
    }

    private async Task TryMove(IMovable movable, GameManager gameManager, Tile endTile)
    {
        turnAction = TurnAction.Moving;
        var moveTask = movable.TryMoveTo(endTile, gameManager.Grid);
        while(!moveTask.IsCompleted)
            await Task.Yield();
        
        turnAction = moveTask.Result ? TurnAction.Moved : TurnAction.Idle;
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
        gameManager.Score += 1;
    }
}