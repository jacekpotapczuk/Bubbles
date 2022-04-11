using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Spawn Statue")]
public class SpawnStatue : CircleAbility
{
    public override void Do(Circle owner, GameManager gameManager)
    {
        Debug.Log("do spawn statue");
        gameManager.Spawner.SpawnStatue(owner.Tile);
    }
}
