using UnityEngine;

public class Imp : Shape
{
    [SerializeField, Range(2, 10)] private int dieAfterTurns = 3; 
    
    [SerializeField] private GameObject targetIndicator;

    public override Color? Color { get; set; } = null;
    
    public ShapeFactory<Imp> Factory
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
    private ShapeFactory<Imp> factory;
    
    private int turnsAlive;
    private Tile targetLocation;

    public override bool MatchColor(Color? color)
    {
        return false;
    }

    public override void Clear()
    {
        turnsAlive = 0;
        targetLocation = null;
    }

    public override void OnNewTurn(GameManager gameManager)
    {
        if (targetLocation != null)
        {
            Tile.Shape = null;
            Tile = targetLocation;
            transform.position = Tile.transform.position;
        }
        
        var emptyTiles = gameManager.Grid.GetEmptyTiles();
        targetLocation = emptyTiles[Random.Range(0, emptyTiles.Count)];
        targetLocation.Shape = this;
        targetIndicator.transform.position = targetLocation.transform.position;
        
        turnsAlive += 1;
        if(turnsAlive > dieAfterTurns)
            Remove();
    }

    public override void Remove()
    {
        Tile.Shape = null;
        if (targetLocation != null)
            targetLocation.Shape = null;
        Factory.Reclaim(this);
    }
}