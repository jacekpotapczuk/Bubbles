using System.Threading.Tasks;
using UnityEngine;

public class Circle : Shape
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

    private Color color;
    
    public override bool MatchColor(Color color)
    {
        return this.color == color;
    }

    public async Task MoveAlong(Tile[] tiles)
    {
        tiles[0].Shape = null;
        tiles[tiles.Length - 1].Shape = this;
        
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
}
