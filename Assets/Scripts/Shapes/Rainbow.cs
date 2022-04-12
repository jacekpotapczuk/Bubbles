using UnityEngine;

public class Rainbow : Shape
{
    public ShapeFactory<Rainbow> Factory
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
    private ShapeFactory<Rainbow> factory;
    
    public override Color? Color { get; set; } = null;
    public override bool MatchColor(Color? color)
    {
        return true;
    }

    public override void Remove()
    {
        Tile.Shape = null;
        Factory.Reclaim(this);
    }

    public override void Clear()
    {
    }
}