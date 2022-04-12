using System.Threading.Tasks;

public interface IMovable
{
    public Task<bool> TryMoveTo(Tile endTile, GameGrid grid); // return false if move is not possible, else true
}
