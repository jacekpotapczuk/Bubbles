using UnityEngine;

public class TestTurnState : IGameState
{
    private Shape selectedShape;

    public IGameState Enter(GameManager gameManager)
    {
        gameManager.Grid.TestWalkable();
        return this;
    }

    public IGameState Update(GameManager gameManager)
    {
        var currentTile = gameManager.Grid.GetTile(Input.mousePosition);
        if (currentTile == null)
            return this;

        gameManager.Grid.TestWalkable();
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButtonDown(0))
        {
            Debug.Log("Spawn");
            gameManager.CircleFactory.SpawnAt(currentTile);
            return this;
        }
        else if (Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButtonDown(1))
        {
            gameManager.CircleFactory.RemoveAt(currentTile);
            return this;
        }

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
                movable.TryMoveTo(currentTile, gameManager.Grid);
            }
        }

        return this;
    }
}