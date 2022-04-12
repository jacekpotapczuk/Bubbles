using UnityEngine;

public class Statue : Shape
{
    [SerializeField, Range(2, 10)] private int dieAfterTurns = 5; 
    
    public override Color? Color { get; set; } = null;
    
    public ShapeFactory<Statue> Factory
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
    private ShapeFactory<Statue> factory;
    
    private int turnsAlive;
    
    public override bool MatchColor(Color? color)
    {
        return false;
    }

    public override void Clear()
    {
        turnsAlive = 0;
    }

    public override void OnNewTurn(GameManager gameManager)
    {
        turnsAlive += 1;
        Debug.Log($"Statue, on new turn: {turnsAlive}, {dieAfterTurns}");
        if(turnsAlive >= dieAfterTurns)
            Remove();
    }

    public override void Remove()
    {
        Tile.Shape = null;
        Factory.Reclaim(this);
    }
}
