using System.Collections.Generic;

public class ByFScore : IComparer<INode>
{
    public int Compare(INode tile1, INode tile2)
    {
        if (tile1.FScore < tile2.FScore)
            return -1;
        if (tile2.FScore > tile1.FScore)
            return 1;
        
        // this order doesn't really matter,
        // we just want to distinguish between tiles and return 0 only of coordinates are the same
        // however this can be changed to alter search preferences
        if (tile1.X < tile2.X)
            return -1;
        if (tile1.X > tile2.X)
            return 1;
        if (tile1.Y < tile2.Y)
            return -1;
        if (tile1.Y > tile2.Y)
            return 1;
        
        return 0;
    }
}