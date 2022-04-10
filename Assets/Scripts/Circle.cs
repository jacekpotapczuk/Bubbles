using UnityEngine;

public class Circle : Shape
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    public Color Color
    {
        get => color;
        set
        {
            color = value;
            spriteRenderer.color = color;
        }
    }

    private Color color;
    
    public override bool MatchColor(Color color)
    {
        return this.color == color;
    }
}
