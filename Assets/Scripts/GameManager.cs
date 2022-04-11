using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{

    [field: SerializeField, Range(3, 10)] public int BubblesToSpawnPerTurn { get; private set; } = 3;
    [field: SerializeField, Range(3, 9)] public int RequiredInARow { get; private set; } = 5;
    
    [SerializeField] private Color[] availableColors; 
    
    [SerializeField] private BonusColor[] bonusColors;
    
    [field: SerializeField]  public GameGrid Grid { get; private set; }
    [field: SerializeField] public CircleFactory CircleFactory {get; private set;}
    
    [field: SerializeField] public GameObject GameOverPanel {get; private set;}

    [SerializeField] private CircleIndicator circleIndicatorPrefab;

    private List<Color> colors;
    private List<BonusColor> colorsToAdd;

    public CircleIndicator[] CircleIndicators { get; private set; }

    private IGameState currentGameState;

    public static event Action<int> OnScoreGain; 
    public int Score
    {
        get => score;
        set
        {
            score = value;
            scoreDirty = true;
        }
    }

    // small optimization to avoid checking for new colors and firing event every time score is increased by one
    // instead we do it later once
    private bool scoreDirty; 
    private int score = 0;
    
    private void Start()
    {
        CircleIndicators = new CircleIndicator[BubblesToSpawnPerTurn];
        var tile = Grid.GetTile(0, 0);
        for (int i = 0; i < BubblesToSpawnPerTurn; i++)
        {
            CircleIndicators[i] = Instantiate(circleIndicatorPrefab, transform);
            CircleIndicators[i].transform.localScale = tile.transform.localScale;
        }
        RestartGame();
    }

    public void MoveCircleIndicators()
    {
        var emptyTiles = Grid.GetEmptyTiles();

        if (emptyTiles.Count <= BubblesToSpawnPerTurn)
        {
            // game over, but first spawn circle to fill screen before ending the game
            foreach(var tile in emptyTiles)
                CircleFactory.SpawnAt(tile, GetRandomColor());
            SwitchState(currentGameState, new GameEndState(this));
            return;
        }
        var usedTiles = new List<Tile>();
        for (int i = 0; i < BubblesToSpawnPerTurn; i++)
        {
            var tile = emptyTiles[Random.Range(0, emptyTiles.Count)];
            while(usedTiles.Contains(tile))
                tile = emptyTiles[Random.Range(0, emptyTiles.Count)];
            
            emptyTiles.Remove(tile);
            usedTiles.Add(tile);
            
            CircleIndicators[i].Tile = tile;
            CircleIndicators[i].Color = GetRandomColor();
            CircleIndicators[i].transform.position = tile.transform.position;
        }
    }

    private void Update()
    {
        var newState = currentGameState.Update();
        if (scoreDirty)
            UpdateScore();
        SwitchState(currentGameState, newState);

        // temporary, for easy testing
        if (Input.GetKeyDown(KeyCode.P))
        {
            currentGameState = new TestTurnState(this);
            currentGameState.Enter();
        }
    }

    private void UpdateScore()
    {
        scoreDirty = false;
        var toRemove = new List<BonusColor>();
        foreach (var bonusColor in colorsToAdd)
        {
            if (bonusColor.scoreRequired <= score)
            {
                colors.Add(bonusColor.color);
                toRemove.Add(bonusColor);
                Debug.Log("Add color");
            }
        }
        foreach (var bs in toRemove)
            colorsToAdd.Remove(bs);
            
        OnScoreGain?.Invoke(score);
    }
    public Color GetRandomColor()
    {
        return colors[Random.Range(0, colors.Count)];
    }

    public void RestartGame()
    {
        colors = new List<Color>(availableColors);
        colorsToAdd = new List<BonusColor>(bonusColors);
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
