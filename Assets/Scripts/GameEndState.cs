using UnityEngine;

public class GameEndState : IGameState
{
    private readonly GameManager gameManager;
    
    public GameEndState(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }
    
    public IGameState Enter()
    {
        gameManager.GameOverPanel.SetActive(true);
        return this;
    }

    public IGameState Update()
    {
        // u can exit using button or any of these keys
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Escape))
            return new GameStartState(gameManager);
        return this;
    }

    public void Exit()
    {
        gameManager.GameOverPanel.SetActive(false);
    }
}
