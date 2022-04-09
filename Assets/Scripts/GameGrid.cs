using UnityEngine;

public class GameGrid : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField, Range(4, 18)] private int gridSize = 9;
    [SerializeField, Range(0f, 1f)] private float margin = 0.1f;

    [Header("References")] 
    [SerializeField] private Tile tilePrefab;

    private Tile[,] tiles;
    private float sideLength;
    private void Awake()
    {
        sideLength = CalculateSideLength();
    }

    private void Start()
    {
        InitializeGrid();
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
        var camera = Camera.main;
        var ortoSize = camera.orthographicSize;
        var aspect = camera.aspect;
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