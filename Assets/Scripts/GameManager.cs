using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField, Range(3, 10)] private int bubblesToSpawnPerTurn = 3;
    [SerializeField, Range(3, 9)] private int requiredInARow = 5;
    [SerializeField] private CircleSpawnSetting[] availableColors;

    [Header("References")]
    [SerializeField] private CircleIndicator circleIndicatorPrefab;
    
    [field: SerializeField]  public GameGrid Grid { get; private set; }
    [field: SerializeField] public GameObject GameOverPanel {get; private set;}
    [field: SerializeField] public Spawner Spawner {get; private set;}
    
    public CircleIndicator[] CircleIndicators { get; private set; }
    
    public int ReaperTargets { get; private set; } 
    
    public int Score
    {
        get => score;
        set
        {
            score = value;
            scoreDirty = true;
        }
    }
    
    public static event Action<int> OnScoreGain; 
    
    private List<Color> colors;
    private List<CircleSpawnSetting> colorsToAdd;
    private IGameState currentGameState;
    
    // small optimization to avoid checking for new colors and firing event every time score is increased by one
    // instead we do it later once
    private bool scoreDirty; 
    private int score = 0;
    
    private void Start()
    {
        CircleIndicators = new CircleIndicator[bubblesToSpawnPerTurn];
        var tile = Grid.GetTile(0, 0);
        for (int i = 0; i < bubblesToSpawnPerTurn; i++)
        {
            CircleIndicators[i] = Instantiate(circleIndicatorPrefab, transform);
            CircleIndicators[i].transform.localScale = tile.transform.localScale;
        }
        RestartGame();
    }
    
    private void Update()
    {
        var newState = currentGameState.Update();
        if (scoreDirty)
            UpdateScore();
        SwitchState(currentGameState, newState);
    }
    
    public void MoveCircleIndicators()
    {
        var emptyTiles = Grid.GetEmptyTiles();

        if (emptyTiles.Count <= bubblesToSpawnPerTurn)
        {
            // game over, but first spawn circle to fill screen before ending the game
            foreach (var tile in emptyTiles)
                Spawner.SpawnCircle(tile);
            SwitchState(currentGameState, new GameEndState(this));
            return;
        }
        var usedTiles = new List<Tile>();
        for (int i = 0; i < bubblesToSpawnPerTurn; i++)
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

    public void UpdateScore()
    {
        scoreDirty = false;
        var toRemove = new List<CircleSpawnSetting>();
        foreach (var color in colorsToAdd)
        {
            if (color.scoreRequired <= score)
            {
                colors.Add(color.color);
                toRemove.Add(color);
            }
        }
        foreach (var gameColor in toRemove)
            colorsToAdd.Remove(gameColor);
            
        OnScoreGain?.Invoke(score);
    }
    
    public Color GetRandomColor()
    {
        return colors[Random.Range(0, colors.Count)];
    }

    public CircleSpawnSetting ColorToGameColor(Color color)
    {
        foreach (var c in availableColors)
        {
            if (c.color == color)
                return c;
        }
        Debug.LogError($"Could not find Circle Ability for color {color}");
        return new CircleSpawnSetting();
    }

    public void RestartGame()
    {
        colors = new List<Color>();
        colorsToAdd = new List<CircleSpawnSetting>(availableColors);
        SwitchState(currentGameState, new GameStartState(this));
    }
    
    public void CheckForPoints(Shape originShape)
    {
        var west = Grid.GetMatchingShapesInDirection(originShape, -1, 0);
        var east = Grid.GetMatchingShapesInDirection(originShape, 1, 0);
        var north = Grid.GetMatchingShapesInDirection(originShape, 0, 1);
        var south = Grid.GetMatchingShapesInDirection(originShape, 0, -1);
        var northWest = Grid.GetMatchingShapesInDirection(originShape, -1, 1);
        var northEast = Grid.GetMatchingShapesInDirection(originShape, 1, 1);
        var southWest = Grid.GetMatchingShapesInDirection(originShape, -1, -1);
        var southEast = Grid.GetMatchingShapesInDirection(originShape, 1, -1);
        
        bool removeCurrent = false;
        removeCurrent |= CheckWholeDirection(west, east);           // horizontal
        removeCurrent |= CheckWholeDirection(north, south);         // vertical
        removeCurrent |= CheckWholeDirection(northWest, southEast); // first diagonal
        removeCurrent |= CheckWholeDirection(northEast, southWest); // second diagonal
        
        if (removeCurrent)
            originShape.Die(this);
    }

    // I don't like this kind of code, but I'm over the time already so I want to finish ASAP
    // and also it's hard to do it better, because there is a lot of edge cases that u need to care about
    public void RequestReaperState(int targetsToKill)
    {
        ReaperTargets += targetsToKill;
    }

    public void ResetReaperTargets()
    {
        ReaperTargets = 0;
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
    
    // return true if points are scored
    private bool CheckWholeDirection(List<Shape> dirPart1, List<Shape> dirPart2)
    {
        var count = dirPart1.Count + dirPart2.Count + 1; // add one, because we don't have mid tile in lists
        if (count >= requiredInARow)
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
            shape.Die(this);
    }
}
