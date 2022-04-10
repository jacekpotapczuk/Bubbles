using UnityEngine;
using Random = UnityEngine.Random;

public class CircleFactory : MonoBehaviour
{
    [SerializeField] private Circle circlePrefab;

    [SerializeField] private Color[] availableColors;

    private ShapeFactory<Circle> factory;
    private void Awake()
    {
        factory = new ShapeFactory<Circle>(circlePrefab, transform);
    }

    public void SpawnAt(Tile tile)
    {
        var circle = factory.SpawnAt(tile);
        if (circle == null)
            return;
        if(circle.Factory == null)
            circle.Factory = this;
        circle.Color = availableColors[Random.Range(0, availableColors.Length)];
    }

    public void RemoveAt(Tile tile)
    {
        factory.RemoveAt(tile);
    }

    public void Remove(Circle circle)
    {
        factory.Reclaim(circle);
    }
}
