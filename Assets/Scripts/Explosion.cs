using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Explosion")]
public class Explosion : CircleAbility
{
    public override void Do(Circle owner, GameManager gameManager)
    {
        Debug.Log("Explosion do");
        RemoveAt(owner.Tile.NorthNeighbour);
        RemoveAt(owner.Tile.SouthNeighbour);
        RemoveAt(owner.Tile.WestNeighbour);
        RemoveAt(owner.Tile.EastNeighbour);
    }

    private void RemoveAt(Tile tile)
    {
        if(tile != null && tile.Shape != null && tile.Shape is Circle)
            tile.Shape.Remove();
    }
}
