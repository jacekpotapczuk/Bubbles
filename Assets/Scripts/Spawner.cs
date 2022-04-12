using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    
    [SerializeField] private Circle circlePrefab;
    [SerializeField] private Statue statuePrefab;
    [SerializeField] private Rainbow rainbowPrefab;
    [SerializeField] private Imp impPrefab;

    private ShapeFactory<Circle> circleFactory;
    private ShapeFactory<Statue> statueFactory;
    private ShapeFactory<Rainbow> rainbowFactory;
    private ShapeFactory<Imp> impFactory;
    
    public HashSet<Shape> Shapes { get; private set; }
    
    private void Awake()
    {
        Shapes = new HashSet<Shape>();
        var t = transform;
        circleFactory =  new ShapeFactory<Circle>(circlePrefab, t);
        statueFactory =  new ShapeFactory<Statue>(statuePrefab, t);
        rainbowFactory = new ShapeFactory<Rainbow>(rainbowPrefab, t);
        impFactory =     new ShapeFactory<Imp>(impPrefab, t);
    }
    
    public void SpawnStatue(Tile tile)
    {
        var shape = statueFactory.SpawnAt(tile, null);
        shape.Factory ??= statueFactory;
        Shapes.Add(shape);
    }
    
    public void SpawnRainbow(Tile tile)
    {
        var shape = rainbowFactory.SpawnAt(tile, null);
        shape.Factory ??= rainbowFactory;
        Shapes.Add(shape);
    }
    
    public void SpawnImp(Tile tile)
    {
        var shape = impFactory.SpawnAt(tile, null);
        shape.Factory ??= impFactory;
        Shapes.Add(shape);
    }
    
    public void SpawnCircle(Tile tile)
    {
        SpawnCircle(tile, gameManager.GetRandomColor());
    }
    
    public void SpawnCircle(Tile tile, Color color)
    {
        var shape = circleFactory.SpawnAt(tile, color);
        shape.Factory ??= circleFactory;
        
        var gameColor = gameManager.ColorToGameColor(color);
        var rand01 = Random.Range(0f, 1f);
        shape.Ability =  rand01 <= gameColor.abilitySpawnChance ? gameColor.circleAbility : null;
        Shapes.Add(shape);
    }
}
