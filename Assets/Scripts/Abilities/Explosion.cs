using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Explosion")]
public class Explosion : CircleAbility
{
    public override void Do(Circle owner, GameManager gameManager)
    {
        RemoveAt(owner.Tile.NorthNeighbour, gameManager);
        RemoveAt(owner.Tile.SouthNeighbour, gameManager);
        RemoveAt(owner.Tile.WestNeighbour, gameManager);
        RemoveAt(owner.Tile.EastNeighbour, gameManager);
    }

    private static void RemoveAt(Tile tile, GameManager gameManager)
    {
        if(tile != null && tile.Shape != null && tile.Shape is Circle)
            tile.Shape.Die(gameManager);
    }
}
