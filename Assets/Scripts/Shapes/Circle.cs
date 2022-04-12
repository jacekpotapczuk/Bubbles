using System.Threading.Tasks;
using UnityEngine;

public class Circle : Shape, IMovable
{
    [Header("Settings")]
    [SerializeField, Range(3f, 15f), Tooltip("In tiles per second.")] private float movementSpeed = 5f;
    [SerializeField] private Color colorOffsetOnSelect;
    
    [Header("References")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private SpriteRenderer abilityMark;
    [SerializeField] private SpriteRenderer blockMark;

    public override Color? Color
    {
        get => color;
        set
        {
            color = value;
            if(value != null)
                spriteRenderer.color = color.Value;
        }
    }

    public CircleAbility Ability
    {
        get => ability;
        set
        {
            ability = value;
            abilityMark.enabled = value != null;
        }
    }
    private CircleAbility ability;
    
    public bool Blocked
    {
        get => blocked;
        private set
        {
            blocked = value;
            blockMark.enabled = value;
        }
    }

    private bool blocked;
    public ShapeFactory<Circle> Factory
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

    private ShapeFactory<Circle> factory;
    private Color? color;
    private AStarPathfinding aStarPathfinding;
    private bool triggeredDeathEffects = false;
    private int blockDuration;
    
    private void Awake()
    {
        aStarPathfinding = new AStarPathfinding();
    }

    public void BlockFor(int numberOfTurns)
    {
        blockDuration += numberOfTurns;
        if (blockDuration > 0)
            Blocked = true;
    }

    public override void OnNewTurn(GameManager gameManager)
    {
        if (!blocked)
            return;
        blockDuration -= 1;
        if (blockDuration < 0)
        {
            Blocked = false;
            blockDuration = 0;
        }
    }

    public override void Select()
    {
        Color = color - colorOffsetOnSelect;
    }

    public override void DeSelect()
    {
        Color = color + colorOffsetOnSelect;
    }

    public override void Remove()
    {
        Tile.Shape = null;
        Ability = null;
        Blocked = false;
        factory.Reclaim(this);
    }

    public override void Clear()
    {
        triggeredDeathEffects = false;
    }

    public override void Die(GameManager gameManager)
    {
        // make sure we don't die twice
        if (triggeredDeathEffects)
            return;
        
        gameManager.Score += 1;
        triggeredDeathEffects = true;
        var circleAbility = Ability;
        Remove();

        if(circleAbility != null)
            circleAbility.Do(this, gameManager);
    }

    public override bool MatchColor(Color? color)
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

    public async Task<bool> TryMoveTo(Tile endTile, GameGrid grid)
    {
        if (blocked)
            return false;
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
