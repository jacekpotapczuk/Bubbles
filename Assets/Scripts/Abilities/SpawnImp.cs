using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Spawn Imp")]
public class SpawnImp : CircleAbility
{
    public override void Do(Circle owner, GameManager gameManager)
    {
        gameManager.Spawner.SpawnImp(owner.Tile);
    }
}