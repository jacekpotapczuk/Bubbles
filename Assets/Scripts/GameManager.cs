using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [field: SerializeField, Range(3, 10)] public int BubblesToSpawnPerTurn { get; private set; } = 3;
    [field: SerializeField, Range(3, 9)] public int RequiredInARow { get; private set; } = 5;
    
    [field: SerializeField]  public GameGrid Grid { get; private set; }
    [field: SerializeField] public CircleFactory CircleFactory {get; private set;}
    
    [field: SerializeField] public GameObject GameOverPanel {get; private set;}


    private IGameState currentGameState;

    public static event Action<int> OnScoreGain; 
    public int Score
    {
        get => score;
        set
        {
            score = value;
            OnScoreGain?.Invoke(score);
        }
    }

    private int score = 0;
    private void Start()
    {
        RestartGame();
    }

    private void Update()
    {
        var newState = currentGameState.Update();
        SwitchState(currentGameState, newState);

        // temporary, for easy testing
        if (Input.GetKeyDown(KeyCode.P))
        {
            currentGameState = new TestTurnState(this);
            currentGameState.Enter();
        }   
    }
    
    public void RestartGame()
    {
        SwitchState(currentGameState, new GameStartState(this));
    }

    private void SwitchState(IGameState prev, IGameState newState)
    {
        if (prev == newState)
            return;
        if (newState == null)
        {
            Debug.LogError("Not possible to change state to null");
            return;
        }
        currentGameState = newState;
        prev?.Exit();
        // I kinda don't like this, with more time I would consider some changes to pattern
        // this makes it too easy to make accidentally infinite loop in more complex situation
        var newNewState = newState.Enter(); 
        SwitchState(newState, newNewState);
    }
}
