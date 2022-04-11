using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField, Range(4, 18)] private int gridSize = 9;
    [SerializeField, Range(0f, 1f)] private float margin = 0.1f;

    [Header("References")] 
    [SerializeField] private Tile tilePrefab;


    public Tile[,] Tiles { get; private set; }
    private float sideLength;
    private Camera mainCamera;

    private void Awake()
    {
        sideLength = CalculateSideLength();
        mainCamera = Camera.main;
        InitializeGrid();
    }
    
    public Tile GetTile(Vector3 screenPosition)
    {
        var worldPos = mainCamera.ScreenToWorldPoint(screenPosition);
        var localPos = transform.InverseTransformPoint(worldPos);

        var tileSize = sideLength / gridSize;
        var x = Mathf.FloorToInt(localPos.x / tileSize);
        var y = Mathf.FloorToInt(localPos.y / tileSize);

        //Debug.Log($"GetTile, localPos:{localPos} => ({x}, {y})");
        return GetTile(x, y);
    }

    public Tile GetTile(int x, int y)
    {
        if (x >= 0 && x < gridSize && y >= 0 && y < gridSize)
            return Tiles[x, y];
        return null;
    }
    
    public List<Tile> GetEmptyTiles()
    {
        var emptyTiles = new List<Tile>();
        
        for (var y = 0; y < gridSize; y++)
        {
            for (var x = 0; x < gridSize; x++)
            {
                if (Tiles[x, y].IsWalkable)
                    emptyTiles.Add(Tiles[x, y]);
            }
        }

        return emptyTiles;
    }

    public void CleanUpTiles()
    {
        for (var y = 0; y < gridSize; y++)
        {
            for (var x = 0; x < gridSize; x++)
            {
                Tiles[x, y].CleanUp();
            }
        }
    }

    // dirX, dirY should always be  one of these: -1, 0, 1
    // for example if u want to get north direction use dirX = 0, dirY = 1
    public List<Shape> GetMatchingShapesInDirection(Shape originShape, int dirX, int dirY)
    {
        var startX = originShape.Tile.X;
        var startY = originShape.Tile.Y;

        Shape next;
        int x;
        int y;
        (next, x, y) = MatchNext(startX, startY, dirX, dirY, originShape.Color);

        var shapes = new List<Shape>();
        while (next != null)
        {
            shapes.Add(next);
            (next, x, y) = MatchNext(x, y, dirX, dirY, originShape.Color);
        }

        return shapes;
    }

    private (Shape, int, int) MatchNext(int x, int y, int dirX, int dirY, Color? color)
    {
        var newX = x + dirX;
        var newY = y + dirY;
        var tile = GetTile(newX, newY);
        if (tile == null)
            return (null, -1, -1);
        if(tile.Shape == null)
            return (null, -1, -1);
        if(!tile.Shape.MatchColor(color))
            return (null, -1, -1);
        return (tile.Shape, newX, newY);
    }
    private void InitializeGrid()
    {
        // adjust position so we stay nicely centered regardless of size
        var pos = transform.position;
        pos.x = -sideLength / 2f;
        transform.position = pos;


        // instantiate tiles
        var tileSize = sideLength / gridSize;
        Tiles = new Tile[gridSize, gridSize];
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
                    south = Tiles[x, y - 1];
                if (x > 0)
                    west = Tiles[x - 1, y];
                if (y < gridSize - 1)
                    north = Tiles[x, y + 1];
                if (x < gridSize - 1)
                    east = Tiles[x + 1, y];
                
                Tiles[x, y].Initialize(x, y, north, south, west, east);
            }
        }
    }

    private void InstantiateTile(int x, int y, float tileSize)
    {
        var tile = Instantiate(tilePrefab, transform, true);
        Tiles[x, y] = tile;
        
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
    
    
    [ContextMenu("Test walkable")]
    public void TestWalkable()
    {
        for (var y = 0; y < gridSize; y++)
        {
            for (var x = 0; x < gridSize; x++)
            {
                Tiles[x, y].GetComponentInChildren<SpriteRenderer>().color = Tiles[x, y].IsWalkable ? Color.green : Color.red;
            }
        }
    }
}