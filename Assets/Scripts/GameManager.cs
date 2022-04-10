using System;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [field: SerializeField, Range(3, 10)] public int BubblesToSpawnPerTurn { get; private set; } = 3;
    [field: SerializeField, Range(3, 9)] public int RequiredInARow { get; private set; } = 5;
    
    [field: SerializeField]  public GameGrid Grid { get; private set; }
    [field: SerializeField] public CircleFactory CircleFactory {get; private set;}
    [SerializeField] public TMP_Text scoreText;
    
    private IGameState currentGameState;
    private int score = 0;
    private void Start()
    {
        currentGameState = new GameStartState(this);
        var gameState  = currentGameState.Enter();
        if (gameState != currentGameState)
            currentGameState = gameState.Enter();
    }

    private void Update()
    {
        var gameState = currentGameState.Update();
        if (gameState != currentGameState)
            currentGameState = gameState.Enter();

        // temporary, for easy testing
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentGameState = new TestTurnState(this);
            currentGameState.Enter();
        }   
    }

    public void AddOneToScore()
    {
        score += 1;
        scoreText.text = score.ToString();
    }

    public void ResetScore()
    {
        score = 0;
        scoreText.text = score.ToString();
    }
}
