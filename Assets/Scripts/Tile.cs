using UnityEngine;

public class Tile : MonoBehaviour
{
    public int X { get; private set; }
    public int Y { get; private set; }
    
    
    public Tile NorthNeighbour { get; private set; }
    public Tile SouthNeighbour { get; private set; }
    public Tile WestNeighbour { get; private set; }
    public Tile EastNeighbour { get; private set; }



    public void Initialize(int x, int y, Tile northNeighbour, Tile southNeighbour, Tile westNeighbour, Tile eastNeighbour)
    {
        X = x;
        Y = y;
        NorthNeighbour = northNeighbour;
        SouthNeighbour = southNeighbour;
        WestNeighbour = westNeighbour;
        EastNeighbour = eastNeighbour;
    }
}