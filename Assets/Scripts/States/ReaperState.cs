using UnityEngine;

public class ReaperState : IGameState
{
    private readonly GameManager gameManager;
    
    private int targetsToKill;
    
    public ReaperState(GameManager gameManager, int targetsToKill)
    {
        this.gameManager = gameManager;
        this.targetsToKill = targetsToKill;
    }
    
    public IGameState Enter()
    {
        gameManager.ResetReaperTargets();
        return this;
    }

    public IGameState Update()
    {
        var currentTile = gameManager.Grid.GetTile(Input.mousePosition);
        if (currentTile == null)
            return this;

        if (!Input.GetMouseButtonDown(0)) 
            return this;

        if (currentTile.Shape == null)
            return this;

        if (currentTile.Shape is Circle)
        {
            currentTile.Shape.Die(gameManager);
            targetsToKill -= 1;
            if(targetsToKill <= 0)
                return new PlayerTurnState(gameManager);
        }

        return this;
    }

    public void Exit()
    {
    }
}
