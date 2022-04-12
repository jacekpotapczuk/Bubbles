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
        turnsAlive += 1;
        if (targetLocation != null)
        {
            Tile.Shape = null;
            Tile = targetLocation;
            transform.position = Tile.transform.position;
        }
        
        if (turnsAlive <= dieAfterTurns - 1)
        {
            var emptyTiles = gameManager.Grid.GetEmptyTiles();
            targetIndicator.gameObject.SetActive(true);
            targetLocation = emptyTiles[Random.Range(0, emptyTiles.Count)];
            targetLocation.Shape = this;
            targetIndicator.transform.position = targetLocation.transform.position;    
        }
        else
            targetIndicator.gameObject.SetActive(false); // hide target indicator if there won't be more moves
        
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