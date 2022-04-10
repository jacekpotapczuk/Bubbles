using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour, INode
{
    public int X { get; private set; }
    public int Y { get; private set; }
    
    public Shape Shape { get; set; }
    
    public Tile NorthNeighbour { get; private set; }
    public Tile SouthNeighbour { get; private set; }
    public Tile WestNeighbour { get; private set; }
    public Tile EastNeighbour { get; private set; }
    

    // pathfinding
    public int GScore { get; set; }
    public int HScore { get; set; }
    public int FScore => GScore + HScore;
    
    public INode Parent { get; set; }

    public bool IsWalkable => Shape == null;

    
    public List<INode> GetNeighbours()
    {
        var neighbours = new List<INode>();
        if (NorthNeighbour != null)
            neighbours.Add(NorthNeighbour);
        if (SouthNeighbour != null)
            neighbours.Add(SouthNeighbour);
        if (WestNeighbour != null)
            neighbours.Add(WestNeighbour);
        if (EastNeighbour != null)
            neighbours.Add(EastNeighbour);
        return neighbours;
    }
    

    public void Initialize(int x, int y, Tile northNeighbour, Tile southNeighbour, Tile westNeighbour, Tile eastNeighbour)
    {
        X = x;
        Y = y;
        NorthNeighbour = northNeighbour;
        SouthNeighbour = southNeighbour;
        WestNeighbour = westNeighbour;
        EastNeighbour = eastNeighbour;
    }

    public void CleanUp()
    {
        if(Shape != null)
            Shape.Remove();
        GScore = 0;
        HScore = 0;
        Parent = null;
    }

    public override string ToString()
    {
        return $"({X},{Y})";
    }
}