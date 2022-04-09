using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField, Range(4, 18)] private int gridSize = 9;
    [SerializeField, Range(0f, 1f)] private float margin = 0.1f;

    [Header("References")] 
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private GameObject circlePrefab;


    private Tile[,] tiles;
    private float sideLength;
    private Camera mainCamera;
    private AStarPathfinding pathfinding;
    
    private void Awake()
    {
        sideLength = CalculateSideLength();
        mainCamera = Camera.main;
    }

    private void Start()
    {
        InitializeGrid();
        pathfinding = new AStarPathfinding();
    }
    
    public Tile GetTile(Vector3 screenPosition)
    {
        var worldPos = mainCamera.ScreenToWorldPoint(screenPosition);
        var localPos = transform.InverseTransformPoint(worldPos);

        var tileSize = sideLength / gridSize;
        var x = Mathf.FloorToInt(localPos.x / tileSize);
        var y = Mathf.FloorToInt(localPos.y / tileSize);

        Debug.Log($"GetTile, localPos:{localPos} => ({x}, {y})");
        return GetTile(x, y);
    }

    public Tile GetTile(int x, int y)
    {
        if (x >= 0 && x < gridSize && y >= 0 && y < gridSize)
            return tiles[x, y];
        return null;
    }

    public Tile[] GetPath(Tile startTile, Tile endTile)
    {
        var nodePath =pathfinding.GetPath(startTile, endTile);
        if (nodePath == null)
            return null;
        var tilePath = new Tile[nodePath.Count];
        for (var i = 0; i < nodePath.Count; i++)
            tilePath[i] = GetTile(nodePath[i].X, nodePath[i].Y);

        return tilePath;
    }

    public void SpawnAt(Tile tile)
    {
        var circle = Instantiate(circlePrefab, transform, true);
        circle.transform.localScale = tile.transform.localScale;
        circle.transform.localPosition = tile.transform.localPosition;
        tile.IsWalkable = false;
    }

    private void InitializeGrid()
    {
        // adjust position so we stay nicely centered regardless of size
        var pos = transform.position;
        pos.x = -sideLength / 2f;
        transform.position = pos;


        // instantiate tiles
        var tileSize = sideLength / gridSize;
        tiles = new Tile[gridSize, gridSize];
        for (var y = 0; y < gridSize; y++)
        {
            for (var x = 0; x < gridSize; x++)
            {
                InstantiateTile(x, y, tileSize);
            }
        }
        
        // when whole grid is set up, we can easily set up references 
        for (var y = 0; y < gridSize; y++)
        {
            for (var x = 0; x < gridSize; x++)
            {
                Tile north = null;
                Tile south = null;
                Tile west = null;
                Tile east = null;
                if (y > 0)
                    south = tiles[x, y - 1];
                if (x > 0)
                    west = tiles[x - 1, y];
                if (y < gridSize - 1)
                    north = tiles[x, y + 1];
                if (x < gridSize - 1)
                    east = tiles[x + 1, y];
                
                tiles[x, y].Initialize(x, y, north, south, west, east);
            }
        }
    }

    private void InstantiateTile(int x, int y, float tileSize)
    {
        var tile = Instantiate(tilePrefab, transform, true);
        tiles[x, y] = tile;
        
        tile.transform.localPosition = new Vector3(x * tileSize, y * tileSize, 0f);
        tile.transform.localScale = new Vector3(tileSize, tileSize, 1f);
    }

    private float CalculateSideLength()
    {
        var cam = mainCamera == null ? Camera.main : mainCamera;
        var ortoSize = cam.orthographicSize;
        var aspect = cam.aspect;
        var sideLen = ortoSize * 2 * aspect - 2 * margin;
        return sideLen;
    }
    
    // draw boundaries of grid
    private void OnDrawGizmos()
    {
        var side = Application.isPlaying ? sideLength : CalculateSideLength();
            
        Gizmos.color = Color.yellow;
        var pos = new Vector3(0f, transform.position.y + side / 2f, 0f);
        Gizmos.DrawWireCube(pos, Vector3.one * side);
        Gizmos.color = Color.white;
    }
    
}