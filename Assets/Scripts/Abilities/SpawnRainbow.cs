using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Spawn Rainbow")]
public class SpawnRainbow : CircleAbility
{
    public override void Do(Circle owner, GameManager gameManager)
    {
        gameManager.Spawner.SpawnRainbow(owner.Tile);
        CheckForPoints(owner.Tile.NorthNeighbour, gameManager);
        CheckForPoints(gameManager.Grid.GetTileInDirection(owner.Tile, 1, 1), gameManager);
        CheckForPoints(owner.Tile.EastNeighbour, gameManager);
        CheckForPoints(gameManager.Grid.GetTileInDirection(owner.Tile, 1, -1), gameManager);
        CheckForPoints(owner.Tile.SouthNeighbour, gameManager);
        CheckForPoints(gameManager.Grid.GetTileInDirection(owner.Tile, 1, -1), gameManager);
        CheckForPoints(owner.Tile.WestNeighbour, gameManager);
        CheckForPoints(gameManager.Grid.GetTileInDirection(owner.Tile, -1, 1), gameManager);
    }

    private static void CheckForPoints(Tile tile, GameManager gameManager)
    {
        if (tile != null && tile.Shape != null) 
            gameManager.CheckForPoints(tile.Shape);
    }
}