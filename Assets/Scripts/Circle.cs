using System.Threading.Tasks;
using UnityEngine;

public class Circle : Shape, IMovable
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField, Range(3f, 15f), Tooltip("In tiles per second.")] private float movementSpeed = 5f;

    public Color Color
    {
        get => color;
        set
        {
            color = value;
            spriteRenderer.color = color;
        }
    }

    public CircleFactory Factory
    {
        get => factory;
        set
        {
            if (factory == null)
            {
                factory = value;
                return;
            }
            Debug.LogError("Factory already has been assigned. You shouldn't change it.");
        }
    }

    private CircleFactory factory;
    private Color color;
    private AStarPathfinding aStarPathfinding;
    private bool selected;

    private void Awake()
    {
        aStarPathfinding = new AStarPathfinding();
    }

    public override void Select()
    {
        Debug.Log("Circle selected");
        Color = color - new Color(0.35f, 0.35f, 0.35f, 0f);
    }

    public override void DeSelect()
    {
        Debug.Log("Circle deselected");
        Color = color + new Color(0.35f, 0.35f, 0.35f, 0f);
    }

    public override void Remove()
    {
        factory.Remove(this);
    }
    
    public override bool MatchColor(Color color)
    {
        return this.color == color;
    }

    private async Task MoveAlong(Tile[] tiles)
    {
        tiles[0].Shape = null;
        tiles[tiles.Length - 1].Shape = this;
        Tile = tiles[tiles.Length - 1];
        
        for (int i = 0; i < tiles.Length - 1; i++)
            await MoveBetween(tiles[i], tiles[i + 1]);
    }

    private async Task MoveBetween(Tile tile1, Tile tile2) // assumes tile1 and tile2 are neighbours
    {
        float t = 0;
        while (t <= 1)
        {
            var pos = Vector3.Lerp(tile1.transform.position, tile2.transform.position, t);
            transform.position = pos;
            t += movementSpeed * Time.deltaTime;
            await Task.Yield();
        }

        transform.position = tile2.transform.position;

    }

    public async Task<bool> TryMoveTo(Tile endTile, GameGrid grid) // return false if move is not possible, else true
    {
        Debug.Log($"Move {Tile} -> {endTile}");
        var nodePath = aStarPathfinding.GetPath(Tile, endTile);
        if (nodePath == null)
            return false;
        
        var tilePath = new Tile[nodePath.Count];
        for (var i = 0; i < nodePath.Count; i++)
            tilePath[i] = grid.GetTile(nodePath[i].X, nodePath[i].Y);
        
        await MoveAlong(tilePath);
        return true;
    }
}
