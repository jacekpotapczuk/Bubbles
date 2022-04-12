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
    private bool anyCircleCanMove = false;
    private readonly GameManager gameManager;
    
    public PlayerTurnState(GameManager gameManager)
    {
        this.gameManager = gameManager;
        turnAction = TurnAction.Idle;
    }
    
    public IGameState Enter()
    {
        foreach (var shape in gameManager.Spawner.Shapes)
        {
            if(shape.gameObject.activeSelf)
                shape.OnNewTurn(gameManager);
        }
        foreach (var circleIndicator in gameManager.CircleIndicators)
        {
            if(circleIndicator.Tile.Shape == null)
                gameManager.Spawner.SpawnCircle(circleIndicator.Tile, circleIndicator.Color);
        }

        // do the point check after all shapes has been spawned
        foreach (var circleIndicator in gameManager.CircleIndicators)
        {
            if(circleIndicator.Tile.Shape != null) // could be null when tile before removed this shapes after scoring 
                gameManager.CheckForPoints(circleIndicator.Tile.Shape);
        }
        gameManager.MoveCircleIndicators();

        return this;
    }

    public IGameState Update()
    {
        if (!AnyCircleCanMove())
        {
            Debug.Log("No moves available, move to next turn");
            return new PlayerTurnState(gameManager);
        }
        
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
                gameManager.CheckForPoints(tryingToMoveShape); // here we know tryingToMoveShape actually moved
                if(gameManager.ReaperTargets > 0)          
                    return new ReaperState(gameManager, gameManager.ReaperTargets);
                else
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
#pragma warning disable 4014
            TryMove(movable, gameManager, currentTile);  
#pragma warning restore 4014
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

    private bool AnyCircleCanMove()
    {
        if (anyCircleCanMove)
            return true;
        foreach (var shape in gameManager.Spawner.Shapes)
        {
            if (!shape.gameObject.activeSelf)
                continue;
            if (!(shape is Circle circle)) 
                continue;
            if (circle.Blocked)
                continue;
            if (circle == null)
                continue;
            if (circle.Tile.NorthNeighbour != null && circle.Tile.NorthNeighbour.Shape == null)
            {
                anyCircleCanMove = true;
                return true;
            }
            if (circle.Tile.SouthNeighbour != null && circle.Tile.SouthNeighbour.Shape == null)
            {
                anyCircleCanMove = true;
                return true;
            }
            if (circle.Tile.WestNeighbour != null && circle.Tile.WestNeighbour.Shape == null)
            {
                anyCircleCanMove = true;
                return true;
            }

            if (circle.Tile.EastNeighbour != null && circle.Tile.EastNeighbour.Shape == null)
            {
                anyCircleCanMove = true;
                return true;
            }
        }

        return false;
    }
}